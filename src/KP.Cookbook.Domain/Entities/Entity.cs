namespace KP.Cookbook.Domain.Entities
{
    /// <summary>
    /// Базовый класс сущности предметной области.
    /// </summary>
    /// <remarks>
    /// Объекты отличаются идентификатором.
    /// </remarks>
    public abstract class Entity
    {
        public long Id { get; private set; }

        protected Entity() { }
        protected Entity(long id)
        {
            if (id == default)
                throw new InvariantException("Нельзя создать сущность на основе ID по умолчанию");

            Id = id;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity other)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (Id.Equals(default) || other.Id.Equals(default))
                return false;

            return Id.Equals(other.Id);
        }

        public static bool operator ==(Entity? a, Entity? b)
        {
            if (a is null && b is null)
                return true;

            if (a is null || b is null)
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity? a, Entity? b) => !(a == b);

        public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();
    }
}
