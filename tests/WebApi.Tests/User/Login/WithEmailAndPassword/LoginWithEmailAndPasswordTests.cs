using CommonTestUtilities.Requests;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.User.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordTests : BaseIntegrationTests
{
    private const string REQUEST_URI = "/authentication";
    private readonly UserIdentityManager _user1;

    public LoginWithEmailAndPasswordTests(MyRecipeBookApplicationFactory factory) : base(factory) //base repassa esse valor para o construtor da classe pai tb
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        //Arrange
        var request = new RequestLoginJson
        {
            Email = _user1.getEmail(),
            Password = _user1.getPassword()
        };

        //Act
        var response = await Post(REQUEST_URI, request);

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        
        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_user1.getName()); //no getProperty() deve ser com letra minuscula
        responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString().ShouldBeEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task ShouldThrowException_WhenUserDontExist(string culture)
    {
        //Arrange
        var request = RequestLoginJsonBuilder.Build();

        //Act
        var response = await Post(REQUEST_URI, request, culture);

        //Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        errors.ShouldSatisfyAllConditions(errorsList =>
        {
            errorsList.Count().ShouldBe(1);
            errorsList.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(ResourceMessagesException.ResourceManager.GetString("VALIDATION_LOGIN_INVALID", new CultureInfo(culture)))); //shouldContain espera bool

        });
    
}
}
