using System.Linq;
using System.Threading.Tasks;
using Bank.Data.Repositories.Base;

namespace Bank.Data.Repositories.Disposition
{
    public interface IDispositionRepository : IAsyncRepository<Models.Disposition>
    {
        Task<IQueryable<Models.Disposition>> ListAllByCustomerIdAsync(int customerId);
    }
}