import { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

import "./Game.css";
import { useParams } from "react-router-dom";
import { GameService } from "../../service";
import { GameSummary } from "../../models/game-summary";
import { GameState } from "../../models/enums";
import { CardComponent } from "../card/CardComponent";
import { Card } from "../../models/card";
import { CardBack } from "../card/CardBack";

export const Game = () => {

    const { gameId } = useParams() as any;
    const [service] = useState(new GameService());
    const [connection, setConnection] = useState<HubConnection>();
    const [connected, setConnected] = useState(false);
    const [game, setGame] = useState<GameSummary | undefined>(undefined);

    // Local State
    const [hasSeenNext, setHasSeenNext] = useState(false);
    const [drawnCard, setDrawnCard] = useState<Card|undefined>(undefined);

    async function getSummary() {
        const result = await service.getGameSummary(gameId);
        if (result) {
            setGame(result);
        }
    }

    useEffect(() => {
        getSummary();

        const newConnection = new HubConnectionBuilder()
            .configureLogging(signalR.LogLevel.Debug)
            .withUrl("hubs/gamehub", {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets,
            })
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);

        return function () {
            console.log("Connection Closed");
            connection?.stop();
        };
    }, []);

    useEffect(() => {
        if (connection) {
            connection
                .start()
                .then((result) => {
                    console.log("Connected!");

                    setConnected(true);

                    // Join the current Game Group on the Server
                    connection
                        .send("JoinGame", gameId)
                        .then(() => {
                            console.log("JoinGame Called");
                        })
                        .catch(function (err) {
                            return console.error(err.toString());
                        });

                    // This user is already in the game but waiting
                    // on someone to join.  When player 2 joins, it
                    // should trigger this message and cause player 1
                    // to get their summary
                    connection.on("GameStarted", () => {
                        console.log("Received GameStarted");
                        getSummary();
                    });

                    // A Generic Connectivity Test
                    connection.on("ConnectivityTest", (message: string) => {
                        console.log("Received ConnectivityTest");
                        window.alert(message);
                    });
                })
                .catch((e) => {
                    console.log("Connection failed: ", e);
                });
        }
    }, [connection]);

    const drawCard = async () => {

        const result = await service.drawCard(gameId);
        if(result) {
            setDrawnCard(result);
            setHasSeenNext(true);
        }

    }

    return game && game.state === GameState.InPlay ? (
        <>
            <div className="status-bar">
                <div>
                        Status:
                        <span className={connected ? "connected" : "not-connected"}>
                            {connected ? " Connected" : " Disconnected"}
                        </span>
                    </div>
                    <div>
                        Turn:
                        <span className={game.yourMove ? "connected" : "not-connected"}>
                            { game.yourMove ? " Your Turn" : " Waiting for Your Turn"}
                        </span>
                    </div>
            </div>
            
            <div className="playing-surface">
                <div className="center-column">
                    <h1>Deck</h1>

                    {
                        ! game.yourMove
                            ? <CardBack />
                            : hasSeenNext && drawnCard
                                ? <CardComponent 
                                    suite={ drawnCard.suite } 
                                    value={ drawnCard?.value } 
                                    key={1} 
                                />
                                : <CardBack onClick={ drawCard } />
                    }
                </div>

                <div className="center-column">
                    <h1>Discard</h1>
                    {
                        game.lastDiscard 
                            ? <CardComponent 
                                suite={game.lastDiscard.suite} 
                                value={game.lastDiscard.value} 
                                key={0} 
                            />
                            : null
                    }
                </div>
            </div>

            <div>
                <h1>Your Cards</h1>
            </div>
            <div className="my-cards">
                {
                    game.hand.map((itm: any, idx: number) => 
                        <CardComponent 
                            suite={itm.suite} 
                            value={itm.value} 
                            key={idx} 
                        />
                    )
                }
            </div>
        </>
    ) : (
        <div>
            <h1>Waiting on Player 2 to Join!</h1>
        </div>
    );
};
