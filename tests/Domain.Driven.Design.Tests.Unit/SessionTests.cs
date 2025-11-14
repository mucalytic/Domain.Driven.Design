using Domain.Driven.Design.Tests.Unit.Factories;
using Domain.Driven.Design.Tests.Unit.Services;
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

    [Fact]
    public void CancellingReservation_WhenCancellationIsTooCloseToSession_ShouldFailCancellation()
    {
        // arrange
        // create a session
        var session = SessionFactory.CreateSession(
            date: Constants.Session.Date,
            startTime: Constants.Session.StartTime,
            endTime: Constants.Session.EndTime);
        // create a participant
        var participant = ParticipantFactory.CreateParticipant(userId: Guid.NewGuid(), id: Guid.NewGuid());
        // reserve a spot for the participant in the session
        session.ReserveSpot(participant);
        // create a dateTimeProvider
        var dateTimeProvider = new TestDateTimeProvider(Constants.Session.Date.ToDateTime(TimeOnly.MinValue));

        // act
        // cancel the reservation less than 24 hours before the session
        var action = () => session.CancelReservation(participant, dateTimeProvider);

        // assert
        // the cancellation fails
        action.Should().Throw<InvalidOperationException>();
    }
}
