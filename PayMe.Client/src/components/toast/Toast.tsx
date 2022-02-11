import { useEffect, useState } from 'react';
import { AppActionType } from '../../context/app-reducer';
import { useAppContext } from '../../context/context';
import './Toast.css';

export const Toasts = () => {

    const autoDelete = true;
    const dismissTime = 7500;
    const position = "bottom-middle";
    const { appState, appDispatch } = useAppContext();
    
    useEffect(() => {
        const interval = setInterval(() => {
            if (autoDelete && appState.toasts.length && appState.toasts.length) {
                deleteToast(appState.toasts[0].id!);
            }
        }, dismissTime);
        
        return () => {
            clearInterval(interval);
        }

        // eslint-disable-next-line
    }, [appState.toasts, autoDelete, dismissTime]);

    const deleteToast = (id:number) => {
        appDispatch({
            type: AppActionType.RemoveToast,
            payload: id
        });
    }

    const getBackground = (type:string) => {
        switch (type) {
            case "success":
                return "#5cb85c";
            case "error":
                return "#d9534f";
            case "info":
                return "#5bc0de";
            case "warning":
                return "#ff9600";
        }
    }

    return (
        <>
            <div className={`notification-container ${position}`}>
                {
                    appState.toasts.map((toast, i) =>     
                        <div 
                            key={i}
                            className={`notification panini ${position}`}
                            style={{ backgroundColor: getBackground(toast.type) }}
                        >
                            <button onClick={() => deleteToast(toast.id!)}>
                                X
                            </button>
                            <div className="notification-content">
                                <p className="notification-title">{toast.title}</p>
                                <p className="notification-message">
                                    {toast.description}
                                </p>
                            </div>
                        </div>
                    )
                }
            </div>
        </>
    );
}

