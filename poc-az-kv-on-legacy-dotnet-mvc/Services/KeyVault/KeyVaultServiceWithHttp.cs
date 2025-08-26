using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace poc_az_kv_on_legacy_dotnet_mvc.Services.KeyVault
{
    public class KeyVaultServiceWithHttp : IKeyVaultService
    {
        private readonly string _tenantId;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _vaultBaseUrl;

        private string _cachedToken;
        private DateTimeOffset _tokenExpiresOn;

        public KeyVaultServiceWithHttp(string tenantId, string clientId, string clientSecret, string vaultBaseUrl)
        {
            _tenantId = tenantId;
            _clientId = clientId;
            _clientSecret = clientSecret;
            _vaultBaseUrl = vaultBaseUrl;
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            var token = await GetAccessTokenAsync();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync($"{_vaultBaseUrl}/secrets/{secretName}?api-version=7.3");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                dynamic secret = JsonConvert.DeserializeObject(json);

                return secret.value;
            }
        }

        private async Task<string> GetAccessTokenAsync()
        {
            // Return cached token if still valid
            if (!string.IsNullOrEmpty(_cachedToken) && _tokenExpiresOn > DateTimeOffset.UtcNow.AddMinutes(5))
            {
                return _cachedToken;
            }
            
            using (var client = new HttpClient())
            {
                var body = new FormUrlEncodedContent(new[]
               {
                    new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    new KeyValuePair<string, string>("client_id", _clientId),
                    new KeyValuePair<string, string>("client_secret", _clientSecret),
                    new KeyValuePair<string, string>("resource", "https://vault.azure.net")
                });

                var response = await client.PostAsync($"https://login.microsoftonline.com/{_tenantId}/oauth2/token", body);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                dynamic tokenResult = JsonConvert.DeserializeObject(json);

                _cachedToken = tokenResult.access_token;
                _tokenExpiresOn = DateTimeOffset.UtcNow.AddSeconds((int)tokenResult.expires_in);
            }

            return _cachedToken;
        }
    }
}