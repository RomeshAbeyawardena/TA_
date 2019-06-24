using System;
using TA.Domains.Enumerations;

namespace TA.Contracts
{
    public interface ITokenKeyGenerator
    {
        string GenerateKey(HashAlgorithm hashAlgorithm, Guid? gUid = null);
    }
}