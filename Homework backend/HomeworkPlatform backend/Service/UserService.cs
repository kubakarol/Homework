using HomeworkPlatform_backend.Models;
using HomeworkPlatform_backend.Service.IService;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace HomeworkPlatform_backend.Service
{
    public class UserService : IUserService
    {
        private readonly HomeworkPlatformDbContext _context;
        private readonly ITokenService _tokenService;

        public UserService(HomeworkPlatformDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<User> RegisterAsync(Register model)
        {
            var user = new User
            {
                UserName = model.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // Save changes to get the generated Id
            return user;
        }

        public async Task<string> LoginAsync(Login model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == model.UserName);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return null;
            }

            return _tokenService.GenerateToken(user);
        }
    }
}
