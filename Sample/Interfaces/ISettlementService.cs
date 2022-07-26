using Sample.DTO;

namespace Sample.Interfaces
{
    public interface ISettlementService
    {
        bool IsBusinessHours(TimeOnly time);
        SettlementData? StoreSettlement(TimeOnly time, string name);
    }
}