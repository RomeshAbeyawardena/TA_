using System;

namespace TA.Contracts.Providers
{
    public interface IDefaultValueProvider<in TModel>
    {
        Action<TModel> Defaults { get; }
        void Assign(TModel model);
    }
}