using MyRecipeBook.Domain.Security.Tokens;

namespace MyRecipeBook.Api.Token;

public class HttpContextTokenProvider : IAccessTokenProvider ///esta sendo implementado aqui porque o projeto de api
{                                                             ///tem acesso a requisiçao completa
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpContextTokenProvider(IHttpContextAccessor contextAccessor)
    {
            _contextAccessor = contextAccessor;
    }

    public string GetToken()
    {
        var accessToken = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return accessToken;
    }
}
