using System;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController(IUnitOfWork unitOfWork) : BaseApiController
{
    [HttpPost("{likedUserId:int}")]
    public async Task<ActionResult> ToggleLike(int likedUserId)
    {
        var sourceUserId = User.GetUserId();


        if (sourceUserId == likedUserId) return BadRequest("You cannot like yourself");

        var existingLike = await unitOfWork.LikesRepository.GetUserLike(sourceUserId, likedUserId);

        if (existingLike == null)
        {
            var like = new UserLike
            {
                SourceUserId = sourceUserId,
                LikedUserId = likedUserId
            };

            unitOfWork.LikesRepository.AddLike(like);
        }
        else
        {
            unitOfWork.LikesRepository.DeleteLike(existingLike);
        }

        if (await unitOfWork.Complete()) return Ok();

        return BadRequest("Failed to like user");
    }




    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<int>>> GetUserLikeIds()
    {
        return Ok(await unitOfWork.LikesRepository.GetCurrentUserLikeIds(User.GetUserId()));
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery] LikesParams likesParams)
    {
        likesParams.UserId = User.GetUserId();
        var users = await unitOfWork.LikesRepository.GetUserLikes(likesParams);

        Response.AddPaginationHeader(users);

        return Ok(users);
    }

}
