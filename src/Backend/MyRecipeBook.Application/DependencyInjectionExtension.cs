using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.UseCases.User.Login.WithEmailAndPassword;
using MyRecipeBook.Application.UseCases.User.Register;

namespace MyRecipeBook.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services) //o this pega o objeto que esta chamando essa funçao
    {
        services.AddScoped<IRegisterUserAccountUseCase, RegisterUserAccountUseCase>();
        services.AddScoped<ILoginWithEmailAndPasswordUseCase, LoginWithEmailAndPasswordUseCase>();
    }
}
