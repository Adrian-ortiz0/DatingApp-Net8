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
        var query = dataContext.Users.AsQueryable();
        
        query = query.Where(x => x.Name != userParams.CurrentUsername);

        if (userParams.Gender != null)
        {
            query = query.Where(x => x.Gender == userParams.Gender);
        }
        
        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));
        query = query.Where(x => x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);
        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(x => x.Created),
            _ => query.OrderByDescending(x => x.LastActive)
        };

        return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            , userParams.PageNumber, userParams.Pagesize);

    }

    public async Task<MemberDto?> GetMemberAsync(string name)
    {
        return await dataContext.Users
            .Where(x => x.Name == name)    
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }
}