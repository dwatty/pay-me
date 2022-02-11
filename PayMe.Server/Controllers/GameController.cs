using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using PayMe.Server.Commands;
using PayMe.Shared.Enums;
using PayMe.Shared.Models;
using PayMe.Server.Queries;
using PayMe.Shared.ViewModels;

namespace PayMe.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IGrainFactory _grainFactory;

    /// <summary>
    /// Constructor
    /// </summary>
    public GameController(IGrainFactory grainFactory, ISender mediator)
    {
        _grainFactory = grainFactory;
        _mediator = mediator;
    }

    /// <summary>
    /// Set username
    /// </summary>
    /// <param name="username">The username for the player</param>
    [HttpPost("setname")]
    public async Task<ActionResult<Guid>> SetUser([FromBody] string username)
    {
        var cmd = new SetUsernameCommand()
        {
            PlayerId = this.GetPlayerId(),
            Username = username
        };

        return await _mediator.Send(cmd);
    }

    /// <summary>
    /// Get the list of games in the system
    /// </summary>
    [HttpGet]
    [Route("info")]
    public async Task<ActionResult<GetGamesQueryResponse>> GetGames()
    {
        var cmd = new GetGamesQuery() { PlayerId = this.GetPlayerId() };
        return await _mediator.Send(cmd);
    }

    /// <summary>
    /// Create a new game as the current user
    /// </summary>
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> CreateGame()
    {
        var cmd = new CreateGameCommand() { PlayerId = this.GetPlayerId() };
        return await _mediator.Send(cmd);
    }

    /// <summary>
    /// Get the current user to join the provided game ID
    /// </summary>
    [HttpPut("join")]
    public async Task<ActionResult<GameState>> Join()
    {
        var cmd = new JoinGameCommand()
        {
            PlayerId = this.GetPlayerId(),
            GameId = this.GetGameId()
        };

        return await _mediator.Send(cmd);
    }

    /// <summary>
    /// Get the summary of the game for initializing client state
    /// </summary>
    [HttpGet("summary")]
    public async Task<PlayerGameSummary> GetGameSummary()
    {
        var cmd = new GetGameSummaryQuery()
        {
            PlayerId = this.GetPlayerId(),
            GameId = this.GetGameId()
        };

        return await _mediator.Send(cmd);
    }

    /// <summary>
    /// Draw the next card as part of a player's turn
    /// </summary>
    [HttpPost("drawcard")]
    public async Task<Card> DrawCard()
    {
        var cmd = new DrawCardCommand()
        {
            PlayerId = this.GetPlayerId(),
            GameId = this.GetGameId()
        };

        return await _mediator.Send(cmd);
    }

    /// <summary>
    /// Take the current discard as part of a player's turn
    /// </summary>
    [HttpPost("drawdiscard")]
    public async Task<object> DrawDiscard()
    {
        var cmd = new PickDiscardCommand()
        {
            PlayerId = this.GetPlayerId(),
            GameId = this.GetGameId()
        };

        return await _mediator.Send(cmd);
    }

    /// <summary>
    /// Take the current discard as part of a player's turn
    /// </summary>
    [HttpPut("discard")]
    public async Task Discard(DiscardCardCommand request)
    {            
        request.PlayerId = this.GetPlayerId();
        request.GameId = this.GetGameId();    
        await _mediator.Send(request);
    }

    /// <summary>
    /// Claim a win
    /// </summary>
    [HttpPut("claimwin")]
    public async Task<ClaimResult> ClaimWin([FromBody]List<List<Card>> handGroups)
    {
        var cmd = new ClaimWinCommand()
        {
            PlayerId = this.GetPlayerId(),
            GameId = this.GetGameId(),
            Groups = handGroups
        };

        return await _mediator.Send(cmd);
    }

    /// <summary>
    /// GEnd the player's turn
    /// </summary>
    [HttpPut("endturn")]
    public async Task EndTurn([FromBody]List<List<Card>> handGroups)
    {
        var cmd = new EndTurnCommand()
        {
            PlayerId = this.GetPlayerId(),
            GameId = this.GetGameId(),
            Groups = handGroups
        };

        await _mediator.Send(cmd);
    }

    /// <summary>
    /// Start the next round
    /// </summary>
    [HttpPut("nextround")]
    public async Task StartNextRound()
    {
        var cmd = new StartNextRoundCommand()
        {
            PlayerId = this.GetPlayerId(),
            GameId = this.GetGameId()
        };

        await _mediator.Send(cmd);
    }

    /// <summary>
    /// Get the game history
    /// </summary>
    [HttpGet("history")]
    public async Task<GameHistoryQueryResponse> GetGameHistory()
    {
        var cmd = new GetGameHistoryQuery()
        {
            PlayerId = this.GetPlayerId(),
            GameId = this.GetGameId()
        };

        return await _mediator.Send(cmd);
    }

}