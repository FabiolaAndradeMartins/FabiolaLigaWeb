using LigaWeb.Data.Entities;

namespace LigaWeb
{
    public interface IUserService
    {
        Task<User> GetUserAsync();
        Task<User?> GetLoggedInUserAsync();
    }
}
