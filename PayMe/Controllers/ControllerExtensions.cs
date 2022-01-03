
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace PayMe.Controllers
{
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
            if (controller.Request.Cookies["playerId"] is { Length: > 0 } idCookie)
            {
                return Guid.Parse(idCookie);
            }

            var guid = Guid.NewGuid();
            controller.Response.Cookies.Append("playerId", guid.ToString());
            return guid;
        }
    }
}