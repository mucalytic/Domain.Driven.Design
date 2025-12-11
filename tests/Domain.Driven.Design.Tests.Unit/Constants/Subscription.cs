using Domain.Driven.Design.Domain.Common.ValueObjects;
using Domain.Driven.Design.Domain.SubscriptionAggregate;

namespace Domain.Driven.Design.Tests.Unit.Constants;

public static class Subscriptions
{
    public static readonly SubscriptionType DefaultSubscriptionType = SubscriptionType.Free;
    public static readonly Guid Id = Guid.NewGuid();
    public const int MaxDailySessionsFreeTier = 4;
    public const int MaxRoomsFreeTier = 1;
    public const int MaxGymsFreeTier = 1;
}
