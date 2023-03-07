import React, { FormEvent, useContext, useState } from 'react';
import { Page } from "../Page/Page";
import { LoginContext } from "../../Components/LoginManager/LoginManager";
import "./Login.scss";
import { checkAuthHeader } from '../../Api/apiClient';

export function Login(): JSX.Element {
    const loginContext = useContext(LoginContext);

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    async function tryLogin(event: FormEvent) {
        event.preventDefault();
        loginContext.setUsername(username);
        loginContext.setPassword(password);

        // call api, backend;
        // check whether anyone is logged in;
        var check = await checkAuthHeader(username, password);
        if (check) {
            console.log("true");
            loginContext.logIn();
        }
        else {
            console.log("false");
            loginContext.logOut();
        }
    }

    return (
        <Page containerClassName="login">
            <h1 className="title">Log In</h1>
            <form className="login-form" onSubmit={tryLogin}>
                <label className="form-label">
                    Username
                    <input className="form-input" type={"text"} value={username} onChange={event => setUsername(event.target.value)} />
                </label>

                <label className="form-label">
                    Password
                    <input className="form-input" type={"password"} value={password} onChange={event => setPassword(event.target.value)} />
                </label>

                <button className="submit-button" type="submit">Log In</button>
            </form>
        </Page>
    );
}