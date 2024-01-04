namespace PKWAT.ScoringPoker.Contracts.EstimationMethods
{
    using System.Collections.Generic;

    public class GetEstimationMethodsResponse
    {
        public IEnumerable<EstimationMethodDto> EstimationMethods { get; set; }
    }
}
