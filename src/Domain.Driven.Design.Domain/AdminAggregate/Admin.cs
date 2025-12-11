using Domain.Driven.Design.Domain.Common;

namespace Domain.Driven.Design.Domain.AdminAggregate;

public class Admin(Guid userId, Guid subscriptionId, Guid? id = null) : AggregateRoot(id);
