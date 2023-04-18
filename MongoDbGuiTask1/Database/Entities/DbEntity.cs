using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Entities
{
    public abstract class DbEntity
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public abstract string Shortcut { get; }
    }
}
