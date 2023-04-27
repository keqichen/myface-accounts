import React, { useEffect, useState } from "react";
import Cookies from 'js-cookie';
import {NavLink} from "react-router-dom";
import './Header.scss';

export function Username(): JSX.Element {
    //var username = Cookies.get("username") || "";
    
    //const [username, setUsername] = useState<string | null>(Cookies.get("username") || null);

    const [username, setUsername] = useState<string>("");

    useEffect(() => {
        const usernameFromCookie = Cookies.get("username");
        setUsername(usernameFromCookie || "");
    }, [username]);
    
    if (username !==null && username.length !== 0){
        return <div>
            Hi, {username}!
        </div>
    } else {
        return <NavLink className="login" to="/">Login</NavLink>
    }
}