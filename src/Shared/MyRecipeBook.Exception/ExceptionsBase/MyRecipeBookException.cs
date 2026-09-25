using System.Net;

namespace MyRecipeBook.Exception.ExceptionsBase;

public abstract class MyRecipeBookException : System.Exception
{
    public abstract HttpStatusCode getStatusCode();
    public abstract List<string> getErrorMessages();
}
