using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace SampleWebAPI.Models
{
    public class Users
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Username")]
        public string UserName { get; set; } = null;
        public string Password { get; set; } = null;
        public string PhoneNumber { get; set; } = null;
        public string Role { get; set; } = null;
        
    }
}
