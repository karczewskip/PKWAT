namespace PKWAT.ScoringPoker.Domain.ScoringTask.ValueObjects
{
    using PKWAT.ScoringPoker.Domain.Abstraction;
    using System.Collections.Generic;

    public class ScoringTaskName : ValueObject
    {
        public string Name { get; protected set; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
        }
    }
}
