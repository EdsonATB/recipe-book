using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Security.PasswordHashing;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) //o this pega o objeto que esta chamando essa funçao
    {
        services.AddScoped<IPasswordHasher, Argon2PasswordHasher>(); //registra o hasher de senha no serviço de injeçao de dependencia
                                                                     // "Quando alguem solicitar um objeto que implementa IPasswordHasher vc devolve uma instancia da classe Argon2PasswordHasher"
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>(); // O lugar que estiver chamando o objeto que implementa a interface so vai ter acesso aos metodos que essa interface implementa, mesmo que o objeto seja o mesmo

        services.AddDbContext<MyRecipeBookDbContext>(config =>
        {
            var connectionString = configuration.GetConnectionString("DbConnection");
            config.UseMySQL(connectionString!);
        });
    }
}
