using LigaWeb.Data.Entities;
using LigaWeb.Data.Repositories.Interfaces;
using LigaWeb.Helpers.Interfaces;

namespace LigaWeb.Data.Repositories.Impl
{
    public class StadiumRepository : GenericRepository<Stadium>, IStadiumRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public StadiumRepository(DataContext context, IUserHelper userHelper) : base(context) 
        {
            _userHelper = userHelper;
        }
    }
}
