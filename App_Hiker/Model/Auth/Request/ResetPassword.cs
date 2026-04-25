namespace App_Hiker.Model.Auth.Request
{
    public class ResetPassword
    {
        public string token { get; set; } = String.Empty;

        public string senha { get; set; } = String.Empty;
    }
}
