﻿namespace PKWAT.ScoringPoker.Server.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;
    using PKWAT.ScoringPoker.Contracts.LiveEstimation;
    using PKWAT.ScoringPoker.Domain.EstimationMethod.ValueObjects;
    using PKWAT.ScoringPoker.Server.Data;
    using PKWAT.ScoringPoker.Server.Factories;
    using PKWAT.ScoringPoker.Server.Stores;
    using System.Security.Claims;

    [Authorize]
    public class LiveEstimationHub : Hub<ILiveEstimationClient>, ILiveEstimationHub
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

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var observer = _liveEstimationObserversInMemoryStore.GetObserver(Context.ConnectionId);
            if (observer != null)
            {
                _liveEstimationObserversInMemoryStore.RemoveObserver(Context.ConnectionId);

                await SendInfoForAllObservers(observer.ScoringTaskId);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task ObserveScoringTask(int scoringTaskId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, scoringTaskId.ToString());

            _liveEstimationObserversInMemoryStore.AddObserver(new LiveEstimationObserverInfo(int.Parse(Context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value), Context.User?.Identity?.Name, Context.ConnectionId, scoringTaskId));

            await SendInfoForAllObservers(scoringTaskId);
        }

        public async Task StartEstimating()
        {
            var observer = _liveEstimationObserversInMemoryStore.GetObserver(Context.ConnectionId);
            if (observer is null)
            {
                return;
            }

            var scoringTask = await _dbContext.ScoringTasks.Include(x => x.UserEstimations).FirstOrDefaultAsync(x => x.Id == observer.ScoringTaskId);
            if (scoringTask is null)
            {
                return;
            }

            var user = await _userManager.FindByNameAsync(Context.User?.Identity?.Name);
            if (user is null)
            {
                return;
            }

            var time = DateTime.UtcNow;
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

            var time = DateTime.UtcNow;
            scoringTask.AppendEstimation(time, user.Id, scoringTask.EstimationMethodId, EstimationMethodValue.Create(value.EstimationMethodValue.Value));

            await _dbContext.SaveChangesAsync();

            await SendInfoForAllObservers(observer.ScoringTaskId);
        }

        public async Task Estimate(int optionId)
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
            if (value is null)
            {
                return;
            }

            scoringTask.Approve(user.Id, scoringTask.EstimationMethodId, EstimationMethodValue.Create(value.EstimationMethodValue.Value));

            await _dbContext.SaveChangesAsync();

            await SendInfoForAllObservers(observer.ScoringTaskId);
        }

        private async Task SendInfoForAllObservers(int scoringTaskId)
        {
            var statusDto = await _liveEstimationScoringTaskStatusFactory.GenerateStatusDtoForScoringTask(scoringTaskId);

            await Clients.Group(scoringTaskId.ToString()).ReceiveScoringTaskStatus(statusDto);
        }
    }
}
