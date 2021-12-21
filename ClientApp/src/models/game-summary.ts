import { Card } from "./card";
import { GameRound, GameState, TurnState } from "./enums";

export class GameSummary {

    public gameId: string = "";
    
    public state: GameState = GameState.AwaitingPlayers;
    
    public name: string = "";
    
    public numPlayers: number = 1;
    
    public lastDiscard: Card|undefined;
    
    public availablePile: Card[] = [];
    
    public hand: Card[] = [];
    
    public yourMove: boolean = false;

    public round : GameRound = GameRound.Threes;

    public playerTurnState : TurnState = TurnState.NotStarted;

    

    // TBD
    public gameStarter: boolean = true;
    public numMoves: number = 0;
    public outcome: number = 0;
    public usernames: string[] = [];
}
