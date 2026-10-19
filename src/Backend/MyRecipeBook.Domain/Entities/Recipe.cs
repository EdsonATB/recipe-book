using MyRecipeBook.Domain.Enums;

namespace MyRecipeBook.Domain.Entities;

public class Recipe : EntityBase
{
    public string Title { get; set; } = string.Empty;
    public ICollection<RecipeIngredient> Ingredients { get; set; } = []; //new List<RecipeIngredient>
    public ICollection<RecipeInstruction> Instructions { get; set; } = []; 
    public ICollection<RecipeDishType> DishTypes { get; set; } = []; 
    public CookTime CookTime { get; set; } // como so poder ter 1 opçao selecionada nao precisa de outra entidade no meio como DishTypes tem
    public Guid UserId { get; set; }
}
