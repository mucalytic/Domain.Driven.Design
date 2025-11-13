using Domain.Driven.Design.Tests.Unit.Factories;
using FluentAssertions;

namespace Domain.Driven.Design.Tests.Unit;

public class SessionTests
{
    [Fact]
    public void ReserveSpot_WhenNoMoreRoom_ShouldFailReservation()
    {
        // arrange
        // create a session with a maximum of one participant
        var session = SessionFactory.CreateSession(maxParticipants: 1);
        // create two participants
        var participant1 = ParticipantFactory.CreateParticipant(userId: Guid.NewGuid(), id: Guid.NewGuid());
        var participant2 = ParticipantFactory.CreateParticipant(userId: Guid.NewGuid(), id: Guid.NewGuid());

        // act
        // add participant one
        session.ReserveSpot(participant1);
        // add participant two
        var action = () => session.ReserveSpot(participant2);

        // assert
        // participant two reservation failed
        action.Should().Throw<InvalidOperationException>();
    }
}
