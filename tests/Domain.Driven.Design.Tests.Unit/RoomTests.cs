using Domain.Driven.Design.Tests.Unit.Factories;
using Domain.Driven.Design.Domain.RoomAggregate;
using FluentAssertions;

namespace Domain.Driven.Design.Tests.Unit;

public class RoomTests
{
    [Fact]
    public void ScheduleSession_WhenMoreThanSubscriptionAllows_ShouldFail()
    {
        // arrange
        var room = RoomFactory.CreateRoom(maxDailySessions: 1);

        var session1 = SessionFactory.CreateSession(id: Guid.NewGuid());
        var session2 = SessionFactory.CreateSession(id: Guid.NewGuid());

        // act
        var scheduleSession1Result = room.ScheduleSession(session1);
        var scheduleSession2Result = room.ScheduleSession(session2);

        // assert
        scheduleSession1Result.IsError.Should().BeFalse();
        scheduleSession2Result.IsError.Should().BeTrue();
        scheduleSession2Result.FirstError.Should().Be(RoomErrors.CannotHaveMoreSessionThanSubscriptionAllows);
    }

    [Theory]
    [InlineData(1, 3, 1, 3)] // exact overlap
    [InlineData(1, 3, 2, 3)] // second session inside first session
    [InlineData(1, 3, 2, 4)] // second session ends after session, but overlaps
    [InlineData(1, 3, 0, 2)] // second session starts before second session, but overlaps
    public void ScheduleSession_WhenSessionOverlapsWithAnotherSession_ShouldFail(
        int startHourSession1,
        int endHourSession1,
        int startHourSession2,
        int endHourSession2)
    {
        // arrange
        var room = RoomFactory.CreateRoom(
            maxDailySessions: 2);

        var session1 = SessionFactory.CreateSession(
            date: Constants.Session.Date,
            time: TimeRangeFactory.CreateFromHours(startHourSession1, endHourSession1),
            id: Guid.NewGuid());

        var session2 = SessionFactory.CreateSession(
            date: Constants.Session.Date,
            time: TimeRangeFactory.CreateFromHours(startHourSession2, endHourSession2),
            id: Guid.NewGuid());

        // act
        var scheduleSession1Result = room.ScheduleSession(session1);
        var scheduleSession2Result = room.ScheduleSession(session2);

        // assert
        scheduleSession1Result.IsError.Should().BeFalse();
        scheduleSession2Result.IsError.Should().BeTrue();
        scheduleSession2Result.FirstError.Should().Be(RoomErrors.CannotHaveTwoOrMoreOverlappingSessions);
    }
}
