using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyRecipeBook.Infrastructure.Security.Tokens.Access;

internal sealed class JwtTokenHandler : IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes; //uint n aceita negativo
    private readonly string _signingKey;

    public JwtTokenHandler(uint expirationTimeMinutes, string signingKey)
    {
        _expirationTimeMinutes = expirationTimeMinutes;
        _signingKey = signingKey;
    }

    public string Generate(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Sub, user.Id.ToString())

        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            SigningCredentials = new SigningCredentials(Credentials(), SecurityAlgorithms.HmacSha256), //algoritmo mais usado // usado pra fazer a ultima parte do token
            Subject = new ClaimsIdentity(claims) //define o que vai estar no payload do token
        };

        var handler = new JsonWebTokenHandler();

        return handler.CreateToken(tokenDescriptor);
    }

    private SymmetricSecurityKey Credentials()
    {
        var key = Encoding.UTF8.GetBytes(_signingKey);

        return new SymmetricSecurityKey(key); //esse objeto espera um array de bytes
    }
}
