import { useState } from "react"
import { AppActionType } from "../../context/app-reducer";
import { useAppContext } from "../../context/context";
import { GameService } from "../../service";

import './Start.css';

export const Start = () => {

    const { appState, appDispatch } = useAppContext();
    const [service] = useState(new GameService(appState.playerId));
    const [username, setUsername] = useState('');

    const setPlayerName = async () => {

        if(!username || username.length === 0) {
            return;
        }

        try {
            const pID = await service.setPlayerName(username);            
            appDispatch({
                type: AppActionType.SetUser,
                payload: {
                    username: username,
                    playerId: pID ?? ''
                }
            });
        }
        catch(err) {
            console.error(err);
        }
    }

    return (
        <div className="table-background">
            <h1 className="app-title">Pay<br/>Me</h1>
            <div className="start-container">
                <div className="row">
                    <div className="col">
                        <h2>Enter your name below to get started.</h2>
                    </div>
                </div>
                <div className="row">
                    <div className="col flex-center">
                        <input type="test" onChange={ (e) => setUsername(e.target.value)} />
                        <button disabled={ username.length === 0 } onClick={ setPlayerName }>Set Name</button>
                    </div>
                </div>
            </div>
        </div>
    )

}