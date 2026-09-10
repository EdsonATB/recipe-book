using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;
using Xunit.Sdk;

namespace UseCases.Tests.User.Register;

public class RegisterUserAccountUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        //AAA

        //Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        var useCase = CreateUseCase();

        //Act
        var result = await useCase.Execute(request);

        //Assert
        result.ShouldNotBeNull();
        result.Tokens.ShouldNotBeNull();
        result.Name.ShouldBe(request.Name);
        result.Tokens.AccessToken.ShouldBeEmpty();
        result.Tokens.RefreshToken.ShouldBeEmpty();
    }

    [Fact]
    public async Task Validate_ShouldThrowException_WhenNameIsEmpty()
    {
        //Arrange

        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        //Act e assert juntos

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();
        exception.getErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_NAME_REQUIRED);

        });
    }
    [Fact]
    public async Task Validate_ShouldThrowException_WhenEmailAlreadyExists()
    {
        //Arrange

        var request = RequestRegisterUserAccountJsonBuilder.Build();

        var useCase = CreateUseCase(request.Email);

        //Act e assert juntos

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();
        exception.getErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS);

        });
    }

    private RegisterUserAccountUseCase CreateUseCase(string? emailThatAlreadyExists = null)
    {
        var unitOfWork = IUnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = IUserWriteOnlyRepositoryBuilder.Build();
        var passwordHasher = new IPasswordHasherBuilder().Build();
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        if (emailThatAlreadyExists.IsNotEmpty())
        {
            userReadOnlyRepositoryBuilder.ExistActiveUserWithEmail(emailThatAlreadyExists);
        }

        return new RegisterUserAccountUseCase(passwordHasher, userWriteOnlyRepository, userReadOnlyRepositoryBuilder.Build(), unitOfWork);
    }
}
