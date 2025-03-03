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

namespace DatingApp.Controllers
{
    public class AccountController(DataContext dataContext, ITokenService tokenService) : BaseApiController
    {
        [AllowAnonymous]
        [HttpPost("register")] // account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Name))
            {
                return BadRequest("The username is taken");
            }

            return Ok();
            // using var hmac = new HMACSHA512();
            //
            // var user = new AppUser
            // {
            //     Name = registerDto.Name.ToLower(),
            //     PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            //     PasswordSalt = hmac.Key
            // };
            //
            // dataContext.Users.Add(user);
            // await dataContext.SaveChangesAsync();
            //
            // return new UserDto
            // {
            //     Username = user.Name,
            //     Token = tokenService.CreateToken(user),
            // };
        }
        private async Task<bool> UserExists(String name)
        {
            return await dataContext.Users.AnyAsync(x => x.Name.ToLower() == name.ToLower());
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await dataContext.Users.FirstOrDefaultAsync(x => x.Name == loginDto.Name.ToLower());

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
                Username = loginDto.Name,
                Token = tokenService.CreateToken(user)
            };

        }
    }
}
