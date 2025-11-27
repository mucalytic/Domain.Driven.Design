using Domain.Driven.Design.Domain.Common;
using Domain.Driven.Design.Domain.Errors;
using ErrorOr;

namespace Domain.Driven.Design.Domain.Entities;

public class Gym(int maxRooms, Guid subscriptionId, Guid? id = null) : Entity<Guid>(id ?? Guid.NewGuid())
{
    private readonly List<Guid> _roomIds = [];

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
