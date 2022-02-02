import { HttpTransportType, HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { createContext, useContext, useEffect, useState } from "react";

const SignalRContext = createContext<{
    connection: HubConnection|null;
    isConnected: boolean;
    note: string
}>({
    connection: null,
    isConnected: false,
    note: 'test'
});

export function SignalRWrapper({ children } : any) {

    const [isConnected, setIsConnected] = useState(false);
    const [note, setNote] = useState('yar');
    const [connection, setConnection] = useState<HubConnection>(() => {

        const nenv = process.env.NODE_ENV;
        const reqUrl = nenv === "development"
            ? `http://localhost:5152/hubs/gamehub`
            : `/hubs/gamehub`;

        return new HubConnectionBuilder()
            .withUrl(reqUrl, {
                skipNegotiation: true,
                transport: HttpTransportType.WebSockets        
            })
            .withAutomaticReconnect()
            .build();
    });

    useEffect(() => {
        if (connection) {
            connection
                .start()
                .then((result) => {
                    console.log("[GAME] :: Connected to SignalR!");
                    setIsConnected(true);
                    setNote('dabble');
                })
                .catch((e) => {
                    console.log("Connection failed: ", e);
                });
        }
    }, [connection]);
    
    return (
        <SignalRContext.Provider value={{ connection: connection, isConnected: isConnected, note: note }}>
            {children}
        </SignalRContext.Provider>
    );
}

export function useSignalR() {
    return useContext(SignalRContext);
}