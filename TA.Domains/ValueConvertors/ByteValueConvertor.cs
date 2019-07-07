using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using WebToolkit.Contracts.Providers;

namespace TA.Domains.ValueConvertors
{
    public class ByteValueConvertor<TSource, TDestination> : IValueResolver<TSource, TDestination,IEnumerable<byte>>
    {
        private readonly IEncodingProvider _encodingProvider;

        public ByteValueConvertor(IEncodingProvider encodingProvider)
        {
            _encodingProvider = encodingProvider;
        }

        public IEnumerable<byte> Resolve(TSource source, TDestination destination, IEnumerable<byte> destMember, ResolutionContext context)
        {
            throw new System.NotImplementedException();
        }
    }
}