namespace PKWAT.ScoringPoker.Server.Extensions
{
    using PKWAT.ScoringPoker.Contracts.ScoringTasks;
    using PKWAT.ScoringPoker.Domain.ScoringTask.Entities;

    public static class Mapper
    {
        public static ScoringTaskDto ToDto(this ScoringTask scoringTask)
        {
            return new ScoringTaskDto
            {
                Id = scoringTask.Id,
                Name = scoringTask.Name.Name,
                Status = scoringTask.Status.ToDto(),
                FinalEstimation = scoringTask.FinalEstimationValue?.Value
            };
        }

        public static ScoringTaskStatusDto ToDto(this ScoringTaskStatusId scoringTaskStatus)
        {
            return scoringTaskStatus switch
            {
                ScoringTaskStatusId.Created => ScoringTaskStatusDto.Created,
                ScoringTaskStatusId.EstimationStarted => ScoringTaskStatusDto.EstimationStarted,
                ScoringTaskStatusId.EstimationFinished => ScoringTaskStatusDto.EstimationFinished,
                ScoringTaskStatusId.Approved => ScoringTaskStatusDto.Approved,
                _ => throw new ArgumentOutOfRangeException(nameof(scoringTaskStatus), scoringTaskStatus, null)
            };
        }
    }
}
