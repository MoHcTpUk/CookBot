using Core.Module.MongoDb;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;
using System.IO;

namespace CookBot.DAL.MongoDb
{
    public class MongoDatabaseFactory : IMongoDatabaseFactory
    {
        private const string AppSettingsFile = "config.json";
        private const string ConnectionStringName = "LocalConnection";

        public string GetDbConnectionString()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), AppSettingsFile);

            try
            {
                if (File.Exists(path))
                {
                    var builder = new ConfigurationBuilder();
                    builder.SetBasePath(Directory.GetCurrentDirectory());

                    var config = builder.AddJsonFile(AppSettingsFile).Build();

                    string readedConnectionString = config.GetConnectionString(ConnectionStringName);
                    return string.IsNullOrWhiteSpace(readedConnectionString) ? "" : readedConnectionString;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(@$"Error while reading appsettings.json: {ex.Message}");
            }

            throw new Exception(@$"Error while reading appsettings.json: File {path} not found");
        }
        public IMongoDatabase GetMongoDatabase()
        {
            var dataBaseName = "cookBot";
            var Client = new MongoClient(GetDbConnectionString());

            return Client.GetDatabase(dataBaseName);
        }
    }
}