using DatingApp.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DatingApp.Data
{
    public class DataContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<AppUser> Users { get; set; }
    }
}
