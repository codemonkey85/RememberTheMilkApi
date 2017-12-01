using Newtonsoft.Json;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("list")]
    public class RtmApiListObject
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        public RtmApiListObject()
        {
            Id = string.Empty;
            Name = string.Empty;
        }
    }
}