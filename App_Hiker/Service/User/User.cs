using Newtonsoft.Json;

namespace App_Hiker.Service.User
{
    public class User : Api
    {
        public User() : base()
        {
            // Execução do construtor da classe pai.
        }

        public async Task<Model.DataResponse<Model.User.Response.CreateUser>> Create(Model.User.Request.CreateUser payload)
        {
            string request_json = JsonConvert.SerializeObject(payload);

            string response_json = await Api.PostData("/user", request_json);

            return JsonConvert.DeserializeObject<Model.DataResponse<Model.User.Response.CreateUser>>(response_json) ?? new Model.DataResponse<Model.User.Response.CreateUser>();
        }
    }
}
