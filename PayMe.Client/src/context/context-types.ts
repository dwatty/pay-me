import { ToastMessage } from '../models/toast-message';

export type AppState = {
    username: string; 
    playerId: string;
    toasts: ToastMessage[];
}