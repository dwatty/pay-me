using MediatR;
using Orleans;
using PayMe.Grains;

namespace PayMe.Queries
{

    public class CreateGameResponse
    {
        public List<GameSummary> GameSummaries { get; set; }
        public PairingSummary[] AvailableGames { get; set; }
    }

    public class GetGamesQuery : IRequest<CreateGameResponse>
    {
        public Guid PlayerId { get; set; }
    }

    public class GetGamesQueryHandler : IRequestHandler<GetGamesQuery, CreateGameResponse>
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

        public async Task<CreateGameResponse> Handle(GetGamesQuery request, CancellationToken cancellationToken)
        {
            var player = _grainFactory.GetGrain<IPlayerGrain>(request.PlayerId);

            var gamesTask = player.GetGameSummaries();
            var availableTask = player.GetAvailableGames();
            await Task.WhenAll(gamesTask, availableTask);

            return new CreateGameResponse
            {
                GameSummaries = gamesTask.Result,
                AvailableGames = availableTask.Result
            };
        }
    }
}