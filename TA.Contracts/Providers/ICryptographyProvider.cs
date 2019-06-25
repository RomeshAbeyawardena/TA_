using System.Collections.Generic;
using System.Text;
using TA.Domains.Enumerations;

namespace TA.Contracts.Providers
{
    public interface ICryptographyProvider
    {
        IEnumerable<byte> HashBytes(HashAlgorithm hashAlgorithm, IEnumerable<byte> sourceBytes);
        IEnumerable<byte> HashBytes(HashAlgorithm hashAlgorithm, string value, Encoding encoding);
    }
}