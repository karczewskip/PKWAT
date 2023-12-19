namespace PKWAT.ScoringPoker.Domain.Abstraction
{
    using System.Collections.Generic;

    public abstract class Entity<T> : IEntity<T>
    {
        public virtual T Id { get; protected set; }

        protected Entity()
        {
        }

        protected Entity(T id) => Id = id;

        public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(Id);

        public override bool Equals(object obj)
        {
            if (obj is not Entity<T> other)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            if (IsTransient() || other.IsTransient())
            {
                return false;
            }

            return EqualityComparer<T>.Default.Equals(Id, other.Id);
        }

        private bool IsTransient() => Id == null || Id.Equals(default(T));

        protected bool Equals(Entity<T> other) => Equals(other as object);

        public static bool operator ==(Entity<T> a, Entity<T> b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Entity<T> a, Entity<T> b) => !(a == b);
    }
}
