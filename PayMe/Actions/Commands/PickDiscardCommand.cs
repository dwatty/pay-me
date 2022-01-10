using MediatR;
using Orleans;
using PayMe.Grains;

namespace PayMe.Commands;

public class PickDiscardCommand : CommandQueryBase, IRequest<object> { }

public class PickupDiscardCommandHandler : IRequestHandler<PickDiscardCommand, object>
{
    private readonly ILogger<PickDiscardCommand> _logger;
    private readonly IGrainFactory _grainFactory;

    public PickupDiscardCommandHandler(
        ILogger<PickDiscardCommand> logger,
        IGrainFactory grainFactory
    )
    {
        _logger = logger;
        _grainFactory = grainFactory;
    }

    public async Task<object> Handle(PickDiscardCommand request, CancellationToken cancellationToken)
    {
        // if take discard
            // pop off discard pile,
            // associate that card to user
        var gameGrain = _grainFactory.GetGrain<IGameGrain>(request.GameId);
        var card = await gameGrain.TakeDiscard(request.PlayerId);
        return card;
    }
}