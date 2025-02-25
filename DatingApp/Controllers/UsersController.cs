using DatingApp.Data;
using DatingApp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Controllers
{
    public class UsersController(DataContext dataContext) : BaseApiController
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await dataContext.Users.ToListAsync();
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id:long}")] // api/users/1 el long es obligario
        public async Task<ActionResult<AppUser>> GetUser(long id)
        {
            var user = await dataContext.Users.FindAsync(id);

            if(user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

    }
}
