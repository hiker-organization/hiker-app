using System.Text;

using System.Net.Http;
using System.Net.Http.Headers;

using System.Diagnostics;

namespace App_Hiker.Service
{
    internal abstract class ApiService
    {
        private static HttpClient? connection = null;

        private static async Task CreateConnection()
        {
            if (connection == null)
            {
                connection = new HttpClient();

                connection.BaseAddress = new Uri("http://localhost:3000");
            }

            string auth_token = await SecureStorage.GetAsync("token") ?? "";

            if (auth_token != String.Empty)
            {
                connection.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth_token);
            }
        }

        internal static async Task<string> GetData(string endpoint)
        {
            await CreateConnection();

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

        internal static async Task<string> PostData(string endpoint, string json_data)
        {
            await CreateConnection();

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

        internal static async Task<string> PutData(string endpoint, string json_data)
        {
            await CreateConnection();

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

        internal static async Task<string> PatchData(string endpoint, string json_data)
        {
            await CreateConnection();

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

        internal static async Task<string> DeleteData(string endpoint)
        {
            await CreateConnection();

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
