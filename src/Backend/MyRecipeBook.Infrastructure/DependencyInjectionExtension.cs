using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Infrastructure.Security.PasswordHashing;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services) //o this pega o objeto que esta chamando essa funçao
    {
        services.AddScoped<IPasswordHasher, Argon2PasswordHasher>(); //registra o hasher de senha no serviço de injeçao de dependencia
                                                                     // "Quando alguem solicitar um objeto que implementa IPasswordHasher vc devolve uma instancia da classe Argon2PasswordHasher"
    }
}
