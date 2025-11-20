using Domain.Driven.Design.Domain.Errors;
using ErrorOr;

namespace Domain.Driven.Design.Domain.Objects;

public class Gym(int maxRooms, Guid subscriptionId, Guid? id = null)
{
    private readonly List<Guid> _roomIds = [];

    public Guid Id { get; } = id ?? Guid.NewGuid();

    public ErrorOr<Success> AddRoom(Room room)
    {
        if (_roomIds.Contains(room.Id))
        {
            return Error.Conflict(description: "Room already exists in gym");
        }
        if (_roomIds.Count >= maxRooms)
        {
            return GymErrors.CannotHaveMoreRoomsThanSubscriptionAllows;
        }
        _roomIds.Add(room.Id);
        return Result.Success;
    }
}
