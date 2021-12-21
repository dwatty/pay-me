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

export const initialAppState : AppState = localStorage.getItem('appdata') 
    ? JSON.parse(localStorage.getItem('appdata')!)
    : { username : ''};

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