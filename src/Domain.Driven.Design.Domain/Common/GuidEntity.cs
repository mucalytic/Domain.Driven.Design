namespace Domain.Driven.Design.Domain.Common;

public abstract class GuidEntity(Guid? id) : Entity<Guid>(id ?? Guid.NewGuid());
