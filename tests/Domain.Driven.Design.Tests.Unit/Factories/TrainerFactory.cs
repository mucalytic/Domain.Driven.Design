using Domain.Driven.Design.Domain.TrainerAggregate;

namespace Domain.Driven.Design.Tests.Unit.Factories;

public static class TrainerFactory
{
    public static Trainer CreateTrainer(
        Guid? userId = null,
        Guid? id = null)
    {
        return new Trainer(
            userId: userId ?? Constants.User.Id,
            id: id ?? Constants.Trainer.Id);
    }
}
