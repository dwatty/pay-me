import React, { useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { GameSummary } from '../../models/game-summary';
import { PairingSummary } from '../../models/pairing-summary';
import { GameService } from '../../service';

export const Lobby = () => {

    const history = useHistory();
    const [service] = useState(new GameService());
    const [games, setGames] = useState<GameSummary[]>([]);
    const [availables, setAvailables] = useState<PairingSummary[]>([]);

    async function init() {
        const result = await service.getOpenGames();
        setGames(result.gameSummaries);
        setAvailables(result.availableGames);
    }

    useEffect(() => {
        init();
    }, []);

    const createGame = async () => {
        const gameId = await service.createGame();
        history.push(`/play/${ gameId }`)
    }

    const joinGame = async (gameId : string) => {
        await service.joinGame(gameId);
        history.push(`/play/${ gameId }`);
    }

    return (
        <>
        <div className="row">
            <div className="col">
                <h1>Lobby</h1>
                <button onClick={ createGame }>Create Game</button>
            </div>
        </div>
        <div className="row">
            <div className="col">
                <h1>Your Games</h1>
                <table className="table">
                    <thead>
                        <tr>
                            <th>Game ID</th>
                            <th>Name</th>
                            <th>Players</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                    {
                        games.map((itm : GameSummary) => 
                            <tr key={ itm.gameId }>
                                <td>{ itm.gameId }</td>
                                <td>{ itm.name }</td>
                                <td>{ itm.numPlayers }</td>
                                <td>
                                    <button onClick={ () => joinGame(itm.gameId) }>Join</button>
                                </td>
                            </tr>
                        )
                    }
                    </tbody>
                </table>
            </div>
        </div>

        <div className="row">
            <div className="col">
                <h2>Available Games</h2>                
                <table className="table">
                    <thead>
                        <tr>
                            <th>Game ID</th>
                            <th>Name</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                    {
                        availables.map((itm : PairingSummary) => 
                            <tr key={ itm.gameId }>
                                <td>{ itm.gameId }</td>
                                <td>{ itm.name }</td>
                                <td>
                                    <button onClick={ () => joinGame(itm.gameId) }>Join</button>
                                </td>
                            </tr>
                        )
                    }
                    </tbody>
                </table>
            </div>
        </div>
        </>
    )
}