import { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';

const Profile = () => {
    const { username } = useParams();
    const [record, setRecord] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        fetchRecord();
    }, []);

    if (loading) {
        return <div>Загрузка...</div>;
    }

    if (error) {
        return <div>Ошибка: {error}</div>;
    }

    return (
        <div style={{padding:'20px', fontSize:'1.5rem'}}>
            <h2>Личный рекорд пользователя {record.userName}</h2>
            <p>Всего игр: {record.totalGames}</p>
            <p>Лучшее время: {msToTime(record.bestTimeMilliseconds)}</p>
        </div>
    );

    async function fetchRecord() {
        try {
            const response = await fetch(`/api/records/@${username}`);
            if (!response.ok) {
                throw new Error('Ошибка при получении данных');
            }
            const data = await response.json();
            setRecord(data);
        } catch (error) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    }
};

function msToTime(ms) {
    let milliseconds = (ms % 1000),
        seconds = Math.floor((ms / 1000) % 60),
        minutes = Math.floor((ms / (1000 * 60)) % 60);
    minutes = minutes < 10 ? "0" + minutes : minutes;
    seconds = seconds < 10 ? "0" + seconds : seconds;
    return minutes + ":" + seconds + "." + milliseconds;
}

export default Profile;
