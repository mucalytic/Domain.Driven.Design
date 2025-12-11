using Domain.Driven.Design.Domain.RoomAggregate;
using Domain.Driven.Design.Domain.Common;
using ErrorOr;

namespace Domain.Driven.Design.Domain.GymAggregate;

public class Gym(int maxRooms, Guid subscriptionId, Guid? id = null) : AggregateRoot(id)
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
