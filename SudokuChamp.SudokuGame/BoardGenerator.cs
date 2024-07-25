namespace SudokuChamp.API.SudokuGame
{
    internal class BoardGenerator
    {
        private Board board = new();
        public Board Generate(int difficulty)
        {
            RecursiveGeneration();
            RemoveCells(difficulty);
            UndoMutability();
            return board;
        }

        private void UndoMutability()
        {
            for (int row = 0; row < SudokuConstants.BOARD_SIZE; row++)
            {
                for (int col = 0; col < SudokuConstants.BOARD_SIZE; col++)
                {
                    if (board[row, col] is not null)
                    {
                        board[row, col]!.ChangeEditableToFalse();
                    }
                }
            }
        }

        private bool RecursiveGeneration()
        {
            // находим пустую ячейку
            var emptyCellPositionAtBoard = board.FindFirstEmptyCell();

            if (emptyCellPositionAtBoard.cellRow != -1 
                && emptyCellPositionAtBoard.cellCol != -1)
            {
                // генерируем список допустимых чисел в рандомном порядке
                var nums = GenerateAllowedNumsWithRandomSorting();
                // пытаемся вставить число из списка
                for (int i = 0; i < nums.Count; i++)
                {
                    // валидация вставляемого значения
                    if (!ValidateValueAt(
                        emptyCellPositionAtBoard.cellRow,
                        emptyCellPositionAtBoard.cellCol,
                        nums[i]))
                        continue;
                    
                    board[emptyCellPositionAtBoard.cellRow, emptyCellPositionAtBoard.cellCol] =
                        new Cell(nums[i], false);

                    if (RecursiveGeneration())
                        return true;

                    // если на следующем этапе рекурсии ни одно значение не подходит,
                    // то на текущем сбрасываем вставленное значение
                    board[emptyCellPositionAtBoard.cellRow, emptyCellPositionAtBoard.cellCol] = null;
                }

                return false;
            }

            return true;
        }
        
        private void RemoveCells(int count)
        {
            int i = 0;
            while (i < count)
            {
                int x = Random.Shared.Next(SudokuConstants.BOARD_SIZE);
                int y = Random.Shared.Next(SudokuConstants.BOARD_SIZE);

                if (board[x, y] is not null)
                {
                    board[x, y] = null;
                    i++;
                }
            }
        }

        private static List<int> GenerateAllowedNumsWithRandomSorting()
        {
            int[] ArrayOfNums = new int[SudokuConstants.BOARD_SIZE];
            for (int num = 1; num <= SudokuConstants.BOARD_SIZE; num++)
            {
                ArrayOfNums[num - 1] = num;
            }
            Random.Shared.Shuffle(ArrayOfNums);

            return new List<int>(ArrayOfNums);
        }

        private bool ValidateValueAt(int row, int col, int value)
        {
            var ValidateInRow = (int _row) =>
            {
                for (int _col = 0; _col < SudokuConstants.BOARD_SIZE; _col++)
                {
                    if (board[_row, _col] is null)
                        continue;

                    if (board[_row, _col]!.Value == value && _col != col)
                        return false;
                }
                return true;
            };

            var ValidateInColumn = (int _col) =>
            {
                for (int _row = 0; _row < SudokuConstants.BOARD_SIZE; _row++)
                {
                    if (board[_row, _col] is null)
                        continue;

                    if (board[_row, _col]!.Value == value && _row != row)
                        return false;
                }
                return true;
            };

            var ValidateInBlock = () =>
            {
                var firstRowInBlock = row - row % SudokuConstants.BLOCK_SIZE;
                var firstColInBlock = col - col % SudokuConstants.BLOCK_SIZE;

                for (int _row = firstRowInBlock;
                    _row < firstRowInBlock + SudokuConstants.BLOCK_SIZE; _row++)
                {
                    for (int _col = firstColInBlock;
                        _col < firstColInBlock + SudokuConstants.BLOCK_SIZE; _col++)
                    {
                        if (board[_row, _col] is null)
                            continue;

                        if (board[_row, _col]!.Value == value
                            && _row != row
                            && _col != col)
                            return false;
                    }
                }

                return true;
            };

            return ValidateInRow(row)
                && ValidateInColumn(col)
                && ValidateInBlock();
        }
    }
}
