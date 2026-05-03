namespace App_Hiker.Model.Auth.Request
{
    public class LoginRequest
    {
        public string email { get; set; } = String.Empty;

        public string password { get; set; } = String.Empty;
    }
}
