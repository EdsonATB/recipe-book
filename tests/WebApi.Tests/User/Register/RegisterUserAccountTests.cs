using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Tests.InlineData;

namespace WebApi.Tests.User.Register;

public class RegisterUserAccountTests : IClassFixture<MyRecipeBookApplicationFactory>
{
    private readonly HttpClient _httpClient;
    private const string REQUEST_URI = "/users";


    public RegisterUserAccountTests(MyRecipeBookApplicationFactory factory) //o factory representa o server rodando a API
    {
        _httpClient = factory.CreateClient(); //instancia de HttpClient
    }


    [Fact]
    public async Task Success()
    {
        //Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();

        //Act
        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        //await = assincrono, using = libera esse espaço na memoria depois que for usado, ReadAsStreamAsync mais leve que ReadAsStringAsync
        
        var responseData = await JsonDocument.ParseAsync(responseBody);
        responseData.RootElement.GetProperty("name").GetString().ShouldBe(request.Name); //no getProperty() deve ser com letra minuscula
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldBeEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task Validate_ShouldBeAnErrorResponse_WhenNameIsEmpty(string culture)
    {
        //Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Name = string.Empty;

        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);
        
        //Act
        var response = await _httpClient.PostAsJsonAsync(REQUEST_URI, request);

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(ResourceMessagesException.ResourceManager.GetString("VALIDATION_NAME_REQUIRED", new CultureInfo(culture)))); //shouldContain espera bool

        });
    }

}

