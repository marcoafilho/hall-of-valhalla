using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HallOfValhalla.Contracts;
using HallOfValhalla.Contracts.V1;
using HallOfValhalla.Contracts.V1.Requests;
using HallOfValhalla.Contracts.V1.Responses;
using HallOfValhalla.Domain;
using HallOfValhalla.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HallOfValhalla.Controllers.V1
{
    public class ConventionsController : ControllerBase
    {
        private readonly IConventionService _conventionService;

        public ConventionsController(IConventionService conventionService)
        {
            _conventionService = conventionService;
        }

        [HttpGet(ApiRoutes.Conventions.Index)]
        public async Task<IActionResult> Index()
        {
            return Ok(await _conventionService.GetConventionsAsync());
        }

        [HttpGet(ApiRoutes.Conventions.Show)]
        public async Task<IActionResult> Show([FromRoute] Guid conventionId)
        {
            var convention = await _conventionService.GetConventionByIdAsync(conventionId);

            if (convention == null)
                return NotFound();

            return Ok(convention);
        }

        [HttpPost(ApiRoutes.Conventions.Create)]
        public async Task<IActionResult> Create([FromBody] CreateConventionRequest request)
        {
            var convention = new Convention
            {
                Name = request.Name
            };


            await _conventionService.CreateConventionAsync(convention);

            var conventionResponse = new ConventionResponse { Id = convention.Id };

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Conventions.Show.Replace("{conventionId}", convention.Id.ToString());
            return Created(locationUri, conventionResponse);
        }

        [HttpPut(ApiRoutes.Conventions.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid conventionId, [FromBody] UpdateConventionRequest request)
        {
            var convention = new Convention
            {
                Id = conventionId,
                Name = request.Name
            };

            var updated = await _conventionService.UpdateConventionAsync(convention);

            return updated ? Ok() : NotFound();
        }

        [HttpDelete(ApiRoutes.Conventions.Destroy)]
        public async Task<IActionResult> Destroy([FromRoute] Guid conventionId)
        {
            var deleted = await _conventionService.DeleteConventionAsync(conventionId);

            return deleted ? NoContent() : NotFound();
        }
    }
}
