using AutoMapper;
using Newtonsoft.Json.Linq;
using TA.Domains.Models;
using WebToolkit.Common.Extensions;
using Encoding = System.Text.Encoding;

namespace TA.App.ViewModels
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<AssetViewModel, Asset>()
                .ForMember(member => member.Attributes, 
                    options => options
                            .MapFrom(member =>JObject.FromObject(member.Attributes)));

            CreateMap<SiteViewModel, Site>()
                .ForMember(member => member.Attributes, 
                    options => options
                        .MapFrom(member =>JObject.FromObject(member.Attributes)));

            CreateMap<UserViewModel, User>()
                .ForMember(member => member.EmailAddress,
                options => options
                    .MapFrom(source => source.EmailAddress.ToByteEnumerable(Encoding.ASCII)))
                .ForMember(member => member.Password, options => options
                    .MapFrom(source => source.Password.ToByteEnumerable(Encoding.ASCII)));
        }
    }
}