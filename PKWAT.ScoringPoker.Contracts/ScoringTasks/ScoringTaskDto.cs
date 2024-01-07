namespace PKWAT.ScoringPoker.Contracts.ScoringTasks
{
    public class ScoringTaskDto
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required ScoringTaskStatusDto Status { get; set; }
        public required string? FinalEstimation { get; set; }
    }

    public enum ScoringTaskStatusDto
    {
        Created,
        EstimationStarted,
        EstimationFinished,
        Approved
    }
}
