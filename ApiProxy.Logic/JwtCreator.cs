using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace ApiProxy.Logic;

public interface IJwtCreator
{
    object Create(string? merchantId);
}

public class JwtCreator : IJwtCreator
{
    public object Create(string? merchantId)
    {
        var claims = new List<Claim>
        {
            new("MerchantGuid", merchantId), 
            new("IdentityRole", "RefreshToken")
        };
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(60)),
            signingCredentials: new SigningCredentials(AuthOptions.KEY, SecurityAlgorithms.HmacSha256)
        );
        var rt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return new { RefreshToken = rt };
}