using DPK.SCF.Grafana.Contracts;
using DPK.SCF.Grafana.Models;
using DPK.SCF.Grafana.Models.Constaints;
using Microsoft.AspNetCore.Mvc;

namespace DPK.SCF.TestGrafana.Controllers
{
    [ApiController]
    [Route("api/platform/health")]
    /// <summary>
    /// سرویس مشاهده وضعیت سلامت سرویس جاری را در اختیار سامانه‌های مانیتورینگ قرار می‌دهد.
    /// </summary>
    public class PlatformHealthController : ControllerBase
    {
        private readonly IHealthCheckEngine _healthCheckEngine;

        public PlatformHealthController(IHealthCheckEngine healthCheckEngine)
        {
            _healthCheckEngine = healthCheckEngine;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceHealthDto>> Get()
        {
            var result = await _healthCheckEngine.CheckAsync();

            if (result.Status == ServiceHealthStatus.Down)
                return StatusCode(StatusCodes.Status503ServiceUnavailable, result);

            return Ok(result);
        }
    }




}
