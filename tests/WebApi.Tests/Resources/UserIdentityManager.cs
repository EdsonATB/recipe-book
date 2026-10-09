namespace WebApi.Tests.Resources;

public class UserIdentityManager
{
    private readonly MyRecipeBook.Domain.Entities.User _user;
    private readonly string _password;
    private readonly string _accessToken;

    public UserIdentityManager(MyRecipeBook.Domain.Entities.User user, string password, string accessToken)
    {
        _user = user;
        _password = password;
        _accessToken = accessToken;
    }

    public Guid getId() => _user.Id;
    public string getName() => _user.Name;
    public string getEmail() => _user.Email;
    public string getPassword() => _password;
    public string getToken() => _accessToken;
}
