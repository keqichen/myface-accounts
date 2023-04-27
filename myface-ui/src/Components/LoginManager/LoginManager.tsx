import Cookies from "js-cookie";
import React, { createContext, ReactNode, useState } from "react";


export const LoginContext = createContext({
    isLoggedIn: false,
    isAdmin: false,
    logIn: () => { },
    logOut: () => { },
    username: "",
    //this allows us to write the context;
    setUsername: (username: string) => { },
    password: "",
    setPassword: (password: string) => { }
});

interface LoginManagerProps {
    children: ReactNode
}

export function LoginManager(props: LoginManagerProps): JSX.Element {
    const [loggedIn, setLoggedIn] = useState(Cookies.get("isLoggedIn")==="true");
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    function logIn() {
        // can i add a method to make it return response
        setLoggedIn(true);
        Cookies.set("isLoggedIn", "true");
    }

    function logOut() {
        setLoggedIn(false);
        Cookies.remove("isLoggedIn");
    }

    const context = {
        isLoggedIn: loggedIn,
        isAdmin: loggedIn,
        logIn: logIn,
        logOut: logOut,
        username: username,
        setUsername: setUsername,
        password: password,
        setPassword: setPassword
    };

    return (
        <LoginContext.Provider value={context}>
            {props.children}
        </LoginContext.Provider>
    );
}