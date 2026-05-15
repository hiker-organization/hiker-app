using App_Hiker.Model.Api;
using App_Hiker.Model.Auth.Request;
using App_Hiker.Model.Auth.Response;

using App_Hiker.Model.User.Response;

using Newtonsoft.Json;

namespace App_Hiker.Service.Auth
{
    public static class AuthService
    {
        public static async Task<DataResponse<UserDataResponse>> Me()
        {
            string response_json = await ApiService.GetData("/user/me");

            return JsonConvert.DeserializeObject<DataResponse<UserDataResponse>>(response_json) ?? new DataResponse<UserDataResponse>();
        }

        public static async Task<LoginResponse> Login(LoginRequest payload)
        {
            string request_json = JsonConvert.SerializeObject(payload);

            string response_json = await ApiService.PostData("/auth/login", request_json);

            return JsonConvert.DeserializeObject<LoginResponse>(response_json) ?? new LoginResponse();
        }

        public static async Task<MessageResponse> ForgotPassword(string email)
        {
            string request_json = JsonConvert.SerializeObject(new { email });

            string response_json = await ApiService.PostData("/auth/forgot-password", request_json);

            return JsonConvert.DeserializeObject<MessageResponse>(response_json) ?? new MessageResponse();
        }

        public static async Task<MessageResponse> ResetPassword(ResetPasswordRequest payload)
        {
            string request_json = JsonConvert.SerializeObject(payload);

            string response_json = await ApiService.PostData("/auth/reset-password", request_json);

            return JsonConvert.DeserializeObject<MessageResponse>(response_json) ?? new MessageResponse();
        }
    }
}
