using DatingApp.DTOs;
using DatingApp.Entities;

namespace DatingApp.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveAllAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser?> GetUserByIdAsync(long id);
    Task<AppUser?> GetUserByNameAsync(string name);
    Task<IEnumerable<MemberDto>> GetMembersAsync();
    Task<MemberDto?> GetMemberAsync(string name);
}