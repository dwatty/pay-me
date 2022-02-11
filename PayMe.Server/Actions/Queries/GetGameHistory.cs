using MediatR;
using Orleans;
using PayMe.Server.Commands;
using PayMe.Shared.Interfaces;
using PayMe.Shared.Models;

namespace PayMe.Server.Queries;

public class GameHistoryQueryResponse
{
    public List<GameEvent> History { get; set; } = new List<GameEvent>();
}

public class GetGameHistoryQuery : CommandQueryBase, IRequest<GameHistoryQueryResponse> {}

public class GetGameHistoryQueryHandler : IRequestHandler<GetGameHistoryQuery, GameHistoryQueryResponse>
{
    private readonly ILogger<GetGameHistoryQuery> _logger;
    private readonly IGrainFactory _grainFactory;

    public GetGameHistoryQueryHandler(
        ILogger<GetGameHistoryQuery> logger,
        IGrainFactory grainFactory)
    {
        _logger = logger;
        _grainFactory = grainFactory;
    }

    public async Task<GameHistoryQueryResponse> Handle(GetGameHistoryQuery request, CancellationToken cancellationToken)
    {
        var game = _grainFactory.GetGrain<IGameGrain>(request.GameId);
        var history = await game.GetGameHistory();
        return new GameHistoryQueryResponse
        {
            History = history
        };
    }
}