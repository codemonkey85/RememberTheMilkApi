using Newtonsoft.Json;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("transaction")]
    public class RtmApiTransaction
    {
        [JsonProperty("id")]
        public string Id
        {
            get; set;
        }

        [JsonProperty("undoable")]
        public string Undoable
        {
            get; set;
        }
    }
}