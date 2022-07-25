using Sample.DTO;
using Sample.Interfaces;

namespace Sample.Services
{
    public class SettlementService : ISettlementService
    {
        private static readonly SemaphoreSlim _dataSemaphore = new SemaphoreSlim(1);
        private static readonly TimeOnly _firstSlot = new TimeOnly(9, 0);
        private static readonly TimeOnly _lastSlot = new TimeOnly(16, 0);
        private static readonly IList<SettlementData> _datastore = new List<SettlementData>();

        public SettlementService()
        {
        }

        /// <summary>
        /// Creates new Settlement item and stores it in the "database"
        /// </summary>
        /// <param name="time">Timeslot for stored settlement</param>
        /// <param name="name">Customer name</param>
        /// <returns>Returns SettlementData containing Booking Identifier
        /// If the timeslot is full NULL is returned</returns>
        public SettlementData? StoreSettlement(TimeOnly time, string name)
        {
            SettlementData? settlement = null;
            var endTime = time.AddMinutes(59);
            _dataSemaphore.Wait();
            try
            {
                var bookings = _datastore
                    .Where(x => (x.BookingTime <= time && x.BookingEndTime >= time)
                             || (x.BookingTime <= endTime && x.BookingEndTime >= endTime))
                    .ToList();

                if (bookings.Count < 4)
                {
                    settlement = new SettlementData
                    {
                        BookingEndTime = time.AddMinutes(59),
                        BookingId = Guid.NewGuid(),
                        BookingTime = time,
                        Name = name,
                    };
                    _datastore.Add(settlement);
                }
            }
            finally
            {
                _dataSemaphore.Release();
            }
            return settlement;
        }

        /// <summary>
        /// Confirm that the timeslot is in a valid range
        /// </summary>
        /// <param name="time"></param>
        /// <returns>True if in buisiness hours</returns>
        public bool IsBusinessHours(TimeOnly time)
        {
            return time >= _firstSlot && time <= _lastSlot;
        }
    }
}
