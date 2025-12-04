namespace Domain.Driven.Design.Domain.Common;

public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public bool Equals(T? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override bool Equals(object? obj) =>
        obj is T other && Equals(other);

    public override int GetHashCode() =>
        GetEqualityComponents().Aggregate(0, HashCode.Combine);

    public static bool operator ==(ValueObject<T>? left, ValueObject<T>? right)
        => left?.Equals(right) ?? right is null;

    public static bool operator !=(ValueObject<T>? left, ValueObject<T>? right)
        => !(left == right);
}
