using Domain.Driven.Design.Domain.Common;

namespace Domain.Driven.Design.Domain.Entities;

public class Admin(Guid? id = null) : GuidEntity(id)
{
    private readonly Guid _userId;
    private readonly Guid _subscriptionId;
}
