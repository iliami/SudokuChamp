using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SudokuChamp.Server.DTO.Sudoku;
using SudokuChamp.Server.Services.Abstract;

namespace SudokuChamp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SudokuController : ControllerBase
    {
        private readonly ISudokuService sudokuService;

        public SudokuController(ISudokuService sudokuService)
        {
            this.sudokuService = sudokuService;
        }

        [HttpGet("{sudokuId:guid}")]
        public async Task<SudokuDTO> GetSudoku(Guid sudokuId)
            => await sudokuService.Get(sudokuId);

        [HttpGet("{page:int}")]
        public async Task<IEnumerable<SudokuDTO>> GetSudokusByPage(int page)
            => await sudokuService.GetByPage(page);
    }
}
