using HomeworkPlatform_backend.Models;
using HomeworkPlatform_backend.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

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

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] CreatePost model)
        {
            _logger.LogInformation("Received create post request: {@Model}", model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("Extracted UserId from claims: {UserId}", userId);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID not found in claims.");
                return Unauthorized("User ID not found.");
            }

            model.UserId = userId;

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state invalid: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating post with Title: {Title}, Content: {Content}, UserId: {UserId}", model.Title, model.Content, model.UserId);

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
        public async Task<IActionResult> AddComment([FromBody] Comment model)
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

            model.UserId = userId;
            model.UserName = userName; // Set the UserName

            _logger.LogInformation($"Adding comment with PostId: {model.PostId}, Content: {model.Content}, UserId: {model.UserId}, UserName: {model.UserName}");

            try
            {
                var comment = await _postService.AddCommentAsync(model);
                if (comment == null)
                {
                    return BadRequest("Adding comment failed.");
                }
                return Ok(comment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding comment.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("getAll")]
        public async Task<ActionResult<List<Post>>> GetAllPosts()
        {
            try
            {
                var posts = await _postService.GetAllPostsAsync();
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving posts.");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
