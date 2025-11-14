namespace Domain.Driven.Design.Tests.Unit;

public static partial class Constants
{
    public static class Session
    {
        public static readonly Guid Id = Guid.NewGuid();
        public static readonly DateOnly Date = new(1911, 03, 13);
        public static readonly TimeOnly StartTime = new(19, 50);
        public static readonly TimeOnly EndTime = new(20, 25);
        public const int MaxParticipants = 1;
    }
}
