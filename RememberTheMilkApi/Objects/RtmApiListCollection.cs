using Newtonsoft.Json;
using System.Collections.Generic;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("lists")]
    public class RtmApiListCollection
    {
        [JsonProperty("list")]
        public IList<RtmApiListObject> Lists { get; set; }

        public RtmApiListCollection()
        {
            Lists = new List<RtmApiListObject>();
        }
    }
}