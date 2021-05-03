using System.Linq;
using System.Threading.Tasks;

namespace Bank.Data.Repositories.Base
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IQueryable<T>> ListAllAsync();
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IQueryable<T>> GetPagedResponseAsync(int page, int size);
    }
}