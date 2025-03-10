using Newtonsoft.Json;
namespace Ecommerce.Core.DTOS
{
    public class GoogleTokenInfoDTO
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }
    }
}
