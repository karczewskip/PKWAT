namespace PKWAT.ScoringPoker.Contracts.EstimationMethods
{
    using System.Collections.Generic;

    public class GetEstimationMethodWithValuesResponse
    {
        public EstimationMethodDto EstimationMethod { get; set; }
        public IEnumerable<EstimationMethodValueDto> EstimationMethodValues { get; set; }
    }
}
