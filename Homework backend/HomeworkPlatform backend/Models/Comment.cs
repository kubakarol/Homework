using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HomeworkPlatform_backend.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PostId { get; set; }

        [ForeignKey("PostId")]
        [JsonIgnore]
        public Post Post { get; set; }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
    }
}
