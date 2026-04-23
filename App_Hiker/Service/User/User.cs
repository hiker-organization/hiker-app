using App_Hiker.Model.User.Request;
using App_Hiker.Model.User.Response;

using Newtonsoft.Json;

namespace App_Hiker.Service.User
{
    public class User : Api
    {
        public User() : base()
        {
            //
        }

        public async Task<Model.Api<Model.User.Response.CreateUser>> Create(Model.User.Request.CreateUser payload)
        {
            string request_json = JsonConvert.SerializeObject(payload);

            string response_json = await Api.PostData("/user", request_json);

            return JsonConvert.DeserializeObject<Model.Api<Model.User.Response.CreateUser>>(response_json) ?? new Model.Api<Model.User.Response.CreateUser>();
        }
    }
}
