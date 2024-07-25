using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuChamp.Server.DTO.Record;
using SudokuChamp.Server.Services.Abstract;

namespace SudokuChamp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecordsController : ControllerBase
    {
        private readonly IRecordService recordService;

        public RecordsController(IRecordService recordService)
        {
            this.recordService = recordService;
        }

        [HttpGet("@{userName}")]
        public async Task<ActionResult<RecordResponseDTO>> Get(string userName) 
        {
            try
            {
                var record = await recordService.Get(userName);
                return Ok(record);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{sudokuId}")]
        public async Task<ActionResult<IEnumerable<SudokuRecordResponseDTO>>> Get(Guid sudokuId)
        {
            try
            {
                var records = await recordService.GetBySudokuId(sudokuId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("new-record")]
        public async Task<IActionResult> PostNewRecord(SudokuRecordRequestDTO requestDTO)
        {
            try
            {
                var userId = Guid.Parse(User.Claims.First(c => c.Type == "userId").Value);
                await recordService.AddRecord(userId, requestDTO.SudokuId, requestDTO.TimeMilliseconds);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
