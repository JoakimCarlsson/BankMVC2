using AutoMapper;
using Bank.Data.Models;
using Bank.Web.ViewModels.Statistics;

namespace Bank.Web.Mapping
{
    public class DispositionProfiles : Profile
    {
        public DispositionProfiles()
        {
            CreateMap<Disposition, TopCustomerViewModel>()
                .ForMember(i => i.FirstName, opt => opt.MapFrom(c => c.Customer.Givenname))
                .ForMember(i => i.LastName, opt => opt.MapFrom(i => i.Customer.Surname))
                .ForMember(i => i.TotalBalance, opt => opt.MapFrom(i => i.Account.Balance));
        }
    }
}