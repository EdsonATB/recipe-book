using FluentValidation;
using MyRecipeBook.Application.UseCases.Shared;
using MyRecipeBook.Communication.Requests;

namespace MyRecipeBook.Application.UseCases.User.ChangePassword;

public class ChangePasswordUseCaseValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordUseCaseValidator()
    {
        RuleFor(request => request.NewPassword).Password();
    }
}
