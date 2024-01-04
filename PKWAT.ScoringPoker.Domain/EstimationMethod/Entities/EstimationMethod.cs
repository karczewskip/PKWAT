namespace PKWAT.ScoringPoker.Domain.EstimationMethod.Entities
{
    using PKWAT.ScoringPoker.Domain.Abstraction;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.ValueObjects;

    public class EstimationMethod : Entity<int>, IAggregateRoot<int>
    {
        protected EstimationMethod()
        {
        }

        public EstimationMethodName Name { get; protected set; }
        public ICollection<EstimationMethodPossibleValue> PossibleValues { get; protected set; }

        public static EstimationMethod CreateNew(EstimationMethodName name)
        {
            return new EstimationMethod()
            {
                Name = name
            };
        }

        public void AddPossibleValue(EstimationMethodValue value)
        {
            DomainException.ThrowIf(
                PossibleValues.Any(x => x.EstimationMethodValue == value),
                $"Estimation method already contains value {value}");

            PossibleValues.Add(EstimationMethodPossibleValue.CreateNew(Id, value));
        }
    }
}
