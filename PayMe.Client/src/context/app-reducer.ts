import { SetUserAction } from "../models/set-user-action";
import { AppState } from "./context-types";

export enum AppActionType {
    SetUser,
    ClearUser    
}

export interface SetUser {
    type: AppActionType.SetUser;
    payload: SetUserAction;
}

export interface ClearUser {
    type: AppActionType.ClearUser
}

export type AppActions = SetUser | ClearUser;

/**
 * Check in local storage for our saved state.  Do a basic
 * check that we have a playerID of GUID length.  If we don't
 * just remove the data to avoid a weird state
 * @returns A JSON object with our local storage state
 */
function getSavedState() {
    const savedData = localStorage.getItem('appdata');
    try {
        if(savedData) {
            const jsonData = JSON.parse(savedData!);
            if(jsonData && jsonData.playerId && jsonData.playerId.length === 36) {
                return jsonData;
            }
            else {
                localStorage.removeItem('appdata');
            }
        }
    }
    catch(err) { }   
    return { username : ''};
}

export const initialAppState : AppState = getSavedState();

export function appReducer(state: AppState, action: AppActions) {
    let newState;
    switch (action.type) {
        case AppActionType.SetUser:
            newState = { 
                ...state, 
                username: action.payload.username, 
                playerId: action.payload.playerId 
            };
            break;
        case AppActionType.ClearUser:
            newState = { 
                ...state, 
                username: '', 
                playerId: '' 
            };
            break;
        default:
            newState = state;
    }

    saveState(newState);
    return newState;
}

export const saveState = (state:AppState) => {
    try {
      const serializedState = JSON.stringify(state);
      localStorage.setItem('appdata', serializedState);
    } catch (err) {
      // Ignore write errors.
    }
  };