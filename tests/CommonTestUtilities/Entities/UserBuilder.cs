using Bogus;
using CommonTestUtilities.Security;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{

    public static MyRecipeBook.Domain.Entities.User Build()
    {
        return new Faker<MyRecipeBook.Domain.Entities.User>()
            .RuleFor(user => user.Name, f => f.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, _ => GenerateRandomPassword());
    }

    private static string GenerateRandomPassword()
    {
        var passwordHasherBuilder = new IPasswordHasherBuilder().Build();

        var password = new Faker().Internet.Password();

        return passwordHasherBuilder.HashPassword(password);
    }
}
