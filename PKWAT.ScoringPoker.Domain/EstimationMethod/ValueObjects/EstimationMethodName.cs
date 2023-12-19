namespace PKWAT.ScoringPoker.Domain.EstimationMethod.ValueObjects
{
    using PKWAT.ScoringPoker.Domain.Abstraction;
    using System.Collections.Generic;

    public class EstimationMethodName : ValueObject
    {
        public const int MaxLength = 50;

        public string Value { get; private set; }

        private EstimationMethodName() { }

        public static EstimationMethodName Create(string value)
        {
            DomainException.ThrowIf(
                string.IsNullOrWhiteSpace(value),
                "Estimation method name cannot be empty");

            DomainException.ThrowIf(
                value.Length > MaxLength,
                $"Estimation method name cannot be longer than {MaxLength} characters");

            return new EstimationMethodName()
            {
                Value = value
            };
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
