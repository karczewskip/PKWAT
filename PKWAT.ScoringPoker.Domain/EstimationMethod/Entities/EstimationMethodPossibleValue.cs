namespace PKWAT.ScoringPoker.Domain.EstimationMethod.Entities
{
    using PKWAT.ScoringPoker.Domain.Abstraction;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.ValueObjects;

    public class EstimationMethodPossibleValue : Entity<int>
    {
        public int EstimationMethodId { get; protected set; }
        public EstimationMethodValue EstimationMethodValue { get; protected set; }

        protected EstimationMethodPossibleValue()
        {
        }

        public static EstimationMethodPossibleValue CreateNew(int estimationMethodId, EstimationMethodValue methodValue)
        {
            return new EstimationMethodPossibleValue()
            {
                EstimationMethodId = estimationMethodId,
                EstimationMethodValue = methodValue
            };
        }
    }
}
