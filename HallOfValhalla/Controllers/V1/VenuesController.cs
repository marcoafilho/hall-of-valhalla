using System;
using System.Threading.Tasks;
using HallOfValhalla.Contracts.V1;
using HallOfValhalla.Services;
using Microsoft.AspNetCore.Mvc;

namespace HallOfValhalla.Controllers.V1
{
    public class VenuesController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenuesController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        [HttpGet(ApiRoutes.Venues.Index)]
        public async Task<IActionResult> Index()
        {
            return Ok(await _venueService.GetVenuesAsync());
        }
    }
}
