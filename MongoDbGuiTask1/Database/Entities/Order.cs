using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public class Order : DbEntity
    {
        public const string CUSTOMER_PROPERTY = "customer";
        public const string STATUS_PROPERTY = "status";
        public const string CATEGORY_ID_PROPERTY = "category";
        public const string ITEM_ID_PROPERTY = "item_id";
        public const string QUANTITY_PROPERTY = "quantity";
        public const string DATE_PROPERTY = "date";

        [BsonElement(CUSTOMER_PROPERTY), JsonPropertyName(CUSTOMER_PROPERTY)]
        public string Customer { get; set; }

        [BsonElement(STATUS_PROPERTY), JsonPropertyName(STATUS_PROPERTY)]
        public int Status { get; set; }

        [BsonElement(CATEGORY_ID_PROPERTY), JsonPropertyName(CATEGORY_ID_PROPERTY)]
        public int CategoryId { get; set; }

        [BsonElement(ITEM_ID_PROPERTY), JsonPropertyName(ITEM_ID_PROPERTY)]
        public int ItemId { get; set; }

        [BsonElement(QUANTITY_PROPERTY), JsonPropertyName(QUANTITY_PROPERTY)]
        public int Quantity { get; set; }

        [BsonElement(DATE_PROPERTY), JsonPropertyName(DATE_PROPERTY)]
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

        [JsonIgnore]
        public override string Shortcut
        {
            get => $"{ID_PROPERTY}: {Id}, {STATUS_PROPERTY}: {Status}, {DATE_PROPERTY}: {Date}";
        }

        [JsonIgnore]
        public static Order Default { get => new("unknown", default, default, default, default); }

        public override bool IsDefault()
        {
            return this == Default;
        }
    }
}
