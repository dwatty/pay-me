using Orleans.Hosting;
using System.Net;
using MediatR;
using PayMe.Hubs;
using PayMe.Infrastructure;

await Host.CreateDefaultBuilder(args)
    .UseOrleans((ctx, siloBuilder) =>
    {
        // In order to support multiple hosts forming a cluster, they must listen on different ports.
        // Use the --InstanceId X option to launch subsequent hosts.
        var instanceId = ctx.Configuration.GetValue<int>("InstanceId");
        siloBuilder.UseLocalhostClustering(
            siloPort: 11111 + instanceId,
            gatewayPort: 30000 + instanceId,
            primarySiloEndpoint: new IPEndPoint(IPAddress.Loopback, 11111));
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
        webBuilder.ConfigureKestrel((ctx, kestrelOptions) =>
        {
            // To avoid port conflicts, each Web server must listen on a different port.
            var instanceId = ctx.Configuration.GetValue<int>("InstanceId");
            kestrelOptions.ListenLocalhost(5000 + instanceId);
        });
    })
    .RunConsoleAsync();

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        //services.AddCors();
        services.AddMediatR(typeof(Startup));
        services.AddSignalR(hubOptions => {
            hubOptions.EnableDetailedErrors = true;
        });

        services.AddTransient<IDeck, StandardDeck>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        // app.UseCors(builder =>
        // {
        //     builder.WithOrigins("https://localhost:5000/", "http://localhost:5000", "https://localhost:44486/", "http://localhost:44486/")
        //         .AllowAnyHeader()
        //         .AllowAnyMethod()
        //         .AllowCredentials();
        // });

        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();            
            endpoints.MapHub<GameHub>("/hubs/gamehub");
            endpoints.MapFallbackToFile("index.html");
        });

    }
}