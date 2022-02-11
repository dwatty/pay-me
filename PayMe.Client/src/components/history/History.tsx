import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useAppContext } from "../../context/context";
import { GameService } from "../../service";
import { GameEvent } from '../../models/game-event';
import { SectionHeader } from "../shared/SectionHeader";
import { ContentWrap } from "../shared/ContentWrap";
import { NavMenu } from "../navbar/NavMenu";
import { GameEvents } from "../../models/enums";
import './History.css';
export const History = () => {

    const { appState } = useAppContext();
    const { gameId } = useParams() as any;
    const [service] = useState(new GameService(appState.playerId));
    const [events, setEvents] = useState<GameEvent[]>([]);

    async function getHistory() {
        const res = await service.getHistory(gameId);

        res.history.forEach((itm: GameEvent) => {
            itm.eventBodyObj = JSON.parse(itm.eventBody);
            itm.eventTimeFmt = new Date(itm.eventTime).toUTCString();
        });

        setEvents(res.history);
    }

    useEffect(() => {
        getHistory();
    }, []);

    const getEventName = (evt : GameEvents) => {
        switch(evt) {
            case GameEvents.Unknown:
                return "Unknown";
            case GameEvents.GameStarted:
                return "Game Started";
            case GameEvents.PlayerAdded:
                return "Player Added";
            case GameEvents.RoundOver:
                return "Round Over";
            case GameEvents.ScoreResult:
                return "Score Result";
            case GameEvents.DrawCard:
                return "Draw Card";
            case GameEvents.DrawDiscard:
                return "Draw Discard";
            case GameEvents.Discard:
                return "Discard";
            case GameEvents.ClaimWin:
                return "Claim Win";
            case GameEvents.ClaimFail:
                return "Claim Fail";
            case GameEvents.GetSumamry:
                return "Get Summary";
            case GameEvents.SetName:
                return "Set Name";
            case GameEvents.HandDealt:
                return "Hand Dealt";
            default:
                return "";
        }
    }

    // Shout out to https://stackoverflow.com/questions/4810841/how-can-i-pretty-print-json-using-javascript
    const colorizeJson = (toColor: object): string => {

        let json = JSON.stringify(toColor, null, 2);
        json = json.replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;');

        return json.replace(/("(\\u[a-zA-Z0-9]{4}|\\[^u]|[^\\"])*"(\s*:)?|\b(true|false|null)\b|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?)/g, function (match) {
            let cls = 'number';
            if (/^"/.test(match)) {
                if (/:$/.test(match)) {
                    cls = 'key';
                } else {
                    cls = 'string';
                }
            }
            else if (/true|false/.test(match)) {
                cls = 'boolean';
            }
            else if (/null/.test(match)) {
                cls = 'null';
            }

            return '<span class="json-' + cls + '">' + match + '</span>';
        });

    }

    return (
        <>
            <NavMenu />
            <ContentWrap>            
                <SectionHeader title="Game History" />
                
                <div className="row">
                    <div className="col">
                        {
                            events.length > 0
                                ? events.map((itm: GameEvent) => 
                                    <div className="history-event">
                                        <h6>
                                            <p>{ getEventName(itm.event) }</p>
                                            <p>Time: { itm.eventTimeFmt }</p>
                                        </h6>
                                        <div>
                                            <pre className="eventbody" 
                                                dangerouslySetInnerHTML={{ __html: colorizeJson(itm.eventBodyObj) }} >                                                    
                                            </pre>
                                        </div>                                        
                                    </div>
                                )
                                : null
                        }
                    </div>
                </div>
            </ContentWrap>
        </>
    )
}