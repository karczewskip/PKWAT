namespace PKWAT.ScoringPoker.Domain.ScoringTask.ValueObjects
{
    using PKWAT.ScoringPoker.Domain.Abstraction;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.Entities;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.ValueObjects;
    using System.Collections.Generic;

    public class Estimation : ValueObject
    {
        public EstimationMethodKey MethodKey { get; protected set; }
        public EstimationMethodValue Value { get; protected set; }

        private Estimation() { }

        public static Estimation Create(EstimationMethodKey methodKey, EstimationMethodValue value)
        {
            return new Estimation()
            {
                MethodKey = methodKey,
                Value = value
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return MethodKey;
            yield return Value;
        }
    }
}
