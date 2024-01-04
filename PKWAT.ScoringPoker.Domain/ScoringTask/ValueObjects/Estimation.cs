namespace PKWAT.ScoringPoker.Domain.ScoringTask.ValueObjects
{
    using PKWAT.ScoringPoker.Domain.Abstraction;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.ValueObjects;
    using System.Collections.Generic;

    public class Estimation : ValueObject
    {
        public int EstimationMethodId { get; protected set; }
        public EstimationMethodValue Value { get; protected set; }

        private Estimation() { }

        public static Estimation Create(int estimationMethodId, EstimationMethodValue value)
        {
            return new Estimation()
            {
                EstimationMethodId = estimationMethodId,
                Value = value
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return EstimationMethodId;
            yield return Value;
        }
    }
}
