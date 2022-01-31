using MediatR;
using Orleans;
using PayMe.Shared.Enums;
using PayMe.Shared.Interfaces;
using PayMe.Shared.Models;

namespace PayMe.Server.Commands;

public class ClaimWinCommand : CommandQueryBase, IRequest<ClaimResult>
{
    public List<List<Card>> Groups { get; set; } = new List<List<Card>>();
}

public class ClaimWinCommandHandler : IRequestHandler<ClaimWinCommand, ClaimResult>
{

    private readonly ILogger<ClaimWinCommand> _logger;
    private readonly IGrainFactory _grainFactory;

    public ClaimWinCommandHandler(
        ILogger<ClaimWinCommand> logger,
        IGrainFactory grainFactory
    )
    {
        _logger = logger;
        _grainFactory = grainFactory;
    }

    public async Task<ClaimResult> Handle(ClaimWinCommand request, CancellationToken cancellationToken)
    {
        var gameGrain = _grainFactory.GetGrain<IGameGrain>(request.GameId);
        return await gameGrain.ClaimWin(request.PlayerId, request.Groups);
    }
    
}