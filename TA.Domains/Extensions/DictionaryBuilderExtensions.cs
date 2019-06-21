using Newtonsoft.Json.Linq;
using WebToolkit.Contracts.Builders;

namespace TA.Domains.Extensions
{
    public static class DictionaryBuilderExtensions
    {
        public static JObject ToJObject<TKey, TValue>(this IDictionaryBuilder<TKey, TValue> dictionaryBuilder)
        {
            var jObject = new JObject();
            foreach (var (key, value) in dictionaryBuilder)
            {
                jObject.Add(key.ToString(), JToken.FromObject(value));
            }

            return jObject;
        }
    }
}