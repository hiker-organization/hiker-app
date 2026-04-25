using App_Hiker.Model;

using Newtonsoft.Json;

namespace App_Hiker.Service.Auth
{
    public class Auth : Api
    {
        public Auth() : base()
        {
            // Execução do construtor da classe pai.
        }

        public async Task<Model.Auth.Response.LoginUser> Login(Model.Auth.Request.LoginUser payload)
        {
            string request_json = JsonConvert.SerializeObject(payload);

            string response_json = await Api.PostData("/auth/login", request_json);

            return JsonConvert.DeserializeObject<Model.Auth.Response.LoginUser>(response_json) ?? new Model.Auth.Response.LoginUser();
        }

        public async Task<MessageResponse> ForgotPassword(string email)
        {
            string request_json = JsonConvert.SerializeObject(new { email });

            string response_json = await Api.PostData("/auth/forgot-password", request_json);

            return JsonConvert.DeserializeObject<MessageResponse>(response_json) ?? new MessageResponse();
        }

        public async Task<MessageResponse> ResetPassword(Model.Auth.Request.ResetPassword payload)
        {
            string request_json = JsonConvert.SerializeObject(payload);

            string response_json = await Api.PostData("/auth/reset-password", request_json);

            return JsonConvert.DeserializeObject<MessageResponse>(response_json) ?? new MessageResponse();
        }
    }
}
