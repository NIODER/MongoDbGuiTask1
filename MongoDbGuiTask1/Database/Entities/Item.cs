using MongoDB.Bson.Serialization.Attributes;

namespace Database.Entities
{
    public class Item : DbEntity
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("price")]
        public long Price { get; set; }
        [BsonElement("count")]
        public int Count { get; set; }
        [BsonElement("company")]
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

        public override string Shortcut
        {
            get => $"_id: {Id}, name: {Name}, price: {Price}, count: {Count}, company: {Company}";
        }

        public static Item Default { get => new ("unknown", default, default, "unknown"); }

        public override bool IsDefault()
        {
            return this == Default;
        }
    }
}
