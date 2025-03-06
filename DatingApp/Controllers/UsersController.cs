using System.Security.Claims;
using AutoMapper;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Extensions;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.Controllers
{
    [Authorize]
    public class UsersController(IUserRepository userRepository, 
        IMapper mapper, IPhotoService photoService) : BaseApiController
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
            var user = await userRepository.GetUserByNameAsync(User.GetUsername());

            if (user == null) return BadRequest("Could not find user");
            
            mapper.Map(memberUpdateDto, user);
            
            if(await userRepository.SaveAllAsync()) return NoContent();
            
            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var user = await userRepository.GetUserByNameAsync(User.GetUsername());
            
            if (user == null) return BadRequest("Could not find user");
            
            var result = await photoService.AddPhotoAsync(file);
            
            if(result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
            };
            
            user.Photos.Add(photo);
            if(await userRepository.SaveAllAsync()) 
                return CreatedAtAction(nameof(GetUserByName)
                    , new {name = user.Name}, mapper.Map<PhotoDto>(photo));
            return BadRequest("Failed to add photo");
        }

    }
}
