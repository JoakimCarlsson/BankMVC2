using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Bank.API.ViewModels.Customer;
using Bank.Data.Models;
using Bank.Data.Repositories.Customer;
using Bank.Data.Repositories.Disposition;

namespace Bank.API.Services.Customer
{
    class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly IDispositionRepository _dispositionRepository;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, IDispositionRepository dispositionRepository)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _dispositionRepository = dispositionRepository;
        }
        public Task<bool> UserExists(int id)
        {
            return Task.FromResult(_customerRepository.GetByIdAsync(id) is not null);
        }

        public async Task<CustomerDetailsViewModel> GetCustomerDetails(int id)
        {
            var model =  _mapper.Map<CustomerDetailsViewModel>(await _customerRepository.GetByIdAsync(id));
            model.Accounts = await GetAccountsAsync(model).ConfigureAwait(false);
            return model;
        }
        
        private async Task<IEnumerable<AccountCustomerViewModel>> GetAccountsAsync(CustomerDetailsViewModel model)
        {
            var result = await _dispositionRepository.ListAllByCustomerIdAsync(model.CustomerId).ConfigureAwait(false);
            
            var tmpList = new List<AccountCustomerViewModel>();
            foreach (Disposition disposition in result)
            {
                tmpList.Add(new AccountCustomerViewModel
                {
                    AccountId = disposition.AccountId,
                    Balance = disposition.Account.Balance,
                    Type = disposition.Type,
                });
            }

            return tmpList;
        }
    }
}