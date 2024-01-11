namespace PKWAT.ScoringPoker.Contracts.LiveEstimation
{
    using System.Threading.Tasks;

    public interface ILiveEstimationHub
    {
        Task ObserveScoringTask(int scoringTaskId);
        Task StartEstimating();
        Task AppendEstimation(int optionId);
        Task Estimate(int optionId);
    }
}
