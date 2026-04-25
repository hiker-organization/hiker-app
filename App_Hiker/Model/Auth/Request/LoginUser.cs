using System;
using System.Collections.Generic;
using System.Text;

namespace App_Hiker.Model.Auth.Request
{
    public class LoginUser
    {
        public string email { get; set; } = String.Empty;

        public string password { get; set; } = String.Empty;
    }
}
