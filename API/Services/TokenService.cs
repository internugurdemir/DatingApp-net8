using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    // private readonly SymmetricSecurityKey _key;
    // private readonly UserManager<AppUser> _userManager;
    // public TokenService(IConfiguration config, UserManager<AppUser> userManager)
    // {
    //     _userManager = userManager;
    //     _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
    // }

    // public async Task<string> CreateToken(AppUser user)
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access to token");
        if (tokenKey.Length <64) throw new Exception("invalid token");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        // var claims = new List<Claim>
        //     {
        //         new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
        //         new Claim(JwtRegisteredClaimNames.Name, user.UserName),
        //     };

        var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.UserName),
            }; 



        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = creds
        };


        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

    

        // var roles = await _userManager.GetRolesAsync(user);

        // claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));



        return tokenHandler.WriteToken(token);
    }
}
