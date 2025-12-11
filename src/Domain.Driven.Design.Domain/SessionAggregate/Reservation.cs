using Domain.Driven.Design.Domain.Common;

namespace Domain.Driven.Design.Domain.SessionAggregate;

public class Reservation(Guid participantId, Guid? id = null) : GuidEntity(id)
{
    public Guid ParticipantId { get; } = participantId;
}
