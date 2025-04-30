using DatingApp.DTOs;
using DatingApp.Entities;

namespace DatingApp.Interfaces;

public interface ILikesRepository
{
    Task<UserLike?> GetUserLike(long sourceUserId, long targetUserId);
    Task<IEnumerable<MemberDto>> GetUserLikes(string predicate, long userId);
    Task<IEnumerable<long>> GetCurrentUserLikeIds(long currentUserId);
    void DeleteLike(UserLike like);
    void AddLike(UserLike like);
    Task<bool> SaveChanges();
}