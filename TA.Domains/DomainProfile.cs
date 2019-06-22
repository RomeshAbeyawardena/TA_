using AutoMapper;
using TA.Domains.Models;

namespace TA.Domains
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Asset, Dtos.Asset>().ReverseMap();
            CreateMap<Site, Dtos.Site>().ReverseMap();
        }
    }
}