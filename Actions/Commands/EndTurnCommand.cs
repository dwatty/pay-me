using MediatR;
using Orleans;
using PayMe.Grains;

namespace PayMe.Commands
{
    public class EndTurnCommand : CommandQueryBase, IRequest { }

    public class EndTurnCommandHandler : IRequestHandler<EndTurnCommand>
    {
        private readonly ILogger<EndTurnCommand> _logger;
        private readonly IGrainFactory _grainFactory;

        public EndTurnCommandHandler(
            ILogger<EndTurnCommand> logger,
            IGrainFactory grainFactory
        )
        {
            _logger = logger;
            _grainFactory = grainFactory;            
        }

        public async Task<Unit> Handle(EndTurnCommand request, CancellationToken cancellationToken)
        {
            var gameGrain = _grainFactory.GetGrain<IGameGrain>(request.GameId);
            await gameGrain.EndTurn(request.PlayerId);
            return Unit.Value;
        }
    }

}