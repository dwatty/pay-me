import { useState } from "react";
import {
    Collapse,
    Container,
    Navbar,
    NavbarBrand,
    NavbarToggler,
    NavItem,
    NavLink,
} from "reactstrap";
import { Link } from "react-router-dom";
import "./NavMenu.css";
import { useAppContext } from "../../context/context";
import { AppActionType } from "../../context/app-reducer";

export const NavMenu = () => {

    const { appState, appDispatch } = useAppContext();
    const [collapsed, setCollapsed] = useState(false);

    const toggleNavbar = () => {
        setCollapsed(!collapsed);
    };

    const quit = () => {
        appDispatch({
            type: AppActionType.ClearUser,
        });
    };

    return (
        <header>
            <Navbar
                className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3"
                light
            >
                <Container>
                    <NavbarBrand tag={Link} to="/">
                        Pay Me | Welcome { appState.username }
                    </NavbarBrand>
                    <NavbarToggler onClick={toggleNavbar} className="mr-2" />
                    <Collapse
                        className="d-sm-inline-flex flex-sm-row-reverse"
                        isOpen={!collapsed}
                        navbar
                    >
                        <ul className="navbar-nav flex-grow">
                            <NavItem>
                                <NavLink
                                    tag={Link}
                                    className="text-dark"
                                    to="/"
                                >
                                    Lobby
                                </NavLink>
                            </NavItem>
                            <NavItem>
                                <button
                                    className="text-dark"
                                    onClick={quit}
                                >
                                    Quit
                                </button>
                            </NavItem>
                        </ul>
                    </Collapse>
                </Container>
            </Navbar>
        </header>
    );
};
