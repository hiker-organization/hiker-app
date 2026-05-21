namespace App_Hiker.Model.Auth.Exception
{
    public class AuthenticationFailedException: System.Exception
    {
        public AuthenticationFailedException() { }

        public AuthenticationFailedException(string message) : base(message) { }

        public AuthenticationFailedException(string message, System.Exception inner) : base(message, inner) { }
    }
}
