using MediatR;
using Orleans;
using PayMe.Grains;
using PayMe.Models;

namespace PayMe.Commands
{
    public class DrawCardCommand : IRequest<Card>
    {
        public Guid PlayerId { get; set; }
        public Guid GameId { get; set; }
    }

    public class DrawCardCommandHandler : IRequestHandler<DrawCardCommand, Card>
    {
        private readonly ILogger<DrawCardCommand> _logger;
        private readonly IGrainFactory _grainFactory;

        public DrawCardCommandHandler(
            ILogger<DrawCardCommand> logger,
            IGrainFactory grainFactory
        )
        {
            _logger = logger;
            _grainFactory = grainFactory;
        }
        public async Task<Card> Handle(DrawCardCommand request, CancellationToken cancellationToken)
        {
            var gameGrain = _grainFactory.GetGrain<IGameGrain>(request.GameId);
            var result = await gameGrain.DrawCard(request.PlayerId);
            return result;
        }
    }

}