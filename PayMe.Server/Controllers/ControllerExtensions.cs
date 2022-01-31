using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace PayMe.Server.Controllers;

public static class ControllerExtensions
{
    public static Guid GetGameId(this ControllerBase controller)
    {
        StringValues gameId = "";
        if(controller.Request.Headers.TryGetValue("X-GAME-ID", out gameId))
        {
            return Guid.Parse(gameId);
        }

        throw new Exception("Invalid Request");
    }

    public static Guid GetPlayerId(this ControllerBase controller)
    {
        StringValues playerId = "";
        if(controller.Request.Headers.TryGetValue("X-PLAYER-ID", out playerId))
        {
            if(StringValues.IsNullOrEmpty(playerId))
            {
                playerId = Guid.NewGuid().ToString();
            }

            return Guid.Parse(playerId);
        }

        throw new Exception("Invalid Request");
    }
}