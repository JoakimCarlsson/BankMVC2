using AutoMapper;
using Bank.Core.Model;
using Bank.Core.ViewModels.CustomerVm;

namespace Bank.Core.Mapping
{
    class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Customer, CustomerIndexViewModel>().ReverseMap();
        }
    }
}
