using MediatR;
using Orleans.Hosting;
using PayMe.Server.Hubs;
using PayMe.Server.Services;
using PayMe.Shared.Infrastructure;
using PayMe.Shared.Interfaces;
using PayMe.Shared;
using Orleans;
using PayMe.Grains;
using StackExchange.Redis;

await Host.CreateDefaultBuilder(args)
    .UseOrleans((ctx, siloBuilder) =>
    {
        siloBuilder.ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(GameGrain).Assembly).WithReferences());

        if (ctx.HostingEnvironment.IsDevelopment())
        {
            siloBuilder.UseDashboard(options =>
             {
                 options.Username = "local";
                 options.Password = "local";
                 options.Host = "*";
                 options.Port = 8080;
                 options.HostSelf = true;
             });

            siloBuilder.UseLocalhostClustering();
            siloBuilder.AddMemoryGrainStorage(Constants.GAME_GRAIN_STORAGE_NAME);
            siloBuilder.AddMemoryGrainStorage(Constants.PLAYER_GRAIN_STORAGE_NAME);
            siloBuilder.AddMemoryGrainStorage(Constants.PAIRING_GRAIN_STORAGE_NAME);            
        }
        else
        {
            // In Kubernetes, we use environment variables and the pod manifest
            siloBuilder.UseKubernetesHosting();

            // Use Redis for clustering & persistence
            var redisConnectionString = $"{Environment.GetEnvironmentVariable("REDIS")}:6379";
            siloBuilder.UseRedisClustering(options => options.ConnectionString = redisConnectionString);             
            siloBuilder.AddRedisGrainStorage(Constants.GAME_GRAIN_STORAGE_NAME, options => options.ConnectionString = redisConnectionString); 
            siloBuilder.AddRedisGrainStorage(Constants.PLAYER_GRAIN_STORAGE_NAME, options => options.ConnectionString = redisConnectionString); 
            siloBuilder.AddRedisGrainStorage(Constants.PAIRING_GRAIN_STORAGE_NAME, options => options.ConnectionString = redisConnectionString); 
        }

        
    })
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    })    
    .RunConsoleAsync();

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {        
        var useBackplane = Configuration.GetValue<bool>("SignalR:UseBackplane");
        if(useBackplane) 
        {
            var redisConnectionString = $"{Environment.GetEnvironmentVariable("REDIS")}:6379";
            services.AddSignalR(hubOptions => 
            {
                hubOptions.EnableDetailedErrors = true;
            })
            .AddStackExchangeRedis(redisConnectionString, options => 
            {
                options.Configuration.ChannelPrefix = "PayMeApp";
                options.ConnectionFactory = async writer =>
                    {
                        var config = new ConfigurationOptions
                        {
                            AbortOnConnectFail = false,
                            Password = ""
                        };

                        Console.WriteLine("[REDIS] :: Configuring Endpoint...");
                        config.EndPoints.Add(Environment.GetEnvironmentVariable("REDIS"), 6379);
                        config.ResolveDns = true;
                        config.SetDefaultPorts();
                                                
                        var connection = await ConnectionMultiplexer.ConnectAsync(config, writer);
                        connection.ConnectionFailed += (_, e) =>
                        {
                            Console.WriteLine("[REDIS] :: Connection to Redis failed.");
                        };

                        if (!connection.IsConnected)
                        {
                            Console.WriteLine("[REDIS] :: Did not connect to Redis.");
                        }
                        else
                        {
                            Console.WriteLine("[REDIS] :: Connected to Redis!");
                        }

                        return connection;
                    };
            });
        }
        else
        {
            services.AddSignalR(hubOptions => 
            {
                hubOptions.EnableDetailedErrors = true;
            });
        }
            
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins", builder =>
            {
                builder.WithOrigins("http://localhost:3000");
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
                builder.AllowCredentials();
            });
        });

        services.AddTransient<IDeck, StandardDeck>();
        services.AddSingleton<IClientNotification, ClientNotifcation>();            
        services.AddMediatR(typeof(Startup));
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseCors("AllowAllOrigins");

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GameHub>("/hubs/gamehub");
            });

    }
}