using System;
using TA.Contracts.Providers;

namespace TA.Services.Providers
{
    public class DefaultValuesProvider<TModel> : IDefaultValueProvider<TModel>
    {
        public DefaultValuesProvider(Action<TModel> defaults)
        {
            Defaults = defaults;
        }

        public Action<TModel> Defaults { get; }
        public void Assign(TModel model)
        {
            Defaults(model);
        }
    }
}