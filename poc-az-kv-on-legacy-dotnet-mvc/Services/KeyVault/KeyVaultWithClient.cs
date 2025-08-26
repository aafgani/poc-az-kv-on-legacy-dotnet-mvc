using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System.Threading.Tasks;

namespace poc_az_kv_on_legacy_dotnet_mvc.Services.KeyVault
{
    public class KeyVaultWithClient : IKeyVaultService
    {
        private readonly string _vaultBaseUrl;

        public KeyVaultWithClient(string vaultBaseUrl)
        {
            _vaultBaseUrl = vaultBaseUrl;
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            // Use this line to authenticate using Managed Identity (if available, e.g., in Azure VM, App Service, or Function)
            var azureServiceTokenProvider = new AzureServiceTokenProvider();

            // Use this line if you want to authenticate using Azure CLI logged-in user during development
            //var azureServiceTokenProvider = new AzureServiceTokenProvider(connectionString: "RunAs=Developer; DeveloperTool=AzureCli"); 
            
            var keyVaultClient = new KeyVaultClient(
                new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            var secret = await keyVaultClient.GetSecretAsync($"{_vaultBaseUrl}/secrets/{secretName}").ConfigureAwait(false);

            return secret.Value;
        }
    }
}