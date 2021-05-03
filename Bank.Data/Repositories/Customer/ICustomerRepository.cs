using System.Linq;
using System.Threading.Tasks;
using Bank.Data.Repositories.Base;

namespace Bank.Data.Repositories.Customer
{
    public interface ICustomerRepository : IAsyncRepository<Models.Customer>
    {
        Task<IQueryable<Models.Customer>> GetPagedResponseAsync(int page, int size, string q);
        Task<int> GetQueryCount(string q);
    }
}