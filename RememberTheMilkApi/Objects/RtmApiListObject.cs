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

        [JsonProperty("archived")]
        public string Archived { get; set; }

        [JsonProperty("deleted")]
        public string Deleted { get; set; }

        [JsonProperty("locked")]
        public string Locked { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }

        [JsonProperty("smart")]
        public string Smart { get; set; }

        public RtmApiListObject()
        {
            Id = string.Empty;
            Name = string.Empty;
            Archived = string.Empty;
            Deleted = string.Empty;
            Locked = string.Empty;
            Position = string.Empty;
            Smart = string.Empty;
        }

        public override string ToString()
        {
            return Name ?? string.Empty;
        }
    }
}