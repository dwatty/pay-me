import { Route } from "react-router";
import { Container } from "reactstrap";
import { useAppContext } from "../../context/context";
import { Game } from "../game/Game";
import { Lobby } from "../lobby/Lobby";
import { Start } from "../start/Start";

import './Main.css';

function Main() {

    const { appState } = useAppContext();

    return (
        appState.username
            ? <div>

                <Container fluid={true}>
                    <Route exact path="/" component={Lobby} />
                    <Route path='/play/:gameId' component={Game} />
                </Container>
            </div>
            : <Start />
    );
}

export default Main;