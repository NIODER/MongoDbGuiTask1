using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public class Employee : DbEntity
    {
        [BsonElement("full_name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("position")]
        public string Position { get; set; } = string.Empty;
        [BsonElement("salary")]
        public int Salary { get; set; }
        [BsonElement("password_data")]
        public long PasswordData { get; set; }
        [BsonElement("residential_address")]
        public string Address { get; set; } = string.Empty;
        [BsonElement("phone_number")]
        public string PhoneNumber { get; set; } = string.Empty;
        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        public Employee()
        {

        }

        public Employee(string name, string position, int salary, long passwordData, string address, string phoneNumber, string email)
        {
            Name = name;
            Position = position;
            Salary = salary;
            PasswordData = passwordData;
            Address = address;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public override string Shortcut
        {
            get => $"_id: {Id}, name: {Name}";
        }

        public static Employee Default { get => new("unknown", "unknown", default, default, "unknown", "unknown", "unknown"); }

        public override bool IsDefault()
        {
            return this == Default;
        }
    }
}
