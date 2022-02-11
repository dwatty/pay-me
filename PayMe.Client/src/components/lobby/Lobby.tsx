import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAppContext } from "../../context/context";
import { GameSummary } from "../../models/game-summary";
import { PairingSummary } from "../../models/pairing-summary";
import { GameService } from "../../service";
import { useSignalR } from "../../signalr/context";
import { NavMenu } from "../navbar/NavMenu";
import { ContentWrap } from "../shared/ContentWrap";
import styled from "styled-components";
import "./Lobby.css";
import { AppActionType } from "../../context/app-reducer";

const NoGames = styled.h1`
    background-color: #eee;
    text-align: center;
    padding: 1rem;
    border-radius: 4px;
    border: 1px solid #999;
    margin-top: 15px;
    font-weight: lighter;
    font-size: 1.8rem;
`;

export const Lobby = () => {
    const signalR = useSignalR();
    const history = useNavigate();
    const { appState, appDispatch } = useAppContext();
    const [service] = useState(new GameService(appState.playerId));
    const [games, setGames] = useState<GameSummary[]>([]);
    const [availables, setAvailables] = useState<PairingSummary[]>([]);

    async function init() {
        const result = await service.getOpenGames();
        setGames(result.gameSummaries);
        setAvailables(result.availableGames);
    }

    useEffect(() => {
        if (signalR.isConnected) {
            signalR.connection!.on("GameCreated", () => {
                init();
            });
        }
    }, [signalR.isConnected]);

    useEffect(() => {
        init();
    }, []);

    const createGame = async () => {
        const gameId = await service.createGame();
        if (gameId === '00000000-0000-0000-0000-000000000000') {

            // This scenario is typically that the backend as restarted
            // Logout and clear state
            appDispatch({
                type: AppActionType.AddToast,
                payload: {
                    title: 'Error',
                    type: 'error',
                    description: 'Please sign-in again.'
                }
            });
            
            appDispatch({
                type: AppActionType.ClearUser,
            });

            
        }
        else if (gameId) {
            history(`/play/${gameId}`);
        }
        else {
            appDispatch({
                type: AppActionType.AddToast,
                payload: {
                    title: 'Error',
                    type: 'error',
                    description: 'Could not create game!  Please try again later.'
                }
            });
        }
    };

    const joinGame = async (gameId: string) => {
        await service.joinGame(gameId);
        history(`/play/${gameId}`);
    };

    return (
        <>
            <NavMenu />
            <ContentWrap>
                <div className="row mb-3">
                    <div className="col d-flex justify-content-between">
                        <h1 className="mb-0">Lobby</h1>
                        <button className="btn btn-light" onClick={createGame}>
                            + Create New Game
                        </button>
                    </div>
                </div>

                <div className="row mb-3">
                    <div className="col">
                        <h2>Active Games</h2>
                        <p>Here's all the games you've previously joined that aren't finished yet!</p>
                        {games.length > 0 ? (
                            <table className="table table-striped">
                                <thead>
                                    <tr>
                                        <th>Name</th>
                                        <th>Players</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {games.map((itm: GameSummary) => (
                                        <tr key={itm.gameId}>
                                            <td
                                                className="align-middle"
                                                aria-label={itm.gameId}
                                            >
                                                {itm.name}
                                            </td>
                                            <td className="align-middle">
                                                {itm.numPlayers}
                                            </td>
                                            <td>
                                                <button
                                                    type="button"
                                                    className="btn btn-light"
                                                    onClick={() =>
                                                        joinGame(itm.gameId)
                                                    }
                                                >
                                                    Join
                                                </button>
                                                <a className="btn btn-light" href={`/history/${itm.gameId}`}>
                                                    Events
                                                </a>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        ) : (
                            <NoGames>
                                You haven't joined any games!<br/>What are you waiting for?<br/>👏
                            </NoGames>
                        )}
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <h2>Available Games</h2>
                        <p>Looking for a game?  Join one of these!</p>
                        {availables.length > 0 ? (
                            <table className="table table-striped">
                                <thead>
                                    <tr>
                                        <th>Name</th>
                                        <th>Action</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {availables.map((itm: PairingSummary) => (
                                        <tr key={itm.gameId}>
                                            <td className="align-middle">
                                                {itm.name}
                                            </td>
                                            <td>
                                                <button
                                                    type="button"
                                                    className="btn btn-light"
                                                    onClick={() =>
                                                        joinGame(itm.gameId)
                                                    }
                                                >
                                                    Join
                                                </button>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        ) : (
                            <NoGames>There aren't any games available.<br/>Maybe you should start one?<br/>❓</NoGames>
                        )}
                    </div>
                </div>
            </ContentWrap>
        </>
    );
};
