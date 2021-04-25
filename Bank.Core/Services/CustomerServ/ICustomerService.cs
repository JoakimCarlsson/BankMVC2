using System.Threading.Tasks;
using Bank.Core.ViewModels.CustomerVm;

namespace Bank.Core.Services.CustomerServ
{
    public interface ICustomerService
    {
        Task<CustomerIndexViewModel> GetByIdAsync(int id);
    }
}