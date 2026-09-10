using Moq;
using MyRecipeBook.Domain.Security.PasswordHashing;

namespace CommonTestUtilities.Security;

public class IPasswordHasherBuilder
{
    private readonly Mock<IPasswordHasher> _moq;

    public IPasswordHasherBuilder()
    {
        _moq = new Mock<IPasswordHasher>();

        _moq.Setup(ipasswordhasher => ipasswordhasher.HashPassword(It.IsAny<string>())).Returns("hashed-password");
    }

    public void VerifyPassword(string password) //isso vai ser setado pra true no teste quando for teste de login pois vai usar o verify
    {
        _moq.Setup(ipasswordhasher => ipasswordhasher.VerifyPassword(password, It.IsAny<string>())).Returns(true);
    }

    public IPasswordHasher Build() => _moq.Object;
}
