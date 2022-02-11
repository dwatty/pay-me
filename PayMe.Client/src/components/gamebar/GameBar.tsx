import { TurnBadge } from '../game/TurnBadge';
import './GameBar.css';

interface IProps {
    hasDiscarded: boolean;
    hasSeenNext: boolean;
    connected: boolean;
    isYourMove: boolean;
    claimWin: any;
    endTurn: any;
    drawCard: any;
    drawDiscard: any;
}

export const GameBar = (props: IProps) => {
    return (
        <div className="game-bar">

            <div className="gamebar-status">
                <span className={props.isYourMove ? "status-dot connected" : "status-dot not-connected"} />
                
                <TurnBadge
                    hasDiscarded={props.hasDiscarded}
                    hasSeenNext={props.hasSeenNext}
                    isYourTurn={props.isYourMove}
                />
            </div>
            
            <div className="gamebar-buttons">
            {
                props.hasDiscarded 
                    ? <button className="btn btn-primary" disabled={!props.isYourMove} onClick={props.claimWin}>Claim Win</button>
                    : <button className="btn btn-primary" disabled={!props.isYourMove} onClick={props.drawCard}>Draw Card</button>
            }
            {
                props.hasDiscarded 
                    ? <button className="btn btn-primary" disabled={!props.isYourMove} onClick={props.endTurn}>End Turn</button>
                    : <button className="btn btn-primary" disabled={!props.isYourMove} onClick={props.drawDiscard}>Draw Discard</button>
            }    
            </div>        
        </div>
    );
};
