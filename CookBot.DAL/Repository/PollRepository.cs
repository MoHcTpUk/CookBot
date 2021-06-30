using CookBot.DAL.Entities;
using Core.Module.MongoDb;
using Core.Module.MongoDb.Repository;

namespace CookBot.DAL.Repository
{
    public class PollRepository : MongoDbRepositoryAbstract<PollEntity>
    {
        public PollRepository(IMongoDatabaseFactory mongoDatabaseFactory) : base(mongoDatabaseFactory)
        {
        }
    }
}
