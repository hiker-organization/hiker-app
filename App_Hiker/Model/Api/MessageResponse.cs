using Newtonsoft.Json;

namespace App_Hiker.Model.Api
{
    public class MessageResponse
    {
        [JsonConverter(typeof(StringOrArrayConverter))]
        public string message { get; set; } = String.Empty;

        public int statusCode { get; set; } = 200;
    }
}
