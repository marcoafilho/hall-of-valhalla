using System;
using System.Collections.Generic;
using Cosmonaut.Attributes;
using Newtonsoft.Json;

namespace HallOfValhalla.Domain
{
    [CosmosCollection("conventions")]
    public class ConventionDto
    {
        [CosmosPartitionKey]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("participants")]
        public List<string> Participants { get; set; }
    }
}
