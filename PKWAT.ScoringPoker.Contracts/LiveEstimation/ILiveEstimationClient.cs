namespace PKWAT.ScoringPoker.Contracts.LiveEstimation
{
    public interface ILiveEstimationClient
    {
        Task ReceiveScoringTaskStatus(LiveEstimationScoringTaskStatusDto statusDto);
    }
}
