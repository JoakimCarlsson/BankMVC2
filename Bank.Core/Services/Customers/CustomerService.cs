using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Core.Model;
using Bank.Core.Repository.AccountRep;
using Bank.Core.Repository.CustomerRep;
using Bank.Core.Repository.DispositionRep;
using Bank.Core.ViewModels.Accounts;
using Bank.Core.ViewModels.Customers;

namespace Bank.Core.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDispositionRepository _dispositionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;

        public CustomerService(ICustomerRepository customerRepository, IDispositionRepository dispositionRepository, IAccountRepository accountRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _dispositionRepository = dispositionRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<CustomerDetailsViewModel> GetByIdAsync(int id)
        {
            var result = await _customerRepository.GetByIdAsync(id).ConfigureAwait(false);
            var model = _mapper.Map<CustomerDetailsViewModel>(result);
            model.Accounts = await GetAccountsAsync(model).ConfigureAwait(false);

            return model;
        }

        public async Task<CustomerSearchListViewModel> GetPagedSearchAsync(string q, int page, int pageSize)
        {
            var pageCount =  (double)_customerRepository.ListAllAsync().Result.Count() / pageSize;

            var model = new CustomerSearchListViewModel{Q = q, Page = page, PageSize = pageSize, TotalPages = (int)Math.Ceiling(pageCount) };

            var result = await _customerRepository.GetPagedResponseAsync(page, 50).ConfigureAwait(false);
            model.Customers = _mapper.Map<IEnumerable<CustomerSearchViewModel>>(result);
            //var pageCount = (double)totalRowCount / pageSize;
            //viewModel.TotalPages = (int)Math.Ceiling(pageCount);

            return model;
        }

        private async Task<IEnumerable<AccountCustomerViewModel>> GetAccountsAsync(CustomerDetailsViewModel model)
        {
            var result = await _dispositionRepository.ListAllByCustomerIdAsync(model.CustomerId).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<AccountCustomerViewModel>>(result.Select(i => i.Account));
        }
    }
}