using Microsoft.AspNetCore.Identity;

namespace DatingApp.DTOs
{
    public class LoginDto
    {
        public required String Name { get; set; }
        public required String Password {get; set;}
    }
}
