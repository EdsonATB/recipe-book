using Bogus;
using CommonTestUtilities.Security;
using MyRecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{

    public static (User user, string password) Build() //retorno nomeado, para que quando a funcao for chamada sabemos qual é qual mesmo ela devolvendo 2 valores
    {
        var (password, passwordHashed) = GenerateRandomPassword();
        
        var user = new Faker<MyRecipeBook.Domain.Entities.User>()
            .RuleFor(user => user.Name, f => f.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, _ => passwordHashed);

        return (user, password);
    }

    private static (string password, string passwordHashed) GenerateRandomPassword()
    {
        var passwordHasherBuilder = new IPasswordHasherBuilder().Build();

        var password = new Faker().Internet.Password();

        return (password, passwordHasherBuilder.HashPassword(password));
    }
}
