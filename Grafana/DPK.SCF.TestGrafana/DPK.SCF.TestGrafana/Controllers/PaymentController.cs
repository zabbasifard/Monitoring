using DPK.SCF.TestGrafana.Services;
using Microsoft.AspNetCore.Mvc;

namespace DPK.SCF.TestGrafana.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> Pay()
        {
            await _paymentService.PayAsync();

            return Ok();
        }
    }
}
