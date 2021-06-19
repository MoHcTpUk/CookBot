using Core.DAL.Entities;
using Core.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace CookBot.DAL.Repository
{
    public class MongoDbRepositoryAbstract<TEntity> : IRepository<TEntity> where TEntity : AbstractEntity
    {
        private string User { get; set; }
        private string Pass { get; set; }
        private string Host { get; set; }
        private string Port { get; set; }
        private string DbName { get; set; }

        private string ConnectionString { get; set; }
        private MongoClient Client { get; set; }
        private IMongoDatabase Db { get; set; }

        public MongoDbRepositoryAbstract()
        {
            User = "admin";
            Pass = "";
            Host = "localhost";
            Port = "27017";
            DbName = "test";

            //  mongodb://[username:password@]hostname[:port][/[database][?options]]
            ConnectionString = @$"mongodb://{User}:{Pass}@{Host}:{Port}/";

            Client = new MongoClient(ConnectionString);
            Db = Client.GetDatabase(DbName);
        }

        public Task<TEntity> CreateAsync(TEntity item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Func<TEntity, bool> predicate)
        {
            using var cursor = await Client.ListDatabasesAsync();
            var databaseDocuments = await cursor.ToListAsync();
            foreach (var databaseDocument in databaseDocuments)
            {
                Console.WriteLine(databaseDocument["name"]);
            }

            throw new NotImplementedException();
        }

        public TEntity Get(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> UpdateAsync(TEntity item)
        {
            throw new NotImplementedException();
        }
    }
}