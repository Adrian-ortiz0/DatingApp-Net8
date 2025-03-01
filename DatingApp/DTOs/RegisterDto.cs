using System.ComponentModel.DataAnnotations;

namespace DatingApp.DTOs
{
    public class RegisterDto
    {
        [Required]
        public String Name { get; set; } = String.Empty;
        [Required]
        [StringLength(8, MinimumLength = 4)]
        public String Password { get; set; } = String.Empty;
    }
}
