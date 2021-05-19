using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using Bank.Data.Data;
using Bank.Data.Models;
using Bank.Data.Repositories.Transaction;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Bank.Data.Test.Repositories
{
    public class TransactionRepositoryTest
    {
        [Fact]
        public async Task ListAllByAccountIdShouldReturnTransactionsWithTheRightId()
        {
            //Arrange
            var context = CreateDbContext();
            ITransactionRepository sut = new TransactionRepository(context);
            var fixture = new Fixture();
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var transactions = fixture.Build<Transaction>()
                .With(x => x.AccountId, 1)
                .Without(i => i.TransactionId)
                .Without(i => i.AccountNavigation).CreateMany<Transaction>(5);

            await context.AddRangeAsync(transactions).ConfigureAwait(false);

            //Act
            var actual = await sut.ListAllByAccountIdAsync(1).ConfigureAwait(false);

            //Assert
            actual.ShouldBe(context.Transactions.Where(i => i.AccountId == 1));
        }

        [Fact]
        public async Task AddAsyncShouldAddValidEntity()
        {
            //Arrange
            var context = CreateDbContext();
            ITransactionRepository sut = new TransactionRepository(context);
            var tranasction = new Transaction
            {
                AccountId = 1,
                Date = DateTime.Now,
                Type = "Credit",
                Operation = "Credit In Cash",
                Amount = 700,
                Balance = 700,
                Symbol = "old-age pension"
            };
            //Act
            var actual = await sut.AddAsync(tranasction).ConfigureAwait(false);
            var expected = await context.Transactions.FirstOrDefaultAsync(i => i.TransactionId == actual.TransactionId).ConfigureAwait(false);
            //Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task UpdateAsyncShouldUpdateEntity()
        {
            //Arrange
            var context = CreateDbContext();
            ITransactionRepository sut = new TransactionRepository(context);
            var expected = await context.Transactions.FirstOrDefaultAsync().ConfigureAwait(false);
            expected.Balance = 5000;

            //Act
            await sut.UpdateAsync(expected).ConfigureAwait(false);
            var actual = context.Transactions.First(i => i.TransactionId == expected.TransactionId);

            //Assert

            actual.ShouldBe(expected);
        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnValidEntityIfValidId()
        {
            //Arrange
            var context = CreateDbContext();
            var random = new Random();
            var transactions = context.Transactions.ToList();

            ITransactionRepository sut = new TransactionRepository(context);

            var expected = transactions[random.Next(0, context.Transactions.Count())];

            //Act
            var actual = await sut.GetByIdAsync(expected.TransactionId).ConfigureAwait(false);

            //Assert
            actual.ShouldBe(expected);

        }

        [Fact]
        public async Task GetByIdAsyncShouldReturnNullIfInvalidId()
        {
            //Arrange
            var context = CreateDbContext();

            ITransactionRepository sut = new TransactionRepository(context);

            //Act
            var actual = await sut.GetByIdAsync(-5).ConfigureAwait(false);

            //Assert
            actual.ShouldBeNull();
        }

        [Fact]
        public async Task ListAllAsyncShouldReturnValidEntities()
        {
            //Arrange
            var context = CreateDbContext();
            ITransactionRepository sut = new TransactionRepository(context);

            //Act
            var actual = await sut.ListAllAsync().ConfigureAwait(false);

            //Assert
            actual.ShouldBeEquivalentTo(context.Transactions);
        }

        [Fact]
        public async Task DeleteAsyncShouldDeleteTheGivenEntity()
        {
            //Arrange
            var context = CreateDbContext();
            var random = new Random();
            var transactions = context.Transactions.ToList();

            ITransactionRepository sut = new TransactionRepository(context);

            var transaction = transactions[random.Next(0, context.Transactions.Count())];

            //Act
            await sut.DeleteAsync(transaction).ConfigureAwait(false);
            var actual = context.Transactions.FirstOrDefault(i => i.TransactionId == transaction.TransactionId);

            //Assert
            actual.ShouldBeNull();
        }

        private ApplicationDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var dbContext = new ApplicationDbContext(options);

            var fixture = new Fixture();
            
            fixture.Behaviors.Add(new OmitOnRecursionBehavior()); 
            dbContext.AddRange(fixture.CreateMany<Transaction>(20));
            dbContext.SaveChanges();

            return dbContext;
        }
    }
}
