import { Suites } from "./enums";

export class Card {

    public id : string = '';
    
    public value : number = -1;
    
    public suite : Suites = Suites.Unknown;
    
}