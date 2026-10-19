using CommonTestUtilities.Requests;
using Shouldly;
using System.Net;

namespace WebApi.Tests.User.UpdateUser;

public class UpdateUserInvalidTokenTests : BaseIntegrationTests
{
    private const string REQUEST_URI = "users/profile";
    private readonly string _tokenUserNotExistInDatabase;

    public UpdateUserInvalidTokenTests(MyRecipeBookApplicationFactory factory) : base(factory)
    {
        _tokenUserNotExistInDatabase = factory.TOKEN_USER_NOT_FOUND_IN_DATABASE;
    }


    [Fact]
    public async Task Error_When_UserToken_NotFound()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await Put(REQUEST_URI, request, _tokenUserNotExistInDatabase);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_When_TokenInvalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await Put(REQUEST_URI, request, "Token");

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_When_TokenEmpty()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = await Put(REQUEST_URI, request, accessToken: string.Empty);

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}
