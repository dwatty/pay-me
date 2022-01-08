using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using PayMe.Commands;
using PayMe.Enums;
using PayMe.Grains;
using PayMe.Models;
using PayMe.Queries;

namespace PayMe.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly IGrainFactory _grainFactory;

    //
    // Ctor
    public GameController(IGrainFactory grainFactory, ISender mediator)
    {
        _grainFactory = grainFactory;
        _mediator = mediator;
    }

    //
    // Set the user's name for their session
    [HttpPost("setname")]
    public async Task<ActionResult<bool>> SetUser([FromBody] string username)
    {
        var cmd = new SetUsernameCommand()
        {
            PlayerId = this.GetPlayerId(),
            Username = username
        };

        return await _mediator.Send(cmd);
    }

    //
    // Get the list of games in the system
    [HttpGet]
    [Route("info")]
    public async Task<ActionResult<CreateGameResponse>> GetGames()
    {
        var cmd = new GetGamesQuery() { PlayerId = this.GetPlayerId() };
        return await _mediator.Send(cmd);
    }

    //
    // Create a new game as the current user
    [HttpPost("create")]
    public async Task<ActionResult<Guid>> CreateGame()
    {
        var cmd = new CreateGameCommand() { PlayerId = this.GetPlayerId() };
        return await _mediator.Send(cmd);
    }

    // 
    // Get the current user to join the provided game ID
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

    //
    // Test connectivity to clients
    [HttpPost("alertclients")]
    public async Task AlertClients()
    {
        var cmd = new AlertClientsCommand() { Message = "Testing 1 2 3" };
        await _mediator.Send(cmd);
    }

    //
    // Get the summary of the game for initializing client state
    [HttpGet("summary")]
    public async Task<GameSummary> GetGameSummary()
    {
        var cmd = new GetGameSummaryQuery()
        {
            PlayerId = this.GetPlayerId(),
            GameId = this.GetGameId()
        };

        return await _mediator.Send(cmd);
    }

    //
    // Draw the next card as part of a player's turn
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

    //
    // Take the current discard as part of a player's turn
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

    //
    // Take the current discard as part of a player's turn
    [HttpPut("discard")]
    public async Task Discard(DiscardCardCommand request)
    {            
        request.PlayerId = this.GetPlayerId();
        request.GameId = this.GetGameId();    
        await _mediator.Send(request);
    }


    //
    // Claim a win
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

    //
    // End the player's turn
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









    // [HttpGet("{id}/moves")]
    // public async Task<IActionResult> GetMoves(Guid id)
    // {
    //     var game = _grainFactory.GetGrain<IGameGrain>(id);
    //     var moves = await game.GetMoves();
    //     var summary = await game.GetSummary(this.GetPlayerId());
    //     return Ok(new { moves = moves, summary = summary });
    // }

    // [HttpPost("move")]
    // public async Task<IActionResult> MakeMove(Guid id, int x, int y)
    // {
    //     var game = _grainFactory.GetGrain<IGameGrain>(id);
    //     // var move = new GameMove { PlayerId = this.GetGuid(), X = x, Y = y };
    //     // var state = await game.MakeMove(move);
    //     //return Ok(state);
    //     return Ok();
    // }

    // [HttpGet("query")]
    // public async Task<IActionResult> QueryGame(Guid id)
    // {
    //     var game = _grainFactory.GetGrain<IGameGrain>(id);
    //     var state = await game.GetState();
    //     return Ok(state);
    // }
}