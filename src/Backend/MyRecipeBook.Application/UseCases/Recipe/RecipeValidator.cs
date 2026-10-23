using FluentValidation;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exception;

namespace MyRecipeBook.Application.UseCases.Recipe;

public class RecipeValidator :AbstractValidator<RequestRecipeJson>
{
    public RecipeValidator()
    {
        RuleFor(recipe => recipe.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_TITLE_REQUIRED)
            .MaximumLength(250)
            .WithMessage(ResourceMessagesException.VALIDATION_TITLE_MAX_LENGTH);

        RuleFor(recipe => recipe.CookTime)
            .IsInEnum()
            .WithMessage(ResourceMessagesException.VALIDATION_COOKTIME_INVALID);

        RuleForEach(recipe => recipe.DishTypes) //for each por ser uma lista de enum
            .IsInEnum()
            .WithMessage(ResourceMessagesException.VALIDATION_DISHTYPE_INVALID);

        RuleFor(recipe => recipe.Ingredients)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_INGREDIENT_REQUIRED);

        RuleForEach(recipe => recipe.Ingredients)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_INGREDIENT_REQUIRED)
            .MaximumLength(250)
            .WithMessage(ResourceMessagesException.VALIDATION_INGREDIENT_MAX_LENGTH);

        RuleFor(recipe => recipe.Instructions)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_INSTRUCTION_DESCRIPTION_REQUIRED);

        RuleForEach(recipe => recipe.Instructions).ChildRules(instruction =>
        {
            instruction.RuleFor(item => item.Order)
            .GreaterThan(0)
            .WithMessage(ResourceMessagesException.VALIDATION_INSTRUCTION_GREATER_THAN_ZERO);

            instruction.RuleFor(item => item.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceMessagesException.VALIDATION_INSTRUCTION_DESCRIPTION_REQUIRED)
            .MaximumLength(2000)
            .WithMessage(ResourceMessagesException.VALIDATION_INSTRUCTION_DESCRIPTION_MAX_LENGTH);
        });

    }
}
