using System.Linq.Expressions;

namespace LigaWeb.Data.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);

        Task<T> GetByIdAsync(int id);

        Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes);

        Task CreateAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistAsync(int id);
    }
}
