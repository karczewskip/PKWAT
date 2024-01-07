namespace PKWAT.ScoringPoker.Contracts.LiveEstimation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class LiveEstimationScoringTaskStatusDto
    {
        public required string ScoringTaskName { get; set; }
        public required string ScoringTaskStatus { get; set; }
        public required string[] ScoringTaskObservers { get; set; }
        public required string ScoringTaskOwner { get; set; }
        public required string ScoringTaskEstimationMethod { get; set; }
        public required LiveEstimationScoringTaskEstimationMethodPossibleValueDto[] ScoringTaskEstimationMethodPossibleValues { get; set; }
        public required LiveEstimationUserEstimationDto[] UsersEstimations { get; set; }

        public required bool CanBeStarted { get; set; }
        public required bool CanAppendUserEstimation { get; set; }
        public required bool CanShowUserEstimationValues { get; set; }
    }

    public class LiveEstimationScoringTaskEstimationMethodPossibleValueDto 
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
    }

    public class LiveEstimationUserEstimationDto
    {
        public required string UserName { get; set; }
        public required string? UserEstimation { get; set; }
    }
}
