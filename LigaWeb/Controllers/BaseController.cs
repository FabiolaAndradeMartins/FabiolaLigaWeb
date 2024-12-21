using LigaWeb.Data.Entities;
using LigaWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LigaWeb.Controllers
{
    public class BaseController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;

        public BaseController(UserManager<User> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<(string FirstName, string ClubName)> GetUserInfoAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.Users
                    .Include(u => u.Club)
                    .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

                if (user != null)
                {
                    string firstName = user.FirstName;
                    string clubName = user.Club?.Name ?? string.Empty;

                    return (firstName, clubName);
                }
            }

            return (string.Empty, string.Empty);
        }
    }
}
