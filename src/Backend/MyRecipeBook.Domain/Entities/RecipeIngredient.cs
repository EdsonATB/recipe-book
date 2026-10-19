namespace MyRecipeBook.Domain.Entities;

public class RecipeIngredient : EntityBase
{
    public string item {  get; set; } = string.Empty;
    public Guid RecipeId { get; private set; }
}
