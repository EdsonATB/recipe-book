using Moq;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class IUserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Builder()
    {
        var moq = new Mock<IUserWriteOnlyRepository>();

        return moq.Object;
    }
}
