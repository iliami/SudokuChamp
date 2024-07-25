namespace SudokuChamp.Server.Services.Abstract
{
    public interface ISoloGameService
    {
        int[,] CreateEasySudoku();
        int[,] CreateMediumSudoku();
        int[,] CreateHardSudoku();
        bool IsTrueSolved(int[,] sudokuBoard);
    }
}
