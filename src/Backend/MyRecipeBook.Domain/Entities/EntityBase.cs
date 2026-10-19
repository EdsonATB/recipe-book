namespace MyRecipeBook.Domain.Entities;

public abstract class EntityBase
{
    public Guid id { get; set; } = Guid.CreateVersion7();
    public bool Active { get; set; } = true;
}
