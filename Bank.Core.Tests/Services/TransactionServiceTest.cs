using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Bank.Core.Extensions;
using Bank.Core.Mapping;
using Bank.Core.Model;
using Bank.Core.Repository.AccountRep;
using Bank.Core.Repository.TranasctionsRep;
using Bank.Core.Services.Transactions;
using Bank.Core.ViewModels.Transactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bank.Core.Tests.Services
{
    [TestClass]
    public class TransactionServiceTest
    {
        private Mock<IAccountRepository> _accountRepository;
        private Mock<ITransactionRepository> _transactionRepository;
        private IMapper _mapper;
        private Account _account;

        [TestInitialize]
        public void TestInitialize()
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

        [DataTestMethod]
        [DataRow(5000)]
        [DataRow(4635)]
        public async Task ShouldSaveDepositTransaction(int amount)
        {
            //Arrange
            Transaction savedTransaction = null;
            _transactionRepository.Setup(x => x.AddAsync(It.IsAny<Transaction>())).Callback<Transaction>(transaction =>
            {
                savedTransaction = transaction;
            });

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

            await sut.SaveDepositAsync(model).ConfigureAwait(false);

            //Assert
            _transactionRepository.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Once());
            _accountRepository.Verify(x => x.GetByIdAsync(_account.AccountId), Times.Once());
            _accountRepository.Verify(x => x.UpdateAsync(_account), Times.Once());

            Assert.IsNotNull(savedTransaction);
            Assert.AreEqual("Deposit", savedTransaction.Operation);
            Assert.AreEqual("Credit", savedTransaction.Type);
            Assert.AreEqual(DateTime.Now.ToString(), savedTransaction.Date.ToString());
            Assert.AreEqual(model.Amount, savedTransaction.Amount);
            Assert.AreEqual(model.AccountId, savedTransaction.AccountId);
            Assert.AreEqual(_account.Balance, savedTransaction.Balance);
        }

        [DataTestMethod]
        [DataRow(400)]
        [DataRow(635)]
        public async Task ShouldSaveWithdrawTransaction(int amount)
        {
            //Arrange
            Transaction savedTransaction = null;
            _transactionRepository.Setup(x => x.AddAsync(It.IsAny<Transaction>())).Callback<Transaction>(transaction =>
            {
                savedTransaction = transaction;
            });

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
            await sut.SaveWithdrawAsync(model).ConfigureAwait(false);

            //Assert
            _transactionRepository.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Once());
            _accountRepository.Verify(x => x.GetByIdAsync(_account.AccountId), Times.Once());
            _accountRepository.Verify(x => x.UpdateAsync(_account), Times.Once());
            Assert.IsNotNull(savedTransaction);
            Assert.AreEqual("Withdraw", savedTransaction.Operation);
            Assert.AreEqual("Credit", savedTransaction.Type);
            Assert.AreEqual(DateTime.Now.ToString(), savedTransaction.Date.ToString());
            Assert.AreEqual(-model.Amount, savedTransaction.Amount);
            Assert.AreEqual(model.AccountId, savedTransaction.AccountId);
            Assert.AreEqual(_account.Balance, savedTransaction.Balance);
        }

        [DataTestMethod]
        [DataRow(400)]
        [DataRow(600)]
        public async Task ShouldSaveTransferTransactions(int amount)
        {
            //Arrange
            var toAccount = new Account
            {
                AccountId = 1,
                Balance = 1000
            };

            List<Transaction> transactions = new List<Transaction>();
            _transactionRepository.Setup(x => x.AddAsync(It.IsAny<Transaction>())).Callback<Transaction>(transaction =>
            {
                transactions.Add(transaction);
            });

            _accountRepository.Setup(x => x.GetByIdAsync(_account.AccountId)).ReturnsAsync(_account);
            _accountRepository.Setup(x => x.GetByIdAsync(toAccount.AccountId)).ReturnsAsync(toAccount);
            _accountRepository.Setup(x => x.UpdateAsync(_account));
            _accountRepository.Setup(x => x.UpdateAsync(toAccount));

            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<WithdrawViewModel, Transaction>().IgnoreAllNonExisting());
            configuration.AssertConfigurationIsValid();

            var model = new TransferViewModel()
            {
                FromAccountId = _account.AccountId,
                ToAccountId = toAccount.AccountId,
                Amount = amount,
            };

            //Act
            var sut = new TransactionService(_mapper, _transactionRepository.Object, _accountRepository.Object);
            await sut.SaveTransferAsync(model).ConfigureAwait(false);

            //Assert
            _transactionRepository.Verify(x => x.AddAsync(It.IsAny<Transaction>()), Times.Exactly(2));
            _accountRepository.Verify(x => x.GetByIdAsync(It.IsAny<int>()), Times.Exactly(2));
            _accountRepository.Verify(x => x.UpdateAsync(It.IsAny<Account>()), Times.Exactly(2));

            Assert.AreEqual(transactions.Count, 2);
            Assert.AreEqual("Transfer to another account.", transactions[0].Operation);
            Assert.AreEqual("Credit", transactions[0].Type);
            Assert.AreEqual(DateTime.Now.ToString(), transactions[0].Date.ToString());
            Assert.AreEqual(-model.Amount, transactions[0].Amount);
            Assert.AreEqual(model.FromAccountId, transactions[0].AccountId);
            Assert.AreEqual(_account.Balance, transactions[0].Balance);

            Assert.AreEqual("Transfer from another account.", transactions[1].Operation);
            Assert.AreEqual("Credit", transactions[1].Type);
            Assert.AreEqual(DateTime.Now.ToString(), transactions[1].Date.ToString());
            Assert.AreEqual(model.Amount, transactions[1].Amount);
            Assert.AreEqual(model.ToAccountId, transactions[1].AccountId);
            Assert.AreEqual(toAccount.Balance, transactions[1].Balance);
        }
    }
}
