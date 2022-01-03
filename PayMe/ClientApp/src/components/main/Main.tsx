import { Route } from "react-router";
import { Container } from "reactstrap";
import { useAppContext } from "../../context/context";
import { Game } from "../game/Game";
import { Lobby } from "../lobby/Lobby";
import { NavMenu } from "../navbar/NavMenu";
import { Start } from "../start/Start";

function Main() {

    const { appState } = useAppContext();

    return (
        appState.username
            ? <div>
                <NavMenu />
                <Container>
                    <Route exact path="/" component={Lobby} />
                    <Route path='/play/:gameId' component={Game} />
                </Container>
            </div>
            : <Start />
    );
}

export default Main;