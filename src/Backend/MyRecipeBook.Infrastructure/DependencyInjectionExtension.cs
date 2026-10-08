using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Identity;
using MyRecipeBook.Infrastructure.Security.PasswordHashing;
using MyRecipeBook.Infrastructure.Security.Tokens.Access;
using System.Reflection;

namespace MyRecipeBook.Infrastructure;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddInfrastructure(IConfiguration configuration) //o this pega o objeto que esta chamando essa funçao
        {
            services.AddRepositories();
            services.AddScoped<IPasswordHasher, Argon2PasswordHasher>(); //registra o hasher de senha no serviço de injeçao de dependencia
                                                                         // "Quando alguem solicitar um objeto que implementa IPasswordHasher vc devolve uma instancia da classe Argon2PasswordHasher"
            services.AddScoped<ILoggedUser, LoggedUser>();

            services.AddDbContext<MyRecipeBookDbContext>(config =>
            {
                var connectionString = configuration.GetConnectionString("DbConnection");
                config.UseMySQL(connectionString!);
            });
 
            services.AddFluentMigratorCore().ConfigureRunner(config =>
            {
                config
                .AddMySql5()
                .WithGlobalConnectionString(_ =>
                {
                    var connectionString = configuration.GetConnectionString("DbConnection");
                    return connectionString;
                })
                .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure"))
                .For.All();

            });

            services.AddTokenHandlers(configuration);
        }


        private void AddRepositories()
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>(); // O lugar que estiver chamando o objeto que implementa a interface so vai ter acesso aos metodos que essa interface implementa, mesmo que o objeto seja o mesmo
            services.AddScoped<IUserReadOnlyRepository, UserRepository>(); // O lugar que estiver chamando o objeto que implementa a interface so vai ter acesso aos metodos que essa interface implementa, mesmo que o objeto seja o mesmo

        }

        private void AddTokenHandlers(IConfiguration configuration)
        {
            services.AddScoped<IAccessTokenGenerator>(provider =>
            {
                var expirationTimeMinutes = configuration.GetValue<uint>("Jwt:ExpirationTimeMinutes"); //.GetValue vem do pacote nuget Binder
                var signingKey = configuration.GetValue<string>("Jwt:SigningKey")!;

                return new JwtTokenHandler(expirationTimeMinutes, signingKey);
            });
        }
    }
    
}
