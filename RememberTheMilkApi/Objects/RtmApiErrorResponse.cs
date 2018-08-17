using Newtonsoft.Json;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("err")]
    public class RtmApiErrorResponse
    {
        [JsonProperty("code")]
        public int Code
        {
            get; set;
        }

        [JsonProperty("msg")]
        public string Message
        {
            get; set;
        }

        public RtmApiErrorResponse()
        {
            Code = 0;
            Message = string.Empty;
        }
    }
}