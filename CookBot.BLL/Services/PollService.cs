using System;
using CookBot.DAL.Entities;
using Core.Module.MongoDb.Repository;
using Core.Module.MongoDb.Services;

namespace CookBot.BLL.Services
{
    public class PollService : MongoDbServiceAbstract<PollEntity>
    {
        public PollService(IMongoDbRepository<PollEntity> repository) : base(repository)
        {
        }

        public int GetVotedYes(DateTime startDate, DateTime endDate)
        {
            return 10;
        }
    }
}