using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public class Employee : DbEntity
    {
        public const string NAME_PROPERTY = "full_name";
        public const string POSITION_PROPERTY = "position";
        public const string SALARY_PROPERTY = "salary";
        public const string PASSWORD_DATA_PROPERTY = "password_data";
        public const string RESIDENTIAL_ADDRESS_PROPERTY = "residential_address";
        public const string PHONE_PROPERTY = "phone_number";
        public const string EMAIL_PROPERTY = "email";

        [BsonElement(NAME_PROPERTY), JsonPropertyName(NAME_PROPERTY)]
        public string Name { get; set; } = string.Empty;

        [BsonElement(POSITION_PROPERTY), JsonPropertyName(POSITION_PROPERTY)]
        public string Position { get; set; } = string.Empty;

        [BsonElement(SALARY_PROPERTY), JsonPropertyName(SALARY_PROPERTY)]
        public int Salary { get; set; }

        [BsonElement(PASSWORD_DATA_PROPERTY), JsonIgnore]
        public long PasswordData { get; set; }

        [BsonElement(RESIDENTIAL_ADDRESS_PROPERTY), JsonPropertyName(RESIDENTIAL_ADDRESS_PROPERTY)]
        public string Address { get; set; } = string.Empty;

        [BsonElement(PHONE_PROPERTY), JsonPropertyName(PHONE_PROPERTY)]
        public string PhoneNumber { get; set; } = string.Empty;

        [BsonElement(EMAIL_PROPERTY), JsonPropertyName(EMAIL_PROPERTY)]
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

        [JsonIgnore]
        public override string Shortcut
        {
            get => $"{ID_PROPERTY}: {Id}, {NAME_PROPERTY}: {Name}";
        }

        [JsonIgnore]
        public static Employee Default { get => new("unknown", "unknown", default, default, "unknown", "unknown", "unknown"); }

        public override bool IsDefault()
        {
            return this == Default;
        }
    }
}
