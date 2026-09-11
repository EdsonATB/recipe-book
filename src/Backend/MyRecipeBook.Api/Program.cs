using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MyRecipeBook.Api.Filters;
using System.Globalization;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Application;
using MyRecipeBook.Api.Converters;
using MyRecipeBook.Infrastructure.Migrations;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.Configure<RequestLocalizationOptions>(options => 
{
    var supportedCultures = new List<CultureInfo> { new("en"), new("pt-BR") };

    options.DefaultRequestCulture = new RequestCulture("en");
    
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.RequestCultureProviders = [ new AcceptLanguageHeaderRequestCultureProvider() ];
});

builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());

builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(localizationOptions.Value);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await ExecuteMigration();

app.Run();

//------------------------------------------------------
async Task ExecuteMigration()
{
    await using var scope = app.Services.CreateAsyncScope();

    DatabaseMigration.ExecuteMigrations(scope.ServiceProvider);
}

public partial class Program { } //so pra dar uma referencia pros testes de integraçao ANTES do tempo de compilaçao (o partial junta essa aqui com a classe gerada em tempo de compilaçao) (na teoria isso aqui tambem é uma classe, mas so em tempo de compilaçao)