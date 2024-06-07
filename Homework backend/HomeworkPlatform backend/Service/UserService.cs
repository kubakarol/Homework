using HomeworkPlatform_backend.Models;
using HomeworkPlatform_backend.Service.IService;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HomeworkPlatform_backend.Service
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly ITokenService _tokenService;

        public UserService(IOptions<MongoSettings> settings, IMongoClient client, ITokenService tokenService)
        {
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _users = database.GetCollection<User>("users");
            _tokenService = tokenService;
        }

        public async Task<User> RegisterAsync(Register model)
        {
            var user = new User
            {
                UserName = model.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password)
            };

            await _users.InsertOneAsync(user);
            return user;
        }

        public async Task<string> LoginAsync(Login model)
        {
            var user = await _users.Find(u => u.UserName == model.UserName).FirstOrDefaultAsync();
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return null;
            }

            return _tokenService.GenerateToken(user);
        }
    }
}
