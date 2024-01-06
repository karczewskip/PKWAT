namespace PKWAT.ScoringPoker.Server.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using PKWAT.ScoringPoker.Contracts.LiveEstimation;
    using PKWAT.ScoringPoker.Server.Data;

    [Authorize]
    public class LiveEstimationHub : Hub<ILiveEstimationClient>
    {
        private readonly ApplicationDbContext _dbContext;

        public LiveEstimationHub(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task OnConnectedAsync()
        {
            await Clients.Client(Context.ConnectionId).ReceiveNotification(
                $"Thank you connecting {Context.User?.Identity?.Name}");

            await base.OnConnectedAsync();
        }

        public async Task ObserveScoringTask(int scoringTaskId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, scoringTaskId.ToString());

            await Clients
                .Group(scoringTaskId.ToString())
                .ReceiveNotification($"{Context.User?.Identity?.Name} joined to task {scoringTaskId}");

            var scoringTask = await _dbContext.ScoringTasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id == scoringTaskId);

            await Clients.Client(Context.ConnectionId).ReceiveScoringTaskStatus(new LiveEstimationScoringTaskStatusDto
            {
                ScoringTaskName = scoringTask.Name.Name
            });
        }
    }

    public interface ILiveEstimationClient
    {
        Task ReceiveNotification(string message);
        Task ReceiveScoringTaskStatus(LiveEstimationScoringTaskStatusDto statusDto);
    }
}
