using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using MyRecipeBook.Api.Filters;
using System.Globalization;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Application;
using MyRecipeBook.Api.Converters;
using MyRecipeBook.Infrastructure.Migrations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MyRecipeBook.Domain.Repositories.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MyRecipeBook.Domain.Extensions;
using Org.BouncyCastle.Tls;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Exception;


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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtoptions =>
    {
        var signingKey = builder.Configuration.GetValue<string>("Jwt:SigningKey")!;

        jwtoptions.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        jwtoptions.Events = new JwtBearerEvents()
        {
            OnTokenValidated = async context => {
                var subject = context.Principal?.FindFirstValue(JwtRegisteredClaimNames.Sub)
                ?? context.Principal?.FindFirstValue(ClaimTypes.NameIdentifier); // ?? = se for nulo faça o outro
         
                if (Guid.TryParse(subject, out var userId) == false)
                {
                    context.Fail("Invalid Token Subject");
                    return;
                }

                var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserReadOnlyRepository>();

                var UserExists = await userRepository.ExistActiveUserWithId(userId);
                if (UserExists == false)
                {
                    context.Fail("User not found or inactive");
                }
            },
            OnChallenge = async context => //é executado quando um .Fail() for chamado
            {
                context.HandleResponse();

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var response = context.AuthenticateFailure switch
                {
                    null => new ResponseErrorJson(ResourceMessagesException.VALIDATION_ACCESS_TOKEN_REQUIRED),
                    SecurityTokenExpiredException => new ResponseErrorJson("Token Expired", true),
                    _ => new ResponseErrorJson(ResourceMessagesException.VALIDATION_RESOURCE_ACCESS_DENIED)
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        };
    });

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