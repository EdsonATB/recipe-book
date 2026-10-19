using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;

namespace WebApi.Tests.User.Profile;

public class GetUserProfileInvalidTokenTests : BaseIntegrationTests
{
    private const string REQUEST_URI = "/users";
    private readonly string _tokenUserNotExistInDatabase;

    public GetUserProfileInvalidTokenTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistInDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }


    [Fact]
    public async Task Error_When_TokenUser_NotFound()
    {
        var result = await Get(REQUEST_URI, _tokenUserNotExistInDatabase);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_When_TokenUser_Invalid()
    {
        var result = await Get(REQUEST_URI, "Token");

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
    [Fact]
    public async Task Error_When_TokenUser_IsEmpty()
    {
        var result = await Get(REQUEST_URI, accessToken: string.Empty);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
