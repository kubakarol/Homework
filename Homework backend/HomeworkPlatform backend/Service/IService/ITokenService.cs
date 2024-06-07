using HomeworkPlatform_backend.Models;

namespace HomeworkPlatform_backend.Service.IService
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
