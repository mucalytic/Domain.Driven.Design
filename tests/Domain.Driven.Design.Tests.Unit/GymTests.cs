using Domain.Driven.Design.Tests.Unit.Factories;
using Domain.Driven.Design.Domain.Errors;
using FluentAssertions;

namespace Domain.Driven.Design.Tests.Unit;

public class GymTests
{
    [Fact]
    public void AddRoom_WhenMoreThanSubscriptionAllows_ShouldFail()
    {
        // arrange
        var gym = GymFactory.CreateGym(maxRooms: 1);
        var room1 = RoomFactory.CreateRoom(id: Guid.NewGuid());
        var room2 = RoomFactory.CreateRoom(id: Guid.NewGuid());

        // act
        var addRoom1Result = gym.AddRoom(room1);
        var addRoom2Result = gym.AddRoom(room2);

        // assert
        addRoom1Result.IsError.Should().BeFalse();
        addRoom2Result.IsError.Should().BeTrue();
        addRoom2Result.FirstError.Should().Be(GymErrors.CannotHaveMoreRoomsThanSubscriptionAllows);
    }
}
