using System.Security.Claims;
using AutoMapper;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
    [Authorize]
    public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await userRepository.GetMembersAsync();
            return Ok(users);
        }

        [HttpGet("{name}")] 
        public async Task<ActionResult<MemberDto>> GetUserByName(string name)
        {
            var user = await userRepository.GetMemberAsync(name);

            if(user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var name = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if(name == null) return BadRequest("No username found in token");

            var user = await userRepository.GetUserByNameAsync(name);

            if (user == null) return BadRequest("Could not find user");
            
            mapper.Map(memberUpdateDto, user);
            
            if(await userRepository.SaveAllAsync()) return NoContent();
            
            return BadRequest("Failed to update user");
        }

    }
}
