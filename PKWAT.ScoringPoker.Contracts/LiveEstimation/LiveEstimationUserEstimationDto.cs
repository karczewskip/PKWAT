namespace PKWAT.ScoringPoker.Contracts.LiveEstimation
{
    public class LiveEstimationUserEstimationDto
    {
        public required string UserName { get; set; }
        public required bool EstimationAdded { get; set; }
        public required string? UserEstimation { get; set; }
    }
}
