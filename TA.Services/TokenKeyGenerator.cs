using System;
using System.Linq;
using TA.Contracts;
using WebToolkit.Contracts.Providers;

namespace TA.Services
{
    public class TokenKeyGenerator : ITokenKeyGenerator
    {
        private readonly ICryptographyProvider _cryptographyProvider;

        public string GenerateKey(HashAlgorithm hashAlgorithm, Guid? gUid = null)
        {
            if(!gUid.HasValue)
                gUid = Guid.NewGuid();

            var hashedBytes = _cryptographyProvider.HashBytes(hashAlgorithm, gUid.Value.ToByteArray());

            return BitConverter.ToString(hashedBytes.ToArray()).Replace("-", "");
        }

        public TokenKeyGenerator(ICryptographyProvider cryptographyProvider)
        {
            _cryptographyProvider = cryptographyProvider;
        }
    }
}