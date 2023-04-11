using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace JsonTransformerApi.DataModels
{
    /// <summary>
    /// Represents a list of PersonData objects.
    /// </summary>
    public class PersonDataList
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public PersonData[] Data { get; set; }
    }
}
