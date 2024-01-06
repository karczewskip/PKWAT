namespace PKWAT.ScoringPoker.Server.BackgroundServices
{
    using Microsoft.AspNetCore.SignalR;
    using PKWAT.ScoringPoker.Server.Hubs;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskChangesNotifier : BackgroundService
    {
        private static readonly TimeSpan Period = TimeSpan.FromSeconds(5);

        private readonly IHubContext<LiveEstimationHub, ILiveEstimationClient> _hubContext;
        public TaskChangesNotifier(IHubContext<LiveEstimationHub, ILiveEstimationClient> hubContext)
        {
            _hubContext = hubContext;
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
    }
}
