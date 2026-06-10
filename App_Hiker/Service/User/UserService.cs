using App_Hiker.Model.Api;
using App_Hiker.Model.User.Request;
using App_Hiker.Model.User.Response;

using Newtonsoft.Json;

namespace App_Hiker.Service.User
{
    public static class UserService
    {
        public static async Task<DataResponse<UserDataResponse>> GetByNick(string nick)
        {
            string endpoint = $"/user/{Uri.EscapeDataString(nick)}";
            string response_json = await ApiService.GetData(endpoint);

            return JsonConvert.DeserializeObject<DataResponse<UserDataResponse>>(response_json) ?? new DataResponse<UserDataResponse>();
        }

        public static async Task<DataResponse<CreateUserResponse>> Create(CreateUserRequest payload)
        {
            string request_json = JsonConvert.SerializeObject(payload);

            string response_json = await ApiService.PostData("/user", request_json);

            return JsonConvert.DeserializeObject<DataResponse<CreateUserResponse>>(response_json) ?? new DataResponse<CreateUserResponse>();
        }

        public static async Task<DataResponse<UpdateUserResponse>> Update(UpdateUserRequest payload)
        {
            string request_json = JsonConvert.SerializeObject(payload);

            string response_json = await ApiService.PatchData("/user/change-data", request_json);

            return JsonConvert.DeserializeObject<DataResponse<UpdateUserResponse>>(response_json) ?? new DataResponse<UpdateUserResponse>();
        }

        public static async Task<DataResponse<UpdateUserResponse>> UpdatePhoto(FileResult file)
        {
            using Stream stream = await file.OpenReadAsync();
            using StreamContent stream_content = new StreamContent(stream);

            stream_content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

            using MultipartFormDataContent form_data = new MultipartFormDataContent();
            form_data.Add(stream_content, "foto", file.FileName);

            string response_json = await ApiService.PatchMultipart("/user/change-data", form_data);

            return JsonConvert.DeserializeObject<DataResponse<UpdateUserResponse>>(response_json) ?? new DataResponse<UpdateUserResponse>();
        }
    }
}