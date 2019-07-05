using System.ComponentModel.DataAnnotations;

namespace TA.Domains.Contracts
{
    public interface IIdentity<TIdentity>
    {
        TIdentity Id { get; set; }
    }

    public interface IIdentity : IIdentity<int>
    {

    }
}