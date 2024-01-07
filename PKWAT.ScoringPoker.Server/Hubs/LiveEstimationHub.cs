namespace PKWAT.ScoringPoker.Server.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using PKWAT.ScoringPoker.Contracts.LiveEstimation;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.ValueObjects;
    using PKWAT.ScoringPoker.Domain.ScoringTask.Entities;
    using PKWAT.ScoringPoker.Domain.ScoringTask.ValueObjects;
    using PKWAT.ScoringPoker.Server.Data;
    using PKWAT.ScoringPoker.Server.Factories;

    [Authorize]
    public class LiveEstimationHub : Hub<ILiveEstimationClient>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILiveEstimationObserversInMemoryStore _liveEstimationObserversInMemoryStore;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILiveEstimationScoringTaskStatusFactory _liveEstimationScoringTaskStatusFactory;

        public LiveEstimationHub(
            ApplicationDbContext dbContext, 
            ILiveEstimationObserversInMemoryStore liveEstimationObserversInMemoryStore,
            UserManager<ApplicationUser> userManager,
            ILiveEstimationScoringTaskStatusFactory liveEstimationScoringTaskStatusFactory)
        {
            _dbContext = dbContext;
            _liveEstimationObserversInMemoryStore = liveEstimationObserversInMemoryStore;
            _userManager = userManager;
            _liveEstimationScoringTaskStatusFactory = liveEstimationScoringTaskStatusFactory;
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
                    .ReceiveNotification($"{observer.UserName} left task {observer.ScoringTaskId}");

                _liveEstimationObserversInMemoryStore.RemoveObserver(Context.ConnectionId);

                await SendInfoForAllObservers(observer.ScoringTaskId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task ObserveScoringTask(int scoringTaskId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, scoringTaskId.ToString());

            _liveEstimationObserversInMemoryStore.AddObserver(new LiveEstimationObserverInfo(0 ,Context.User?.Identity?.Name, Context.ConnectionId, scoringTaskId));

            await Clients
                .Group(scoringTaskId.ToString())
                .ReceiveNotification($"{Context.User?.Identity?.Name} joined to task {scoringTaskId}");

            await SendInfoForAllObservers(scoringTaskId);
        }

        public async Task StartEstimating()
        {
            var observer = _liveEstimationObserversInMemoryStore.GetObserver(Context.ConnectionId);
            if (observer is null)
            {
                return;
            }

            var scoringTask = await _dbContext.ScoringTasks.FirstOrDefaultAsync(x => x.Id == observer.ScoringTaskId);
            if (scoringTask is null)
            {
                return;
            }

            var user = await _userManager.FindByNameAsync(Context.User?.Identity?.Name);
            if (user is null)
            {
                return;
            }

            var time = DateTime.Now;
            scoringTask.StartEstimation(user.Id, time, time.AddSeconds(10));

            await _dbContext.SaveChangesAsync();

            await SendInfoForAllObservers(observer.ScoringTaskId);
        }

        public async Task AppendEstimation(int optionId)
        {
            var observer = _liveEstimationObserversInMemoryStore.GetObserver(Context.ConnectionId);
            if (observer is null)
            {
                return;
            }

            var scoringTask = await _dbContext.ScoringTasks.Include(x => x.EstimationMethod).ThenInclude(x => x.PossibleValues).Include(x => x.UserEstimations).FirstOrDefaultAsync(x => x.Id == observer.ScoringTaskId);
            if (scoringTask is null)
            {
                return;
            }

            var user = await _userManager.FindByNameAsync(Context.User?.Identity?.Name);
            if (user is null)
            {
                return;
            }

            var value = scoringTask.EstimationMethod.PossibleValues.FirstOrDefault(x => x.Id == optionId);
            if(value is null)
            {
                return;
            }

            var time = DateTime.Now;
            scoringTask.AppendEstimation(time, user.Id, scoringTask.EstimationMethodId, EstimationMethodValue.Create(value.EstimationMethodValue.Value));

            await _dbContext.SaveChangesAsync();

            await SendInfoForAllObservers(observer.ScoringTaskId);
        }

        private async Task SendInfoForAllObservers(int scoringTaskId)
        {
            var statusDto = await _liveEstimationScoringTaskStatusFactory.GenerateStatusDtoForScoringTask(scoringTaskId);

            await Clients.Group(scoringTaskId.ToString()).ReceiveScoringTaskStatus(statusDto);
        }
    }

    public interface ILiveEstimationClient
    {
        Task ReceiveNotification(string message);
        Task ReceiveScoringTaskStatus(LiveEstimationScoringTaskStatusDto statusDto);
    }
}
