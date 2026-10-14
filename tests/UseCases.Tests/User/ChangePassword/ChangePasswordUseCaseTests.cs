using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.User.ChangePassword;

public class ChangePasswordUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, password) = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.CurrentPassword = password;
        var useCase = createUseCase(user, password);

        await useCase.Execute(request).ShouldNotThrowAsync();
    }

    [Fact]
    public async Task ShouldHaveError_WhenCurrentPasswordIsIncorrect()
    {
        var (user, password) = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        var useCase = createUseCase(user, password);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.getErrorMessages().Count.ShouldBe(1);
        exception.getErrorMessages().ShouldContain(ResourceMessagesException.VALIDATION_CURRENT_PASSWORD);
    }
    [Fact]
    public async Task ShouldHaveError_WhenNewPasswordIsEmpty()
    {
        var (user, password) = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.CurrentPassword = password;
        request.NewPassword = string.Empty;
        
        var useCase = createUseCase(user, password);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.getErrorMessages().Count.ShouldBe(1);
        exception.getErrorMessages().ShouldContain(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED);
    }

    
    private ChangePasswordUseCase createUseCase(MyRecipeBook.Domain.Entities.User user, string password)
    {
        var loggedUser = ILoggedUserBuilder.Build(user);
        var passwordHasher = new IPasswordHasherBuilder().VerifyPassword(password).Build();
        var updateOnlyRepository = IUserUpdateOnlyRepositoryBuilder.Build();
        
        return new ChangePasswordUseCase(loggedUser, passwordHasher, updateOnlyRepository);
    }
}
