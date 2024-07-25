import { Route, Routes } from "react-router-dom";
import { NavBar } from "./NavBar.jsx";
import { Solo } from "./pages/Solo.jsx";
import { Home } from "./pages/Home.jsx";
import { Login } from "./pages/Login.jsx";
import { Register } from "./pages/Register.jsx";
import { AuthProvider } from "./AuthProvider.jsx";
import Levels from "./pages/Levels.jsx";
import Profile from "./pages/Profile.jsx";
import { Game } from "./pages/Game.jsx";

function App() {
    return (
        <AuthProvider>
            <NavBar />
            <Routes>
                <Route path="/" element={ <Home /> } />
                <Route path="/login" element={ <Login /> } />
                <Route path="/registration" element={ <Register /> } />
                <Route path="/profile/:username" element={ < Profile /> } />
                <Route path="/training" element={ <Solo /> } />
                <Route path="/levels" element={ <Levels /> } />
                <Route path="/game/:id" element={ <Game /> } />
            </Routes>
        </AuthProvider>
    );
}

export default App;