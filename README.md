# Pay Me

A card game to play around with the Orleans framework.  The client is written in React and the backend is in C# using the Orleans framework.

## Tech Stack
* Orleans
* WebAPI
* React
* SignalR
* xUnit

# High Level Overview
A user set's their name which creates a player grain in Orleans.

Once in the lobby, the player can create a game which is also represented by a grain in Orleans.

A second player can join that game, at which point the game is considered to be started and the back and forth beings.

A user draws/discards cards from the deck or the discard pile, they can claim a win for the round and end a turn.  All of those actions are done with API requests.  The server will broadcast SignalR messages to the connected clients to keep state in sync across sessions.

## Docker

### PayMe.Client
Regular localized development can be done via
```
npm start
```

The app can be deployed to a container, with the src directory mounted to a volume and then development can be done in the conatiner.  The can be running the docker-compose file:
```
docker-compose -f docker-compose.yaml up
```



### PayMe.Server
The server's Dockerfile is built from the root level script `server-build` which will also deploy the app + redis to your local k8s environment.
```
./server-build.sh
```

After it's deployed, you can see the dashboard by doing the following: 
```
dashboard start
kubectl proxy
```

Visit and paste in the token
* http://localhost:8001/api/v1/namespaces/kubernetes-dashboard/services/https:kubernetes-dashboard:/proxy/#/workloads?namespace=default
