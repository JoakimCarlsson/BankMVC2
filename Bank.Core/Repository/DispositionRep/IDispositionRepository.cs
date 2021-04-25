using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bank.Core.Model;
using Bank.Core.Repository.Base;
using Bank.Core.Repository.CustomerRep;
using Bank.Core.ViewModels.Customers;

namespace Bank.Core.Repository.DispositionRep
{
    public interface IDispositionRepository : IAsyncRepository<Disposition>
    {
        Task<IQueryable<Disposition>> ListAllByCustomerIdAsync(int customerId);
    }
}