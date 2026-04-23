using App_Hiker.Model;

using Newtonsoft.Json;

namespace App_Hiker.Service.User
{
    public class User : Api
    {
        public User() : base()
        {
            //
        }

        public async Task<Model.Api<Model.User>> Create(Model.User payload)
        {
            string request_json = JsonConvert.SerializeObject(payload);

            Api.ShowResponseInConsole(request_json);

            string response_json = await Api.PostData("/user", request_json);

            Api.ShowResponseInConsole(response_json);

            return JsonConvert.DeserializeObject<Model.Api<Model.User>>(response_json) ?? new Model.Api<Model.User>();
        }
    }
}
