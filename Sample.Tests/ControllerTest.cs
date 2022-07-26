using Moq;
using Sample.Interfaces;
using Sample.Controllers;
using Microsoft.AspNetCore.Mvc;
using Sample.DTO;

namespace Sample.Tests
{
    public class ControllerTest
    {

        [Fact]
        public void ValidParameters_ShouldReturn200Status()
        {
            var service = new Mock<ISettlementService>();
            service.Setup(_ => _.IsBusinessHours(It.IsAny<TimeOnly>())).Returns(true);
            service.Setup(_ => _.StoreSettlement(It.IsAny<TimeOnly>(), It.IsAny<string>())).Returns(new SettlementData
            {
                BookingId = Guid.NewGuid(),
            });
            var controller = new SettlementController(service.Object);
            var actionResult = controller.Reserve(new SettlementRequest()
            {
                BookingTime = "12:30",
                Name = "not empty",
            });

            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);

            var value = result?.Value as SettlementResult;
            Assert.NotNull(value);
            Assert.NotNull(value?.BookingId);
            Assert.True(value?.BookingId != Guid.Empty);
        }

        [Fact]
        public void EmptyParameters_ShouldReturn400Status()
        {
            var service = new Mock<ISettlementService>();

            service.Verify(_ => _.IsBusinessHours(It.IsAny<TimeOnly>()), Times.Never());
            service.Verify(_ => _.StoreSettlement(It.IsAny<TimeOnly>(), It.IsAny<string>()), Times.Never());
            var controller = new SettlementController(service.Object);
            var actionResult = controller.Reserve(new DTO.SettlementRequest()
            {
                BookingTime = "",
                Name = "",
            });

            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        }

        [Fact]
        public void SlotFull_ShouldReturn409Status()
        {
            var service = new Mock<ISettlementService>();
            service.Setup(_ => _.IsBusinessHours(It.IsAny<TimeOnly>())).Returns(true);
            service.Setup(_ => _.StoreSettlement(It.IsAny<TimeOnly>(), It.IsAny<string>())).Returns(() => null);

            var controller = new SettlementController(service.Object);
            var actionResult = controller.Reserve(new SettlementRequest()
            {
                BookingTime = "12:30",
                Name = "not empty",
            });

            Assert.IsType<ConflictObjectResult>(actionResult.Result);
        }
    }
}