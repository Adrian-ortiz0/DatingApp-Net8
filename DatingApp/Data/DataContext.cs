using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DatingApp.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<UserLike> Likes { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserLike>()
                .HasKey(l => new { l.SourceUserId, l.TargetUserId });
            
            builder.Entity<UserLike>()
                .HasOne(l => l.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(l => l.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.Entity<UserLike>()
                .HasOne(l => l.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(l => l.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
