using System.Text.Json.Serialization;
namespace Sample.DTO
{
    public class SettlementRequest
    {
        [JsonPropertyName("bookingTime")]
        public string? BookingTime { get; set; }
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}