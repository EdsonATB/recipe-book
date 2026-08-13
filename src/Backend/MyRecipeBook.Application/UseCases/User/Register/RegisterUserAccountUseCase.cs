using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserAccountUseCase : IRegisterUserAccountUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    public RegisterUserAccountUseCase(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public void Execute(RequestRegisterUserAccountJson request)
    {
        ValidateAndThrowOnFailures(request);

        var user = request.Adapt<Domain.Entities.User>(); // Mapeando os dados do request para a Entidade User. (Usando a lib "Mapster"). Funciona pois os nomes dos atributos do objeto da request e os nomes da entidade sao os mesmos.

        user.Password = _passwordHasher.HashPassword(request.Password);    
    }



    private void ValidateAndThrowOnFailures(RequestRegisterUserAccountJson request)
    {
        var validator = new RegisterUserAccountValidator();

        var result = validator.Validate(request);

        if (!result.IsValid) //verifica se teve algum erro nas validacoes
        {
            var errorMessage = result.Errors.Select(err => err.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessage);
        }
    }
}
