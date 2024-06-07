using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HomeworkPlatform_backend.Models
{
    public class CreatePost
    {
        public string Title { get; set; }
        public string Content { get; set; }

        [BindNever]
        public string UserId { get; set; }
    }
}
