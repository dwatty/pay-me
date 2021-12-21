
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using PayMe.Commands;
using PayMe.Enums;
using PayMe.Grains;
using PayMe.Models;
using PayMe.Queries;

namespace PayMe.Controllers
{
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
        public async Task<ActionResult<bool>> SetUser([FromBody]string username)
        {
            var cmd = new SetUsernameCommand() 
            { 
                PlayerId = this.GetGuid(),
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
            var cmd = new GetGamesQuery() { PlayerId = this.GetGuid() };
            return await _mediator.Send(cmd);
        }

        //
        // Create a new game as the current user
        [HttpPost("create")]
        public async Task<ActionResult<Guid>> CreateGame()
        {
            var cmd = new CreateGameCommand() { PlayerId = this.GetGuid() };
            return await _mediator.Send(cmd);
        }

        // 
        // Get the current user to join the provided game ID
        [HttpPut("join/{id}")]
        public async Task<ActionResult<GameState>> Join(Guid id)
        {
            var cmd = new JoinGameCommand() 
            { 
                PlayerId = this.GetGuid(),
                GameId = id
            };

            return await _mediator.Send(cmd);
        }

        //
        // Test connectivity to clients
        [HttpPost("alertclients")]
        public async Task AlertClients()
        {
            var cmd = new AlertClientsCommand() { Message = "Testing 1 2 3"};
            await _mediator.Send(cmd);
        }

        //
        // Get the summary of the game for initializing client state
        [HttpGet("summary/{id}")]
        public async Task<GameSummary> GetGameSummary(Guid id)
        {
            var cmd = new GetGameSummaryQuery()
            {
                PlayerId = this.GetGuid(),
                GameId = id
            };

            return await _mediator.Send(cmd);
        }

        //
        // Draw the next card as part of a player's turn
        [HttpPost("drawcard/{id}")]
        public async Task<Card> DrawCard(Guid id)
        {
            var cmd = new DrawCardCommand()
            {
                PlayerId = this.GetGuid(),
                GameId = id
            };

            return await _mediator.Send(cmd);
        }

        //
        // Take the current discard as part of a player's turn
        [HttpPost("drawdiscard/{id}")]
        public async Task<object> DrawDiscard(Guid id)
        {
            var cmd = new PickDiscardCommand()
            {
                PlayerId = this.GetGuid(),
                GameId = id
            };

            return await _mediator.Send(cmd);
        }

        //
        // Take the current discard as part of a player's turn
        [HttpPut("discard/{id}")]
        public async Task Discard(Guid id, DiscardRequest request)
        {
            var cmd = new DiscardCardCommand()
            {
                PlayerId = this.GetGuid(),
                GameId = id,
                Suite = request.Suite,
                Value = request.Value
            };

            await _mediator.Send(cmd);
        }


       //
        // End the player's turn
        [HttpPut("endturn/{id}")]
        public async Task EndTurn(Guid id)
        {
            var cmd = new EndTurnCommand()
            {
                PlayerId = this.GetGuid(),
                GameId = id
            };

            await _mediator.Send(cmd);
        }










        [HttpGet("{id}/moves")]
        public async Task<IActionResult> GetMoves(Guid id)
        {
            var game = _grainFactory.GetGrain<IGameGrain>(id);
            var moves = await game.GetMoves();
            var summary = await game.GetSummary(this.GetGuid());
            return Ok(new { moves = moves, summary = summary });
        }

        [HttpPost("move")]
        public async Task<IActionResult> MakeMove(Guid id, int x, int y)
        {
            var game = _grainFactory.GetGrain<IGameGrain>(id);
           // var move = new GameMove { PlayerId = this.GetGuid(), X = x, Y = y };
            // var state = await game.MakeMove(move);
            //return Ok(state);
            return Ok();
        }

        [HttpGet("query")]
        public async Task<IActionResult> QueryGame(Guid id)
        {
            var game = _grainFactory.GetGrain<IGameGrain>(id);
            var state = await game.GetState();
            return Ok(state);
        }

        
    }
}