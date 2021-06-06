using Newtonsoft.Json;

namespace HallOfValhalla.Domain
{
    public class EventDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
