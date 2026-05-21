namespace App_Hiker.Model.User.Exception
{
    public class InvalidUsernameException : System.Exception
    {
        public InvalidUsernameException() { }

        public InvalidUsernameException(string message) : base(message) { }

        public InvalidUsernameException(string message, System.Exception inner) : base(message, inner) { }
    }
}
