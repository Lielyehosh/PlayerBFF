using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using BFF.Service.Extensions;
using BFF.Service.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace BFF.Service.Controllers
{
    [DisableCors]
    [Route("api/settings")]
    [ApiController]
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ILogger<SettingsController> _logger;
        private readonly IConfiguration _config;


        public SettingsController(
            ILogger<SettingsController> logger, 
            IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        
    
        [HttpPost]
        [Route("edit")]
        public Task<IActionResult> EditSettingsAsync([FromBody] EditSettingsRequest body, CancellationToken ct)
        {
            _logger.LogDebug("Receive edit settings request - site={Title} user-{User}", body.Title, HttpContext.GetUserId());
            return Task.FromResult<IActionResult>(Ok());
        }
    }
}