using MediatR;
using Orleans;
using PayMe.Grains;

namespace PayMe.Commands;

public class StartNextRoundCommand : CommandQueryBase, IRequest { }

public class StartNextRoundCommandHandler : IRequestHandler<StartNextRoundCommand>
{
    private readonly ILogger<StartNextRoundCommand> _logger;
    private readonly IGrainFactory _grainFactory;

    public StartNextRoundCommandHandler(
        ILogger<StartNextRoundCommand> logger,
        IGrainFactory grainFactory
    )
    {
        _logger = logger;
        _grainFactory = grainFactory;
    }

    public async Task<Unit> Handle(StartNextRoundCommand request, CancellationToken cancellationToken)
    {
        var gameGrain = _grainFactory.GetGrain<IGameGrain>(request.GameId);
        await gameGrain.StartNextRound();

        return await Task.FromResult(Unit.Value);
    }
}