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
            var scoringTask = await _dbContext
                .ScoringTasks
                .Include(x => x.EstimationMethod)
                .ThenInclude(x => x.PossibleValues)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == scoringTaskId);

            if(scoringTask is null)
            {
                throw new ArgumentException($"Scoring task with id {scoringTaskId} not exists.");
            }

            var owner = await _dbContext.Users.Where(x => x.Id == scoringTask.OwnerId).Select(x => x.UserName).FirstOrDefaultAsync();

            var statusDto = new LiveEstimationScoringTaskStatusDto
            {
                ScoringTaskName = scoringTask.Name.Name,
                ScoringTaskStatus = scoringTask.Status.ToFriendlyString(),
                ScoringTaskObservers = _liveEstimationObserversInMemoryStore.GetObservers(scoringTaskId).Select(x => x.UserName).ToArray(),
                ScoringTaskOwner = owner,
                ScoringTaskEstimationMethod = scoringTask.EstimationMethod.Name.Value,
                ScoringTaskEstimationMethodPossibleValues = scoringTask
                    .EstimationMethod
                    .PossibleValues
                    .Select(x => new LiveEstimationScoringTaskEstimationMethodPossibleValueDto() 
                    {
                        Id = x.Id,
                        Name = x.EstimationMethodValue.Value
                    }).ToArray(),
                UsersEstimations = scoringTask.TaskEstimations.Select(x => new LiveEstimationUserEstimationDto() { UserName = _liveEstimationObserversInMemoryStore.GetObserverByUserId(x.UserId).UserName, UserEstimation = scoringTask.CanShowUserEstimationValues() ? x.Value.Value : null }).ToArray(),
                CanBeStarted = scoringTask.CanBeStarted(),
                CanAppendUserEstimation = scoringTask.CanAppendUserEstimation(),
                CanShowUserEstimationValues = scoringTask.CanShowUserEstimationValues()
            };

            return statusDto;
        }
    }
}
