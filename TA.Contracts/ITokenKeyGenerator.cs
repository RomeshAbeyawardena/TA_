using System;
using WebToolkit.Contracts.Providers;

namespace TA.Contracts
{
    public interface ITokenKeyGenerator
    {
        string GenerateKey(HashAlgorithm hashAlgorithm, Guid? gUid = null);
    }
}