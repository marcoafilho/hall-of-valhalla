using System.Collections.Generic;
using Newtonsoft.Json;

namespace HallOfValhalla.Domain
{
    public class TalkDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("speaker")]
        public string Speaker { get; set; }

        [JsonProperty("participants")]
        public HashSet<string> Participants { get; set; }
    }
}
