namespace PKWAT.ScoringPoker.Contracts.ScoringTasks
{
    public class GetScoringTasksResponse
    {
        public IEnumerable<ScoringTaskDto> ScoringTasks { get; set; }
    }
}
