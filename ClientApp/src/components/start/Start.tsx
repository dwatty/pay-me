import { useState } from "react"
import { AppActionType } from "../../context/app-reducer";
import { useAppContext } from "../../context/context";
import { GameService } from "../../service";

import './Start.css';

export const Start = () => {

    const { appDispatch } = useAppContext();
    const [service] = useState(new GameService());
    const [username, setUsername] = useState('');

    const setPlayerName = async () => {
        try {
            await service.setPlayerName(username);
            
            const playerValue = document.cookie.split('; ').find(c => c.startsWith('playerId'))
            const id = playerValue?.split("=")[1];
            
            appDispatch({
                type: AppActionType.SetUser,
                payload: {
                    username: username,
                    playerId: id ?? ''
                }
            });
        }
        catch(err) {
            console.error(err);
        }
    }

    return (
        <div className="start-container">
            <div className="row">
                <div className="col">
                    <h1>Get Started</h1>
                    <h2>Enter your name below to get started.</h2>
                </div>
            </div>
            <div className="row">
                <div className="col">
                    <input type="test" onChange={ (e) => setUsername(e.target.value)} />
                    <button onClick={ setPlayerName }>Set Name</button>
                </div>
            </div>
        </div>
    )

}