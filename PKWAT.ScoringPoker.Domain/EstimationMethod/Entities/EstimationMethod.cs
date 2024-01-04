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

        public EstimationMethodPossibleValue AddPossibleValue(EstimationMethodValue value)
        {
            DomainException.ThrowIf(
                PossibleValues.Any(x => x.EstimationMethodValue == value),
                $"Estimation method already contains value {value}");

            var newPossibleValue = EstimationMethodPossibleValue.CreateNew(Id, value);

            PossibleValues.Add(newPossibleValue);

            return newPossibleValue;
        }
    }
}
