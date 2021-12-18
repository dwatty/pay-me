import { Card } from "./models/card";
import { GameSummary } from "./models/game-summary";

type METHOD = 'GET'|'POST'|'PUT'|'DELETE';

export class GameService {

    public async getOpenGames() {
        return this.makeRequest("info", "GET");
    }

    public async getGameSummary(gameId : string) : Promise<GameSummary> {
        return this.makeRequest(`summary/${ gameId }`, 'GET');
    }

    public async createGame() {
        return this.makeRequest("create", "POST");
    }

    public async setPlayerName(name : string) {
        return this.makeRequest("setName", "POST", name);
    }

    public async joinGame(gameId : string) {
        return this.makeRequest(`join/${ gameId }`, "PUT");
    }

    public async drawCard(gameId : string) : Promise<Card> {
        return this.makeRequest(`drawcard/${ gameId }`, "POST");
    }


    /**
     * Make an API request
     * @param url The path off the game controller to hit
     * @param method The verb to use
     * @param obj An optional body object
     * @returns A promise for the request to be awaited
     */
    private async makeRequest(url : string, method: METHOD, obj?: any) {
        return fetch(`game/${ url }`, {
            method: method,
            headers: {
                Accept: "application/json, text/plain",
                "Content-Type": "application/json;charset=UTF-8",
            },
            credentials: 'same-origin',
            body: obj ? JSON.stringify(obj) : null
        })
        .then(res => res.json())
        .catch(err => {
            console.error(err);
        });
    }

}