using System.Collections.Generic;
using System.Threading.Tasks;
using Bank.Core.Services.Customers;
using Bank.Core.ViewModels.Customers;
using Bank.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;

namespace Bank.Web.Tests.Controllers
{
    [TestClass]
    public class CustomerControllerTest
    {
        private Mock<ICustomerService> _customerService;
        private CustomerController _sut;

        [TestInitialize]
        public void TestInitialize()
        {
            _customerService = new Mock<ICustomerService>();
        }

        [TestMethod]
        public async Task CustomerControllerShouldReturnCustomerIndexView()
        {
            //Arrange
            var customerDetailsViewModel = new CustomerDetailsViewModel
            {
                CustomerId = 1,
            };

            _customerService.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(customerDetailsViewModel);
            _sut = new CustomerController(_customerService.Object);

            //Act
            var actual = await _sut.CustomerDetails(customerDetailsViewModel.CustomerId).ConfigureAwait(false) as ViewResult;

            //Assert
            actual.ShouldNotBeNull();
            actual.Model.ShouldBeEquivalentTo(customerDetailsViewModel);
        }

        [TestMethod]
        public async Task CustomerControllerShouldReturnErrorIfInvalidId()
        {
            //Arrange

            _customerService.Setup(x => x.GetByIdAsync(It.IsAny<int>()));
            _sut = new CustomerController(_customerService.Object);

            //Act
            var actual = await _sut.CustomerDetails(1).ConfigureAwait(false) as ViewResult;

            //Assert
            actual.ShouldNotBeNull();
            actual.ViewName.ShouldBeEquivalentTo("_Error");
        }
    }
}
