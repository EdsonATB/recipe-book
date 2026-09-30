namespace WebApi.Tests.Resources;

public class UserIdentityManager
{
    private readonly MyRecipeBook.Domain.Entities.User _user;
    private readonly string _password;

    public UserIdentityManager(MyRecipeBook.Domain.Entities.User user, string password)
    {
        _user = user;
        _password = password;
    }

    public Guid getId() => _user.Id;
    public string getName() => _user.Name;
    public string getEmail() => _user.Email;
    public string getPassword() => _password;
}
