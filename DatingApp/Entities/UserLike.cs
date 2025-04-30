namespace DatingApp.Entities;

public class UserLike
{
    public AppUser SourceUser { get; set; } = null;
    public long SourceUserId { get; set; }
    public AppUser TargetUser { get; set; } = null;
    public long TargetUserId { get; set; }

}