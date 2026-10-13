using FluentValidation;
using MyRecipeBook.Exception;

namespace MyRecipeBook.Application.UseCases.Shared;

public static class PasswordValidator
{
    extension<TRequest>(IRuleBuilderInitial<TRequest, string> ruleBuilder)
    {
        internal IRuleBuilderOptions<TRequest, string> Password() //metodo de extensao
        { //TRequest = objeto generico
            return ruleBuilder
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_REQUIRED)
                .Length(6, 30)
                .WithMessage(ResourceMessagesException.VALIDATION_PASSWORD_LENGTH);
        }
    }

}
