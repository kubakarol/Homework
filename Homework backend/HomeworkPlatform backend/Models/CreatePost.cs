using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace HomeworkPlatform_backend.Models
{
    public class CreatePost
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [BindNever]
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
