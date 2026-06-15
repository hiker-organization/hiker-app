using System.Text;

using App_Hiker.Model.Api;

namespace App_Hiker.Service
{
    internal abstract class ApiService
    {
        private static readonly HttpClient _authClient;
        private static readonly HttpClient _client;

        static ApiService()
        {
            const string baseUrl = "https://hikerapi.azurewebsites.net";

            _authClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(15)
            };

            _client = new HttpClient(new AuthRefreshHandler(_authClient))
            {
                BaseAddress = new Uri(baseUrl),
                Timeout = TimeSpan.FromSeconds(15)
            };
        }

        // Para endpoints de auth (login, refresh, logout, forgot-password) — sem retry de 401
        internal static async Task<string> PostAuth(string endpoint, string json_data)
        {
            HttpResponseMessage response = await _authClient.PostAsync(
                endpoint,
                new StringContent(json_data, Encoding.UTF8, "application/json"));

            string api_response_json = await response.Content.ReadAsStringAsync();
            App.ShowInDebugConsole(api_response_json);

            if (!response.IsSuccessStatusCode)
                throw new ApiHttpException(api_response_json);

            return api_response_json;
        }

        // Para endpoints protegidos — passa pelo AuthRefreshHandler
        internal static async Task<string> GetData(string endpoint)
        {
            HttpResponseMessage response = await _client.GetAsync(endpoint);
            string api_response_json = await response.Content.ReadAsStringAsync();
            App.ShowInDebugConsole(api_response_json);

            if (!response.IsSuccessStatusCode)
                throw new ApiHttpException(api_response_json);

            return api_response_json;
        }

        internal static async Task<string> PostData(string endpoint, string json_data)
        {
            HttpResponseMessage response = await _client.PostAsync(
                endpoint,
                new StringContent(json_data, Encoding.UTF8, "application/json"));

            string api_response_json = await response.Content.ReadAsStringAsync();
            App.ShowInDebugConsole(api_response_json);

            if (!response.IsSuccessStatusCode)
                throw new ApiHttpException(api_response_json);

            return api_response_json;
        }

        internal static async Task<string> PostMultipart(string endpoint, MultipartFormDataContent form_data)
        {
            HttpResponseMessage response = await _client.PostAsync(endpoint, form_data);
            string api_response_json = await response.Content.ReadAsStringAsync();
            App.ShowInDebugConsole(api_response_json);

            if (!response.IsSuccessStatusCode)
                throw new ApiHttpException(api_response_json);

            return api_response_json;
        }

        internal static async Task<string> PutData(string endpoint, string json_data)
        {
            HttpResponseMessage response = await _client.PutAsync(
                endpoint,
                new StringContent(json_data, Encoding.UTF8, "application/json"));

            string api_response_json = await response.Content.ReadAsStringAsync();
            App.ShowInDebugConsole(api_response_json);

            if (!response.IsSuccessStatusCode)
                throw new ApiHttpException(api_response_json);

            return api_response_json;
        }

        internal static async Task<string> PatchData(string endpoint, string json_data)
        {
            HttpResponseMessage response = await _client.PatchAsync(
                endpoint,
                new StringContent(json_data, Encoding.UTF8, "application/json"));

            string api_response_json = await response.Content.ReadAsStringAsync();
            App.ShowInDebugConsole(api_response_json);

            if (!response.IsSuccessStatusCode)
                throw new ApiHttpException(api_response_json);

            return api_response_json;
        }

        internal static async Task<string> PatchMultipart(string endpoint, MultipartFormDataContent form_data)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, endpoint) { Content = form_data };
            HttpResponseMessage response = await _client.SendAsync(request);
            string api_response_json = await response.Content.ReadAsStringAsync();
            App.ShowInDebugConsole(api_response_json);

            if (!response.IsSuccessStatusCode)
                throw new ApiHttpException(api_response_json);

            return api_response_json;
        }

        internal static async Task<string> DeleteData(string endpoint)
        {
            HttpResponseMessage response = await _client.DeleteAsync(endpoint);
            string api_response_json = await response.Content.ReadAsStringAsync();
            App.ShowInDebugConsole(api_response_json);

            if (!response.IsSuccessStatusCode)
                throw new ApiHttpException(api_response_json);

            return api_response_json;
        }
    }
}
