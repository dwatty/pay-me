import { useEffect, useRef, useState } from "react";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { useParams } from "react-router-dom";
import { GameService } from "../../service";
import { GameSummary } from "../../models/game-summary";
import { GameState, RoundState, Suites, TurnState } from "../../models/enums";
import { CardComponent } from "../card/CardComponent";
import { Card } from "../../models/card";
import { CardBack } from "../card/CardBack";
import { useAppContext } from "../../context/context";
import { CardEmpty } from "../card/CardEmpty";
import { TurnBadge } from "./TurnBadge";
import { PlayerHand } from "./PlayerHand";

import "./Game.css";
import { Toast } from "../toast/Toast";

export const Game = () => {
    const { appState } = useAppContext();
    const { gameId } = useParams() as any;
    const [service] = useState(new GameService());
    const [connection, setConnection] = useState<HubConnection>();
    const [connected, setConnected] = useState(false);
    const [game, setGame] = useState<GameSummary>(new GameSummary());
    const [toasts, setToasts] = useState<any[]>([]);

    // Used as a static ref for signalR work
    const gameRef = useRef<GameSummary>();
    gameRef.current = game;

    // Local State
    const [hasSeenNext, setHasSeenNext] = useState(false);
    const [hasDiscarded, setHasDiscarded] = useState(false);
    const [handGroups, setHandGroups] = useState<Array<Card[]>>([]);
    const [showNextRoundBtn, setShowNextRoundBtn] = useState(false);

    async function getSummary() {
        const result = await service.getGameSummary(gameId);
        if (result) {
            setHandGroups([[...result.hand]]);

            setGame(result);

            switch (result.playerTurnState) {
                case TurnState.DrewCard:
                    setHasSeenNext(true);
                    setHasDiscarded(false);
                    break;
                case TurnState.Discarded:
                    setHasSeenNext(true);
                    setHasDiscarded(true);
                    break;
                case TurnState.NotStarted:
                case TurnState.TurnStarted:
                default:
                    setHasSeenNext(false);
                    setHasDiscarded(false);
                    break;
            }
        }
    }

    useEffect(() => {
        getSummary();

        const newConnection = new HubConnectionBuilder()
            .withUrl("hubs/gamehub")
            .withAutomaticReconnect()
            .build();

        setConnection(newConnection);

        return function () {
            console.log("Connection Closed");
            connection?.stop();
        };
    }, []);

    useEffect(() => {
        setHandGroups([[...game.hand]]);
    }, [game]);

    //
    // SignalR Event Handlers

    //
    // When player 2 joins, player 1 should go get
    // their summary
    const onGameStarted = () => {
        console.log("[GAME] :: Received GameStarted");
        getSummary();
    };

    //
    // A Generic Connectivity Test
    const onConnectivityTest = (message: string) => {
        console.log("[GAME] :: Received ConnectivityTest");
        window.alert(message);
    };

    //
    // Someone ended their turn, check if it's your turn
    const onEndTurn = (newPlayer: string) => {
        console.log("[GAME] :: Received EndTurn");

        const tmpGame = { ...gameRef.current } as GameSummary;
        if (tmpGame) {
            tmpGame.yourMove = newPlayer === appState.playerId;
            setGame(tmpGame as any);
        }

        setHasSeenNext(false);
        setHasDiscarded(false);
    };

    //
    // Another player took the discard so update the UI
    const onNewDiscardAvailable = (newDiscard: Card) => {
        console.log("[GAME] :: Received NewDiscardAvailable");

        const tmpGame = { ...gameRef.current } as GameSummary;
        if (tmpGame && !tmpGame.yourMove) {
            tmpGame.lastDiscard = newDiscard;
            setGame(tmpGame as any);
        }
    };

    //
    // Card Discarded Event
    const onCardDiscarded = (discarded: Card) => {
        console.log("[GAME] :: Received CardDiscarded");

        const tmpGame = { ...gameRef.current } as GameSummary;
        if (tmpGame && !tmpGame.yourMove) {
            tmpGame.lastDiscard = discarded;
            setGame(tmpGame as any);
        }
    };

    //
    // Round Won Event
    const onRoundWon = (winner: string) => {
        console.log("[GAME] :: Received RoundWon");
        showToast('success', "Pay Me!", `${winner} has won this round.`);
    };

    //
    // End Round Event
    const onEndRound = (gameResults: any) => {
        console.log("[GAME] :: Received EndRound");

        const tmpGame = { ...gameRef.current } as GameSummary;
        tmpGame.roundState = RoundState.Finished;
        setGame(tmpGame);
    };

    //
    // When the connection is created,
    // join the group and setup our callbacks
    useEffect(() => {
        if (connection) {
            connection
                .start()
                .then((result) => {
                    console.log("[GAME] :: Connected to SignalR!");

                    setConnected(true);

                    // Join the current Game Group on the Server
                    connection
                        .send("JoinGame", gameId)
                        .then(() => console.log("[GAME] :: JoinGame Called"))
                        .catch((err) =>
                            console.error("[GAME] :: " + err.toString())
                        );

                    // Setup Our Event Handlers
                    connection.on("GameStarted", onGameStarted);
                    connection.on("ConnectivityTest", onConnectivityTest);
                    connection.on("EndTurn", onEndTurn);
                    connection.on("NewDiscardAvailable", onNewDiscardAvailable);
                    connection.on("CardDiscarded", onCardDiscarded);
                    connection.on("RoundWon", onRoundWon);
                    connection.on("EndRound", onEndRound);
                })
                .catch((e) => {
                    console.log("Connection failed: ", e);
                });
        }
    }, [connection]);

    //
    // UI Event Handlers

    //
    // Called when a user is drawing a new card from the deck
    // Uses API to get the next card and adds that to their hand
    //
    // Return:
    //      The next card to add to your hand
    // Pre Reqs:
    //      It must be your turn
    //      You must not have drawn already
    // Side Effects:
    //      No side effects for other players
    const drawCard = async () => {
        if (game?.yourMove && !hasSeenNext) {
            const result = await service.drawCard(gameId);
            if (result) {
                setHasSeenNext(true);

                const tmp = { ...game };
                tmp.hand.push(result);
                setGame(tmp);
            }
        }
    };

    //
    // Called when a user is drawing the discard from the stack
    // Uses API to get the top discard and adds that to their hand.
    // This method updates the current session and the other players
    // should update via SignalR.
    //
    // Returns:
    //      The "old" discard to add to your hand and the new discard
    //      to show in the UI.
    // Pre Reqs:
    //      It must be your turn
    //      You must not have drawn already
    // Side Effects
    //      NewDiscardAvailable is raised for other players UI to update
    const drawDiscard = async () => {
        if (game?.yourMove && !hasSeenNext) {
            const result = await service.drawDiscard(gameId);
            if (result && result.nextCard && result.nextDiscard) {
                setHasSeenNext(true);

                // Push the drawn card to the hand, replace our discard
                const tmp = { ...game };
                tmp.hand.push(result.nextCard);
                tmp.lastDiscard = result.nextDiscard;
                setGame(tmp);
            }
        }
    };

    //
    // Called when a user clicks on a card in their hand to discard
    // Will update a local state variable to show the end turn button
    // This method updates the current session and the other players
    // should update via SignalR.
    //
    // Returns:
    //      Nothing
    // Pre Reqs:
    //      It must be your move
    //      You must have draw a card or picked up the discard
    // Side Effects:
    //      CardDiscarded is raised for other players to update thier UI
    const discard = async (suite: Suites, value: number) => {
        if (game?.yourMove && hasSeenNext && !hasDiscarded) {
            await service.discard(gameId, suite, value);

            const tmp = { ...game };
            const idx = tmp.hand?.findIndex((itm: Card) => {
                return itm.suite === suite && itm.value === value;
            });

            if (idx > -1) {
                const discarded = tmp.hand.splice(idx, 1);
                tmp.lastDiscard = discarded[0];
            }

            setHasDiscarded(true);
            setGame(tmp);
        }
    };

    //
    // Called when a user decides their turn is over.
    //
    // Returns:
    //      Nothing
    // Pre Reqs:
    //      It must boe your move
    //      You must have drawn a card from the deck or discard
    //      You must have discarded a card
    // Side Effects:
    //      EndTurn is raised for all players to update their UI
    const endTurn = async () => {
        if (game.yourMove && hasSeenNext && hasDiscarded) {
            await service.endTurn(gameId);
        }
    };

    //
    // Called when a user decides to claim a win.
    //
    // Returns:
    //      Nothing
    // Pre Reqs:
    //      It must boe your move
    //      You must have drawn a card from the deck or discard
    //      You must have discarded a card
    // Side Effects:
    //      TODO
    const claimWin = async () => {
        if (game.yourMove && hasSeenNext && hasDiscarded) {
            await service.claimWin(gameId, handGroups);
        }
    };

    const onCreateGroup = () => {
        setHandGroups([...handGroups, []]);
    };

    const startNextRound = async () => {
        await service.startNextRound(gameId);
    };

    const showToast = (type: string, title: string, description: string) => {
        const id = Math.floor(Math.random() * 101 + 1);
        let toastProperties = {
            id: id,
            title: title,
            description: description,
            backgroundColor: ""
        };

        switch (type) {
            case "success":
                toastProperties.backgroundColor = "#5cb85c";
                break;
            case "danger":
                toastProperties.backgroundColor = "#d9534f";
                break;
            case "info":
                toastProperties.backgroundColor = "#5bc0de";
                break;
            case "warning":
                toastProperties.backgroundColor = "#f0ad4e";
                break;
            default:
                setToasts([]);
        }

        setToasts([...toasts, toastProperties]);
    };

    return game && game.state === GameState.InPlay ? (
        <>
            {game.roundState === RoundState.Finished ? (
                game.gameOwner === appState.playerId ? (
                    <div className="next-round-available">
                        <h1>
                            Round Over! Click below to start the next round.
                        </h1>
                        <button onClick={startNextRound}>
                            Start Next Round
                        </button>
                    </div>
                ) : (
                    <div className="next-round-available">
                        <h1>Round Over!</h1>
                    </div>
                )
            ) : null}

            <div className="status-bar">
                <div>
                    Status:
                    <span className={connected ? "connected" : "not-connected"}>
                        {connected ? " Connected" : " Disconnected"}
                    </span>
                </div>
                <div>
                    Turn:
                    <span
                        className={
                            game.yourMove ? "connected" : "not-connected"
                        }
                    >
                        {game.yourMove
                            ? " Your Turn"
                            : " Waiting for Your Turn"}
                    </span>
                </div>
            </div>

            <div className="playing-surface">
                <div className="center-column">
                    <h1>Deck</h1>
                    {game.yourMove && !hasSeenNext ? (
                        <CardBack onClick={drawCard} />
                    ) : (
                        <CardBack />
                    )}
                </div>

                <div className="center-column">
                    <h1>Discard</h1>
                    {game.lastDiscard ? (
                        <CardComponent
                            key={"card-discarded"}
                            suite={game.lastDiscard.suite}
                            value={game.lastDiscard.value}
                            click={drawDiscard}
                        />
                    ) : (
                        <CardEmpty />
                    )}
                </div>
            </div>

            <div className="my-cards-title">
                <h1>
                    Your Cards <br />
                    <TurnBadge
                        hasDiscarded={hasDiscarded}
                        hasSeenNext={hasSeenNext}
                        isYourTurn={game.yourMove}
                    />
                </h1>
                {hasDiscarded ? (
                    <>
                        <button onClick={claimWin}>Claim Win</button>
                        <button onClick={endTurn}>End Turn</button>
                    </>
                ) : null}
            </div>
            <div className="my-cards">
                <PlayerHand
                    hand={handGroups}
                    onNewGroupClick={onCreateGroup}
                    onCardClick={discard}
                />
            </div>

            <button onClick={() => showToast('success', 'tst', 'test')}>HELLO</button>

            <Toast
                toastList={toasts}
                position={"bottom-right"}
                autoDelete={true}
                dismissTime={10000}
            />
        </>
    ) : (
        <div className="waiting-screen">
            <h1>Waiting on Another Player to Join!</h1>
        </div>
    );
};
