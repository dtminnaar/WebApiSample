using Microsoft.AspNetCore.Mvc;
using Sample.DTO;
using Sample.Interfaces;

namespace Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SettlementController : ControllerBase
    {
        private readonly ISettlementService _service;

        public SettlementController(ISettlementService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("reserve")]
        [Consumes("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(EmptyResult), 400)]
        [ProducesResponseType(typeof(EmptyResult), 409)]
        public ActionResult<SettlementResult> Reserve([FromBody] SettlementRequest request)
        {
            if (string.IsNullOrEmpty(request?.Name) || string.IsNullOrEmpty(request?.BookingTime))
            {
                return BadRequest("");
            }
            if (!TimeOnly.TryParseExact(request.BookingTime, "HH:mm", out var time))
            {
                return BadRequest("");
            }
            if (!_service.IsBusinessHours(time))
            {
                return BadRequest("");
            }

            var settlement = _service.StoreSettlement(time, request.Name);
            if (settlement == null)
            {
                return Conflict("");
            }

            return Ok(new SettlementResult
            {
                BookingId = settlement.BookingId,
            });
        }
    }
}