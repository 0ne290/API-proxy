using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace ApiProxy.Logic;

public interface IJwtCreator
{
    string? GetName(HttpContext context);
    object GetRefreshToken(Dto.Repository.Merchant merchant);
    object GetAccessToken(Dto.Repository.Merchant merchant);
}

public class JwtCreator : IJwtCreator
{
    public object GetRefreshToken(Dto.Repository.Merchant merchant)
    {
        var claims = new List<Claim>
        {
            new("MerchantGuid", merchant.Guid),
            new("IdentityRole", "RefreshToken")
        };

        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromDays(60)),
            signingCredentials: new SigningCredentials(AuthOptions.KEY, SecurityAlgorithms.HmacSha256)
        );

        var refreshToken = new JwtSecurityTokenHandler().WriteToken(jwt);
        return new { RefreshToken = refreshToken };
    }

    public object GetAccessToken(Dto.Repository.Merchant merchant)
    {
        var claims = new List<Claim>
        {
            new("MerchantGuid", merchant.Guid), 
            new("IdentityRole", "AccessToken")
        };
        
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
            signingCredentials: new SigningCredentials(AuthOptions.KEY, SecurityAlgorithms.HmacSha256)
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);
        return new { AccessToken = accessToken };
    }

    public string? GetName(HttpContext context) => context.User.Identity?.Name;
}