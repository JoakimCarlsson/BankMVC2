using System.Linq;
using AutoMapper;
using Bank.Data.Models;
using Bank.Web.ViewModels.Customers;
using Bank.Web.ViewModels.Statistics;

namespace Bank.Web.Mapping
{
    public class CustomerProfiles : Profile 
    {
        public CustomerProfiles()
        {
            CreateMap<Customer, CustomerDetailsViewModel>().ReverseMap();
            CreateMap<Customer, CustomerSearchViewModel>().ReverseMap();
            
            CreateMap<CustomerRegisterViewModel, Customer>()
                .ForMember(i => i.Gender, opt => opt.MapFrom(i => i.SelectedGender)); 

            CreateMap<CustomerEditViewModel, Customer>()
                .ForMember(i => i.Gender, opt => opt.MapFrom(i => i.SelectedGender));
            
            CreateMap<Customer, CustomerEditViewModel>()
                .ForMember(i => i.SelectedGender, opt => opt.MapFrom(i => i.Gender));
            
            CreateMap<Customer, TopCustomerViewModel>()
                .ForMember(i => i.FirstName, opt => opt.MapFrom(c => c.Givenname))
                .ForMember(i => i.LastName, opt => opt.MapFrom(i => i.Surname))
                .ForMember(i => i.TotalBalance, opt => opt.MapFrom(i => i.Dispositions.Sum(d => d.Account.Balance)));
        }
    }
}