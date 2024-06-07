﻿using HomeworkPlatform_backend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HomeworkPlatform_backend.Service
{
    public interface IPostService
    {
        Task<Post> CreatePostAsync(CreatePost model);
        Task<Comment> AddCommentAsync(AddComment model);
        Task<List<Post>> GetAllPostsAsync();
    }

    public class PostService : IPostService
    {
        private readonly IMongoCollection<Post> _posts;

        public PostService(IOptions<MongoSettings> settings, IMongoClient client)
        {
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _posts = database.GetCollection<Post>("posts");
        }

        public async Task<Post> CreatePostAsync(CreatePost model)
        {
            var post = new Post
            {
                Title = model.Title,
                Content = model.Content,
                UserId = model.UserId
            };

            await _posts.InsertOneAsync(post);
            return post;
        }

        public async Task<Comment> AddCommentAsync(AddComment model)
        {
            var comment = new Comment
            {
                UserId = model.UserId,
                Content = model.Content
            };

            var filter = Builders<Post>.Filter.Eq(p => p.Id, model.PostId);
            var update = Builders<Post>.Update.Push(p => p.Comments, comment);

            await _posts.UpdateOneAsync(filter, update);
            return comment;
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            return await _posts.Find(_ => true).ToListAsync();
        }
    }
}