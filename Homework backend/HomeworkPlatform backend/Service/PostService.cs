using HomeworkPlatform_backend.Models;
using HomeworkPlatform_backend.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeworkPlatform_backend.Service
{
    public class PostService : IPostService
    {
        private readonly HomeworkPlatformDbContext _context;

        public PostService(HomeworkPlatformDbContext context)
        {
            _context = context;
        }

        public async Task<Post> CreatePostAsync(CreatePost model)
        {
            var post = new Post
            {
                Title = model.Title,
                Content = model.Content,
                UserId = model.UserId,
                UserName = model.UserName
            };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            return post;
        }

        public async Task<Comment> AddCommentAsync(Comment model)
        {
            _context.Comments.Add(model);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _context.Posts.Include(p => p.Comments).ToListAsync();
        }

        public async Task<List<Post>> GetPostsByUserAsync(string userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .Include(p => p.Comments)
                .ToListAsync();
        }
    }
}
