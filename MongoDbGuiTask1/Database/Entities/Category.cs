using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Database.Entities
{
    public class Category : DbEntity
    {
        [BsonElement("category_name")]
        public string CategoryName { get; set; }
        public List<Item>? Items { get; set; }

        public Category(string categoryName)
        {
            CategoryName = categoryName;
        }

        public override string Shortcut
        {
            get => $"_id: {Id}, category_name: {CategoryName}";
        }

        public static Category Default
        {
            get => new("unknown");
        }

        public override bool IsDefault() => this == Default;
    }
}
