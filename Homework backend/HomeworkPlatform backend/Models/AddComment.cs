using System.ComponentModel.DataAnnotations;

namespace HomeworkPlatform_backend.Models
{
    public class AddComment
    {
        [Required]
        public int PostId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
