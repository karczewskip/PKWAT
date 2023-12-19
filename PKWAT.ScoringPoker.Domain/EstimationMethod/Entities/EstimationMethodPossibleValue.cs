namespace PKWAT.ScoringPoker.Domain.EstimationMethod.Entities
{
    using PKWAT.ScoringPoker.Domain.Abstraction;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.ValueObjects;

    public class EstimationMethodPossibleValueKey : ValueObject
    {
        public EstimationMethodKey EstimationMethodKey { get; protected set; }
        public EstimationMethodValue EstimationMethodValue { get; protected set; }

        private EstimationMethodPossibleValueKey() { }

        public static EstimationMethodPossibleValueKey Create(EstimationMethodKey methodKey, EstimationMethodValue methodValue)
        {
            return new EstimationMethodPossibleValueKey()
            {
                EstimationMethodKey = methodKey,
                EstimationMethodValue = methodValue
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EstimationMethodKey;
        }
    }

    public class EstimationMethodPossibleValue : Entity<EstimationMethodPossibleValueKey>
    {
        protected EstimationMethodPossibleValue()
        {
        }

        public static EstimationMethodPossibleValue CreateNew(EstimationMethodPossibleValueKey key)
        {
            return new EstimationMethodPossibleValue()
            {
                Id = key
            };
        }
    }
}
