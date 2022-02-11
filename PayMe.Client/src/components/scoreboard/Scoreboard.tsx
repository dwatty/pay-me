import { useMemo, useState } from "react";
import { RoundResult } from "../../models/round-result";
import { TableButton } from "../shared/TableButton";
import { Player } from '../../models/game-summary';
import "./Scoreboard.css";

interface IProps {
    scores: any;
    players: Player[];
}

type RoundScore = {
    score: number;
    won: boolean;
}

type PlayerResult = {
    playerName: string;
    playerId: string;
    scores: RoundScore[];
    total: number;
};

export const Scoreboard = (props: IProps) => {
    const [showScoreboard, setShowScoreboard] = useState<boolean>(false);

    const trueModel = useMemo(() => {
        const players: PlayerResult[] = [];

        props.players.forEach((player : Player) => {

            const i = {
                playerName: player.playerName,
                playerId: player.playerId,
                scores: [
                    { score: 0, won: false }, 
                    { score: 0, won: false },
                    { score: 0, won: false },
                    { score: 0, won: false },
                    { score: 0, won: false },
                    { score: 0, won: false },
                    { score: 0, won: false },
                    { score: 0, won: false },
                    { score: 0, won: false },
                    { score: 0, won: false },
                    { score: 0, won: false },
                    { score: 0, won: false }
                ],
                total: 0
            } as PlayerResult;

            players.push(i);

        });

        if (props.scores && "Threes" in props.scores) {
            Object.keys(props.scores).forEach((key: any, idx: number) => {
                
                const rResults = props.scores[key] as RoundResult[];
                rResults.forEach((itm: RoundResult) => {

                    players.forEach((player: any) => {
                        if (player.playerId === itm.playerId) {
                            player.scores[idx] = {
                                score: itm.score,
                                won: itm.wonRound,
                            };
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
                            onClick={() => setShowScoreboard(false)}>
                            X
                        </button>
                    </div>
                </div>
                <div className="row">
                    <div className="col">
                        <table className="scores">
                            <thead>
                                <tr>
                                    <th></th>
                                    <th>3</th>
                                    <th>4</th>
                                    <th>5</th>
                                    <th>6</th>
                                    <th>7</th>
                                    <th>8</th>
                                    <th>9</th>
                                    <th>10</th>
                                    <th>J</th>
                                    <th>Q</th>
                                    <th>K</th>
                                    <th>A</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>                                
                            {
                                trueModel.map((itm: PlayerResult) => (
                                    <tr key={`${itm.playerId}-score-round-row`}>
                                        <td data-label="Player">{itm.playerName}</td>
                                        {
                                            itm.scores.map((sItm: any, idx: number) => (
                                                <td
                                                    className={sItm.won ? "payme" : ""}
                                                    data-label={getRoundName(idx)}
                                                    key={`${itm.playerId}-score-round-${idx}`}>
                                                    {sItm.score}
                                                    {sItm.won ? <span className="payme">*</span> : null}
                                                </td>
                                            )
                                        )}
                                        <td data-label="Total">{itm.total}</td>
                                    </tr>
                                    )
                                )
                            }
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    ) : (
        <TableButton
            top="1.5rem"
            right="1.5rem"
            onClick={() => setShowScoreboard(!showScoreboard)}
        >
            Score
        </TableButton>
    );
};
