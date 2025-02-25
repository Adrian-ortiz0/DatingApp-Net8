namespace DatingApp.Entities
{
    public class AppUser
    {

        public long Id {  get; set; }
        public required string Name { get; set; }

        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
    }
}
