using HomeworkPlatform_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeworkPlatform_backend.Service.IService
{
    public interface IPostService
    {
        Task<Post> CreatePostAsync(CreatePost model);
        Task<Comment> AddCommentAsync(Comment model);
        Task<List<Post>> GetAllPostsAsync();
        Task<List<Post>> GetPostsByUserAsync(string userId);
        Task<Post> GetPostByIdAsync(int postId);
        Task DeletePostAsync(int postId);
        Task<Comment> GetCommentByIdAsync(int commentId);
        Task DeleteCommentAsync(int commentId);
        Task<List<Post>> SearchPostsAsync(string query);
        Task UpdateCommentAsync(Comment comment);

    }
}
