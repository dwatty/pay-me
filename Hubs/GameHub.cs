using Microsoft.AspNetCore.SignalR;

namespace PayMe.Hubs
{
    public class GameHub : Hub
    {
        private readonly ILogger<GameHub> _logger;
        private readonly string NOT_PLAYING = "Not Playing";

        public GameHub(ILogger<GameHub> logger)
        {   
            _logger = logger;
        }
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, NOT_PLAYING);
            await base.OnConnectedAsync();
        }

        public async Task JoinGame(Guid gameId)
        {
            _logger.LogDebug("Joined Game");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, NOT_PLAYING);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId.ToString());
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());   
        }

    }
}