using HomeworkPlatform_backend.Models;

namespace HomeworkPlatform_backend.Service.IService
{
    public interface IUserService
    {
        Task<User> RegisterAsync(Register model);
        Task<string> LoginAsync(Login model);
    }
}
