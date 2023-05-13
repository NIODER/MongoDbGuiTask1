using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public class Category : DbEntity
    {
        public const string CATEGORY_NAME_PROPERTY = "category_name";

        [BsonElement(CATEGORY_NAME_PROPERTY), JsonPropertyName(CATEGORY_NAME_PROPERTY)]
        public string CategoryName { get; set; }

        [JsonIgnore]
        public List<Item>? Items { get; set; }

        public Category(string categoryName)
        {
            CategoryName = categoryName;
        }

        [JsonIgnore]
        public override string Shortcut
        {
            get => $"{ID_PROPERTY}: {Id}, {CATEGORY_NAME_PROPERTY}: {CategoryName}";
        }

        [JsonIgnore]
        public static Category Default
        {
            get => new("unknown");
        }

        public override bool IsDefault() => this == Default;
    }
}
