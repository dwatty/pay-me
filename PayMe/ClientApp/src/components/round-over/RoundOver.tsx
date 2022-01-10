import { useAppContext } from "../../context/context"
import { RoundState } from "../../models/enums"
import { GameSummary } from "../../models/game-summary"

interface IProps {
    game : GameSummary;
    startNextRound: any;
}

export const RoundOver = (props: IProps) => {

    const { appState } = useAppContext();

    return (
        
            props.game.roundState === RoundState.Finished ? (
                props.game.gameOwner === appState.playerId ? (
                <div className="next-round-available">
                    <h1>Round Over!</h1>
                    <p> Click below to start the next round.</p>
                    <button className="btn btn-primary" onClick={ props.startNextRound }>
                        Start Next Round
                    </button>
                </div>
            ) : (
                <div className="next-round-available">
                    <h1>Round Over!</h1>
                    <p>Waiting for the next round to start!</p>
                </div>
            )
        ) : null
    
    )

}