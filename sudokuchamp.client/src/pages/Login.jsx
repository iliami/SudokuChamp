import { useState, useContext } from "react";
import { AuthContext } from "../AuthProvider.jsx";
import "./styles/Login.css";

export function Login() {
    const { setIsAuthenticated } = useContext(AuthContext);
    const { username, setUsername } = useContext(AuthContext);
    const [password, setPassword] = useState("");

    const handleSubmit = async (event) => {
        event.preventDefault();
        await sendData();
      };

    return (
        <div className="login">
            <div className="login-form">
                <form onSubmit={handleSubmit}>
                    <input
                        type="text"
                        name="username"
                        placeholder="Имя пользователя"
                        required
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                    />
                    <input
                        type="password"
                        name="password"
                        placeholder="Пароль"
                        required
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                    <input type="submit" value="Войти" />
                </form>
            </div>
        </div>
    );

    async function sendData() {
        const res = await fetch("api/auth/login", {
            method: "POST",
            credentials: "include",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                username: username,
                password: password,
            }),
        });
        if (res.ok) {
            setIsAuthenticated(true);
            const values = await res.json();
            const access_token = values.token;
            sessionStorage.setItem('accessKey', access_token);
        }
    }
}
