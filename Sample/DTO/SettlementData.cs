namespace Sample.DTO
{
    public class SettlementData
    {
        public Guid BookingId { get; set; }
        public TimeOnly BookingTime { get; set; }
        public TimeOnly BookingEndTime { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}