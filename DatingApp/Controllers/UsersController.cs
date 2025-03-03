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
    public class UsersController(IUserRepository userRepository) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await userRepository.GetMembersAsync();
            return Ok(users);
        }

        [Authorize]
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

    }
}
