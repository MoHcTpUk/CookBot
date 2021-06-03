using CookBot.DAL.EF;
using CookBot.DAL.Entities;
using Core.DAL.Repository;
using Microsoft.EntityFrameworkCore;

namespace CookBot.DAL.Repository
{
    public class PollRepository : AbstractRepository<PollEntity>
    {
        public PollRepository(IDbContextFactory<ApplicationDbContext> context) : base(context)
        { }
    }
}