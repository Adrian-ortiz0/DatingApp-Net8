using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Helpers;
using DatingApp.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data;

public class UserRepository(DataContext dataContext, IMapper mapper) : IUserRepository
{
    public void Update(AppUser user)
    {
        dataContext.Entry(user).State = EntityState.Modified;
    }

    public async Task<bool> SaveAllAsync()
    {
        return await dataContext.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await dataContext.Users
            .Include(x => x.Photos )    
            .ToListAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(long id)
    {
        return await dataContext.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByNameAsync(string name)
    {
        return await dataContext.Users
            .Include(x => x.Photos )
            .SingleOrDefaultAsync(x => x.Name == name);
    }
    
    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        var query = dataContext.Users
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider);

        return await PagedList<MemberDto>.CreateAsync(query, userParams.PageNumber, userParams.Pagesize);

    }

    public async Task<MemberDto?> GetMemberAsync(string name)
    {
        return await dataContext.Users
            .Where(x => x.Name == name)    
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }
}