using DatingApp.Entities;

namespace DatingApp.Interfaces
{
    public interface ITokenService
    {
        String CreateToken(AppUser appUser);

    }
}
