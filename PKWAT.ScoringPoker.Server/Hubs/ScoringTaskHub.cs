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
    }

    public interface IScoringTaskClient
    {
        Task ReceiveNotification(string message);
    }
}
