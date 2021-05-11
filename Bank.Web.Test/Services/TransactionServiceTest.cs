using AutoMapper;
using Bank.Data.Models;
using Bank.Data.Repositories.Account;
using Bank.Data.Repositories.Transaction;
using Bank.Web.Extensions;
using Bank.Web.Mapping;
using Bank.Web.Services.Transactions;
using Bank.Web.ViewModels.Transactions;
using Moq;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Bank.Web.Test.Services
{
    public class TransactionServiceTest
    {
        private readonly Mock<IAccountRepository> _accountRepository;
        private readonly Mock<ITransactionRepository> _transactionRepository;
        private readonly IMapper _mapper;
        private readonly Account _account;

        public TransactionServiceTest()
        {
            _accountRepository = new Mock<IAccountRepository>();
            _transactionRepository = new Mock<ITransactionRepository>();

            if (_mapper != null) return;
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles());
            });

            var mapper = mappingConfig.CreateMapper();
            _mapper = mapper;

            _account = new Account
            {
                AccountId = 5,
                Balance = 1500,
            };
        }

        [Theory]
        [InlineData(5000)]
        [InlineData(4635)]
        public async Task ShouldSaveDepositTransaction(int amount)
        {
            //Arrange
            Transaction actual = null;
            _transactionRepository.Setup(x => x.AddAsync(It.IsAny<Transaction>()))
                .Callback<Transaction>(transaction => { actual = transaction; })
                .ReturnsAsync(() => actual);

            _accountRepository.Setup(x => x.GetByIdAsync(_account.AccountId)).ReturnsAsync(_account);
            _accountRepository.Setup(x => x.UpdateAsync(_account));

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<DepositViewModel, Transaction>().IgnoreAllNonExisting());
            configuration.AssertConfigurationIsValid();

            var model = new DepositViewModel
            {
                AccountId = _account.AccountId,
                Amount = amount
            };

            //Act
            var sut = new TransactionService(_mapper, _transactionRepository.Object, _accountRepository.Object);

            await sut.SaveTransaction(model).ConfigureAwait(false);

            //Assert
            _transactionRepository.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Once());
            _accountRepository.Verify(x => x.GetByIdAsync(_account.AccountId), Times.Once());
            _accountRepository.Verify(x => x.UpdateAsync(_account), Times.Once());

            actual.ShouldNotBeNull();
            actual.Operation.ShouldBeEquivalentTo("Deposit");
            actual.Type.ShouldBeEquivalentTo("Credit");
            actual.Date.ToString().ShouldBeEquivalentTo(DateTime.Now.ToString());
            actual.Amount.ShouldBeEquivalentTo(model.Amount);
            actual.AccountId.ShouldBeEquivalentTo(model.AccountId);
            actual.Balance.ShouldBeEquivalentTo(_account.Balance);

        }

        [Theory]
        [InlineData(400)]
        [InlineData(635)]
        public async Task ShouldSaveWithdrawTransaction(int amount)
        {
            //Arrange
            Transaction actual = null;
            _transactionRepository.Setup(x => x.AddAsync(It.IsAny<Transaction>()))
                .Callback<Transaction>(transaction => { actual = transaction; })
                .ReturnsAsync(() => actual);

            _accountRepository.Setup(x => x.GetByIdAsync(_account.AccountId)).ReturnsAsync(_account);
            _accountRepository.Setup(x => x.UpdateAsync(_account));

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<WithdrawViewModel, Transaction>().IgnoreAllNonExisting());
            configuration.AssertConfigurationIsValid();

            var model = new WithdrawViewModel
            {
                AccountId = _account.AccountId,
                Amount = amount,
            };

            //Act
            var sut = new TransactionService(_mapper, _transactionRepository.Object, _accountRepository.Object);
            await sut.SaveTransaction(model).ConfigureAwait(false);

            //Assert
            _transactionRepository.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Once());
            _accountRepository.Verify(x => x.GetByIdAsync(_account.AccountId), Times.Once());
            _accountRepository.Verify(x => x.UpdateAsync(_account), Times.Once());

            //Assert.AreEqual(-model.Amount, savedTransaction.Amount);

            actual.ShouldNotBeNull();
            actual.Operation.ShouldBeEquivalentTo("Withdraw");
            actual.Type.ShouldBeEquivalentTo("Credit");
            actual.Date.ToString().ShouldBeEquivalentTo(DateTime.Now.ToString());
            actual.Amount.ShouldBeEquivalentTo(-model.Amount);
            actual.AccountId.ShouldBeEquivalentTo(model.AccountId);
            actual.Balance.ShouldBeEquivalentTo(_account.Balance);
        }

        [Theory]
        [InlineData(400)]
        [InlineData(600)]
        public async Task ShouldSaveTransferTransactions(int amount)
        {
            //Arrange
            var toAccount = new Account
            {
                AccountId = 1,
                Balance = 1000
            };

            List<Transaction> transactions = new List<Transaction>();
            _transactionRepository.Setup(x => x.AddAsync(It.IsAny<Transaction>()))
                .Callback<Transaction>(transaction => { transactions.Add(transaction); })
                .ReturnsAsync(() => transactions[0]);

            _accountRepository.Setup(x => x.GetByIdAsync(_account.AccountId)).ReturnsAsync(_account);
            _accountRepository.Setup(x => x.GetByIdAsync(toAccount.AccountId)).ReturnsAsync(toAccount);
            _accountRepository.Setup(x => x.UpdateAsync(_account));
            _accountRepository.Setup(x => x.UpdateAsync(toAccount));

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<WithdrawViewModel, Transaction>().IgnoreAllNonExisting());
            configuration.AssertConfigurationIsValid();

            var model = new TransferViewModel()
            {
                AccountId = _account.AccountId,
                ToAccountId = toAccount.AccountId,
                Amount = amount,
            };

            //Act
            var sut = new TransactionService(_mapper, _transactionRepository.Object, _accountRepository.Object);
            await sut.SaveTransaction(model).ConfigureAwait(false);

            //Assert
            _transactionRepository.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Exactly(2));
            _accountRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
            _accountRepository.Verify(x => x.UpdateAsync(It.IsAny<Account>()), Times.Exactly(2));

            transactions.Count.ShouldBeEquivalentTo(2);

            transactions[0].ShouldNotBeNull();
            transactions[0].Operation.ShouldBeEquivalentTo("Transfer to another account.");
            transactions[0].Type.ShouldBeEquivalentTo("Credit");
            transactions[0].Date.ToString().ShouldBeEquivalentTo(DateTime.Now.ToString());
            transactions[0].Amount.ShouldBeEquivalentTo(-model.Amount);
            transactions[0].AccountId.ShouldBeEquivalentTo(model.AccountId);
            transactions[0].Balance.ShouldBeEquivalentTo(_account.Balance);

            transactions[1].ShouldNotBeNull();
            transactions[1].Operation.ShouldBeEquivalentTo("Transfer from another account.");
            transactions[1].Type.ShouldBeEquivalentTo("Credit");
            transactions[1].Date.ToString().ShouldBeEquivalentTo(DateTime.Now.ToString());
            transactions[1].Amount.ShouldBeEquivalentTo(model.Amount);
            transactions[1].AccountId.ShouldBeEquivalentTo(model.ToAccountId);
            transactions[1].Balance.ShouldBeEquivalentTo(toAccount.Balance);
        }
    }
}
