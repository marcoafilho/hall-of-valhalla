using System.Text.Json.Serialization;

namespace HallOfValhalla.Domain
{
    public class Venue
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
