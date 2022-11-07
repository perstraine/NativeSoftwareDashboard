using ConsoleUser.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ConsoleUser.Data
{
    public class SupportLevelDbContext : DbContext
    {
        public SupportLevelDbContext(DbContextOptions<SupportLevelDbContext> options) : base(options)
        {

        }

        public DbSet<CustomerSupportLevel> CustomerSupportLevel { get; set; }
    }
}
