using System.Net;

namespace MyRecipeBook.Exception.ExceptionsBase;

public class InvalidLoginException : MyRecipeBookException
{
    public override List<string> getErrorMessages() => [ResourceMessagesException.VALIDATION_LOGIN_INVALID]; // [] transforma em lista

    public override HttpStatusCode getStatusCode() => HttpStatusCode.Unauthorized;
}
