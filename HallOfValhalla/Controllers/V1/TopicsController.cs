using System;
using System.Threading.Tasks;
using HallOfValhalla.Contracts.V1;
using HallOfValhalla.Services;
using Microsoft.AspNetCore.Mvc;

namespace HallOfValhalla.Controllers.V1
{
    public class TopicsController : ControllerBase
    {
        private readonly ITopicService _topicService;

        public TopicsController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet(ApiRoutes.Topics.Index)]
        public async Task<IActionResult> Index()
        {
            return Ok(await _topicService.GetTopicsAsync());
        }
    }
}
