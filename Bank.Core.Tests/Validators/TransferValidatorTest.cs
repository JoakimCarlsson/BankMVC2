using Bank.Core.Model;
using Bank.Core.Repository.AccountRep;
using Bank.Core.Validators.Transfer;
using Bank.Core.ViewModels.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bank.Core.Tests.Validators
{
    [TestClass]
    public class TransferValidatorTest
    {
        private Mock<IAccountRepository> _accountRepository;
        private Account _account;

        [TestInitialize]
        public void TestInitialize()
        {
            _accountRepository = new Mock<IAccountRepository>();
            _account = new Account
            {
                AccountId = 1337,
                Balance = 1000
            };
        }

        [DataTestMethod]
        [DataRow(5000)]
        [DataRow(4635)]
        public void ShouldNotBePossibleToWithdrawMoreThenYouHaveOnYourAccount(int amount)
        {
            //Arrange
            WithdrawViewModel model = new WithdrawViewModel
            {
                AccountId = _account.AccountId,
                Amount = amount,
            };

            _accountRepository.Setup(x => x.GetByIdAsync(_account.AccountId)).ReturnsAsync(_account);
            
            //act
            var sut = new WithdrawViewModelValidator(_accountRepository.Object);
            var expected = sut.Validate(model).IsValid;

            //assert
            Assert.IsFalse(expected);
        }

        [DataTestMethod]
        [DataRow(5000)]
        [DataRow(4635)]
        public void ShouldNotBePossibleToTransferMoreThenYouHaveOnYourAccount(int amount)
        {
            //Arrange
            var toAccount = new Account {  AccountId = 5};

            TransferViewModel model = new TransferViewModel
            {
                AccountId = _account.AccountId,
                ToAccountId = toAccount.AccountId,
                Amount = amount,
            };

            _accountRepository.Setup(x => x.GetByIdAsync(_account.AccountId)).ReturnsAsync(_account);
            _accountRepository.Setup(x => x.GetByIdAsync(toAccount.AccountId)).ReturnsAsync(toAccount);

            //act
            var sut = new TransferViewModelValidator(_accountRepository.Object);
            var expected = sut.Validate(model).IsValid;

            //assert
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void ShouldNotBePossibleToTransferMoneyToTheSameAccount()
        {
            TransferViewModel model = new TransferViewModel
            {
                AccountId = _account.AccountId,
                ToAccountId = _account.AccountId,
                Amount = 500,
            };

            _accountRepository.Setup(x => x.GetByIdAsync(_account.AccountId)).ReturnsAsync(_account);

            var sut = new TransferViewModelValidator(_accountRepository.Object);
            var expected = sut.Validate(model).IsValid;

            //assert
            Assert.IsFalse(expected);
        }

        [DataTestMethod]
        [DataRow(-125)]
        [DataRow(-4635)]
        public void ShouldNotBePossibleToWithdrawNegativeAmount(int amount)
        {
            //Arrange
            WithdrawViewModel model = new WithdrawViewModel
            {
                AccountId = _account.AccountId,
                Amount = amount,
            };
            _accountRepository.Setup(x => x.GetByIdAsync(_account.AccountId)).ReturnsAsync(_account);

            //act
            var sut = new WithdrawViewModelValidator(_accountRepository.Object);
            var expected = sut.Validate(model).IsValid;

            //assert
            Assert.IsFalse(expected);
        }

        [DataTestMethod]
        [DataRow(-125)]
        [DataRow(-4635)]
        public void ShouldNotBePossibleToDepositNegativeAmount(int amount)
        {
            //Arrange
            DepositViewModel model = new DepositViewModel
            {
                AccountId = _account.AccountId,
                Amount = amount,
            };

            _accountRepository.Setup(x => x.GetByIdAsync(_account.AccountId)).ReturnsAsync(_account);

            //act
            var sut = new DepositViewModelValidator(_accountRepository.Object);
            var expected = sut.Validate(model).IsValid;

            //assert
            Assert.IsFalse(expected);
        }

        [DataTestMethod]
        [DataRow(1337)]
        public void ShouldBePossibleToDepositIfAmountIsCorrectAndAccountIdExist(int amount)
        {
            //Arrange
            DepositViewModel model = new DepositViewModel
            {
                AccountId = _account.AccountId,
                Amount = amount,
            };

            _accountRepository.Setup(x => x.GetByIdAsync(_account.AccountId)).ReturnsAsync(_account);

            //Act
            var sut = new DepositViewModelValidator(_accountRepository.Object);
            var expected = sut.Validate(model).IsValid;

            //Assert
            Assert.IsTrue(expected);
        }

        [DataTestMethod]
        [DataRow(500)]
        public void ShouldBePossibleToWithdrawIfAmountIsCorrectAndAccountIdExist(int amount)
        {
            //Arrange
            WithdrawViewModel model = new WithdrawViewModel
            {
                AccountId = _account.AccountId,
                Amount = amount,
            };
            _accountRepository.Setup(x => x.GetByIdAsync(_account.AccountId)).ReturnsAsync(_account);

            //Act
            var sut = new WithdrawViewModelValidator(_accountRepository.Object);
            var expected = sut.Validate(model).IsValid;

            //Assert
            Assert.IsTrue(expected);
        }

        [DataTestMethod]
        [DataRow(500)]
        public void ShouldBePossibleToTransferIfAmountIsCorrectAndAccountIdsExist(int amount)
        {
            //Arrange
            var toAccount = new Account { AccountId = 5 };

            TransferViewModel model = new TransferViewModel
            {
                AccountId = _account.AccountId,
                ToAccountId = toAccount.AccountId,
                Amount = amount,
            };

            _accountRepository.Setup(x => x.GetByIdAsync(_account.AccountId)).ReturnsAsync(_account);
            _accountRepository.Setup(x => x.GetByIdAsync(toAccount.AccountId)).ReturnsAsync(toAccount);

            //Act
            var sut = new TransferViewModelValidator(_accountRepository.Object);
            var expected = sut.Validate(model).IsValid;

            //Assert
            Assert.IsTrue(expected);
        }
    }
}
