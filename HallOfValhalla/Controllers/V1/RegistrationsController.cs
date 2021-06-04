using System;
using System.Threading.Tasks;
using HallOfValhalla.Contracts.V1;
using HallOfValhalla.Contracts.V1.Requests;
using HallOfValhalla.Services;
using Microsoft.AspNetCore.Mvc;

namespace HallOfValhalla.Controllers.V1
{
    public class RegistrationsController : ControllerBase
    {
        private readonly IConventionService _conventionService;

        public RegistrationsController(IConventionService conventionService)
        {
            _conventionService = conventionService;
        }

        [HttpPost(ApiRoutes.Registrations.Create)]
        public async Task<IActionResult> Create([FromRoute] Guid conventionId, [FromBody] CreateRegistrationRequest request)
        {
            var added = await _conventionService.AddParticipantAsync(conventionId, request.UserId);

            if (!added)
                return NotFound();

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Conventions.Show.Replace("{conventionId}", conventionId.ToString());
            return Created(locationUri, added);
        }
    }
}
