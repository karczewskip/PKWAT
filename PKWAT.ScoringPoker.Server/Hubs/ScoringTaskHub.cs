namespace PKWAT.ScoringPoker.Server.Hubs
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class ScoringTaskHub : Hub<IScoringTaskClient>
    {
        public override async Task OnConnectedAsync()
        {
            Clients.Client(Context.ConnectionId).ReceiveNotification(
                $"Thank you connecting {Context.User?.Identity?.Name}");

            await base.OnConnectedAsync();
        }

        public async Task ObserveScoringTask(int scoringTask)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, scoringTask.ToString());

            await Clients
                .Group(scoringTask.ToString())
                .ReceiveNotification($"{Context.User?.Identity?.Name} joined to task {scoringTask}");
        }
    }

    public interface IScoringTaskClient
    {
        Task ReceiveNotification(string message);
    }
}
