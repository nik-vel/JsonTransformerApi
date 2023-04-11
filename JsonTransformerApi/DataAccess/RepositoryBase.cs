using JsonTransformerApi.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JsonTransformerApi.DataAccess
{
    public abstract class RepositoryBase
    {
        private readonly IMongoClient _mongoClient;

        private readonly MongoDatabaseOptions _dbOptions;

        public RepositoryBase(IMongoClient mongoClient, IOptions<MongoDatabaseOptions> dbOptions)
        {
            _mongoClient = mongoClient;
            _dbOptions = dbOptions.Value;
        }

        protected IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            var _database = _mongoClient.GetDatabase(_dbOptions.Database);

            return _database.GetCollection<T>(collectionName);
        }

    }
}
