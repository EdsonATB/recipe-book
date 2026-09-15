using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("WebApi.Tests")] //por mais q seja esse o nome, essa tag da acesso ao projeto de infra todo
namespace MyRecipeBook.Infrastructure.DataAccess;

internal class MyRecipeBookDbContext : DbContext
{
    public MyRecipeBookDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { } //repassa o parametro para a DbContext
    
    public DbSet<User> Users { get; set; } //O nome dessa propriedade tem que ser o mesmo nome  da tabela do db
}