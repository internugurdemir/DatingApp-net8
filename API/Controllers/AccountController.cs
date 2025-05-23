using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(DataContext context, ITokenService tokenService) : BaseApiController
{

    [HttpPost("register")]//account/register
    // public async Task<ActionResult<AppUser>> Register(string username, string password)
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if(await UserExist(registerDto.Username)) return BadRequest("Username is taken");

            return Ok();
        // using var hmac = new HMACSHA512();
        
        // var user = new AppUser
        // {
        //     UserName = registerDto.Username.ToLower(),
        //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
        //     PasswordSalt = hmac.Key,
        //     City = "baseCity",
        //     Country=  "baseCountry",
        //     Gender=  "baseGender",
        //     KnownAs=  "knownAs",
        //     Created=  DateTime.Now,
        //     DateOfBirth=  DateTime.Now.AddDays(-10000), 
        //     Interests=  "Interests",
        //     Introduction=  "Introduction",
        //     LastActive= DateTime.Now,
        //     LookingFor=  "LookingFor",
        // };


        // context.Users.Add(user);
        // await context.SaveChangesAsync();

        //  return new UserDto{
        //     Username =user.UserName,
        //     Token = tokenService.CreateToken(user)
        // };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        // var user = await context.Users.FirstOrDefaultAsync(a=>a.UserName == loginDto.Username.ToLower());
        //     .Include(p => p.Photos)
        //     .SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
        var user = await context.Users.FirstOrDefaultAsync(a=>a.UserName == loginDto.Username.ToLower());


        if (user == null) return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        }
        return new UserDto{
            Username =user.UserName,
            Token = tokenService.CreateToken(user)
        };
        // var result = await _signInManager
        //     .CheckPasswordSignInAsync(user, loginDto.Password, false);

        // if (!result.Succeeded) return Unauthorized();

        // return new UserDto
        // {
        //     Username = user.UserName,
        //     Token = await _tokenService.CreateToken(user),
        //     PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
        //     KnownAs = user.KnownAs,
        //     Gender = user.Gender
        // };
    }


    private async Task<bool> UserExist(string userName)
    {
        return await context.Users.AnyAsync(a=>a.UserName.ToLower() == userName.ToLower());
    }

}
