namespace PKWAT.ScoringPoker.Server.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using PKWAT.ScoringPoker.Contracts.LiveEstimation;
    using PKWAT.ScoringPoker.Domain.ScoringTask.Entities;
    using PKWAT.ScoringPoker.Server.Data;

    [Authorize]
    public class LiveEstimationHub : Hub<ILiveEstimationClient>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILiveEstimationObserversInMemoryStore _liveEstimationObserversInMemoryStore;

        public LiveEstimationHub(ApplicationDbContext dbContext, ILiveEstimationObserversInMemoryStore liveEstimationObserversInMemoryStore)
        {
            _dbContext = dbContext;
            _liveEstimationObserversInMemoryStore = liveEstimationObserversInMemoryStore;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).ReceiveNotification(
                $"Thank you connecting {Context.User?.Identity?.Name}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var observer = _liveEstimationObserversInMemoryStore.GetObserver(Context.ConnectionId);
            if (observer != null)
            {
                await Clients
                    .Group(observer.ScoringTaskId.ToString())
                    .ReceiveNotification($"{observer.Name} left task {observer.ScoringTaskId}");
            }
            _liveEstimationObserversInMemoryStore.RemoveObserver(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task ObserveScoringTask(int scoringTaskId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, scoringTaskId.ToString());

            _liveEstimationObserversInMemoryStore.AddObserver(new LiveEstimationObserverInfo(Context.User?.Identity?.Name, Context.ConnectionId, scoringTaskId));

            await Clients
                .Group(scoringTaskId.ToString())
                .ReceiveNotification($"{Context.User?.Identity?.Name} joined to task {scoringTaskId}");

            var scoringTask = await _dbContext.ScoringTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == scoringTaskId);

            await Clients.Group(scoringTaskId.ToString()).ReceiveScoringTaskStatus(new LiveEstimationScoringTaskStatusDto
            {
                ScoringTaskName = scoringTask.Name.Name,
                ScoringTaskStatus = scoringTask.Status.ToFriendlyString(),
                ScoringTaskObservers = _liveEstimationObserversInMemoryStore.GetObservers(scoringTaskId).Select(x => x.Name).ToArray()
            });
        }
    }

    public interface ILiveEstimationClient
    {
        Task ReceiveNotification(string message);
        Task ReceiveScoringTaskStatus(LiveEstimationScoringTaskStatusDto statusDto);
    }
}
