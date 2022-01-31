using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Orleans;
using Orleans.Hosting;
using Orleans.TestingHost;
using PayMe.Server.Hubs;
using PayMe.Shared;
using PayMe.Shared.Interfaces;
using Xunit;

namespace PayMe.Tests;

public class ClusterFixture : IDisposable
{
    public TestCluster Cluster { get; }

    public ClusterFixture()
    {
        var builder = new TestClusterBuilder();
        builder.AddSiloBuilderConfigurator<TestSiloConfigurations>();

        Cluster = builder.Build();
        Cluster.Deploy();
    }

    public void Dispose() => Cluster.StopAllSilos();

    private class TestSiloConfigurations : ISiloConfigurator
    {        
        public void Configure(ISiloBuilder siloBuilder)
        {
            // Setup our in-memory grain storage
            siloBuilder.AddMemoryGrainStorage(Constants.GAME_GRAIN_STORAGE_NAME);
            siloBuilder.AddMemoryGrainStorage(Constants.PLAYER_GRAIN_STORAGE_NAME);
            siloBuilder.AddMemoryGrainStorage(Constants.PAIRING_GRAIN_STORAGE_NAME);

            // Mock up a SignalR Hub/Context for DI
            var mockHub = new Mock<IGameHub>();

            var mockClients = new Mock<IHubClients<IGameHub>>();
            mockClients.Setup(clients => clients.All).Returns(mockHub.Object);

            var mockHubContext = new Mock<IHubContext<GameHub, IGameHub>>();
            mockHubContext.Setup(x => x.Clients).Returns(() => mockClients.Object);

            // Provide our fakes and mocks for resolution
            siloBuilder.ConfigureServices(service =>
            {
                service.AddSingleton<IHubContext<GameHub, IGameHub>>(mockHubContext.Object);
                service.AddSingleton<IDeck, FakeDeck>();
                service.AddSingleton<IClientNotification, FakeClientNotification>();
            });
        }
    }
    
}

[CollectionDefinition(ClusterCollection.Name)]
public class ClusterCollection : ICollectionFixture<ClusterFixture>
{
    public const string Name = "PayMeClusterCollection";
}