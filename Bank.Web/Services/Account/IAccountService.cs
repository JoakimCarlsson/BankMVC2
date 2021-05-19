using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Data.Models;
using Bank.Data.Repositories.Account;
using Bank.Data.Repositories.Customer;
using Bank.Data.Repositories.Disposition;
using Bank.Web.ViewModels.Accounts;
using Microsoft.EntityFrameworkCore;

namespace Bank.Web.Services.Account
{
    public interface IAccountService
    {
        public Task SaveAsync(AccountEditViewModel model);
        public Task<AccountEditViewModel> GetEditViewModelFromId(int id);
        public Task<List<AccountDisponentViewModel>> GetAccountUsers(int id);
    }

    class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IDispositionRepository _dispositionRepository;

        public AccountService(IMapper mapper, IDispositionRepository dispositionRepository)
        {
            _mapper = mapper;
            _dispositionRepository = dispositionRepository;
        }
        
        public async Task SaveAsync(AccountEditViewModel model)
        {
            var accountUsers = await GetAccountUsers(model.AccountId);
            if (accountUsers.FirstOrDefault(i => i.CustomerId == model.NewCustomerId) is not null)
            {
                await RemoveDiposition(model);
                return;
            }
            //else
            await AddDiposition(model);
        }

        private async Task RemoveDiposition(AccountEditViewModel model)
        {
            var dispositions = await _dispositionRepository.ListAllByCustomerIdAsync(model.NewCustomerId);
            var disposition = dispositions.First(i => i.AccountId == model.AccountId);
            await _dispositionRepository.DeleteAsync(disposition);
        }

        private async Task AddDiposition(AccountEditViewModel model)
        {
            var disposition = new Disposition
            {
                CustomerId = model.NewCustomerId,
                AccountId = model.AccountId,
                Type = "DISPONENT",
            };

            await _dispositionRepository.AddAsync(disposition);
        }

        public async Task<AccountEditViewModel> GetEditViewModelFromId(int id)
        {
            var result = await GetAccountUsers(id);
            var model = new AccountEditViewModel
            {
                AccountId = id,
                Disponents = result,
            };
            return model;
        }

        public async Task<List<AccountDisponentViewModel>> GetAccountUsers(int id)
        {
            var query = await _dispositionRepository.ListAllAsync();
            var result = query
                .Include(c => c.Customer)
                .Include(a => a.Account)
                .Where(i => i.Account.AccountId == id).AsEnumerable();
            
            return _mapper.Map<List<AccountDisponentViewModel>>(result);
        }
    }
}