using Domain.Driven.Design.Domain.GymAggregate;
using Domain.Driven.Design.Domain.Common;
using ErrorOr;

namespace Domain.Driven.Design.Domain.SubscriptionAggregate;

public class Subscription(SubscriptionType subscriptionType, Guid adminId, Guid? id = null) : AggregateRoot(id)
{
    private readonly List<Guid> _gymIds = [];

    public int GetMaxGyms() => subscriptionType.Name switch
    {
        nameof(SubscriptionType.Free) => 1,
        nameof(SubscriptionType.Starter) => 1,
        nameof(SubscriptionType.Pro) => 3,
        _ => throw new InvalidOperationException()
    };

    public int GetMaxRooms() => subscriptionType.Name switch
    {
        nameof(SubscriptionType.Free) => 1,
        nameof(SubscriptionType.Starter) => 3,
        nameof(SubscriptionType.Pro) => int.MaxValue,
        _ => throw new InvalidOperationException()
    };

    public int GetMaxDailySessions() => subscriptionType.Name switch
    {
        nameof(SubscriptionType.Free) => 4,
        nameof(SubscriptionType.Starter) => int.MaxValue,
        nameof(SubscriptionType.Pro) => int.MaxValue,
        _ => throw new InvalidOperationException()
    };

    public ErrorOr<Success> AddGym(Gym gym)
    {
        if (_gymIds.Contains(gym.Id))
        {
            return Error.Conflict(description: "Gym already exists");
        }
        if (_gymIds.Count >= GetMaxGyms())
        {
            return SubscriptionErrors.CannotHaveMoreGymsThanSubscriptionAllows;
        }
        _gymIds.Add(gym.Id);
        return Result.Success;
    }
}
