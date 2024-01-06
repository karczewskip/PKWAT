namespace PKWAT.ScoringPoker.Server.BackgroundServices
{
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using PKWAT.ScoringPoker.Domain.ScoringTask.Entities;
    using PKWAT.ScoringPoker.Server.Data;
    using PKWAT.ScoringPoker.Server.Factories;
    using PKWAT.ScoringPoker.Server.Hubs;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskChangesNotifier : BackgroundService
    {
        private static readonly TimeSpan Period = TimeSpan.FromSeconds(5);

        private readonly ApplicationDbContext _dbContext;

        private readonly IHubContext<LiveEstimationHub, ILiveEstimationClient> _hubContext;

        private readonly ILiveEstimationScoringTaskStatusFactory _liveEstimationScoringTaskStatusFactory;

        public TaskChangesNotifier(
            ApplicationDbContext dbContext ,
            IHubContext<LiveEstimationHub, ILiveEstimationClient> hubContext, 
            ILiveEstimationScoringTaskStatusFactory liveEstimationScoringTaskStatusFactory)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
            _liveEstimationScoringTaskStatusFactory = liveEstimationScoringTaskStatusFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(Period);

            while(!stoppingToken.IsCancellationRequested
                && await timer.WaitForNextTickAsync(stoppingToken))
            {
                var dateTime = DateTime.Now;

                await _hubContext.Clients.All.ReceiveNotification($"The time is {dateTime}");
            }
        }

        private async Task CheckDeadlines()
        {
            var currentTime = DateTime.Now;
            var scoringTasksToUpdate = await _dbContext.ScoringTasks.Where(x => x.Status == ScoringTaskStatusId.EstimationStarted && x.ScheduledEstimationFinish < currentTime).ToArrayAsync();

            foreach (var scoringTask in scoringTasksToUpdate)
            {
                scoringTask.FinishEstimation();
            }

            await _dbContext.SaveChangesAsync();

            foreach (var scoringTask in scoringTasksToUpdate)
            {
                var statusDto = await _liveEstimationScoringTaskStatusFactory.GenerateStatusDtoForScoringTask(scoringTask.Id);
                await _hubContext.Clients.Groups(scoringTask.Id.ToString()).ReceiveScoringTaskStatus(statusDto);
            }
        }
    }
}
