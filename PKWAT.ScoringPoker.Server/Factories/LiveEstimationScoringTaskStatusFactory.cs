namespace PKWAT.ScoringPoker.Server.Factories
{
    using Microsoft.EntityFrameworkCore;
    using PKWAT.ScoringPoker.Contracts.LiveEstimation;
    using PKWAT.ScoringPoker.Server.Data;
    using PKWAT.ScoringPoker.Domain.ScoringTask.Entities;
    using PKWAT.ScoringPoker.Server.Hubs;

    public interface ILiveEstimationScoringTaskStatusFactory
    {
        Task<LiveEstimationScoringTaskStatusDto> GenerateStatusDtoForScoringTask(int scoringTaskId);
    }

    public class LiveEstimationScoringTaskStatusFactory : ILiveEstimationScoringTaskStatusFactory
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILiveEstimationObserversInMemoryStore _liveEstimationObserversInMemoryStore;

        public LiveEstimationScoringTaskStatusFactory(
            ApplicationDbContext dbContext, 
            ILiveEstimationObserversInMemoryStore liveEstimationObserversInMemoryStore)
        {
            _dbContext = dbContext;
            _liveEstimationObserversInMemoryStore = liveEstimationObserversInMemoryStore;
        }

        public async Task<LiveEstimationScoringTaskStatusDto> GenerateStatusDtoForScoringTask(int scoringTaskId)
        {
            var scoringTask = await _dbContext.ScoringTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == scoringTaskId);
            var owner = await _dbContext.Users.Where(x => x.Id == scoringTask.OwnerId).Select(x => x.UserName).FirstOrDefaultAsync();

            var statusDto = new LiveEstimationScoringTaskStatusDto
            {
                ScoringTaskName = scoringTask.Name.Name,
                ScoringTaskStatus = scoringTask.Status.ToFriendlyString(),
                ScoringTaskObservers = _liveEstimationObserversInMemoryStore.GetObservers(scoringTaskId).Select(x => x.UserName).ToArray(),
                ScoringTaskOwner = owner
            };

            return statusDto;
        }
    }
}
