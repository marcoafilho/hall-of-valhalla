using System.Collections.Generic;
using Newtonsoft.Json;

namespace HallOfValhalla.Domain
{
    public class ConventionDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("participants")]
        public HashSet<string> Participants { get; set; }

        [JsonProperty("talks")]
        public HashSet<TalkDto> Talks { get; set; }
    }
}
