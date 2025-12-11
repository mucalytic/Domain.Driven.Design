using Domain.Driven.Design.Domain.SubscriptionAggregate;
using Domain.Driven.Design.Tests.Unit.Factories;
using FluentAssertions;

namespace Domain.Driven.Design.Tests.Unit;

public class SubscriptionTests
{
    [Fact]
    public void AddGym_WhenMoreThanSubscriptionAllows_ShouldFail()
    {
        // Arrange
        var subscription = SubscriptionFactory.CreateSubscription();

        var gyms = Enumerable.Range(0, subscription.GetMaxGyms() + 1)
            .Select(_ => GymFactory.CreateGym(id: Guid.NewGuid()))
            .ToList();

        // Act
        var addGymResults = gyms.ConvertAll(subscription.AddGym);

        // Assert
        var allButLastAddGymResults = addGymResults.Take(..^1);
        allButLastAddGymResults.Should().AllSatisfy(result => result.IsError.Should().BeFalse());

        var lastAddGymResult = addGymResults.Last();
        lastAddGymResult.IsError.Should().BeTrue();
        lastAddGymResult.FirstError.Should().Be(SubscriptionErrors.CannotHaveMoreGymsThanSubscriptionAllows);
    }
}
