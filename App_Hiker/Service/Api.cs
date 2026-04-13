using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;

namespace App_Hiker.Service
{
    internal abstract class Api
    {
        private static HttpClient? connection = null;

        public Api()
        {
            CreateConnection();
        }

        private static async void CreateConnection()
        {
            connection = new HttpClient();

            connection.BaseAddress = new Uri(""); // A ser definido.

            string auth_token = await SecureStorage.GetAsync("token") ?? "";

            connection.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth_token);
        }

        protected static async Task<string> GetData(string endpoint)
        {
            string api_response_json = "";

            if (connection != null)
            {
                HttpResponseMessage api_response = await connection.GetAsync(endpoint);

                api_response_json = await api_response.Content.ReadAsStringAsync();

                App.ShowInDebugConsole(api_response_json);

                // Dispara uma exceção, caso ocorra um erro.

                api_response.EnsureSuccessStatusCode();
            }

            return api_response_json;
        }

        protected static async Task<string> PostData(string endpoint, string json_data)
        {
            string api_response_json = "";

            if (connection != null)
            {
                HttpResponseMessage api_response = await connection.PostAsync(endpoint, new StringContent(json_data, Encoding.UTF8, "application/json"));

                api_response_json = await api_response.Content.ReadAsStringAsync();

                App.ShowInDebugConsole(api_response_json);

                // Dispara uma exceção, caso ocorra um erro.

                api_response.EnsureSuccessStatusCode();
            }

            return api_response_json;
        }

        protected static async Task<string> PutData(string endpoint, string json_data)
        {
            string api_response_json = "";

            if (connection != null)
            {
                HttpResponseMessage api_response = await connection.PutAsync(endpoint, new StringContent(json_data, Encoding.UTF8, "application/json"));

                api_response_json = await api_response.Content.ReadAsStringAsync();

                App.ShowInDebugConsole(api_response_json);

                // Dispara uma exceção, caso ocorra um erro.

                api_response.EnsureSuccessStatusCode();
            }

            return api_response_json;
        }

        protected static async Task<string> PatchData(string endpoint, string json_data)
        {
            string api_response_json = "";

            if (connection != null)
            {
                HttpResponseMessage api_response = await connection.PatchAsync(endpoint, new StringContent(json_data, Encoding.UTF8, "application/json"));

                api_response_json = await api_response.Content.ReadAsStringAsync();

                App.ShowInDebugConsole(api_response_json);

                // Dispara uma exceção, caso ocorra um erro.

                api_response.EnsureSuccessStatusCode();
            }

            return api_response_json;
        }

        protected static async Task<string> DeleteData(string endpoint)
        {
            string api_response_json = "";

            if (connection != null)
            {
                HttpResponseMessage api_response = await connection.DeleteAsync(endpoint);

                api_response_json = await api_response.Content.ReadAsStringAsync();

                App.ShowInDebugConsole(api_response_json);

                // Dispara uma exceção, caso ocorra um erro.

                api_response.EnsureSuccessStatusCode();
            }

            return api_response_json;
        }
    }
}
