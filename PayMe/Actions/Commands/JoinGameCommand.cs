using MediatR;
using Orleans;
using PayMe.Enums;
using PayMe.Grains;

namespace PayMe.Commands;

public class JoinGameCommand : CommandQueryBase, IRequest<GameState> { }

public class JoinGameCommandHandler : IRequestHandler<JoinGameCommand, GameState>
{
    private readonly ILogger<JoinGameCommand> _logger;
    private readonly IGrainFactory _grainFactory;

    public JoinGameCommandHandler(
        ILogger<JoinGameCommand> logger,
        IGrainFactory grainFactory)
    {
        _logger = logger;
        _grainFactory = grainFactory;
    }

    public async Task<GameState> Handle(JoinGameCommand request, CancellationToken cancellationToken)
    {
        var player = _grainFactory.GetGrain<IPlayerGrain>(request.PlayerId);

        var games = await player.GetActiveGames();
        if(games.Contains(request.GameId))
        {
            _logger.LogInformation("Already Joined");
            return GameState.InPlay;
        }

        return await player.JoinGame(request.GameId);
    }
}