using System.Threading.Tasks;
using Bank.Web.ViewModels.Customers;

namespace Bank.Web.Services.Customers
{
    public interface ICustomerService
    {
        public Task<CustomerDetailsViewModel> GetByIdAsync(int id);
        public Task<CustomerSearchListViewModel> GetAzurePagedSearchAsync(string q, string sortField, string sortOrder, int page, int pageSize);
        public Task SaveCustomerAsync(CustomerBaseViewModel model);
        public Task<CustomerEditViewModel> GetCustomerEditAsync(int id);
    }
}