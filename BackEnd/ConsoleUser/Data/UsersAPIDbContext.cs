using ConsoleUser.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleUser.Data
{
    public class UsersAPIDbContext : DbContext
    {
        public UsersAPIDbContext(DbContextOptions<UsersAPIDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
