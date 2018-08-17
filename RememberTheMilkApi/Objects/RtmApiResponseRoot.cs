using Newtonsoft.Json;

namespace RememberTheMilkApi.Objects
{
    [JsonObject]
    public class RtmApiResponseRoot
    {
        [JsonProperty("rsp")]
        public RtmApiResponse Response
        {
            get; set;
        }

        public RtmApiResponseRoot()
        {
            Response = new RtmApiResponse();
        }
    }
}