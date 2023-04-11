using JsonTransformerApi.DataModels;
using JsonTransformerApi.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using System.Runtime.CompilerServices;

namespace JsonTransformerApi.DataAccess
{
    public class PeopleRepository : RepositoryBase, IPeopleRepository
    {
        private readonly string _collectionName;

        public PeopleRepository(IMongoClient mongoClient, IOptions<MongoDatabaseOptions> dbOptions)
            : base(mongoClient, dbOptions)
        {
            _collectionName = dbOptions.Value.PeopleCollection;
        }

        public async Task Insert(PersonDataList personDataList)
        {
            var people = GetCollection<PersonDataList>(_collectionName);
            await people.InsertOneAsync(personDataList);
        }
    }
}
