using System.Threading.Tasks;
using Bank.Core.ViewModels.Customers;

namespace Bank.Core.Services.Customers
{
    public interface ICustomerService
    {
        public Task<CustomerDetailsViewModel> GetByIdAsync(int id);
    }
}