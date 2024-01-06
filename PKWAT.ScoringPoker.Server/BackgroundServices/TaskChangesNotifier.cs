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

        private readonly IHubContext<LiveEstimationHub, ILiveEstimationClient> _hubContext;
        private readonly IServiceProvider _serviceProvider;

        public TaskChangesNotifier(
            IHubContext<LiveEstimationHub, ILiveEstimationClient> hubContext,
            IServiceProvider serviceProvider)
        {
            _hubContext = hubContext;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(Period);

            while(!stoppingToken.IsCancellationRequested
                && await timer.WaitForNextTickAsync(stoppingToken))
            {
                var dateTime = DateTime.Now;

                await _hubContext.Clients.All.ReceiveNotification($"The time is {dateTime}");

                await CheckDeadlines(dateTime);
            }
        }

        private async Task CheckDeadlines(DateTime currentTime)
        {
            using var scope = _serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var scoringTasksToUpdate = await dbContext.ScoringTasks.Where(x => x.Status == ScoringTaskStatusId.EstimationStarted && x.ScheduledEstimationFinish < currentTime).ToArrayAsync();

            if( scoringTasksToUpdate.Any() )
            {
                foreach (var scoringTask in scoringTasksToUpdate)
                {
                    scoringTask.FinishEstimation();
                }

                await dbContext.SaveChangesAsync();

                var liveEstimationScoringTaskStatusFactory = scope.ServiceProvider.GetRequiredService<ILiveEstimationScoringTaskStatusFactory>();

                foreach (var scoringTask in scoringTasksToUpdate)
                {
                    var statusDto = await liveEstimationScoringTaskStatusFactory.GenerateStatusDtoForScoringTask(scoringTask.Id);
                    await _hubContext.Clients.Groups(scoringTask.Id.ToString()).ReceiveScoringTaskStatus(statusDto);
                }
            }
        }
    }
}
