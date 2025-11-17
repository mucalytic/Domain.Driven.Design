using static Domain.Driven.Design.Domain.Constants.ErrorCodes;
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
        var reservationResult1 = session.ReserveSpot(participant1);
        // add participant two
        var reservationResult2 = session.ReserveSpot(participant2);

        // assert
        // participant two reservation failed
        reservationResult1.IsError.Should().BeFalse();
        reservationResult2.IsError.Should().BeTrue();
        reservationResult2.FirstError.Code.Should().Be(nameof(MaximumNumberOfParticipantsReached));
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
        var result = session.CancelReservation(participant, dateTimeProvider);

        // assert
        // the cancellation fails
        result.IsError.Should().BeTrue();
        result.FirstError.Code.Should().Be(nameof(CancellationIsTooCloseToSession));
    }
}
