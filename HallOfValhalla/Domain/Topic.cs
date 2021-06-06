using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HallOfValhalla.Domain
{
    public class Topics
    {
        [JsonPropertyName("data")]
        public TopicsData Data { get; set; }
    }

    public class TopicsData
    {
        [JsonPropertyName("results")]
        public List<Topic> Topics { get; set; }

    }

    public class Topic
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("thumbnail")]
        public TopicThumbnail Thumbnail { get; set; }

        public class TopicThumbnail
        {
            [JsonPropertyName("path")]
            public string Path { get; set; }

            [JsonPropertyName("extension")]
            public string Extension { get; set; }
        }
    }
}
