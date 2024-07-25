/* eslint-disable react/prop-types */
import { useState, useEffect, useContext, useRef } from "react";
import { useNavigate } from "react-router-dom";
import { Link } from "react-router-dom";
import { AuthContext } from "../AuthProvider.jsx";
import "./styles/Levels.css";

function msToTime(ms) {
    let milliseconds = (ms % 1000),
        seconds = Math.floor((ms / 1000) % 60),
        minutes = Math.floor((ms / (1000 * 60)) % 60);
    minutes = minutes < 10 ? "0" + minutes : minutes;
    seconds = seconds < 10 ? "0" + seconds : seconds;
    return minutes + ":" + seconds + "." + milliseconds;
}

const Card = ({ data }) => {
    const navigate = useNavigate();
    const { isAuthenticated } = useContext(AuthContext);
    const colors = ["green", "yellow", "red"];
    const difficulties = ["легкая", "средняя", "сложная"];
    const [isOpen, setIsOpen] = useState(false);
    const [records, setRecords] = useState([]);

    useEffect(() => { if (isOpen) { getData(); } }, [isOpen]);

    const handleCardClick = () => {
        setIsOpen(!isOpen);
    };

    const handleButtonPlayClick = (event) => {
        event.stopPropagation(); // Предотвращаем вызов handleCardClick
        navigate(`/game/${data.id}`, { state: { sudokuId: data.id, grid: data.board } });
    };

    return (
        <div
            style={{
                backgroundColor: "white",
                margin: "10px",
                padding: "10px",
            }}
            onClick={handleCardClick}
        >
            <div style={{ display: "flex", alignItems: "center" }}>
                <div
                    style={{
                        backgroundColor: colors[data.difficulty - 1],
                        width: "20px",
                        height: "20px",
                        borderRadius: "50%",
                        display: "inline-block",
                        margin: "10px",
                    }}
                ></div>
                <h2
                    style={{
                        color: "black",
                        fontWeight: "bold",
                        display: "inline-block",
                        margin: "10px",
                    }}
                >
                    {difficulties[data.difficulty - 1]}
                </h2>
                {isAuthenticated && (
                    <button
                        className="btn"
                        onClick={handleButtonPlayClick}
                    >
                        Играть
                    </button>
                )}
            </div>
            <p style={{ color: "lightgray", marginLeft: "20px" }}>
                Всего раз решили: {data.totalAttemps}
            </p>
            {isOpen && (
                <table className="table">
                    <thead>
                        <tr>
                            <th>Имя пользователя</th>
                            <th>Время решения</th>
                        </tr>
                    </thead>
                    <tbody>
                    {records.slice(0, 20).map((record, index) => (
                        <tr key={index}>
                            <td><Link to={`/profile/${record.userName}`}>{record.userName}</Link></td>
                            <td>{msToTime(record.timeMilliseconds)}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            )}
        </div>
    );


    async function getData() {
        const res = await fetch(`api/records/${data.id}`)
        const values = await res.json();
        setRecords(values);
    }
};


function Levels() {
    const [data, setData] = useState([]);
    const [page, setPage] = useState(1);

    useEffect(() => {
            loadMore();
    }, []);

    return (
        <div>
            {data.map((item) => (
                <Card key={item.id} data={item} />
            ))}
            <div style={{ display:"flex" }}>
                <button className="btn" style={{ alignItems:"center", margin: "20px" }} onClick={loadMore}>Загрузить еще</button>
            </div>
        </div>
    );

    async function loadMore() {
        const res = await fetch(`api/sudoku/${page}`)
        const values = await res.json();
        const newData = values.sort((a, b) => b.totalAttemps - a.totalAttemps);
        setData((prevData) => [...prevData, ...newData]);
        setPage((prevPage) => prevPage + 1);
    }
}

export default Levels;
