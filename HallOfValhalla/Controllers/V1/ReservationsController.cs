using System;
using System.Threading.Tasks;
using HallOfValhalla.Contracts.V1;
using HallOfValhalla.Contracts.V1.Requests;
using HallOfValhalla.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HallOfValhalla.Controllers.V1
{
    public class ReservationsController : ControllerBase
    {
        private readonly IConventionService _conventionService;

        public ReservationsController(IConventionService conventionService)
        {
            _conventionService = conventionService;
        }

        [Authorize]
        [HttpPost(ApiRoutes.Reservations.Create)]
        public async Task<IActionResult> Create([FromRoute] Guid talkId, [FromBody] CreateReservationRequest request)
        {
            bool added = await _conventionService.ReserveTalkAsync(talkId, request.UserId);

            if (!added)
            {
                return NotFound();
            }

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            return Created(baseUrl, added);
        }
    }
}
