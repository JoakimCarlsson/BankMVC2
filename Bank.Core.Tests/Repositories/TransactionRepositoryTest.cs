using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Headers;
using AutoMapper;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Bank.Core.Data;
using AutoFixture;
using Bank.Core.Model;
using Bank.Core.Repository.TranasctionsRep;
using Shouldly;

namespace Bank.Core.Tests.Repositories
{
    [TestClass]
    public class TransactionRepositoryTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
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

        [TestMethod]
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
            expected.ShouldBe(actual);
        }

        [TestMethod]
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

        [TestMethod]
        public async Task ListAllAsyncShouldReturnValidEntities()
        {
            //Arrange
            var context = CreateDbContext();
            ITransactionRepository sut = new TransactionRepository(context);

            //Act
            var actual = await sut.ListAllAsync().ConfigureAwait(false);

            //Assert
            context.Transactions.ShouldBeEquivalentTo(actual);
        }

        [TestMethod]
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
            var expected = context.Transactions.FirstOrDefault(i => i.TransactionId == transaction.TransactionId);

            //Assert
            expected.ShouldBeNull();
        }

        //[TestMethod]
        //public async Task AddAsyncShouldReturnAValidEntity()
        //{
        //    //Arrange
        //    var context = CreateDbContext();
        //    ITransactionRepository sut = new TransactionRepository(context);

        //    var fixture = new Fixture();
        //    fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        //    var expected = fixture.Build<Transaction>().Without(p => p.TransactionId).Create();

        //    //Act
        //    var actual = await sut.AddAsync(expected).ConfigureAwait(false);

        //    //Assert
        //    expected.ShouldBeEquivalentTo(actual);
        //}
    }
}
