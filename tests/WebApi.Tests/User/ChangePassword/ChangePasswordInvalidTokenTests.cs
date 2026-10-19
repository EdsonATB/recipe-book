using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;
using WebApi.Tests.Resources;

namespace WebApi.Tests.User.ChangePassword;

public class ChangePasswordInvalidTokenTests : BaseIntegrationTests
{
    private const string REQUEST_URI = "users/password";
    private readonly string _tokenUserNotExistInDatabase;

    public ChangePasswordInvalidTokenTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistInDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }

    [Fact]
    public async Task Error_When_TokenUser_NotFound()
    {
        var request = RequestChangePasswordJsonBuilder.Build();

        var result = await Put(REQUEST_URI, request, _tokenUserNotExistInDatabase);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    [Fact]
    public async Task Error_When_TokenEmpty()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        var token = string.Empty;

        var result = await Put(REQUEST_URI, request, token);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    [Fact]
    public async Task Error_When_TokenInvalid()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        

        var result = await Put(REQUEST_URI, request, "Token");

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

}
