import { SetUserAction } from "../models/set-user-action";
import { AddToastAction, ToastMessage } from "../models/toast-message";
import { AppState } from "./context-types";

export enum AppActionType {
    SetUser,
    ClearUser,
    AddToast,
    RemoveToast
}

export interface SetUser {
    type: AppActionType.SetUser;
    payload: SetUserAction;
}

export interface ClearUser {
    type: AppActionType.ClearUser
}

export interface AddToast {
    type: AppActionType.AddToast;
    payload: AddToastAction;
}

export interface RemoveToast {
    type: AppActionType.RemoveToast;
    payload: number;
}

export type AppActions = SetUser | ClearUser | AddToast | RemoveToast;

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
    return { username : '', toasts: []};
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
        case AppActionType.AddToast:
            const id = Math.floor(Math.random() * 101 + 1);
            const toast = {
                ...action.payload,
                id: id
            };            

            newState = {
                ...state,
                toasts: [...state.toasts, toast]
            }
            break;
        case AppActionType.RemoveToast:
            const toastCpy = [...state.toasts];
            const itemIndex = toastCpy.findIndex(e => e.id === action.payload);
            toastCpy.splice(itemIndex, 1);
            
            newState = {
                ...state,
                toasts: toastCpy
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