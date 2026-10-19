using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Http;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.User.ChangePassword;

public class ChangePasswordTests : BaseIntegrationTests
{
    private const string REQUEST_URI = "users/password";
    private readonly UserIdentityManager _user1;
    private readonly UserIdentityManager _userSuccessTest;
    public ChangePasswordTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
        _userSuccessTest = factory.UserPasswordSuccessTest;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.CurrentPassword = _userSuccessTest.getPassword();

        var result = await Put(REQUEST_URI, request, accessToken: _userSuccessTest.getToken());

        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task ShouldHaveError_When_NewPasswordEmpty(string culture)
    {
        var request = new RequestChangePasswordJson
        {
            CurrentPassword = _user1.getPassword(),
            NewPassword = string.Empty
        };

        var response = await Put(REQUEST_URI, request, accessToken: _user1.getToken(), culture);

        await using var responseBody = await response.Content.ReadAsStreamAsync();
        
        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_PASSWORD_REQUIRED", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(error =>
        {
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }

}
