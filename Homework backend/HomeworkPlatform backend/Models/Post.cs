using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace HomeworkPlatform_backend.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }

    public class Comment
    {
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}
