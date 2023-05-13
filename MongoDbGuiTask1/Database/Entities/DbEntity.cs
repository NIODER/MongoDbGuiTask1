using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public abstract class DbEntity
    {
        public const string ID_PROPERTY = "id";

        [BsonId, JsonPropertyName(ID_PROPERTY), JsonIgnore]
        public ObjectId Id { get; set; }

        [BsonIgnore, JsonPropertyName(ID_PROPERTY)]
        public string IdString => Id.ToString();

        [JsonIgnore]
        public abstract string Shortcut { get; }

        public abstract bool IsDefault();
    }
}
