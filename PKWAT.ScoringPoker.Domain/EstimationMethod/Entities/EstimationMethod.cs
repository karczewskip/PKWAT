namespace PKWAT.ScoringPoker.Domain.EstimationMethod.Entities
{
    using PKWAT.ScoringPoker.Domain.Abstraction;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.ValueObjects;
    using System.Collections.Generic;

    public class EstimationMethodKey : ValueObject
    {
        private EstimationMethodKey() { }

        public int Value { get; private set; }

        public static EstimationMethodKey Create(int value)
        {
            return new EstimationMethodKey()
            {
                Value = value
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }

    public class EstimationMethod : Entity<EstimationMethodKey>, IAggregateRoot<EstimationMethodKey>
    {
        protected EstimationMethod()
        {
        }

        public EstimationMethodName Name { get; protected set; }

        public static EstimationMethod CreateNew(EstimationMethodName name)
        {
            return new EstimationMethod()
            {
                Name = name
            };
        }
    }
}
