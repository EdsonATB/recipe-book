using FluentValidation.Results;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;

namespace MyRecipeBook.Application.UseCases.User.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserUseCase(IUserReadOnlyRepository readOnlyRepository, ILoggedUser loggedUser, 
                            IUserUpdateOnlyRepository updateOnlyRepository, IUnitOfWork unitOfWork)
    {
        _readOnlyRepository = readOnlyRepository;
        _loggedUser = loggedUser;
        _updateOnlyRepository = updateOnlyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.Get();

        await Validate(request, loggedUser);

        loggedUser.Name = request.Name;
        loggedUser.Email = request.Email;

        _updateOnlyRepository.UpdateProfile(loggedUser);
        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, Domain.Entities.User loggedUser)
    {
        var validator = new UpdateUserValidator();
        var result = validator.Validate(request);

        if (loggedUser.Email.Equals(request.Email) == false)
        {
            var userExists = await _readOnlyRepository.ExistActiveUserWithEmail(request.Email);
            if (userExists) 
            {
                result.Errors.Add(new ValidationFailure(string.Empty, ResourceMessagesException.VALIDATION_EMAIL_ALREADY_EXISTS));
            }
        }
        if (!result.IsValid) //verifica se teve algum erro nas validacoes
        {
            var errorMessage = result.Errors.Select(err => err.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessage);
        }

    }
}
