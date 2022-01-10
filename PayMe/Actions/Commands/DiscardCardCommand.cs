using MediatR;
using Microsoft.AspNetCore.SignalR;
using Orleans;
using PayMe.Enums;
using PayMe.Grains;
using PayMe.Hubs;

namespace PayMe.Commands;

public class DiscardCardCommand : CommandQueryBase, IRequest 
{
    public Suites Suite { get; set; }
    public int Value { get; set; }
}

public class DiscardCardCommandHandler : IRequestHandler<DiscardCardCommand>
{
    private readonly ILogger<DiscardCardCommand> _logger;
    private readonly IGrainFactory _grainFactory;
    private readonly IHubContext<GameHub> _hub;

    public DiscardCardCommandHandler(
        ILogger<DiscardCardCommand> logger,
        IGrainFactory grainFactory,
        IHubContext<GameHub> hub
    )
    {
        _logger = logger;
        _grainFactory = grainFactory;
        _hub = hub;
    }

    public async Task<Unit> Handle(DiscardCardCommand request, CancellationToken cancellationToken)
    {
        var gameGrain = _grainFactory.GetGrain<IGameGrain>(request.GameId);
        await gameGrain.DiscardCard(request.PlayerId, request.Suite, request.Value);
        return Unit.Value;
    }
}