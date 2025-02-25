using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs
{
    public class RegisterDto
    {
        [Required]
        public required String Name {  get; set; }
        [Required]
        public required String Password { get; set; }
    }
}
