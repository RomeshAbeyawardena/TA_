using Newtonsoft.Json.Linq;

namespace TA.Domains.Extensions
{
    public static class JObjectExtensions
    {
        public static JObject Append(this JObject jObject, string propertyName, object value)
        {
            var jTokenValue = JToken.FromObject(value);
            if (jObject.ContainsKey(propertyName))
            {
                jObject[propertyName] = jTokenValue;
                return jObject;
            }

            jObject.Add(propertyName, jTokenValue);
            return jObject;
        }
    }
}