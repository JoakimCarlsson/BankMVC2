using System.Threading.Tasks;
using Bank.Core.ViewModels.Customers;

namespace Bank.Core.Services.Customers
{
    public interface ICustomerService
    {
        public Task<CustomerDetailsViewModel> GetByIdAsync(int id);
        public Task<CustomerSearchListViewModel> GetPagedSearchAsync(string q, int page, int pageSize);
    }
}