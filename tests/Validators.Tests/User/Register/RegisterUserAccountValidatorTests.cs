using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Exception;
using Shouldly;

namespace Validators.Tests.User.Register;

public class RegisterUserAccountValidatorTests
{
    [Fact]
    public void Success()
    {
        //AAA

        //Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();

        var validator = new RegisterUserAccountValidator();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeTrue(); //o teste deveria dar true (Shouldly NuGet)
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("        ")]
    public void Validate_ShouldHaveError_WhenNameIsEmpty(string name)
    {
        //AAA

        //Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Name = name;

        var validator = new RegisterUserAccountValidator();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_NAME_REQUIRED));
        });
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("        ")]
    public void Validate_ShouldHaveError_WhenEmailIsEmpty(string email)
    {
        //AAA

        //Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Email = email;

        var validator = new RegisterUserAccountValidator();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_EMAIL_REQUIRED));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenPasswordIsEmpty()
    {
        //AAA

        //Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Password = string.Empty;

        var validator = new RegisterUserAccountValidator();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_PASSWORD_LENGTH));
        });
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenEmailIsInvalid()
    {
        //AAA

        //Arrange
        var request = RequestRegisterUserAccountJsonBuilder.Build();
        request.Email = "asda.com";

        var validator = new RegisterUserAccountValidator();

        //Act
        var result = validator.Validate(request);

        //Assert
        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(errors =>
        {
            errors.Count.ShouldBe(1);
            errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_EMAIL_INVALID));
        });
    }
}

