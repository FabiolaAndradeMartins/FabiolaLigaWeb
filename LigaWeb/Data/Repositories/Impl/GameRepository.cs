using LigaWeb.Data.Entities;
using LigaWeb.Data.Repositories.Interfaces;
using LigaWeb.Helpers.Interfaces;

namespace LigaWeb.Data.Repositories.Impl
{
    public class GameRepository : GenericRepository<Game>, IGameRepository
    {
        private readonly DataContext _context;
        private IUserHelper _userHelper;

        public GameRepository(DataContext context, IUserHelper userHelper) : base(context) 
        {
            _userHelper = userHelper;
        }
    }
}
