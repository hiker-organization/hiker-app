using App_Hiker.Model.Auth.Request;
using App_Hiker.Model.Auth.Response;

using Newtonsoft.Json;

namespace App_Hiker.Service.Auth
{
    public class Auth : Api
    {
        public Auth() : base()
        {
            //
        }

        public async Task<Model.Auth.Response.LoginUser> Login(Model.Auth.Request.LoginUser payload)
        {
            string request_json = JsonConvert.SerializeObject(payload);

            string response_json = await Api.PostData("/auth/login", request_json);

            return JsonConvert.DeserializeObject<Model.Auth.Response.LoginUser>(response_json) ?? new Model.Auth.Response.LoginUser();
        }
    }
}
