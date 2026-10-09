using Mapster;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.Application.UseCases.User.Profile;

public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;

    public GetUserProfileUseCase(ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
    }

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.Get();

        return user.Adapt<ResponseUserProfileJson>();
    }
}
