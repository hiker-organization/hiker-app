using App_Hiker.Model.Api;

namespace App_Hiker.Model.Auth.Response
{
    public class LoginResponse : MessageResponse
    {
        public string access_token { get; set; } = string.Empty;
        public string refresh_token { get; set; } = string.Empty;
    }
}
