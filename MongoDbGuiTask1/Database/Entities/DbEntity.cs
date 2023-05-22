using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.Core.Operations;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public abstract class DbEntity
    {
        public const string ID_PROPERTY = "id";

        [BsonId, BsonRepresentation(BsonType.ObjectId), JsonPropertyName(ID_PROPERTY)]
        public string Id { get; set; }

        [JsonIgnore]
        public abstract string Shortcut { get; }

        public abstract bool IsDefault();
    }
}
