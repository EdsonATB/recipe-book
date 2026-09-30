using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using MyRecipeBook.Application.UseCases.User.Login.WithEmailAndPassword;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;
using System.Net;

namespace UseCases.Tests.User.Login.WithEmailAndPassword;

public class LoginWithEmailAndPasswordUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        //AAA
        //Arrange
        var (user, _) = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email; //Forçando eles a serem iguais pois geram emails diferentes (talvez por ter escopos diferentes entre projetos??)

        var useCase = CreateUseCase(request.Password, user);

        //Act
        var result = await useCase.Execute(request);

        //Assert
        result.ShouldNotBeNull();
        result.Name.ShouldNotBeNull();
        result.Name.ShouldBe(user.Name);
        result.Tokens.AccessToken.ShouldBeNullOrEmpty();
        result.Tokens.RefreshToken.ShouldBeNullOrEmpty();
    }
    
    [Fact]
    public async Task ShouldThrowException_WhenUserDontExist()
    {
        //AAA
        //Arrange
        var request = RequestLoginJsonBuilder.Build();
        
        var useCase = CreateUseCase(); //nao precisa nem dar a senha pois vai bater no erro do user primeiro no useCase

        //Act-Assert
        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidLoginException>();
        exception.getErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_LOGIN_INVALID);
        });

    }
    
    [Fact]
    public async Task ShouldThrowException_WhenPasswordIsIncorrect()
    {
        //AAA
        //Arrange
        var (user, _) = UserBuilder.Build();
        var request = RequestLoginJsonBuilder.Build();
        request.Email = user.Email; //Forçando eles a serem iguais pois geram emails diferentes (talvez por ter escopos diferentes entre projetos??)

        var useCase = CreateUseCase(user: user);

        //Act-Assert
        var exception = await useCase.Execute(request).ShouldThrowAsync<InvalidLoginException>();

        exception.getStatusCode().ShouldBe(HttpStatusCode.Unauthorized);
        exception.getErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_LOGIN_INVALID);
        });
    }



    private LoginWithEmailAndPasswordUseCase CreateUseCase(string? password = null,MyRecipeBook.Domain.Entities.User? user = null)
    {
        var passwordHasherBuilder = new IPasswordHasherBuilder();
        var userReadOnlyRepositoryBuilder = new IUserReadOnlyRepositoryBuilder();
        if (user is not null)
        {
            userReadOnlyRepositoryBuilder.GetByEmail(user);
        }
        if (password is not null) 
        {
            passwordHasherBuilder.VerifyPassword(password);
        }

        return new LoginWithEmailAndPasswordUseCase(passwordHasherBuilder.Build(), userReadOnlyRepositoryBuilder.Build());
    }
}
