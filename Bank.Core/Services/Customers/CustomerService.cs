﻿using System;
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

        //todo fix me pliz
        public async Task<CustomerDetailsViewModel> GetByIdAsync(int id)
        {
            var result = await _customerRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (result == null)
                return null;
            
            var model = _mapper.Map<CustomerDetailsViewModel>(result);
            model.Accounts = await GetAccountsAsync(model).ConfigureAwait(false);

            return model;
        }

        public async Task<CustomerSearchListViewModel> GetPagedSearchAsync(string q, int page, int pageSize)
        {
            if (page <= 0)
                page = 1;

            var result = await _customerRepository.GetPagedResponseAsync(page, pageSize, q).ConfigureAwait(false);
            var totalRows = await _customerRepository.GetQueryCount(q).ConfigureAwait(false);

            var pageCount = (double) totalRows / pageSize;
            var currentRowCount = ((page - 1) * pageSize) + 1;
            var rowCount = currentRowCount + result.Count() - 1;

            var model = new CustomerSearchListViewModel
            {
                PagingViewModel = new PagingViewModel
                {
                    Page = page,
                    Q = q,
                    PageSize = pageSize,
                    MaxRowCount = totalRows,
                    TotalPages = (int) Math.Ceiling(pageCount),
                    CurrentRowCount = currentRowCount,
                    RowCount = rowCount
                },
                Customers = _mapper.Map<IEnumerable<CustomerSearchViewModel>>(result)
            };

            return model;
        }

        private async Task<IEnumerable<AccountCustomerViewModel>> GetAccountsAsync(CustomerDetailsViewModel model)
        {
            var result = await _dispositionRepository.ListAllByCustomerIdAsync(model.CustomerId).ConfigureAwait(false);
            return _mapper.Map<IEnumerable<AccountCustomerViewModel>>(result.Select(i => i.Account));
        }
    }
}