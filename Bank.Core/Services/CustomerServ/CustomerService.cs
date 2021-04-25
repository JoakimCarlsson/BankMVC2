using System.Threading.Tasks;
using AutoMapper;
using Bank.Core.Repository.CustomerRep;
using Bank.Core.ViewModels.Customers;

namespace Bank.Core.Services.CustomerServ
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        public async Task<CustomerIndexViewModel> GetByIdAsync(int id)
        {
            var result = await _customerRepository.GetByIdAsync(id);
            return _mapper.Map<CustomerIndexViewModel>(result);
        }
    }
}