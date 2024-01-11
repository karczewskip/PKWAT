namespace PKWAT.ScoringPoker.Contracts.LiveEstimation
{
    public class LiveEstimationScoringTaskStatusDto
    {
        public required string ScoringTaskName { get; set; }
        public required string ScoringTaskStatus { get; set; }
        public required DateTime? ScoringTaskStatusDueTo { get; set; }
        public required string ScoringTaskOwner { get; set; }
        public required string ScoringTaskEstimationMethod { get; set; }
        public required LiveEstimationScoringTaskEstimationMethodPossibleValueDto[] ScoringTaskEstimationMethodPossibleValues { get; set; }
        public required string? ScoringTaskFinalEstimationMethod { get; set; }
        public required string? ScoringTaskFinalValue { get; set; }
        public required LiveEstimationUserEstimationDto[] UsersEstimations { get; set; }

        public required bool CanBeStarted { get; set; }
        public required bool CanAppendUserEstimation { get; set; }
        public required bool CanShowUserEstimationValues { get; set; }
        public required bool CanBeApprovedByOwner { get; set; }
        public required bool CanShowFinalEstimationValue { get; set; }
    }
}
