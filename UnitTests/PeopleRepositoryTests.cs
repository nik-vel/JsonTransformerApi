using JsonTransformerApi.DataAccess;
using JsonTransformerApi.DataModels;
using JsonTransformerApi.Options;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NUnit.Framework.Internal;

namespace UnitTests
{
    [TestFixture]
    public class PeopleRepositoryTests
    {
        [Test]
        public async Task Insert_ShouldInsertPersonDataList()
        {
            // Arrange
            var dbOptions = new Mock<IOptions<MongoDatabaseOptions>>();
            dbOptions.Setup(x => x.Value).Returns(new MongoDatabaseOptions
            {
                Database = "testdb",
                PeopleCollection = "testCollection"
            });

            var mockClient = new Mock<IMongoClient>();
            var mockDatabase = new Mock<IMongoDatabase>();
            var mockCollection = new Mock<IMongoCollection<PersonDataList>>();

            mockClient
                .Setup(x => x.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>()))
                .Returns(mockDatabase.Object);
            mockDatabase
                .Setup(x => x.GetCollection<PersonDataList>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>()))
                .Returns(mockCollection.Object);

            var repository = new PeopleRepository(mockClient.Object, dbOptions.Object);

            var personDataList = new PersonDataList();

            // Act
            await repository.Insert(personDataList);

            // Assert
            mockCollection.Verify(x => x.InsertOneAsync(personDataList, It.IsAny<InsertOneOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
