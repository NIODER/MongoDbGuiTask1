using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using ThirdParty.Json.LitJson;

namespace Database.Entities
{
    public class Item : DbEntity
    {
        public const string NAME_PROPERTY = "name";
        public const string PRICE_PROPERTY = "price";
        public const string COUNT_PROPERTY = "count";
        public const string COMPANY_PROPERTY = "company";

        [BsonElement(NAME_PROPERTY), JsonPropertyName(NAME_PROPERTY)]
        public string Name { get; set; } = string.Empty;
        [BsonElement(PRICE_PROPERTY), JsonPropertyName(PRICE_PROPERTY)]
        public long Price { get; set; }
        [BsonElement(COUNT_PROPERTY), JsonPropertyName(COUNT_PROPERTY)]
        public int Count { get; set; }
        [BsonElement(COMPANY_PROPERTY), JsonPropertyName(COMPANY_PROPERTY)]
        public string Company { get; set; } = string.Empty;

        public Item()
        {

        }

        public Item(string name, long price, int count, string company)
        {
            Name = name;
            Price = price;
            Count = count;
            Company = company;
        }

        [JsonIgnore]
        public override string Shortcut
        {
            get => $"{ID_PROPERTY}: {Id}, {NAME_PROPERTY}: {Name}, {PRICE_PROPERTY}: {Price}, {COUNT_PROPERTY}: {Count}, {COMPANY_PROPERTY}: {Company}";
        }

        [JsonIgnore]
        public static Item Default { get => new ("unknown", default, default, "unknown"); }

        public override bool IsDefault()
        {
            return this == Default;
        }
    }
}
