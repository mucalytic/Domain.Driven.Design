using Domain.Driven.Design.Domain.Entities;

namespace Domain.Driven.Design.Tests.Unit.Factories;

public static class RoomFactory
{
    public static Room CreateRoom(int maxDailySessions = Constants.Room.MaxDailySessions, Guid? gymId = null, Guid? id = null) => 
        new(maxDailySessions: maxDailySessions, gymId: gymId ?? Constants.Gym.Id, id: id ?? Constants.Room.Id);
}
