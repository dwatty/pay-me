import { GameEvents } from './enums';

export class GameEvent {
    public playerId: string = "";
    public event: GameEvents = GameEvents.Unknown;
    public eventBody: string = "";
    public eventTime: string = "";

    public eventBodyObj: any;
    public eventTimeFmt: string = '';
}