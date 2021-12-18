import { createContext, useContext, useReducer } from "react";
import { AppActions } from "./app-reducer";
import { AppState } from "./context-types";
import { appReducer, initialAppState } from "./app-reducer";

const AppContext = createContext<{
    appState: AppState;
    appDispatch: React.Dispatch<AppActions>;
}>({
    appState: initialAppState,
    appDispatch: () => undefined,
});

export function AppWrapper({ children } : any) {
    const [appState, appDispatch] = useReducer(appReducer,initialAppState);
    return (
        <AppContext.Provider value={{ appState: appState, appDispatch: appDispatch }}>
            {children}
        </AppContext.Provider>
    );
}

export function useAppContext() {
    return useContext(AppContext);
}