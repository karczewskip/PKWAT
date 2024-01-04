namespace PKWAT.ScoringPoker.Contracts.EstimationMethods
{
    using System.Collections.Generic;

    public class GetEstimationMethodValuesResponse
    {
        public int EstimationMethodId { get; set; }
        public IEnumerable<EstimationMethodValueDto> EstimationMethodValues { get; set; }
    }
}
