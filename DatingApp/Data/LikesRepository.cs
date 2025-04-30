using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Data;

public class LikesRepository(DataContext context, IMapper mapper) : ILikesRepository
{
    public async Task<UserLike?> GetUserLike(long sourceUserId, long targetUserId)
    {
        return await context.Likes.FindAsync(sourceUserId, targetUserId);
    }

    public async Task<IEnumerable<MemberDto>> GetUserLikes(string predicate, long userId)
    {
        var likes = context.Likes.AsQueryable();
        switch (predicate)
        {
            case "Like":
                return await likes.Where(x => x.SourceUserId == userId).Select(x => x.TargetUserId)
                    .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                    .ToListAsync();
            case "likedBy":
                return await likes.Where(x => x.TargetUserId == userId)
                    .Select(x => x.SourceUserId)
                    .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                    .ToListAsync();
            default:
                var likeIds = await GetCurrentUserLikeIds(userId);
                return await likes
                    .Where(x => x.TargetUserId == userId && likeIds.Contains(x.SourceUserId))
                    .Select(x => x.SourceUser)
                    .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
                    .ToListAsync();
        }
    }

    public async Task<IEnumerable<long>> GetCurrentUserLikeIds(long currentUserId)
    {
        return await context.Likes
            .Where(x => x.SourceUserId == currentUserId)
            .Select(x => x.TargetUserId)
            .ToListAsync();
    }

    public void DeleteLike(UserLike like)
    {
        context.Likes.Remove(like);
    }

    public void AddLike(UserLike like)
    {
        context.Likes.Add(like);
    }

    public async Task<bool> SaveChanges()
    {
        return await context.SaveChangesAsync() > 0;
    }
}