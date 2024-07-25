import { useContext, useEffect } from "react";
import { Link } from "react-router-dom";
import { AuthContext } from "./AuthProvider.jsx";
import "./NavBar.css";

function getCookie(name) {
    let matches = document.cookie.match(
        new RegExp(
            "(?:^|; )" +
                name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, "\\$1") +
                "=([^;]*)"
        )
    );
    return matches ? decodeURIComponent(matches[1]) : undefined;
}

export function NavBar() {
    const { isAuthenticated, setIsAuthenticated, username, setUsername } = useContext(AuthContext);

    useEffect(() => {
        const checkAuthentication = () => {
            const cookie = getCookie("what-is-that");
            if (cookie) {
                setIsAuthenticated(true);
            }
        };
        checkAuthentication();

        var login = document.getElementById("login");
        var register = document.getElementById("registration");
        var profile = document.getElementById("profile");
        var logout = document.getElementById("logout");

        if (isAuthenticated) {
            // Пользователь авторизован
            login.style.display = "none";
            register.style.display = "none";
            profile.style.display = "inline";
            logout.style.display = "inline";
        } else {
            // Пользователь не авторизован
            login.style.display = "inline";
            register.style.display = "inline";
            profile.style.display = "none";
            logout.style.display = "none";
        }
    }, [isAuthenticated]);

    const onLogout = () => {
        var login = document.getElementById("login");
        var register = document.getElementById("registration");
        var profile = document.getElementById("profile");
        var logout = document.getElementById("logout");

        document.cookie =
            "what-is-this=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
        sessionStorage.removeItem('accessKey');
        setIsAuthenticated(false);
        setUsername("");
        login.style.display = "inline";
        register.style.display = "inline";
        profile.style.display = "none";
        logout.style.display = "none";
    };

    return (
        <header>
            <div className="container">
                <Link to="/" id="main">
                    Главная
                </Link>
                <Link to="/training" id="training">
                    Тренировка
                </Link>
                <Link to="/levels" id="levels">
                    Судоку на рекорд
                </Link>
                <Link to="/login" id="login">
                    Войти
                </Link>
                <Link to="/registration" id="registration">
                    Регистрация
                </Link>
                <Link to={`/profile/${username}`} id="profile">
                    Профиль
                </Link>
                <Link to="/" id="logout" onClick={onLogout}>
                    Выход
                </Link>
            </div>
        </header>
    );
}
