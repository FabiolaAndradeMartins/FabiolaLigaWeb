using LigaWeb.Data.Entities;
using LigaWeb.Data.Repositories.Interfaces;
using LigaWeb.Helpers.Interfaces;

namespace LigaWeb.Data.Repositories.Impl
{
    public class PlayerRepository : GenericRepository<Player>, IPlayerRepository
    {
        private DataContext _context;
        private IUserHelper _userHelper;

        public PlayerRepository(DataContext context, IUserHelper userHelper) : base(context) 
        {   
            _userHelper = userHelper;
        }
    }
}
