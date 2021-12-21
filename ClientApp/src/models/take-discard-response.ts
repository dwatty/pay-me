import { Card } from "./card";

export class TakeDiscardResponse {
    public nextCard : Card|undefined = undefined;
    public nextDiscard : Card|undefined = undefined;
}