using MediatR;
using Orleans;
using PayMe.Server.Commands;
using PayMe.Shared.Interfaces;
using PayMe.Shared.Models;
using PayMe.Shared.ViewModels;

namespace PayMe.Server.Queries;

public class GetGameSummaryQuery : CommandQueryBase, IRequest<PlayerGameSummary> {}

public class GetGameSummaryQueryHandler : IRequestHandler<GetGameSummaryQuery, PlayerGameSummary>
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
    public async Task<PlayerGameSummary> Handle(GetGameSummaryQuery request, CancellationToken cancellationToken)
    {
        var gameGrain = _grainFactory.GetGrain<IGameGrain>(request.GameId);
        var summary = await gameGrain.GetPlayerSummary(request.PlayerId);
        return summary;
    }
}