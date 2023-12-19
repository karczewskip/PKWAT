namespace PKWAT.ScoringPoker.Domain.EstimationMethod.ValueObjects
{
    using PKWAT.ScoringPoker.Domain.Abstraction;
    using System.Collections.Generic;

    public class EstimationMethodValue : ValueObject
    {
        public const int MaxLength = 30;
        public string Value { get; private set; }

        private EstimationMethodValue() { }

        public static EstimationMethodValue Create(string value)
        {
            DomainException.ThrowIf(
                string.IsNullOrWhiteSpace(value),
                "Estimation method value cannot be empty");

            DomainException.ThrowIf(
                value.Length > MaxLength,
                $"Estimation method value cannot be longer than {MaxLength} characters");

            return new EstimationMethodValue()
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
