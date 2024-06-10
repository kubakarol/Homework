using HomeworkPlatform_backend.Models;
using HomeworkPlatform_backend.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace HomeworkPlatform_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostController> _logger;

        public PostController(IPostService postService, ILogger<PostController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<IEnumerable<Post>>> GetAllPosts()
        {
            try
            {
                _logger.LogInformation("Fetching all posts.");
                var posts = await _postService.GetAllPostsAsync();
                _logger.LogInformation("Posts fetched successfully.");
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving posts.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] CreatePost model)
        {
            _logger.LogInformation("Received create post request: {@Model}", model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name); // Get the username from the claims

            _logger.LogInformation("Extracted UserId from claims: {UserId}", userId);
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
            {
                _logger.LogWarning("User ID or Username not found in claims.");
                return Unauthorized("User ID or Username not found.");
            }

            model.UserId = userId;
            model.UserName = userName; // Set the username in the model

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state invalid: {ModelState}", ModelState);
                foreach (var error in ModelState)
                {
                    _logger.LogWarning("Key: {Key}, Error: {Error}", error.Key, error.Value);
                }
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating post with Title: {Title}, Content: {Content}, UserId: {UserId}, UserName: {UserName}", model.Title, model.Content, model.UserId, model.UserName);

            try
            {
                var post = await _postService.CreatePostAsync(model);
                if (post == null)
                {
                    _logger.LogWarning("Post creation failed for unknown reasons.");
                    return BadRequest("Post creation failed.");
                }
                return Ok(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating post.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("addComment")]
        [Authorize]
        public async Task<IActionResult> AddComment([FromBody] AddComment model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(userName))
            {
                _logger.LogWarning("User ID or Username not found in claims.");
                return Unauthorized("User ID or Username not found.");
            }

            var comment = new Comment
            {
                PostId = model.PostId,
                UserId = userId,
                UserName = userName,
                Content = model.Content
            };

            _logger.LogInformation($"Adding comment with PostId: {comment.PostId}, Content: {comment.Content}, UserId: {comment.UserId}, UserName: {comment.UserName}");

            try
            {
                var result = await _postService.AddCommentAsync(comment);
                if (result == null)
                {
                    return BadRequest("Adding comment failed.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
