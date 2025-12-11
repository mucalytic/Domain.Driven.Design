using Domain.Driven.Design.Domain.SessionAggregate;
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
        var session = SessionFactory.CreateSession(maxParticipants: 1);
        var participant1 = ParticipantFactory.CreateParticipant(id: Guid.NewGuid(), userId: Guid.NewGuid());
        var participant2 = ParticipantFactory.CreateParticipant(id: Guid.NewGuid(), userId: Guid.NewGuid());

        // act
        var reserveParticipant1Result = session.ReserveSpot(participant1);
        var reserveParticipant2Result = session.ReserveSpot(participant2);

        // assert
        reserveParticipant1Result.IsError.Should().BeFalse();
        reserveParticipant2Result.IsError.Should().BeTrue();
        reserveParticipant2Result.FirstError.Should().Be(SessionErrors.CannotHaveMoreReservationsThanParticipants);
    }

    [Fact]
    public void CancelReservation_WhenCancellationIsTooCloseToSession_ShouldFailCancellation()
    {
        // Arrange
        var session = SessionFactory.CreateSession(
            date: Constants.Session.Date,
            time: Constants.Session.Time);

        var participant = ParticipantFactory.CreateParticipant();
        var cancellationDateTime = Constants.Session.Date.ToDateTime(TimeOnly.MinValue);

        // act
        var reserveSpotResult = session.ReserveSpot(participant);
        var cancelReservationResult = session.CancelReservation(
            participant,
            new TestDateTimeProvider(fixedDateTime: cancellationDateTime));

        // assert
        reserveSpotResult.IsError.Should().BeFalse();
        cancelReservationResult.IsError.Should().BeTrue();
        cancelReservationResult.FirstError.Should().Be(SessionErrors.CannotCancelReservationTooCloseToSession);
    }
}
