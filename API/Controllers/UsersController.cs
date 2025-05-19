using System;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;


[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
// public class UsersController(DataContext context) : BaseApiController
{

    // [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        // var users = await context.Users.ToListAsync();
        var users = await userRepository.GetMembersAsync();


        // return users;
        return Ok(users);
    }

    // [Authorize]
    // [HttpGet("{id:int}")]
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        // var user = await context.Users.FindAsync(id);
        var user = await userRepository.GetMemberAsync(username);

        if (user == null) return NotFound();

        return user;
    }

    // [HttpGet]
    // public ActionResult<IEnumerable<AppUser>> GetUsersOK()
    // {
    //     var users = context.Users.ToList();

    //     return Ok(users);
    // }
}
