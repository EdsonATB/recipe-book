using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Infrastructure.DataAccess;
using System.Net.Http.Json;

namespace WebApi.Tests;

public abstract class BaseIntegrationTests : IClassFixture<MyRecipeBookApplicationFactory>, IDisposable
{
    private readonly IServiceScope _scope;
    private readonly HttpClient _httpClient;
    internal readonly MyRecipeBookDbContext DbContext; //tem q ser internal pq o DbContext original tb é


    public BaseIntegrationTests(MyRecipeBookApplicationFactory factory) //o factory representa o server rodando a API
    {
        _httpClient = factory.CreateClient(); //instancia de HttpClient

        _scope = factory.Services.CreateScope();

        DbContext = _scope.ServiceProvider.GetRequiredService<MyRecipeBookDbContext>();
    }

    protected async Task<HttpResponseMessage> Post(string REQUEST_URI, object request, string culture = "en-US")
    {
        ChangeCulture(culture);

        return await _httpClient.PostAsJsonAsync(REQUEST_URI, request);
    }

    private void ChangeCulture(string culture)
    {
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd(culture);
    }

    public void Dispose() //é chamado automaticamente pelo .Net para dispensar as coisas quando nao estiverem um uso mais
    {
        _scope?.Dispose();
        DbContext?.Dispose();
    }
}
