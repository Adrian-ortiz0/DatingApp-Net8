using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DatingApp.Extensions;

namespace DatingApp.Controllers
{
    public class AccountController(DataContext dataContext, ITokenService tokenService, 
        IMapper mapper) : BaseApiController
    {
        [AllowAnonymous]
        [HttpPost("register")] // account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Name))
            {
                return BadRequest("The username is taken");
            }

            using var hmac = new HMACSHA512();

            var user = mapper.Map<AppUser>(registerDto);
            
            user.Name = registerDto.Name.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            
            dataContext.Users.Add(user);
            await dataContext.SaveChangesAsync();
            
            return new UserDto
            {
                Username = user.Name,
                Token = tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }
        private async Task<bool> UserExists(String name)
        {
            return await dataContext.Users.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await dataContext.Users
                .Include(p => p.Photos)    
                    .FirstOrDefaultAsync(x => 
                        x.Name == loginDto.Name.ToLower());

            if(user == null)
            {
                return Unauthorized("Invalid username!");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for(int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid password");
                }
            }
            return new UserDto
            {
                Username = user.Name,
                KnownAs = user.KnownAs,
                Token = tokenService.CreateToken(user),
                Gender = user.Gender,
                PhotoUrl  = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                
            };

        }

        
    }
}
