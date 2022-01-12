using MediatR;
using Orleans;
using PayMe.Shared.Interfaces;

namespace PayMe.Server.Commands;

public class SetUsernameCommand : CommandQueryBase, IRequest<Guid>
{
    public string Username { get; set; } = "";
}

public class SetUserNameCommandHandler : IRequestHandler<SetUsernameCommand, Guid>
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

    public async Task<Guid> Handle(SetUsernameCommand request, CancellationToken cancellationToken)
    {
        var player = _grainFactory.GetGrain<IPlayerGrain>(request.PlayerId);
        await player.SetUsername(request.Username);
        return request.PlayerId;
    }
}