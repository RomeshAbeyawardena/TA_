using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TA.Contracts;
using TA.Contracts.Providers;
using WebToolkit.Common;
using HashAlgorithm = TA.Domains.Enumerations.HashAlgorithm;

namespace TA.Services.Providers
{
    public class CryptographyProvider : ICryptographyProvider
    {
        public IEnumerable<byte> HashBytes(HashAlgorithm hashAlgorithm, IEnumerable<byte> sourceBytes)
        {
            return _hashAlgorithmSwitch.Case(hashAlgorithm).Invoke(sourceBytes);
        }

        public IEnumerable<byte> HashBytes(HashAlgorithm hashAlgorithm, string value, Encoding encoding)
        {
            return HashBytes(hashAlgorithm, encoding.GetBytes(value));
        }

        public CryptographyProvider()
        {
            _hashAlgorithmSwitch = Switch<HashAlgorithm, Func<IEnumerable<byte>, IEnumerable<byte>>>
                .Create();

                _hashAlgorithmSwitch.CaseWhen(HashAlgorithm.Md5, b => {
                    using (var md5 = MD5.Create())
                    {
                        return md5.ComputeHash(b.ToArray());
                    }
                }).CaseWhen(HashAlgorithm.Sha512, b =>
                {
                    using (var sha512 = SHA512.Create())
                    {
                        return sha512.ComputeHash(b.ToArray());
                    }
                });
        }

        private readonly Switch<HashAlgorithm, Func<IEnumerable<byte>, IEnumerable<byte>>> _hashAlgorithmSwitch;
    }
}