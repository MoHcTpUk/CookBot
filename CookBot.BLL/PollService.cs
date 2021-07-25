using CookBot.DAL.Entities;
using Core.Module.MongoDb.Repository;
using Core.Module.MongoDb.Services;

namespace CookBot.BLL
{
    public class PollService : MongoDbServiceAbstract<PollEntity>
    {
        public PollService(IMongoDbRepository<PollEntity> repository) : base(repository)
        {
        }
    }
}