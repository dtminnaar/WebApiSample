using Sample.Services;

namespace Sample.Tests
{
    public class SettlementServiceTest
    {
        [Theory]
        [InlineData(9, 0)]
        [InlineData(12, 30)]
        [InlineData(16, 0)]
        public void IsBusinessHours_InRange_ShouldBeValid(int hour, int minute)
        {
            var service = new SettlementService();
            var result = service.IsBusinessHours(new TimeOnly(hour, minute));
            Assert.True(result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(8, 59)]
        [InlineData(16, 1)]
        [InlineData(23, 59)]
        public void IsBusinessHours_OOH_ShouldBeInvalid(int hour, int minute)
        {
            var service = new SettlementService();
            var result = service.IsBusinessHours(new TimeOnly(hour, minute));
            Assert.False(result);
        }

        [Fact]
        public void StoreSettlement_Once_ShouldBeValid()
        {
            var service = new SettlementService();
            var result = service.StoreSettlement(new TimeOnly(12, 30), "John Doe");
            Assert.NotNull(result);
            Assert.NotNull(result?.BookingId);
            Assert.True(result?.BookingId != Guid.Empty);
        }

        [Fact]
        public void StoreSettlement_FourTimes_ShouldBeValid()
        {
            var service = new SettlementService();
            for (int i = 0; i < 4; i++)
            {
                var result = service.StoreSettlement(new TimeOnly(12, 30), "John Doe");
                Assert.NotNull(result);
                Assert.NotNull(result?.BookingId);
                Assert.True(result?.BookingId != Guid.Empty);
            }
        }

        [Fact]
        public void StoreSettlement_FiveTimes_ShouldBeNull()
        {
            var service = new SettlementService();
            for (int i = 0; i < 4; i++)
            {
                service.StoreSettlement(new TimeOnly(12, 30), "John Doe");
            }
            var result = service.StoreSettlement(new TimeOnly(12, 30), "John Doe");
            Assert.Null(result);
        }
    }
}