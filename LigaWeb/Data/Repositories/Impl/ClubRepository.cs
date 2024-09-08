using LigaWeb.Data.Entities;
using LigaWeb.Data.Repositories.Interfaces;
using LigaWeb.Helpers.Interfaces;

namespace LigaWeb.Data.Repositories.Impl
{
    public class ClubRepository : GenericRepository<Club>, IClubRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public ClubRepository(DataContext context, IUserHelper userHelper) : base(context) 
        {                  
            _userHelper = userHelper;
        }
    }
}