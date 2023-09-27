using CommandHistory.Interfaces;
using CommandHistory.Models;
using CommandHistory.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommandHistory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly MemoryHistory _history;

        public HistoryController(MemoryHistory history)
        {
            _history = history;
        }

        [HttpGet]
        public ActionResult ShowHistory()
        {
            return Ok(_history.CommandMessages);
        }
    }
}
