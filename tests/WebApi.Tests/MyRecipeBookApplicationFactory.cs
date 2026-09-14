using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Testcontainers.MySql;

namespace WebApi.Tests;

public class MyRecipeBookApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
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
    }

    Task IAsyncLifetime.DisposeAsync() // apaga o container apos terminar os testes
    {
        return _mySqlContainer.StopAsync();
    }
}
