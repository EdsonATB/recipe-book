using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.User.Register;

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
        Assert.True(result.IsValid); //o teste deveria dar true
    }
}
