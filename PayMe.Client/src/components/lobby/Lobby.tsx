import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAppContext } from "../../context/context";
import { GameSummary } from "../../models/game-summary";
import { PairingSummary } from "../../models/pairing-summary";
import { GameService } from "../../service";
import { useSignalR } from "../../signalr/context";
import { NavMenu } from "../navbar/NavMenu";
import "./Lobby.css";

export const Lobby = () => {

    const signalR = useSignalR();
    const history = useNavigate();
    const { appState } = useAppContext();
    const [service] = useState(new GameService(appState.playerId));
    const [games, setGames] = useState<GameSummary[]>([]);
    const [availables, setAvailables] = useState<PairingSummary[]>([]);

    async function init() {
        const result = await service.getOpenGames();
        setGames(result.gameSummaries);
        setAvailables(result.availableGames);
    }

    useEffect(() => {
        if(signalR.isConnected) {
            signalR.connection!.on('GameCreated', () => {
                init();
            })
        }     
    }, [signalR.isConnected]);

    useEffect(() => {
        init();
    }, []);

    const createGame = async () => {
        const gameId = await service.createGame();
        if(gameId) {
            history(`/play/${gameId}`);
        }
    };

    const joinGame = async (gameId: string) => {
        await service.joinGame(gameId);
        history(`/play/${gameId}`);
    };

    return (
        <>
            <NavMenu />
            <div className="lobby-container">
                <div className="row mb-3">
                    <div className="col d-flex justify-content-between">
                        <h1 className="mb-0">Game Lobby</h1>
                        <button className="btn btn-light" onClick={createGame}>
                            + Create New Game
                        </button>
                    </div>
                </div>

                <div className="row mb-3">
                    <div className="col">
                        <h2>Your Games</h2>
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
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        ) : (
                            <h1 className="no-games">
                                You haven't created any games yet!
                            </h1>
                        )}
                    </div>
                </div>

                <div className="row">
                    <div className="col">
                        <h2>Available Games</h2>
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
                            <h1 className="no-games-available">
                                There aren't any games available.
                            </h1>
                        )}
                    </div>
                </div>
            </div>
        </>
    );
};
