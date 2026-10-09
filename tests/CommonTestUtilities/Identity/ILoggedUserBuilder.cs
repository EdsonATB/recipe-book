using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Identity;

namespace CommonTestUtilities.Identity;

public class ILoggedUserBuilder
{

    public static ILoggedUser Build(User user)
    {
        var mock = new Mock<ILoggedUser>();

        mock.Setup(config => config.Get()).ReturnsAsync(user);
        mock.Setup(config => config.GetUserId()).Returns(user.Id);

        return mock.Object;
    }
}
