using Newtonsoft.Json;

namespace App_Hiker.Model.Api
{
    public class ApiHttpException : HttpRequestException
    {
        public string ResponseBody { get; }
        public string ApiMessage { get; }

        public ApiHttpException(string responseBody) : base()
        {
            ResponseBody = responseBody;

            try
            {
                MessageResponse? parsed = JsonConvert.DeserializeObject<MessageResponse>(responseBody);
                ApiMessage = !string.IsNullOrWhiteSpace(parsed?.message) ? parsed.message : "Ocorreu um erro.";
            }
            catch
            {
                ApiMessage = "Ocorreu um erro.";
            }
        }
    }
}
