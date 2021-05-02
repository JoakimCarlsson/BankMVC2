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

        [TestMethod]
        public async Task ShouldSaveDepositTransaction()
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
                Amount = 500
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

        [TestMethod]
        public async Task ShouldSaveWithdrawTransaction()
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
                Amount = 500,
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
    }
}
