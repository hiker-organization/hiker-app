using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App_Hiker.Model.Api
{
    public class StringOrArrayConverter : JsonConverter<string>
    {
        public override string ReadJson(JsonReader reader, Type objectType, string? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                JArray array = JArray.Load(reader);
                return string.Join(", ", array.Select(t => t.ToString()));
            }

            return reader.Value?.ToString() ?? string.Empty;
        }

        public override void WriteJson(JsonWriter writer, string? value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}
