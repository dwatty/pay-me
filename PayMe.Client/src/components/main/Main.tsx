import { Route, Routes } from "react-router";
import { Container } from "reactstrap";
import { useAppContext } from "../../context/context";
import { Game } from "../game/Game";
import { Lobby } from "../lobby/Lobby";
import { Start } from "../start/Start";
import { Rules } from "../rules/Rules";
import { SignalRWrapper } from "../../signalr/context";
import './Main.css';
import { History } from "../history/History";
import { Toasts } from "../toast/Toast";

function Main() {

    const { appState } = useAppContext();

    return (
        appState.username
            ? <div>
                <SignalRWrapper>
                    <Container fluid={true}>
                        <Routes>
                            <Route path="/" element={ <Lobby /> } />
                            <Route path='/play/:gameId' element={ <Game />} />
                            <Route path='/history/:gameId' element={ <History />} />
                            <Route path='/how-to-play' element={ <Rules />} />
                        </Routes>

                        <Toasts />

                    </Container>
                </SignalRWrapper>
            </div>
            : <Start />
    );
}

export default Main;