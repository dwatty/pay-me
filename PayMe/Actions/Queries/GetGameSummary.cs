using MediatR;
using Orleans;
using PayMe.Commands;
using PayMe.Grains;

namespace PayMe.Queries;

public class GetGameSummaryQuery : CommandQueryBase, IRequest<GameSummary> {}

public class GetGameSummaryQueryHandler : IRequestHandler<GetGameSummaryQuery, GameSummary>
{
    private readonly ILogger<GetGameSummaryQuery> _logger;
    private readonly IGrainFactory _grainFactory;
    
    public GetGameSummaryQueryHandler(
        ILogger<GetGameSummaryQuery> logger,
        IGrainFactory grainFactory)
    {
        _logger = logger;
        _grainFactory = grainFactory;
    }
    public async Task<GameSummary> Handle(GetGameSummaryQuery request, CancellationToken cancellationToken)
    {
        var gameGrain = _grainFactory.GetGrain<IGameGrain>(request.GameId);
        var summary = await gameGrain.GetSummary(request.PlayerId);
        return summary;
    }
}