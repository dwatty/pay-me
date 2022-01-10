using MediatR;
using Orleans;
using PayMe.Grains;

namespace PayMe.Commands;

public class SetUsernameCommand : CommandQueryBase, IRequest<bool>
{
    public string Username { get; set; } = "";
}

public class SetUserNameCommandHandler : IRequestHandler<SetUsernameCommand, bool>
{
    private readonly ILogger<SetUsernameCommand> _logger;
    private readonly IGrainFactory _grainFactory;

    public SetUserNameCommandHandler(
        ILogger<SetUsernameCommand> logger,
        IGrainFactory grainFactory)
    {
        _logger = logger;
        _grainFactory = grainFactory;
    }

    public async Task<bool> Handle(SetUsernameCommand request, CancellationToken cancellationToken)
    {
        var player = _grainFactory.GetGrain<IPlayerGrain>(request.PlayerId);
        await player.SetUsername(request.Username);
        return true;
    }
}