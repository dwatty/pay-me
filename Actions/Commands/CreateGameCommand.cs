using MediatR;
using Orleans;
using PayMe.Grains;

namespace PayMe.Commands
{
    public class CreateGameCommand : IRequest<Guid>
    {   
        public Guid PlayerId { get; set; }
    }

    public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, Guid>
    {
        private readonly ILogger<CreateGameCommand> _logger;
        private readonly IGrainFactory _grainFactory;

        public CreateGameCommandHandler(
            ILogger<CreateGameCommand> logger,
            IGrainFactory grainFactory)
        {
            _logger = logger;
            _grainFactory = grainFactory;
        }

        public async Task<Guid> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            var player = _grainFactory.GetGrain<IPlayerGrain>(request.PlayerId);
            var gameId = await player.CreateGame();
            return gameId;
        }
    }

}