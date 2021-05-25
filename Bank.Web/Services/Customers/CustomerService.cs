using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bank.AzureSearchService.Services;
using Bank.AzureSearchService.Services.Search;
using Bank.Data.Models;
using Bank.Data.Repositories.Account;
using Bank.Data.Repositories.Customer;
using Bank.Data.Repositories.Disposition;
using Bank.Web.ViewModels.Accounts;
using Bank.Web.ViewModels.Customers;

namespace Bank.Web.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IDispositionRepository _dispositionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IAzureSearch _azureSearch;
        private readonly IAzureUpdater _azureUpdater;

        public CustomerService(ICustomerRepository customerRepository, IDispositionRepository dispositionRepository, IAccountRepository accountRepository, IMapper mapper, IAzureSearch azureSearch, IAzureUpdater azureUpdater)
        {
            _customerRepository = customerRepository;
            _dispositionRepository = dispositionRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _azureSearch = azureSearch;
            _azureUpdater = azureUpdater;
        }

        public async Task<CustomerDetailsViewModel> GetByIdAsync(int id)
        {
            var result = await _customerRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (result == null)
                return null;

            var model = _mapper.Map<CustomerDetailsViewModel>(result);
            model.Accounts = await GetAccountsAsync(model).ConfigureAwait(false);

            return model;
        }

        public async Task<CustomerSearchListViewModel> GetAzurePagedSearchAsync(string q, string sortField, string sortOrder, int page, int pageSize)
        {
            if (page <= 0)
                page = 1;

            var tmpCustomerList = new List<Customer>();
            
            var offset = page == 1 ? 0 : page * pageSize;

            var result = await _azureSearch.SearchCustomersAsync(q, sortField, sortOrder, offset, pageSize);
            
            foreach (var id in result.Ids)
                tmpCustomerList.Add(await _customerRepository.GetByIdAsync(id));
            
            var currentRowCount = ((page - 1) * pageSize) + 1; //first,
            var rowCount = currentRowCount + result.Ids.Count() - 1; //last
            
            var model = new CustomerSearchListViewModel()
            {
                PagingViewModel = new PagingViewModel
                {
                    Page = page,
                    Q = q,
                    PageSize = pageSize,
                    MaxRowCount = (int) result.TotalRowCount,
                    SortField = sortField,
                    SortOrder = sortOrder,
                    OppositeSortOrder = sortOrder == "asc" ? "desc" : "asc",
                    RowCount = rowCount,
                    CurrentRowCount = currentRowCount,
                },
                
                Customers = _mapper.Map<IEnumerable<CustomerSearchViewModel>>(tmpCustomerList)
            };

            return model;
        }

        public async Task SaveCustomerAsync(CustomerBaseViewModel model)
        {
            switch (model)
            {
                case CustomerEditViewModel viewModel:
                    await SaveEditAsync(viewModel).ConfigureAwait(false);
                    break;
                case CustomerRegisterViewModel viewModel:
                    await RegisterNewUserAsync(viewModel).ConfigureAwait(false);
                    break;
            }
        }

        public async Task<CustomerEditViewModel> GetCustomerEditAsync(int id)
        {
            return _mapper.Map<CustomerEditViewModel>(await _customerRepository.GetByIdAsync(id).ConfigureAwait(false));
        }

        private async Task RegisterNewUserAsync(CustomerRegisterViewModel viewModel)
        {
            var customer = _mapper.Map<Customer>(viewModel);
            var account = new Bank.Data.Models.Account
            {
                Created = DateTime.Now,
                Balance = 0,
                Frequency = "Monthly"
            };

            var disposition = new Disposition
            {
                Account = account,
                Customer = customer,
                Type = "OWNER",
            };

            customer.Dispositions.Add(disposition);
            await _azureUpdater.AddOrUpdateCustomerInAzure(customer);
            await _customerRepository.AddAsync(customer).ConfigureAwait(false);
        }

        private async Task SaveEditAsync(CustomerEditViewModel viewModel)
        {
            var customer = _mapper.Map<Customer>(viewModel);
            await _azureUpdater.AddOrUpdateCustomerInAzure(customer);
            await _customerRepository.UpdateAsync(customer).ConfigureAwait(false);
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