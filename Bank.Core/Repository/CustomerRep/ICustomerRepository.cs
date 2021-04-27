using System.Linq;
using System.Threading.Tasks;
using Bank.Core.Model;
using Bank.Core.Repository.Base;

namespace Bank.Core.Repository.CustomerRep
{
    public interface ICustomerRepository : IAsyncRepository<Customer>
    {
        Task<IQueryable<Customer>> GetPagedResponseAsync(int page, int size, string q);
        Task<int> GetQueryCount(string q);
    }
}