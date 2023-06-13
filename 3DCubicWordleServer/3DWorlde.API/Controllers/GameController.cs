using _3DWordle.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _3DWorlde.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        public GameRepository GameRepository { get; set; }

        public GameController(GameRepository gameRepository)
        {
            GameRepository = gameRepository;
        }

        // GET api/<GameController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var game = GameRepository.GetByIdAsync(id);
            if (game == null)
                return BadRequest(game);

            return Ok(game);
        }

        // POST api/<GameController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<GameController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<GameController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await GameRepository.RemoveAsync(id);

            return Ok(deleted);
        }
    }
}
