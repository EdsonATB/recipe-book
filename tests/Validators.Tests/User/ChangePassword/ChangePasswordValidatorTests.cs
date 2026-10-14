using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Exception;
using Shouldly;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Validators.Tests.User.ChangePassword;

public class ChangePasswordValidatorTests
{

    [Fact]
    public void Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        var validator = new ChangePasswordUseCaseValidator();

        var result = validator.Validate(request);

        result.Errors.Count.ShouldBe(0);
        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("        ")]
    public void ShouldHaveError_WhenNewPasswordIsEmpty(string newPassword)
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = newPassword;
        var validator = new ChangePasswordUseCaseValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(31)]
    public void ShouldHaveError_WhenNewPasswordIsShortOrLarge(int passwordLength)
    {
        var request = RequestChangePasswordJsonBuilder.Build(length: passwordLength);
        var validator = new ChangePasswordUseCaseValidator();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.Count.ShouldBe(1);
        result.Errors.ShouldContain(error => error.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_PASSWORD_LENGTH));

    }
}
