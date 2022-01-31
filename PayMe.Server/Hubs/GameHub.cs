using Microsoft.AspNetCore.SignalR;
using PayMe.Shared.Interfaces;

namespace PayMe.Server.Hubs;

public class GameHub : Hub<IGameHub>
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
        await Clients.All.PlayerCreated();
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
