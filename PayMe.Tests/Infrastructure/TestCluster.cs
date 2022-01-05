using System;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Orleans.Hosting;
using Orleans.TestingHost;
using PayMe.Hubs;
using PayMe.Infrastructure;
using Xunit;

namespace PayMe.Tests;

public class ClusterFixture : IDisposable
{
    public ClusterFixture()
    {
        var builder = new TestClusterBuilder();
        builder.AddSiloBuilderConfigurator<TestSiloConfigurations>();
        this.Cluster = builder.Build();

        this.Cluster.Deploy();
    }

    public void Dispose()
    {
        this.Cluster.StopAllSilos();
    }

    public TestCluster Cluster { get; private set; }
}

[CollectionDefinition(ClusterCollection.Name)]
public class ClusterCollection : ICollectionFixture<ClusterFixture>
{
    public const string Name = "PayMeClusterCollection";
}

public class TestSiloConfigurations : ISiloConfigurator {
   
    public void Configure(ISiloBuilder siloBuilder)
    {
        var mockClientProxy = new Mock<IClientProxy>();

        var mockClients = new Mock<IHubClients>();
        mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

        var hub = new Mock<IHubContext<GameHub>>();
        hub.Setup(x => x.Clients).Returns(() => mockClients.Object);

        siloBuilder.ConfigureServices(service => {
            service.AddSingleton<IHubContext<GameHub>>(hub.Object);
            service.AddSingleton<IDeck, MockDeck>();
        });
    }
}
