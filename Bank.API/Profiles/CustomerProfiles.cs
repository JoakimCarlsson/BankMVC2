using AutoMapper;
using Bank.API.ViewModels.Customer;
using Bank.Data.Models;

namespace Bank.API.Profiles
{
    public class CustomerProfiles : Profile
    {
        public CustomerProfiles()
        {
            CreateMap<Customer, CustomerDetailsViewModel>();
        }
    }
}