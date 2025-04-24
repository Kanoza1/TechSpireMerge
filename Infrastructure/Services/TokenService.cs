
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService : ITokenService
{
    private readonly IConfiguration configuration;
    private readonly SymmetricSecurityKey key;
    public TokenService(IConfiguration configuration)
    {
        this.configuration = configuration;
        key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:key"]));
    }
    public string CreateToken(AppUser user)
    {
        List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.GivenName,user.UserName)
            };
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = creds,
            Expires = DateTime.Now.AddDays(10),
            Issuer = configuration["Token:Issuer"],

        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
