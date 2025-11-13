namespace Domain.Driven.Design.Domain;

public class Session(int maxParticipants, Guid trainerId, Guid? id = null)
{
    private readonly List<Guid> _participantIds = [];
    private readonly Guid _id = id ?? Guid.NewGuid();

    public void ReserveSpot(Participant participant)
    {
        if (_participantIds.Count >= maxParticipants)
        {
            throw new InvalidOperationException("Maximum number of participants reached");
        }
        _participantIds.Add(participant.Id);
    }
}
