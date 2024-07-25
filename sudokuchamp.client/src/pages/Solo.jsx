/* eslint-disable react/prop-types */
import { useState, useEffect, useRef } from "react";
import {
    GRID_SIZE,
    BOX_SIZE,
    convertPositionToIndex,
    convertIndexToPosition,
} from "./Sudoku/utilities.js";
import { Sudoku } from "./Sudoku/sudoku.js";
import "./styles/Solo.css";

function Number({ value, onClick }) {
    return (
        <div className="number" onClick={() => onClick(value)}>
            {value}
        </div>
    );
}

export function Solo() {
    const sudokuRef = useRef(null);
    const [grid, setGrid] = useState([]);
    const [isFill, setIsFill] = useState(false);
    const cellsRef = useRef([]);
    const selectedCellIndexRef = useRef(null);
    const selectedCellRef = useRef(null);
    const removerRef = useRef(null);

    useEffect(() => {
        if (!isFill) {
            getData();
        }
    }, [isFill]);

    useEffect(() => {
        if (grid.length > 0) {
            sudokuRef.current = new Sudoku(grid);
            init();
        }

        function init() {
            initCells();
            initRemover();
            initKeyEvent();
        }

        function initCells() {
            fillCells();
            initCellsEvent();
        }

        function fillCells() {
            for (let i = 0; i < GRID_SIZE * GRID_SIZE; i++) {
                const { row, column } = convertIndexToPosition(i);

                if (sudokuRef.current.grid[row][column] !== null) {
                    cellsRef.current[i].classList.add("filled");
                    cellsRef.current[i].innerHTML = sudokuRef.current.grid[row][column];
                }
            }
        }

        function initCellsEvent() {
            cellsRef.current.forEach((cell, index) => {
                cell.addEventListener("click", () => onCellClick(cell, index));
            });
        }

        function onCellClick(clickedCell, index) {
            cellsRef.current.forEach((cell) =>
                cell.classList.remove("highlighted", "selected", "error")
            );

            if (clickedCell.classList.contains("filled")) {
                selectedCellIndexRef.current = null;
                selectedCellRef.current = null;
            } else {
                selectedCellIndexRef.current = index;
                selectedCellRef.current = clickedCell;
                clickedCell.classList.add("selected");
                highlightCellsBy(index);
            }

            if (clickedCell.innerHTML === "") return;
            cellsRef.current.forEach((cell) => {
                if (cell.innerHTML === clickedCell.innerHTML)
                    cell.classList.add("selected");
            });
        }

        function highlightCellsBy(index) {
            highlightColumnBy(index);
            highlightRowBy(index);
            highlightBoxBy(index);
        }

        function highlightColumnBy(index) {
            const column = index % GRID_SIZE;
            for (let row = 0; row < GRID_SIZE; row++) {
                const cellIndex = convertPositionToIndex(row, column);
                cellsRef.current[cellIndex].classList.add("highlighted");
            }
        }

        function highlightRowBy(index) {
            const row = Math.floor(index / GRID_SIZE);
            for (let column = 0; column < GRID_SIZE; column++) {
                const cellIndex = convertPositionToIndex(row, column);
                cellsRef.current[cellIndex].classList.add("highlighted");
            }
        }

        function highlightBoxBy(index) {
            const column = index % GRID_SIZE;
            const row = Math.floor(index / GRID_SIZE);
            const firstRowInBox = row - (row % BOX_SIZE);
            const firstColumnInBox = column - (column % BOX_SIZE);

            for (
                let iRow = firstRowInBox;
                iRow < firstRowInBox + BOX_SIZE;
                iRow++
            ) {
                for (
                    let iColumn = firstColumnInBox;
                    iColumn < firstColumnInBox + BOX_SIZE;
                    iColumn++
                ) {
                    const cellIndex = convertPositionToIndex(iRow, iColumn);
                    cellsRef.current[cellIndex].classList.add("highlighted");
                }
            }
        }

        function initRemover() {
            if (removerRef.current)
                removerRef.current.addEventListener("click", onRemoveClick);
        }

        function onRemoveClick() {
            if (!selectedCellRef.current) return;
            if (selectedCellRef.current.classList.contains("filled")) return;

            cellsRef.current.forEach((cell) =>
                cell.classList.remove("error", "zoom", "shake", "selected")
            );
            selectedCellRef.current.classList.add("selected");
            const { row, column } = convertIndexToPosition(
                selectedCellIndexRef.current
            );
            selectedCellRef.current.innerHTML = "";
            sudokuRef.current.grid[row][column] = null;
        }

        function initKeyEvent() {
            document.addEventListener("keydown", (event) => {
                if (event.key === "Backspace") {
                    onRemoveClick();
                } else if (event.key >= "1" && event.key <= "9") {
                    onNumberClick(parseInt(event.key));
                }
            });
        }
    }, [grid]);

    function onNumberClick(number) {
        if (!selectedCellRef.current) return;
        if (selectedCellRef.current.classList.contains("filled")) return;

        cellsRef.current.forEach((cell) =>
            cell.classList.remove("error", "zoom", "shake", "selected")
        );
        selectedCellRef.current.classList.add("selected");
        setValueInSelectedCell(number);

        if (!sudokuRef.current.hasEmptyCells()) {
            setTimeout(() => winAnimation(), 500);
        }
    }

    function setValueInSelectedCell(value) {
        const { row, column } = convertIndexToPosition(
            selectedCellIndexRef.current
        );
        const duplicatesPositions = sudokuRef.current.getDuplicatePositions(
            row,
            column,
            value
        );
        if (duplicatesPositions.length) {
            highlightDuplicates(duplicatesPositions);
            return;
        }
        sudokuRef.current.grid[row][column] = value;
        selectedCellRef.current.innerHTML = value;
        setTimeout(() => selectedCellRef.current.classList.add("zoom"), 0);
    }

    function highlightDuplicates(duplicatesPositions) {
        duplicatesPositions.forEach((duplicate) => {
            const index = convertPositionToIndex(
                duplicate.row,
                duplicate.column
            );
            setTimeout(
                () => cellsRef.current[index].classList.add("error", "shake"),
                0
            );
        });
    }

    function winAnimation() {
        cellsRef.current.forEach((cell) =>
            cell.classList.remove("highlighted", "selected", "zoom")
        );
        cellsRef.current.forEach((cell, i) => {
            setTimeout(() => cell.classList.add("highlighted", "zoom"), i * 15);
        });
        for (let i = 1; i < 8; i++) {
            setTimeout(
                () =>
                    cellsRef.current.forEach((cell) =>
                        cell.classList.toggle("highlighted")
                    ),
                500 + cellsRef.current.length * 15 + 300 * i
            );
        }
    }

    return (
        <div className="sudoku">
            <div className="wrap">
                <div className="grid">
                    {Array(81)
                        .fill()
                        .map((_, i) => (
                            <div
                                className="cell"
                                key={i}
                                ref={(el) => (cellsRef.current[i] = el)}
                            ></div>
                        ))}
                </div>

                <div className="numbers">
                    {Array.from({ length: 9 }, (_, i) => (
                        <Number key={i} value={i + 1} onClick={onNumberClick} />
                    ))}
                    <div className="remove" ref={removerRef}>
                        ✕
                    </div>
                </div>
            </div>
        </div>
    );

    async function getData() {
          await fetch("api/sologame/easy-game", {
                headers: {
                    "Content-Type": "application/json",
                }
            })
            .then(res => res.json())
            .then(values => {
                for (let row = 0; row < values.length; row++) {
                    for (let col = 0; col < values[row].length; col++) {
                        if (values[row][col] === 0) values[row][col] = null;
                    }
                }

                setGrid(values);
                setIsFill(true);
            });
    }
}
