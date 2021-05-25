using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Web.Controllers;
using Bank.Web.Services.Customers;
using Bank.Web.ViewModels.Customers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace Bank.Web.Test.Controllers
{
    public class CustomerControllerTest
    {
        private Mock<ICustomerService> _customerService;

        public CustomerControllerTest()
        {
            _customerService = new Mock<ICustomerService>();
        }

        [Fact]
        public async Task CustomerControllerShouldReturnCustomerIndexView()
        {
            //Arrange
            var customerDetailsViewModel = new CustomerDetailsViewModel
            {
                CustomerId = 1,
            };

            _customerService.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(customerDetailsViewModel);
            var sut = new CustomerController(_customerService.Object);

            //Act
            var actual = await sut.CustomerDetails(customerDetailsViewModel.CustomerId).ConfigureAwait(false) as ViewResult;

            //Assert
            actual.ShouldNotBeNull();
            actual.Model.ShouldBeEquivalentTo(customerDetailsViewModel);
        }

        [Fact]
        public async Task CustomerControllerShouldReturnErrorIfInvalidId()
        {
            //Arrange
            _customerService.Setup(x => x.GetByIdAsync(It.IsAny<int>()));
            var sut = new CustomerController(_customerService.Object);

            //Act
            var actual = await sut.CustomerDetails(1).ConfigureAwait(false) as ViewResult;

            //Assert
            actual.ShouldNotBeNull();
            actual.ViewName.ShouldBeEquivalentTo("_Error");
        }
    }
}
