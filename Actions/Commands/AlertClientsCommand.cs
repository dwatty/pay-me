using MediatR;
using Microsoft.AspNetCore.SignalR;
using PayMe.Hubs;

namespace PayMe.Commands
{
    public class AlertClientsCommand : IRequest
    {
        public string Message { get; set; } = "";
    }

    public class AlertClientsCommandHandler : IRequestHandler<AlertClientsCommand, Unit>
    {
        private readonly IHubContext<GameHub> _gameHubContext;
        
        public AlertClientsCommandHandler(IHubContext<GameHub> gameHubContext)
        {
            _gameHubContext = gameHubContext;
        }

        public async Task<Unit> Handle(AlertClientsCommand request, CancellationToken cancellationToken)
        {
            await _gameHubContext
                .Clients
                .All
                .SendAsync(
                    "ConnectivityTest", 
                    "Connectivity test in progress, testing testing 1 2 3"
                );

            return Unit.Value;
        }
    }
}