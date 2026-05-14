namespace App_Hiker.Model.Auth.Request
{
    public class ResetPasswordRequest
    {
        public string email { get; set; } = String.Empty;

        public string token { get; set; } = String.Empty;

        public string senha { get; set; } = String.Empty;
    }
}
