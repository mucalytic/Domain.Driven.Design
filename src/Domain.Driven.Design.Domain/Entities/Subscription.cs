using Domain.Driven.Design.Domain.ValueObjects;
using Domain.Driven.Design.Domain.Common;
using Domain.Driven.Design.Domain.Errors;
using ErrorOr;

namespace Domain.Driven.Design.Domain.Entities;

public class Subscription(SubscriptionType subscriptionType, Guid adminId, Guid? id = null) : GuidEntity(id)
{
    private readonly SubscriptionType _subscriptionType =  subscriptionType;
    private readonly Guid _adminId = adminId;
    private readonly List<Guid> _gymIds = [];

    public int GetMaxGyms() => _subscriptionType.Name switch
    {
        nameof(SubscriptionType.Free) => 1,
        nameof(SubscriptionType.Starter) => 1,
        nameof(SubscriptionType.Pro) => 3,
        _ => throw new InvalidOperationException()
    };

    public int GetMaxRooms() => _subscriptionType.Name switch
    {
        nameof(SubscriptionType.Free) => 1,
        nameof(SubscriptionType.Starter) => 3,
        nameof(SubscriptionType.Pro) => int.MaxValue,
        _ => throw new InvalidOperationException()
    };

    public int GetMaxDailySessions() => _subscriptionType.Name switch
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
