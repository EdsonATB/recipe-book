using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserAccountUseCase : IRegisterUserAccountUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    public RegisterUserAccountUseCase(IPasswordHasher passwordHasher, IUserWriteOnlyRepository userWriteOnlyRepository)
    {
        _passwordHasher = passwordHasher;
        _userWriteOnlyRepository = userWriteOnlyRepository;
    }

    public async Task Execute(RequestRegisterUserAccountJson request)
    {
        ValidateAndThrowOnFailures(request); //Validando os inputs do user

        var user = request.Adapt<Domain.Entities.User>(); // Mapeando os dados do request para a Entidade User. (Usando a lib "Mapster"). Funciona pois os nomes dos atributos do objeto da request e os nomes da entidade sao os mesmos.

        user.Password = _passwordHasher.HashPassword(request.Password); // Hashando a senha
        
        await _userWriteOnlyRepository.Add(user);
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
