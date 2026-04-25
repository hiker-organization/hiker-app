namespace App_Hiker.Model.Auth.Response
{
    public class LoginUser : MessageResponse
    {
        public string access_token { get; set; } = String.Empty;
    }
}
