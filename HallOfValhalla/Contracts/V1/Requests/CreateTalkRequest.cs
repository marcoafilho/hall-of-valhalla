using System;
namespace HallOfValhalla.Contracts.V1.Requests
{
    public class CreateTalkRequest
    {
        public Guid ConventionId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Speaker { get; set; }
    }
}
