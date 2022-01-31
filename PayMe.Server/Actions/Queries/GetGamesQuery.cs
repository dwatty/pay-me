using MediatR;
using Orleans;
using PayMe.Server.Commands;
using PayMe.Shared.Interfaces;
using PayMe.Shared.Models;
using PayMe.Shared.ViewModels;

namespace PayMe.Server.Queries;

public class GetGamesQueryResponse
{
    public List<GameListViewModel> GameSummaries { get; set; } = new List<GameListViewModel>();
    public PairingSummary[] AvailableGames { get; set; } = new PairingSummary[0];
}

public class GetGamesQuery : CommandQueryBase, IRequest<GetGamesQueryResponse> {}

public class GetGamesQueryHandler : IRequestHandler<GetGamesQuery, GetGamesQueryResponse>
{
    private readonly ILogger<GetGamesQuery> _logger;
    private readonly IGrainFactory _grainFactory;

    public GetGamesQueryHandler(
        ILogger<GetGamesQuery> logger,
        IGrainFactory grainFactory)
    {
        _logger = logger;
        _grainFactory = grainFactory;
    }

    public async Task<GetGamesQueryResponse> Handle(GetGamesQuery request, CancellationToken cancellationToken)
    {
        var player = _grainFactory.GetGrain<IPlayerGrain>(request.PlayerId);

        var gamesTask = player.GetGameSummaries();
        var availableTask = player.GetAvailableGames();
        await Task.WhenAll(gamesTask, availableTask);

        return new GetGamesQueryResponse
        {
            GameSummaries = gamesTask.Result,
            AvailableGames = availableTask.Result
        };
    }
}