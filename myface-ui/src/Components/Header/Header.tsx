﻿import React from "react";
import {NavLink} from "react-router-dom";
import './Header.scss';

export function Header(): JSX.Element {
    return (
        <header className="header">
            <NavLink className="home-link" to="/">MyFace</NavLink>
        </header>
    );
}

export function Nav(): JSX.Element {
    return (
        <nav className="nav">
            <NavLink className="nav-link" to="/">Posts</NavLink>
            <NavLink className="nav-link" to="/users">Users</NavLink>
        </nav>
    );
}