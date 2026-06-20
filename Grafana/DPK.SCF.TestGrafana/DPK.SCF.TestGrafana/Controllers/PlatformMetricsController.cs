using DPK.SCF.Grafana.Contracts;
using DPK.SCF.Grafana.Models;
using Microsoft.AspNetCore.Mvc;

namespace DPK.SCF.TestGrafana.Controllers
{
    [ApiController]
    [Route("api/platform/metrics")]
    public class PlatformMetricsController : ControllerBase
    {
        private readonly IMetricsCollectorService _metricsCollectorService;

        public PlatformMetricsController(IMetricsCollectorService metricsCollectorService)
        {
            _metricsCollectorService = metricsCollectorService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMetricRequest request)
        {
            await _metricsCollectorService.CollectAsync(request);

            return Ok(new
            {
                status = "accepted"
            });
        }
    }
}
