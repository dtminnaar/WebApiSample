using System.Text.Json.Serialization;
namespace Sample.DTO
{
    public class SettlementResult
    {
        [JsonPropertyName("bookingId")]
        public Guid BookingId { get; set; }

    }
}