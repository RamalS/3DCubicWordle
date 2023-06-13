using _3DWordle.DAL.entities;
using _3DWordle.Repository;
using _3DWorlde.API.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace _3DWorlde.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserRepository UserRepository { get; set; }

        public UserController(UserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = UserRepository.GetByIdAsync(id);
            if (user == null)
                return BadRequest(user);

            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] User user)
        {
            var userEntity = new UserEntity()
            {
                UserName = user.UserName,
                Email = user.Email,
            };

            var response = await UserRepository.AddAsync(userEntity);

            return Ok(response);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await UserRepository.RemoveAsync(id);

            return Ok(deleted);
        }
    }
}
