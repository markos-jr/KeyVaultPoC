using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace KeyVaultPoC.Services
{
    public class SecretService
    {
        private readonly SecretClient _secretClient;

        public SecretService(IConfiguration configuration)
        {
            string vaultUri = configuration["AzureKeyVault:VaultUri"];
            _secretClient = new SecretClient(new Uri(vaultUri), new DefaultAzureCredential());
        }

        public async Task<string> GetSecretAsync(string secretName)
        {
            KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
            return secret.Value;
        }
    }
}
