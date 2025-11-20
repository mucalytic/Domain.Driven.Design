using Domain.Driven.Design.Tests.Unit.Factories;
using Domain.Driven.Design.Domain.Errors;
using FluentAssertions;

namespace Domain.Driven.Design.Tests.Unit;

public class ParticipantTests
{
    [Theory]
    [InlineData(1, 3, 1, 3)]
    [InlineData(1, 3, 2, 3)]
    [InlineData(1, 3, 2, 4)]
    [InlineData(1, 3, 0, 2)]
    public void AddSessionToSchedule_WhenSessionOverlapsWithAnotherSession_ShouldFail(
        int startHourSession1,
        int endHourSession1,
        int startHourSession2,
        int endHourSession2)
    {
        // arrange
        var participant = ParticipantFactory.CreateParticipant();

        var session1 = SessionFactory.CreateSession(
            date: Constants.Session.Date,
            time: TimeRangeFactory.CreateFromHours(startHourSession1, endHourSession1),
            id: Guid.NewGuid());

        var session2 = SessionFactory.CreateSession(
            date: Constants.Session.Date,
            time: TimeRangeFactory.CreateFromHours(startHourSession2, endHourSession2),
            id: Guid.NewGuid());

        // act
        var addSession1Result = participant.AddToSchedule(session1);
        var addSession2Result = participant.AddToSchedule(session2);

        // assert
        addSession1Result.IsError.Should().BeFalse();
        addSession2Result.IsError.Should().BeTrue();
        addSession2Result.FirstError.Should().Be(ParticipantErrors.CannotHaveTwoOrMoreOverlappingSessions);
    }
}
