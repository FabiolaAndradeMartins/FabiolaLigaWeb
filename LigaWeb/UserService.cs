using LigaWeb.Data.Entities;
using LigaWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LigaWeb
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(UserManager<User> userManager, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetUserAsync()
        {
            var user = _httpContextAccessor.HttpContext.User;

            if (user.Identity.IsAuthenticated)
            {
                var currentUser = await _userManager.Users
                    .Include(u => u.Club)
                    .FirstOrDefaultAsync(u => u.UserName == user.Identity.Name);
                if (currentUser != null)
                    return currentUser;               
            }

            return new User();
        }

        public async Task<User?> GetLoggedInUserAsync()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            return user != null ? await _userManager.GetUserAsync(user) : null;
        }
    }
}
