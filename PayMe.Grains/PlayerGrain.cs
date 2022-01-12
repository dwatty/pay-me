using Orleans;
using PayMe.Shared.Enums;
using PayMe.Shared.Models;
using PayMe.Shared.Interfaces;
using PayMe.Shared.ViewModels;
using PayMe.Shared;
using Orleans.Runtime;
using Payme.Shared;

namespace PayMe.Grains;

public class PlayerGrain : Grain, IPlayerGrain
{
    private readonly IPersistentState<PlayerGrainState> _player;
    private readonly IClientNotification _clientNotifcation;

    private int _wins;
    private int _loses;

    public PlayerGrain(
        IClientNotification clientNotifcation,
        [PersistentState("player", Constants.PLAYER_GRAIN_STORAGE_NAME)] IPersistentState<PlayerGrainState> player)
    {
        _clientNotifcation = clientNotifcation;
        _player = player;
    }

    public async Task<PairingSummary[]> GetAvailableGames()
    {
        var pairingGrain = GrainFactory.GetGrain<IPairingGrain>(0);
        var allGames = await pairingGrain.GetGames();

        return allGames
            .Where(x => !_player.State.ActiveGames.Contains(x.GameId))
            .ToArray();
    }

    // create a new game, and add oursleves to that game
    public async Task<Guid> CreateGame()
    {
        _player.State.GamesStarted += 1;

        var gameId = Guid.NewGuid();
        var gameGrain = GrainFactory.GetGrain<IGameGrain>(gameId);  // create new game

        // add ourselves to the game
        var playerId = this.GetPrimaryKey();  // our player id

        await gameGrain.AddPlayerToGame(playerId);
        _player.State.ActiveGames.Add(gameId);
        // _activeGames.Add(gameId);
        
        var name = _player.State.Username + "'s " + AddOrdinalSuffix(_player.State.GamesStarted.ToString()) + " game";
        await gameGrain.SetName(name);

        var pairingGrain = GrainFactory.GetGrain<IPairingGrain>(0);
        await pairingGrain.AddGame(gameId, name);

        await _clientNotifcation.GameCreated();
        await _player.WriteStateAsync();

        return gameId;
    }

    // join a game that is awaiting players
    public async Task<GameState> JoinGame(Guid gameId)
    {
        var gameGrain = GrainFactory.GetGrain<IGameGrain>(gameId);

        var state = await gameGrain.AddPlayerToGame(this.GetPrimaryKey());
        _player.State.ActiveGames.Add(gameId);

        var pairingGrain = GrainFactory.GetGrain<IPairingGrain>(0);
        await pairingGrain.RemoveGame(gameId);

        await _player.WriteStateAsync();

        return state;
    }

    // leave game when it is over
    public async Task LeaveGame(Guid gameId, GameOutcome outcome)
    {
        _player.State.ActiveGames.Remove(gameId);
        _player.State.PastGames.Add(gameId);

        // manage running total
        switch (outcome)
        {
            case GameOutcome.Win:
                _wins++;
                break;
            case GameOutcome.Lose:
                _loses++;
                break;
        }

        await _player.WriteStateAsync();
    }

    public async Task<List<GameListViewModel>> GetGameSummaries()
    {        
        var tasks = new List<Task<GameListViewModel>>();
        foreach (var gameId in _player.State.ActiveGames)
        {
            var game = GrainFactory.GetGrain<IGameGrain>(gameId);
            tasks.Add(game.GetSummary(this.GetPrimaryKey()));
        }

        await Task.WhenAll(tasks);
        return tasks.Select(x => x.Result).ToList();
    }

    public async Task SetUsername(string name)
    {
        _player.State.Username = name;
        await _player.WriteStateAsync();
    }

    public Task<string> GetUsername() => Task.FromResult(_player.State.Username);

    private static string AddOrdinalSuffix(string number)
    {
        var n = int.Parse(number);
        var nMod100 = n % 100;

        return nMod100 switch
        {
            >= 11 and <= 13 => string.Concat(number, "th"),
            _ => (n % 10) switch
            {
                1 => string.Concat(number, "st"),
                2 => string.Concat(number, "nd"),
                3 => string.Concat(number, "rd"),
                _ => string.Concat(number, "th"),
            }
        };
    }

    public Task<List<Guid>> GetActiveGames()
    {
        return Task.FromResult(_player.State.ActiveGames);
    }
}