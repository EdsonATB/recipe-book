using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Infrastructure.DataAccess;

internal class MyRecipeBookDbContext : DbContext
{
    public MyRecipeBookDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { } //repassa o parametro para a DbContext
    
    public DbSet<User> Users { get; set; } //O nome dessa propriedade tem que ser o mesmo nome  da tabela do db
}