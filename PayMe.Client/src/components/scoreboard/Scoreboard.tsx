import { useMemo, useState } from "react";
import { RoundResult } from "../../models/round-result";
import "./Scoreboard.css";

interface IProps {
    scores: any;
}

type PlayerResult = {
    playerName: string;
    playerId: string;
    scores: [];
    total: number;
};

export const Scoreboard = (props: IProps) => {
    const [showScoreboard, setShowScoreboard] = useState<boolean>(false);
    const trueModel = useMemo(() => {
        const players: PlayerResult[] = [];

        if (props.scores && "Threes" in props.scores) {
            props.scores["Threes"].forEach((itm: any) => {
                const i = {
                    playerName: itm.playerName,
                    playerId: itm.playerId,
                    scores: [],
                } as PlayerResult;

                players.push(i);
            });

            Object.keys(props.scores).forEach((key: any) => {
                const rResults = props.scores[key] as RoundResult[];
                rResults.forEach((itm: RoundResult) => {
                    players.forEach((player: any) => {
                        if (player.playerId === itm.playerId) {
                            player.scores.push({
                                score: itm.score,
                                won: itm.wonRound,
                            });
                        }
                    });
                });
            });

            players.forEach((itm: PlayerResult) => {
                itm.total = itm.scores.reduce((total: number, curr: any) => {
                    return total + curr.score;
                }, 0);
            });
        }

        return players;
    }, [props.scores]);

    const getRoundName = (idx: number) => {
        switch (idx) {
            case 0:
                return "3's";
            case 1:
                return "4's";
            case 2:
                return "5's";
            case 3:
                return "6's";
            case 4:
                return "7's";
            case 5:
                return "8's";
            case 6:
                return "9's";
            case 7:
                return "10's";
            case 8:
                return "J's";
            case 9:
                return "Q's";
            case 10:
                return "K's";
            case 11:
                return "A's";
        }
    };

    return showScoreboard ? (
        <div className="scoreboard-view">
            <div className="scoreboard">
                <div className="row">
                    <div className="col d-flex justify-content-between">
                        <h1>Scoreboard</h1>
                        <button
                            className="scoreboard-close"
                            onClick={() => setShowScoreboard(false)}
                        >
                            X
                        </button>
                    </div>
                </div>
                {
                    trueModel.length > 0 
                    ? (
                        <div className="row">
                            <div className="col">
                                <table className="table">
                                    <thead>
                                        <tr>
                                            <th>Player</th>
                                            {trueModel[0].scores.map(
                                                (hItm: any, hIdx: number) => (
                                                    <th key={`th-${hIdx}`}>
                                                        {getRoundName(hIdx)}
                                                    </th>
                                                )
                                            )}
                                            <th>Total</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {trueModel.map(
                                            (itm: PlayerResult, idx: number) => (
                                                <tr
                                                    key={`${itm.playerId}-score-round-row`}
                                                >
                                                    <td>{itm.playerName}</td>
                                                    {itm.scores.map(
                                                        (sItm: any, idx: number) => (
                                                            <td
                                                                key={`${itm.playerId}-score-round-${idx}`}
                                                            >
                                                                {sItm.score}
                                                                {sItm.won ? "*" : ""}
                                                            </td>
                                                        )
                                                    )}
                                                    <td>{itm.total}</td>
                                                </tr>
                                            )
                                        )}
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    )
                    : <p>No Rounds Completed Yet</p>
                }
            </div>
        </div>
    ) : (
        <button
            className="btn scoreboard-btn"
            onClick={() => setShowScoreboard(!showScoreboard)}
        >
            Score
        </button>
    );
};
