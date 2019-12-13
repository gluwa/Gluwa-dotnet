using Nethereum.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Numerics;

namespace Gluwa.Utils
{
    internal static class Converter
    {
        public static JsonSerializerSettings Settings { get; private set; }

        static Converter()
        {
            Settings = new JsonSerializerSettings();
            ConfigureSettings(Settings);
        }

        public static void ConfigureSettings(JsonSerializerSettings settings)
        {
            settings.NullValueHandling = NullValueHandling.Ignore;
            settings.DateParseHandling = DateParseHandling.DateTimeOffset;
            settings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            settings.ContractResolver = new DefaultContractResolver();  // Pascal Casing

            settings.Converters.Add(new StringEnumConverter());
        }

        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj, Settings);
        }
    }
}