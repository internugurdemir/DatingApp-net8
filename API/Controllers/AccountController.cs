using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController(
                                DataContext context,
                                ITokenService tokenService,
                                IMapper mapper) : BaseApiController
{

    [HttpPost("register")]//account/register
    // public async Task<ActionResult<AppUser>> Register(string username, string password)
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if(await UserExist(registerDto.Username)) return BadRequest("Username is taken");

    
        using var hmac = new HMACSHA512();

        var user = mapper.Map<AppUser>(registerDto);
        user.UserName = registerDto.Username.ToLower();
        user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        user.PasswordSalt = hmac.Key;


        context.Users.Add(user);
        await context.SaveChangesAsync();

         return new UserDto
         {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Token = tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(p=>p.IsMain)?.Url
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {

        var user = await context.Users
                .Include(p=>p.Photos)
                .FirstOrDefaultAsync(a=>a.UserName == loginDto.Username.ToLower());


        if (user == null) return Unauthorized("Invalid username");

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        }
        return new UserDto{
            Username =user.UserName,
            KnownAs = user.KnownAs,
            Token = tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
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
