using Azure;
using CommonTestUtilities.Requests;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Tests.InlineData;
using WebApi.Tests.Resources;

namespace WebApi.Tests.User.UpdateUser;

public class UpdateUserTests : BaseIntegrationTests
{
    private const string REQUEST_URI = "users/profile";
    private readonly UserIdentityManager _user1;
    public UpdateUserTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _user1 = factory.User1;
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await Put(REQUEST_URI, request, _user1.getToken());

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var userExists = await DbContext
            .Users
            .AnyAsync(user => user.Active && user.Id == _user1.getId() && user.Name.Equals(request.Name) && user.Email.Equals(request.Email));
        
        userExists.ShouldBeTrue();
    }

    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task ShouldHaveError_When_NameIsEmpty(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var response = await Put(REQUEST_URI, request, _user1.getToken(), culture);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_NAME_REQUIRED", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(error =>
        {
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }
    [Theory]
    [ClassData(typeof(CultureInlineData))]
    public async Task ShouldHaveError_When_NameEmailIsInvalid(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "asda.com";

        var response = await Put(REQUEST_URI, request, _user1.getToken(), culture);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("VALIDATION_EMAIL_INVALID", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(error =>
        {
            errors.Count().ShouldBe(1);
            errors.ShouldContain(error => error.GetString().IsNotEmpty() && error.GetString()!.Equals(expectedMessage));
        });
    }
}
