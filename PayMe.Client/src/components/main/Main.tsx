import { Route, Routes } from "react-router";
import { Container } from "reactstrap";
import { useAppContext } from "../../context/context";
import { Game } from "../game/Game";
import { Lobby } from "../lobby/Lobby";
import { Start } from "../start/Start";
import { SignalRWrapper } from "../../signalr/context";
import './Main.css';

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
                        </Routes>
                    </Container>
                </SignalRWrapper>
            </div>
            : <Start />
    );
}

export default Main;