using Moq;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class IUserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _moq;
    public IUserReadOnlyRepositoryBuilder()
    {
        _moq = new Mock<IUserReadOnlyRepository>();
    }

    public void ExistActiveUserWithEmail(string email)
    {
        _moq.Setup(repo => repo.ExistActiveUserWithEmail(email)).ReturnsAsync(true); //o valor default de boolean normalmente é false
    }

    public IUserReadOnlyRepository Builder() => _moq.Object;
    
}
