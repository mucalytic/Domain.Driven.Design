using Domain.Driven.Design.Domain.GymAggregate;

namespace Domain.Driven.Design.Tests.Unit.Factories;

public static class GymFactory
{
    public static Gym CreateGym(int maxRooms = Constants.Subscriptions.MaxRoomsFreeTier, Guid? id = null) =>
        new(maxRooms, subscriptionId: Constants.Subscriptions.Id, id: id ?? Constants.Gym.Id);
}
