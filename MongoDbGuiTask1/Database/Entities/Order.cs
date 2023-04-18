using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Database.Entities
{
    public class Order : DbEntity
    {
        [BsonElement("customer")]
        public string Customer { get; set; }
        [BsonElement("status")]
        public int Status { get; set; }
        [BsonElement("category_id")]
        public int CategoryId { get; set; }
        [BsonElement("item_id")]
        public int ItemId { get; set; }
        [BsonElement("quantity")]
        public int Quantity { get; set; }
        [BsonElement("date")]
        public string Date { get; set; }

        public Order(string customer, int status, int categoryId, int itemId, int quantity)
        {
            Customer = customer;
            Status = status;
            CategoryId = categoryId;
            ItemId = itemId;
            Quantity = quantity;
            Date = DateTime.UtcNow.ToString();
        }

        public override string Shortcut
        {
            get => $"_id: {Id}, status: {Status}, date: {Date}";
        }

        public static Order Default { get => new("unknown", default, default, default, default); }
    }
}
