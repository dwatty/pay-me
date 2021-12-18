# Pay Me



# High Level Flow

A player clicks create game
* `CreateGame` is called on `GameController`
* That uses `CreateGameCommand`
* That will use `PlayerGrain` to called `CreateGame`
* That will make a new `GameGrain` and add itself to that game
* The user navigates to the `Game.tsx` page, connects to SignalR and waits


A player clicks join on existing game
* `GameController` `Join` takes a Game ID
* That uses `JoinGameCommand`
* That will call `JoinGame` on the `PlayerGrain`
* `JoinGame` will get the `GameGrain` and add the new player to it
* This time, `JoinGame` will see 2 players and publish a message to SignalR to start the game