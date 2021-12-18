import { AppState } from "./context-types";

export enum AppActionType {
    SetUser,
    ClearUser    
}

export interface SetUser {
    type: AppActionType.SetUser;
    payload: string;
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
            newState = { ...state, username: action.payload };
            break;
        case AppActionType.ClearUser:
            newState = { ...state, username: '' };
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