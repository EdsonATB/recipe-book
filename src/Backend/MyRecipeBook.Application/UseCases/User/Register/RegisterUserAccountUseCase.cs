using FluentValidation.Results;
using Mapster;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Register;

public class RegisterUserAccountUseCase : IRegisterUserAccountUseCase
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    public RegisterUserAccountUseCase(IPasswordHasher passwordHasher,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unitOfWork
        )
    {
        _passwordHasher = passwordHasher;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _userReadOnlyRepository = userReadOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserAccountJson request)
    {
        await ValidateAndThrowOnFailures(request); //Validando os inputs do user

        var user = request.Adapt<Domain.Entities.User>(); // Mapeando os dados do request para a Entidade User. (Usando a lib "Mapster"). Funciona pois os nomes dos atributos do objeto da request e os nomes da entidade sao os mesmos.

        user.Password = _passwordHasher.HashPassword(request.Password); // Hashando a senha
        
        await _userWriteOnlyRepository.Add(user);
        await _unitOfWork.Commit();

        return new ResponseRegisteredUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson()
        };
    }



    private async Task ValidateAndThrowOnFailures(RequestRegisterUserAccountJson request)
    {
        var validator = new RegisterUserAccountValidator();
        var result = validator.Validate(request);

        var emailCheck = await _userReadOnlyRepository.ExistActiveUserWithEmail(request.Email);
        if(emailCheck)
        {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS));
        }

        if (!result.IsValid) //verifica se teve algum erro nas validacoes
        {
            var errorMessage = result.Errors.Select(err => err.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessage);
        }
    }
}
