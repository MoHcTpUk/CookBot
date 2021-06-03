using CookBot.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CookBot.DAL.EF
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<PollEntity> Bases { get; set; } = null;
    }
}