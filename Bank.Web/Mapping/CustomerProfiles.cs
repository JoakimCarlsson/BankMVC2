using AutoMapper;
using Bank.Data.Models;
using Bank.Web.ViewModels.Customers;

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
        }
    }
}