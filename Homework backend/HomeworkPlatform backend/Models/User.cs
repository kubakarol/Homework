using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HomeworkPlatform_backend.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
