using DPK.SCF.Grafana.Contracts;
using DPK.SCF.Grafana.Models;
using Microsoft.AspNetCore.Mvc;

namespace DPK.SCF.TestGrafana.Controllers
{
    [ApiController]
    [Route("api/ops/health")]

    /// <summary>
    /// سرویس مشاهده وضعیت سلامت سرویس جاری را در اختیار سامانه‌های مانیتورینگ قرار می‌دهد.
    /// </summary>
    public class OpsHealthController : ControllerBase
    {
        private readonly IHealthAggregatorService _healthAggregatorService;

        public OpsHealthController(IHealthAggregatorService healthAggregatorService)
        {
            _healthAggregatorService = healthAggregatorService;
        }

        [HttpGet]
        public async Task<ActionResult<HealthReportDto>> Get([FromQuery] string? serviceId)
        {
            var result = await _healthAggregatorService.GetHealthAsync(serviceId);
            return Ok(result);
        }
    }
}
