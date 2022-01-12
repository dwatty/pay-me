using MediatR;
using Orleans;
using PayMe.Shared.Enums;
using PayMe.Shared.Interfaces;

namespace PayMe.Server.Commands;

public class DiscardCardCommand : CommandQueryBase, IRequest 
{
    public Suites Suite { get; set; }
    public int Value { get; set; }
}

public class DiscardCardCommandHandler : IRequestHandler<DiscardCardCommand>
{
    private readonly ILogger<DiscardCardCommand> _logger;
    private readonly IGrainFactory _grainFactory;

    public DiscardCardCommandHandler(
        ILogger<DiscardCardCommand> logger,
        IGrainFactory grainFactory
    )
    {
        _logger = logger;
        _grainFactory = grainFactory;
    }

    public async Task<Unit> Handle(DiscardCardCommand request, CancellationToken cancellationToken)
    {
        var gameGrain = _grainFactory.GetGrain<IGameGrain>(request.GameId);
        await gameGrain.DiscardCard(request.PlayerId, request.Suite, request.Value);
        return Unit.Value;
    }
}