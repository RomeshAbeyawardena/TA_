using AutoMapper;
using Newtonsoft.Json.Linq;
using TA.Domains.Dtos;

namespace TA.App.ViewModels
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<AssetViewModel, Asset>()
                .ForMember(member => member.Attributes, 
                    options => options.MapFrom(member =>JObject.Parse(member.JsonAttributes)));

            CreateMap<SiteViewModel, Site>()
                .ForMember(member => member.Attributes, 
                    options => options.MapFrom(member =>JObject.Parse(member.JsonAttributes)));
        }
    }
}