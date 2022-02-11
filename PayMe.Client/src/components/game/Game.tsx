import { useEffect, useRef, useState } from "react";
import { useParams } from "react-router-dom";
import { GameService } from "../../service";
import { GameSummary } from "../../models/game-summary";
import {
    ClaimResult,
    GameState,
    RoundState,
    Suites,
    TurnState,
} from "../../models/enums";
import { CardComponent } from "../card/CardComponent";
import { Card } from "../../models/card";
import { CardBack } from "../card/CardBack";
import { useAppContext } from "../../context/context";
import { CardEmpty } from "../card/CardEmpty";
import { PlayerHand } from "./PlayerHand";
import { GameBar } from "../gamebar/GameBar";
import { Scoreboard } from "../scoreboard/Scoreboard";
import { RoundOver } from "../round-over/RoundOver";
import { GameWaiting } from "./GameWaiting";
import { TableLink } from '../shared/TableButton';
import { useSignalR } from "../../signalr/context";
import "./Game.css";
import { AppActionType } from "../../context/app-reducer";
import { GameWon } from './GameWon';

export const Game = () => {
    
    const signalR = useSignalR();

    const { appState, appDispatch } = useAppContext();
    const { gameId } = useParams() as any;
    const [service] = useState(new GameService(appState.playerId));
    const [game, setGame] = useState<GameSummary>(new GameSummary());

    // Used as a static ref for signalR work
    const gameRef = useRef<GameSummary>();
    gameRef.current = game;

    // Local State
    const [hasSeenNext, setHasSeenNext] = useState(false);
    const [hasDiscarded, setHasDiscarded] = useState(false);
    const [handGroups, setHandGroups] = useState<Array<Card[]>>([]);
    const [scoring, setScoring] = useState<any>({});
    const [isWinner, setIsWinner] = useState<boolean>(false);
   
    async function getSummary() {
        const result = await service.getGameSummary(gameId);
        if (result) {
            setHandGroups([[...result.hand]]);
            setScoring(result.scoreboard);
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
        if(signalR.isConnected && signalR.connection) {            
            // Join the current Game Group on the Server
            signalR.connection
                .send("JoinGame", gameId)
                .then(() => console.log("[GAME] :: JoinGame Called"))
                .catch((err) =>
                    console.error("[GAME] :: " + err.toString())
                );

            // Setup Our Event Handlers
            signalR.connection.on("GameStarted", onGameStarted);
            signalR.connection.on("EndTurn", onEndTurn);
            signalR.connection.on("NewDiscardAvailable", onNewDiscardAvailable);
            signalR.connection.on("CardDiscarded", onCardDiscarded);
            signalR.connection.on("RoundWon", onRoundWon);
            signalR.connection.on("EndRound", onEndRound);
        }  
    }, [signalR.isConnected]);

    useEffect(() => {
        getSummary();        
    }, []);

    useEffect(() => {
        //setHandGroups([[...game.hand]]);
        console.log('used to set hand groups here');
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
        showToast("success", "Pay Me!", `${winner} has won this round.`);
    };

    //
    // End Round Event
    const onEndRound = (gameResults: any) => {
        console.log("[GAME] :: Received EndRound");
        
        const tmpGame = { ...gameRef.current } as GameSummary;
        tmpGame.roundState = RoundState.Finished;
        setGame(tmpGame);
        setScoring(gameResults);
        setIsWinner(false);
    };

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


                const handCpy = [...handGroups];
                handCpy[0].push(result);
                setHandGroups(handCpy);

                // const tmp = { ...game };
                // tmp.hand.push(result);
                // setGame(tmp);
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
            if (result && result.nextCard) {
                setHasSeenNext(true);

                const handCpy = [...handGroups];
                handCpy[0].push(result.nextCard);
                setHandGroups(handCpy);

                // Push the drawn card to the hand, replace our discard
                const tmp = { ...game };
                //tmp.hand.push(result.nextCard);
                tmp.lastDiscard = result.nextDiscard ? result.nextDiscard : undefined;
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

            const gameCpy = { ...game };
            const handCpy = [...handGroups];

            handCpy.forEach((cardAry: Card[]) => {
                const idx = cardAry.findIndex((itm: Card) => {
                    return itm.suite === suite && itm.value === value;
                });

                if (idx > -1) {
                    const discarded = cardAry.splice(idx, 1);
                    gameCpy.lastDiscard = discarded[0];
                }

            })

            

            

            // const idx = gameCpy.hand?.findIndex((itm: Card) => {
            //     return itm.suite === suite && itm.value === value;
            // });

            // if (idx > -1) {
            //     const discarded = gameCpy.hand.splice(idx, 1);
            //     gameCpy.lastDiscard = discarded[0];
            // }

            setHasDiscarded(true);
            setHandGroups(handCpy);
            setGame(gameCpy);
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
            await service.endTurn(gameId, handGroups);
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
            const claimResult = await service.claimWin(gameId, handGroups);
            if (claimResult === ClaimResult.Invalid) {
                showToast(
                    "warning",
                    "Sorry",
                    "That doesn't look like a winning hand"
                );
            }
            else {
                setIsWinner(true);
            }
        }
    };

    const onCreateGroup = () => {
        setHandGroups([...handGroups, []]);
    };

    const dragEnd = (newHands : any) => {
        setHandGroups(newHands);
    }

    const startNextRound = async () => {
        await service.startNextRound(gameId);
    };

    const showToast = (type: string, title: string, description: string) => {        
        appDispatch({
            type: AppActionType.AddToast,
            payload: {
                type: type,
                title: title,
                description: description
            }
        });
    };

    return game && game.state === GameState.InPlay ? (
        <div className="table-background">
            
            <RoundOver 
                game={ game }
                startNextRound={ startNextRound }
            />

            <div className="game-wrapper">
                <div className="playing-surface">
                    <div className="center-column">
                        {game.yourMove && !hasSeenNext ? (
                            <CardBack onClick={drawCard} />
                        ) : (
                            <CardBack />
                        )}
                    </div>

                    <div className="center-column">
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
                    <h1>Your Cards</h1>
                    <button
                        type="button"
                        className="btn btn-primary"
                        disabled={ !game.yourMove }
                        onClick={() => {
                            setHandGroups([...handGroups, []]);
                        }}>
                        Create Group
                    </button>
                </div>

                <PlayerHand
                    hand={ handGroups }
                    onNewGroupClick={ onCreateGroup }
                    onCardClick={ discard }
                    onDragEnd={ dragEnd }
                />

                <GameBar
                    hasDiscarded={hasDiscarded}
                    hasSeenNext={hasSeenNext}
                    connected={signalR.isConnected}
                    isYourMove={game.yourMove}
                    drawCard={drawCard}
                    drawDiscard={drawDiscard}
                    endTurn={endTurn}
                    claimWin={claimWin}
                />

                <TableLink href="/">
                    Lobby
                </TableLink>
                
                <Scoreboard 
                    scores={ scoring } 
                    players={game.players} />

                <GameWon isVisible={ isWinner } />

            </div>
        </div>
    ) : <GameWaiting />;
};
