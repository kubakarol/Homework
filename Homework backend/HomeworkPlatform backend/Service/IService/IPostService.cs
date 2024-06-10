using HomeworkPlatform_backend.Models;

namespace HomeworkPlatform_backend.Service.IService
{
    public interface IPostService
    {
        Task<Post> CreatePostAsync(CreatePost model);
        Task<Comment> AddCommentAsync(Comment model);
        Task<List<Post>> GetAllPostsAsync();
        Task<List<Post>> GetPostsByUserAsync(string userId);

    }
}
