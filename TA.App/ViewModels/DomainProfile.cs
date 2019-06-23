using AutoMapper;
using TA.Domains.Dtos;

namespace TA.App.ViewModels
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Asset, AssetViewModel>().ReverseMap();
            CreateMap<Site, SiteViewModel>().ReverseMap();
        }
    }
}