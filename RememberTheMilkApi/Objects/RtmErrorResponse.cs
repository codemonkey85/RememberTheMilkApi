using Newtonsoft.Json;

namespace RememberTheMilkApi.Objects
{
    [JsonObject("err")]
    public class RtmErrorResponse
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        public RtmErrorResponse()
        {
            Code = 0;
            Message = string.Empty;
        }
    }
}