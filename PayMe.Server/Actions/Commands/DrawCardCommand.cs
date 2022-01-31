using MediatR;
using Orleans;
using PayMe.Shared.Interfaces;
using PayMe.Shared.Models;

namespace PayMe.Server.Commands;

public class DrawCardCommand : CommandQueryBase, IRequest<Card> { }

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
        var drawnCard = await gameGrain.DrawCard(request.PlayerId);
        return drawnCard;
    }
}