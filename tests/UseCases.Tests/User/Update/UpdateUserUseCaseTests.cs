using CommonTestUtilities.Entities;
using CommonTestUtilities.Identity;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Update;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;

namespace UseCases.Tests.User.Update;

public class UpdateUserUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var (user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        await useCase.Execute(request).ShouldNotThrowAsync();

        user.Name.ShouldBe(request.Name);
        user.Email.ShouldBe(request.Email);
    }

    [Fact]
    public async Task ShouldHaveError_WhenRequestEmailAlreadyInUse()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var (user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user, request.Email);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.getErrorMessages().Count.ShouldBe(1);
        exception.getErrorMessages().ShouldContain(ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS);
    }
    [Fact]
    public async Task ShouldHaveError_WhenRequestNameIsEmpty()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;
        var (user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.getErrorMessages().Count.ShouldBe(1);
        exception.getErrorMessages().ShouldContain(ResourceMessagesException.VALIDATION_NAME_REQUIRED);
    }


    private UpdateUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, string? emailThatAlreadyExists = null)
    {
        var readOnlyRepository = new IUserReadOnlyRepositoryBuilder();
        var updateOnlyRepository = IUserUpdateOnlyRepositoryBuilder.Build();
        var loggedUser = ILoggedUserBuilder.Build(user);
        var unitOfWork = IUnitOfWorkBuilder.Build();
        if(emailThatAlreadyExists is not null)
        {
            readOnlyRepository.ExistActiveUserWithEmail(emailThatAlreadyExists);
        }

        return new UpdateUserUseCase(readOnlyRepository.Build(), loggedUser, updateOnlyRepository, unitOfWork);
    }
}
