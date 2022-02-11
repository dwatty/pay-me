import { Card } from "./models/card";
import { ClaimResult, Suites } from "./models/enums";
import { GameSummary } from "./models/game-summary";
import { TakeDiscardResponse } from "./models/take-discard-response";

type METHOD = 'GET'|'POST'|'PUT'|'DELETE';

export class GameService {

    private playerId : string = '';

    constructor(pid: string) {
        if(pid && pid.length > 0) {
            this.playerId = pid;
        }
    }

    public async getOpenGames() {
        return this.makeNonGameRequest("info", "GET");
    }

    public async createGame() {
        return this.makeNonGameRequest("create", "POST");
    }

    public async setPlayerName(name : string) {
        return this.makeNonGameRequest("setName", "POST", name);
    }

    public async getGameSummary(gameId : string) : Promise<GameSummary> {
        return this.makeGameRequest('summary', gameId, 'GET');
    }

    public async joinGame(gameId : string) {
        return this.makeGameRequest('join', gameId, "PUT");
    }

    public async drawCard(gameId : string) : Promise<Card> {
        return this.makeGameRequest('drawcard', gameId, "POST");
    }

    public async drawDiscard(gameId : string) : Promise<TakeDiscardResponse> {
        return this.makeGameRequest('drawdiscard', gameId, "POST");
    }

    public async discard(gameId : string, suite: Suites, value: number) : Promise<Card> {
        const payload = {
            suite: suite,
            value: value
        };

        return this.makeGameRequest('discard', gameId, "PUT", payload);
    }

    public async endTurn(gameId : string, handGroups : Card[][]) {
        return this.makeGameRequest('endturn', gameId, "PUT", handGroups);
    }

    public async claimWin(gameId : string, handGroups : Card[][]) : Promise<ClaimResult> {
        return this.makeGameRequest('claimwin', gameId, "PUT", handGroups);
    }

    public async startNextRound(gameId : string) {
        return this.makeGameRequest('nextround', gameId, "PUT");
    }

    public async getHistory(gameId: string) {
        return this.makeGameRequest('history', gameId, 'GET');
    }

    //
    // Used for making game specific requests, will pass the game ID
    // as a header in the request
    private async makeGameRequest(url: string, gameId: string, method: METHOD, obj? : any) {
        let headers = {
            "X-GAME-ID": gameId
        };

        return this._makeRequest(url, method, headers, obj);
    }

    //
    // Used for making non-game specific requests.
    private async makeNonGameRequest(url: string, method: METHOD, obj?: any) {
        return this._makeRequest(url, method, undefined, obj);
    }

    /**
     * Make an API request
     * @param url The path off the game controller to hit
     * @param method The verb to use
     * @param obj An optional body object
     * @returns A promise for the request to be awaited
     */
    private async _makeRequest(url : string, method: METHOD, headerOverrides?: {}, obj?: any) {

        let headers = {
            Accept: "application/json, text/plain",
            "Content-Type": "application/json;charset=UTF-8",
            "X-PLAYER-ID": this.playerId
        };

        if(headerOverrides) {
            headers = { ...headers, ...headerOverrides };
        }

        const nenv = process.env.NODE_ENV;
        const reqUrl = nenv === "development"
            ? `http://localhost:5152/game/${ url }`
            : `/game/${ url }`;
            
        return fetch(reqUrl, {
            method: method,
            headers: headers,
            mode: 'cors',
            credentials: 'same-origin',
            body: obj ? JSON.stringify(obj) : null
        })
        .then(res => { 
            if(res.status > 200) {
                return `{"error":"${res.status}","message":"${res.statusText}","failure":true}`;
            }
            else {
                return res.text() 
            }
        })
        .then(res => { 
            return res ? JSON.parse(res) : {} 
        })
        .catch(err => {
            console.error(err);
        });
    }

}