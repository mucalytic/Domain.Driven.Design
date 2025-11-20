namespace Domain.Driven.Design.Tests.Unit;

public static partial class Constants
{
    public static class Room
    {
        public static readonly Guid Id = Guid.NewGuid();
        public const int MaxDailySessions = Subscriptions.MaxDailySessionsFreeTier;
    }
}
