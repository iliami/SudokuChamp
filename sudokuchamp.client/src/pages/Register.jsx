import { useState } from "react";
import "./styles/Register.css";

export function Register() {
    const [username, setUsername] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleSubmit = async (event) => {
        event.preventDefault();

        const res = await sendData();
        console.log(username);
        if (res.ok)
        {
            alert("Вы успешно зарегистрировались");
        }
        else
        {
            const resData = await res.text();
            alert(resData);
        }
    };

    return (
        <div className="registration">
            <div className="registration-form">
                <form onSubmit={handleSubmit}>
                    <input 
                        type="text" 
                        name="username" 
                        placeholder="Имя пользователя" 
                        required 
                        value={username} 
                        onChange={e => setUsername(e.target.value)}
                    />
                    <input 
                        type="email" 
                        name="email" 
                        placeholder="Электронная почта" 
                        required 
                        value={email} 
                        onChange={e => setEmail(e.target.value)}
                    />
                    <input 
                        type="password" 
                        name="password" 
                        placeholder="Пароль" 
                        required 
                        value={password} 
                        onChange={e => setPassword(e.target.value)}
                    />
                    <input type="submit" value="Зарегистрироваться" />
                </form>
            </div>
        </div>
    );

    async function sendData() {
        const res = await fetch('api/auth/register', {
            method: 'POST',
            credentials: 'include',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                username: username,
                email: email,
                password: password
            })
        }).catch(err => alert(err));
        return res;
    }
}