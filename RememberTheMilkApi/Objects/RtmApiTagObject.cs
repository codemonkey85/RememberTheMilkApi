using Newtonsoft.Json;
using RememberTheMilkApi.Converters;
using System.Collections.Generic;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("tags")]
    public class RtmApiTagObject
    {
        [JsonProperty("tag")]
        [JsonConverter(typeof(SingleOrArrayConverter<string>))]
        public IList<string> Tag
        {
            get; set;
        }

        public RtmApiTagObject()
        {
            Tag = new List<string>();
        }
    }
}