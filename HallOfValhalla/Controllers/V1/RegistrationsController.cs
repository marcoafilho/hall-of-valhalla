using System;
using System.Threading.Tasks;
using HallOfValhalla.Contracts.V1;
using HallOfValhalla.Contracts.V1.Requests;
using HallOfValhalla.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HallOfValhalla.Controllers.V1
{
    public class RegistrationsController : ControllerBase
    {
        public class Users
        {
            public string Name { get; set; }
        }

        private readonly IConventionService _conventionService;

        public RegistrationsController(IConventionService conventionService)
        {
            _conventionService = conventionService;
        }

        [Authorize]
        [HttpGet(ApiRoutes.Registrations.Show)]
        public IActionResult Show([FromRoute] Guid conventionId)
        {
            return Ok(HttpContext.User.Identity.Name);
        }

        [Authorize]
        [HttpPost(ApiRoutes.Registrations.Create)]
        public async Task<IActionResult> Create([FromRoute] Guid conventionId, [FromBody] CreateRegistrationRequest request)
        {
            bool added = await _conventionService.AddParticipantAsync(conventionId, request.UserId);

            if (!added)
            {
                return NotFound();
            }

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Conventions.Show.Replace("{conventionId}", conventionId.ToString());
            return Created(locationUri, added);
        }
    }
}
