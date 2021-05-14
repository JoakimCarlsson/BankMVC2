using System.Threading.Tasks;
using Bank.API.ViewModels.Customer;

namespace Bank.API.Services.Customer
{
    public interface ICustomerService
    {
        public Task<bool> UserExists(int id);
        public Task<CustomerDetailsViewModel> GetCustomerDetails(int id);
    }
}