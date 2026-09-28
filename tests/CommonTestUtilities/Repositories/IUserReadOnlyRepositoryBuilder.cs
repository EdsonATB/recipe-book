using Moq;
using MyRecipeBook.Domain.Entities;
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
    public void GetByEmail(User user)
    {
        _moq.Setup(repo => repo.GetByEmail(user.Email)).ReturnsAsync(user); //essas funcoes de moq se lê: "retorna o user se o GetByEmail() for chamado com o mesmo email da request (no createUseCase dos testes)"
    }

    public IUserReadOnlyRepository Build() => _moq.Object;
    
}
