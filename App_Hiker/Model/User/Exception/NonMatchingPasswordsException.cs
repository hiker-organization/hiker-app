using System;

namespace App_Hiker.Model.User.Exception
{
    public class NonMatchingPasswordsException : System.Exception
    {
        public NonMatchingPasswordsException() {  }

        public NonMatchingPasswordsException(string message) : base(message) {  }

        public NonMatchingPasswordsException(string message, System.Exception inner) : base(message, inner) {  }
    }
}
