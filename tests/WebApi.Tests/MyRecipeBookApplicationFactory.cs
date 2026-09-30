using CommonTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Security.PasswordHashing;
using MyRecipeBook.Infrastructure.DataAccess;
using Testcontainers.MySql;
using WebApi.Tests.Resources;

namespace WebApi.Tests;

public class MyRecipeBookApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public UserIdentityManager User1 { get; private set; }
    private readonly MySqlContainer _mySqlContainer;
    public MyRecipeBookApplicationFactory() //configura o container que vai ser criado quando rodar os testes
    {
        _mySqlContainer = new MySqlBuilder("mysql:8.0")
        .WithDatabase("meulivrodereceitas")
        .Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Tests")
            .ConfigureAppConfiguration((_, config) =>
            {
                var parameters = new Dictionary<string, string?>
                {
                    ["ConnectionStrings:DbConnection"] = _mySqlContainer.GetConnectionString()
                }; 

                config.AddInMemoryCollection(parameters);
            });
    }

    public async Task InitializeAsync() // inicializa o container antes de rodar cada classe de teste
    {
        await _mySqlContainer.StartAsync(); //await pra esperar executar o container antes de continuar o resto do cod

        //Criado seed no banco para testes de login(integraçao)
        await using var scope = Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        var (user, password) = UserBuilder.Build();
        user.Password = passwordHasher.HashPassword(password);

        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();

        User1 = new UserIdentityManager(user,password); 
        ///settando essas informaçoes na variavel criada nessa classe para que possamos acessar esses valores usando 
        ///os metodos do identityManager so que la na classe de testes, 
        ///instanciamos essa variavel daqui pelo construtor de lá
    }

    Task IAsyncLifetime.DisposeAsync() // apaga o container apos terminar os testes
    {
        return _mySqlContainer.StopAsync();
    }
}
