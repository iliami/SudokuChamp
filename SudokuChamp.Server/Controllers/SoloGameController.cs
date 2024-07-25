using Microsoft.AspNetCore.Mvc;
using SudokuChamp.Server.Services.Abstract;
using SudokuChamp.Server.Utils;

namespace SudokuChamp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SoloGameController : ControllerBase
    {
        private readonly ISoloGameService gameService;

        public SoloGameController(ISoloGameService gameService)
        {
            this.gameService = gameService;
        }

        [HttpGet("easy-game")]
        public ActionResult<int[][]> GetEasyGame()
        {
            var sudokuBoard = gameService.CreateEasySudoku();
            return sudokuBoard.ToJagged();
        }

        [HttpGet("medium-game")]
        public ActionResult<int[][]> GetMediumGame()
        {
            var sudokuBoard = gameService.CreateMediumSudoku();
            return sudokuBoard.ToJagged();
        }

        [HttpGet("hard-game")]
        public ActionResult<int[][]> GetHardGame()
        {
            var sudokuBoard = gameService.CreateHardSudoku();
            return sudokuBoard.ToJagged();
        }

        [HttpPost("validate-game")]
        public ActionResult<bool> ValidateGame([FromBody] int[][] sudokuBoard)
        {
            var sudoku = sudokuBoard.ToMultiD();
            return gameService.IsTrueSolved(sudoku);
        }
    }
}
