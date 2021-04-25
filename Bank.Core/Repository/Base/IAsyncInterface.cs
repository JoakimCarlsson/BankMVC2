using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bank.Core.Repository.Base
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> ListAllAsync(); //IReadOnlyList
        Task<T> AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetPagedResponseAsync(int page, int size); //IReadOnlyList
    }
}