using System.Threading.Tasks;

namespace poc_az_kv_on_legacy_dotnet_mvc.Services.KeyVault
{
    public interface IKeyVaultService
    {
        Task<string> GetSecretAsync(string secretName);
    }
}
