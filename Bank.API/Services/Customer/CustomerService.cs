using System.Threading.Tasks;
using AutoMapper;
using Bank.API.ViewModels.Customer;
using Bank.Data.Repositories.Customer;

namespace Bank.API.Services.Customer
{
    class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }
        public async Task<bool> UserExists(int id)
        {
            return _customerRepository.GetByIdAsync(id) is not null;
        }

        public async Task<CustomerDetailsViewModel> GetCustomerDetails(int id)
        {
            return _mapper.Map<CustomerDetailsViewModel>(await _customerRepository.GetByIdAsync(id));
        }
    }
}