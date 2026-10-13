using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Application.UseCases.User.ChangePassword;
using MyRecipeBook.Application.UseCases.User.Login.WithEmailAndPassword;
using MyRecipeBook.Application.UseCases.User.Profile;
using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Application.UseCases.User.Update;

namespace MyRecipeBook.Application;

public static class DependencyInjectionExtension
{
    extension(IServiceCollection services)
    {
        public void AddApplication() //o this pega o objeto que esta chamando essa funçao
        {
            services.AddUseCases();
        }

        private void AddUseCases()
        {
            services.AddScoped<IRegisterUserAccountUseCase, RegisterUserAccountUseCase>();
            services.AddScoped<ILoginWithEmailAndPasswordUseCase, LoginWithEmailAndPasswordUseCase>();
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        }
    }
}
