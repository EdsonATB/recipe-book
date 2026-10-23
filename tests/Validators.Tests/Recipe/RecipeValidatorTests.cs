using CommonTestUtilities.Requests;
using MyRecipeBook.Application.UseCases.Recipe;
using MyRecipeBook.Communication.Enums;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Exception;
using Shouldly;

namespace Validators.Tests.Recipe;

public class RecipeValidatorTests
{
    [Fact]
    public void Success()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        
        var response = validator.Validate(request);

        response.IsValid.ShouldBeTrue();
        response.Errors.ShouldBeEmpty();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("        ")]
    [InlineData("")]
    public void Validate_Error_WhenTitleEmpty(string title) 
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = title;

        var response = validator.Validate(request);

        response.IsValid.ShouldBeFalse();
        response.Errors.Count.ShouldBe(1);
        response.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_TITLE_REQUIRED));
    }

    [Fact]
    public void Validate_Error_WhenTitle_IsTooLong() 
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Title = new string('a', 251);

        var response = validator.Validate(request);

        response.IsValid.ShouldBeFalse();
        response.Errors.Count.ShouldBe(1);
        response.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_TITLE_MAX_LENGTH));
    }

    [Fact]
    public void Validate_Error_When_CookTime_NotInEnum() 
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.CookTime = (CookTime)1000;

        var response = validator.Validate(request);

        response.IsValid.ShouldBeFalse();
        response.Errors.Count.ShouldBe(1);
        response.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_COOKTIME_INVALID));
    }

    [Fact]
    public void Validate_Error_When_DishTypes_Empty() 
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes = [];

        var response = validator.Validate(request);

        response.IsValid.ShouldBeFalse();
        response.Errors.Count.ShouldBe(1);
        response.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_DISH_TYPE_REQUIRED));
    }

    [Fact]
    public void Validate_Error_When_DishType_Invalid() 
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.DishTypes = [(DishType)1000];

        var response = validator.Validate(request);

        response.IsValid.ShouldBeFalse();
        response.Errors.Count.ShouldBe(1);
        response.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_DISHTYPE_INVALID));
    }

    [Fact]
    public void Validate_Error_When_Ingredients_Empty()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients = [];

        var response = validator.Validate(request);

        response.IsValid.ShouldBeFalse();
        response.Errors.Count.ShouldBe(1);
        response.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INGREDIENT_REQUIRED));
    }

    [Fact]
    public void Validate_Error_When_Ingredient_Item_TooLarge()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients = [new string('a', 251)];

        var response = validator.Validate(request);

        response.IsValid.ShouldBeFalse();
        response.Errors.Count.ShouldBe(1);
        response.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INGREDIENT_MAX_LENGTH));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("        ")]
    [InlineData("")]
    public void Validate_Error_When_Ingredient_Item_Empty(string ingredient)
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Ingredients = [ingredient];

        var response = validator.Validate(request);

        response.IsValid.ShouldBeFalse();
        response.Errors.Count.ShouldBe(1);
        response.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INGREDIENT_REQUIRED));
    }

    [Fact]
    public void Validate_Error_When_Instructions_Empty()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions = [];

        var response = validator.Validate(request);

        response.IsValid.ShouldBeFalse();
        response.Errors.Count.ShouldBe(1);
        response.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INSTRUCTION_DESCRIPTION_REQUIRED));
    }

    [Fact]
    public void Validate_Error_When_Instructions_OrderDuplicated()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions = [new RequestRecipeInstructionJson { Order = 1, Description = "valid desc"},
                                new RequestRecipeInstructionJson { Order = 1, Description = "valid desc"}];

        var response = validator.Validate(request);

        response.IsValid.ShouldBeFalse();
        response.Errors.Count.ShouldBe(1);
        response.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INSTRUCTION_ORDER_DUPLICATED));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(-5)]
    public void Validate_Error_When_Instruction_Order_Invalid(int order)
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions = [new RequestRecipeInstructionJson { Order = order, Description = "Valid desc"}];

        var response = validator.Validate(request);

        response.IsValid.ShouldBeFalse();
        response.Errors.Count.ShouldBe(1);
        response.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INSTRUCTION_GREATER_THAN_ZERO));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("         ")]
    public void Validate_Error_When_Instruction_Description_Empty(string description)
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions = [new RequestRecipeInstructionJson { Order = 1, Description = description}];

        var response = validator.Validate(request);

        response.IsValid.ShouldBeFalse();
        response.Errors.Count.ShouldBe(1);
        response.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INSTRUCTION_DESCRIPTION_REQUIRED));
    }

    [Fact]
    public void Validate_Error_When_Instruction_Description_TooLong()
    {
        var validator = new RecipeValidator();
        var request = RequestRecipeJsonBuilder.Build();
        request.Instructions = [new RequestRecipeInstructionJson { Order = 1, Description = new string('a', 2001)}];

        var response = validator.Validate(request);

        response.IsValid.ShouldBeFalse();
        response.Errors.Count.ShouldBe(1);
        response.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.VALIDATION_INSTRUCTION_DESCRIPTION_MAX_LENGTH));
    }
}
