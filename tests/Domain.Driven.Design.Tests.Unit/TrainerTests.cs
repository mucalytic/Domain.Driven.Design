using Domain.Driven.Design.Tests.Unit.Factories;
using Domain.Driven.Design.Domain.Errors;
using FluentAssertions;

namespace Domain.Driven.Design.Tests.Unit;

public class TrainerTests
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
        var trainer = TrainerFactory.CreateTrainer();

        var session1 = SessionFactory.CreateSession(
            date: Constants.Session.Date,
            time: TimeRangeFactory.CreateFromHours(startHourSession1, endHourSession1),
            id: Guid.NewGuid());

        var session2 = SessionFactory.CreateSession(
            date: Constants.Session.Date,
            time: TimeRangeFactory.CreateFromHours(startHourSession2, endHourSession2),
            id: Guid.NewGuid());

        // act
        var addSession1Result = trainer.AddSessionToSchedule(session1);
        var addSession2Result = trainer.AddSessionToSchedule(session2);

        // assert
        addSession1Result.IsError.Should().BeFalse();
        addSession2Result.IsError.Should().BeTrue();
        addSession2Result.FirstError.Should().Be(TrainerErrors.CannotHaveTwoOrMoreOverlappingSessions);
    }
}
