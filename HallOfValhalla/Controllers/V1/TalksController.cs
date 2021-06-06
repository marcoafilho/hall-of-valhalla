using System;
using System.Threading.Tasks;
using HallOfValhalla.Contracts.V1;
using HallOfValhalla.Contracts.V1.Requests;
using HallOfValhalla.Contracts.V1.Responses;
using HallOfValhalla.Domain;
using HallOfValhalla.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HallOfValhalla.Controllers.V1
{
    public class TalksController : ControllerBase
    {
        private readonly IConventionService _conventionService;

        public TalksController(IConventionService conventionService)
        {
            _conventionService = conventionService;
        }

        [Authorize("create:talks")]
        [HttpPost(ApiRoutes.Talks.Create)]
        public async Task<IActionResult> Create([FromBody] CreateTalkRequest request)
        {
            var talk = new Talk
            {
                Title = request.Title,
                Description = request.Description,
                Speaker = request.Speaker
            };

            await _conventionService.CreateTalkAsync(request.ConventionId, talk);

            var conventionResponse = new TalkResponse { Id = request.ConventionId };

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Conventions.Show.Replace("{conventionId}", request.ConventionId.ToString());
            return Created(locationUri, conventionResponse);
        }
    }
}
