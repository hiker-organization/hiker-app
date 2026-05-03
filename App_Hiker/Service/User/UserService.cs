using App_Hiker.Model.Api;
using App_Hiker.Model.User.Request;
using App_Hiker.Model.User.Response;

using Newtonsoft.Json;

namespace App_Hiker.Service.User
{
    public static class UserService
    {
        public static async Task<DataResponse<CreateUserResponse>> Create(CreateUserRequest payload)
        {
            string request_json = JsonConvert.SerializeObject(payload);

            string response_json = await ApiService.PostData("/user", request_json);

            return JsonConvert.DeserializeObject<DataResponse<CreateUserResponse>>(response_json) ?? new DataResponse<CreateUserResponse>();
        }
    }
}
