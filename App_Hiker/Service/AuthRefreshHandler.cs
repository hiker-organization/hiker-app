using System.Net;
using System.Net.Http.Headers;
using System.Text;

using App_Hiker.Model.Auth.Request;
using App_Hiker.Model.Auth.Response;

using Newtonsoft.Json;

namespace App_Hiker.Service
{
    internal class AuthRefreshHandler : DelegatingHandler
    {
        private readonly HttpClient _authClient;
        private readonly SemaphoreSlim _refreshLock = new SemaphoreSlim(1, 1);

        internal static event Action? SessionExpired;

        internal AuthRefreshHandler(HttpClient authClient) : base(new HttpClientHandler())
        {
            _authClient = authClient;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Adicionar access token ao header antes de enviar
            string? accessToken = await SecureStorage.Default.GetAsync("access_token");
            if (!string.IsNullOrEmpty(accessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            // Capturar body antes do primeiro envio (HttpRequestMessage não pode ser reenviado)
            byte[]? requestBody = null;
            string? contentType = null;
            if (request.Content != null)
            {
                requestBody = await request.Content.ReadAsByteArrayAsync(cancellationToken);
                contentType = request.Content.Headers.ContentType?.ToString();
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode != HttpStatusCode.Unauthorized)
                return response;

            // 401 recebido — tentar renovar token
            string? refreshToken = await SecureStorage.Default.GetAsync("refresh_token");
            if (string.IsNullOrEmpty(refreshToken))
            {
                SessionExpired?.Invoke();
                return response;
            }

            await _refreshLock.WaitAsync(cancellationToken);
            try
            {
                // Verificar se outra thread já renovou o token enquanto esperávamos
                string? freshAccess = await SecureStorage.Default.GetAsync("access_token");
                string? usedAccess = request.Headers.Authorization?.Parameter;

                bool alreadyRefreshed = freshAccess != null && freshAccess != usedAccess;
                if (!alreadyRefreshed)
                {
                    string? currentRefresh = await SecureStorage.Default.GetAsync("refresh_token");
                    bool refreshed = await TryRefreshAsync(currentRefresh ?? string.Empty, cancellationToken);
                    if (!refreshed)
                    {
                        SecureStorage.Default.Remove("access_token");
                        SecureStorage.Default.Remove("refresh_token");
                        SessionExpired?.Invoke();
                        return response;
                    }
                    freshAccess = await SecureStorage.Default.GetAsync("access_token");
                }

                // Recriar a requisição com o novo token (HttpRequestMessage não é reutilizável)
                using var retry = CloneRequest(request, requestBody, contentType, freshAccess ?? string.Empty);
                return await base.SendAsync(retry, cancellationToken);
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        private async Task<bool> TryRefreshAsync(string refreshToken, CancellationToken cancellationToken)
        {
            try
            {
                string json = JsonConvert.SerializeObject(new RefreshTokenRequest { refresh_token = refreshToken });
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _authClient.PostAsync("/auth/refresh", content, cancellationToken);
                if (!response.IsSuccessStatusCode) return false;

                string responseJson = await response.Content.ReadAsStringAsync(cancellationToken);
                LoginResponse? loginResponse = JsonConvert.DeserializeObject<LoginResponse>(responseJson);

                if (loginResponse == null || string.IsNullOrEmpty(loginResponse.access_token))
                    return false;

                await SecureStorage.Default.SetAsync("access_token", loginResponse.access_token);
                await SecureStorage.Default.SetAsync("refresh_token", loginResponse.refresh_token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static HttpRequestMessage CloneRequest(
            HttpRequestMessage original, byte[]? body, string? contentType, string bearerToken)
        {
            var clone = new HttpRequestMessage(original.Method, original.RequestUri);

            foreach (var header in original.Headers)
            {
                if (!header.Key.Equals("Authorization", StringComparison.OrdinalIgnoreCase))
                    clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            clone.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            if (body != null && body.Length > 0)
            {
                clone.Content = new ByteArrayContent(body);
                if (!string.IsNullOrEmpty(contentType))
                    clone.Content.Headers.TryAddWithoutValidation("Content-Type", contentType);
            }

            return clone;
        }
    }
}
