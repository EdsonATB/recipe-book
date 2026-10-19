namespace MyRecipeBook.Domain.Entities;

public class RecipeInstruction : EntityBase
{
    public int Order { get; set; }
    public string description { get; set; } = string.Empty;
    public Guid RecipeId { get; private set; }
}
