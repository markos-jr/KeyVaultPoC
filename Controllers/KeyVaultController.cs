using Microsoft.AspNetCore.Mvc;
using KeyVaultPoC.Services;

namespace KeyVaultPoC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeyVaultController : ControllerBase
    {
        private readonly SecretService _secretService;

        public KeyVaultController(SecretService secretService)
        {
            _secretService = secretService;
        }

        [HttpGet("get-credentials")]
        public async Task<IActionResult> GetCredentials()
        {
            string user = await _secretService.GetSecretAsync("ApiUser");
            string password = await _secretService.GetSecretAsync("ApiUser");

            return Ok(new { User = user, Password = password });
        }
    }
}
