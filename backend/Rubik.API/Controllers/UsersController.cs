using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rubik.API.Models;
using Rubik.API.Services;

namespace Rubik.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpPost("auth")]
        public IActionResult Authenticate(AuthenticateRequest req)
        {
            var res = usersService.Authenticate(req);

            if (res == null)
            {
                return BadRequest(new { message = "Nieprawidłowa nazwa użytkownika lub hasło" });
            }

            return Ok(res);
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest user)
        {
            var res = usersService.Register(user);

            if (res == null)
            {
                return BadRequest(new { message = "Użytkownik już istnieje" });
            }

            return Ok(new { message = "Rejestracja zakończona sukcesem" });
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet(Name = "Get all users")]
        public ActionResult<IEnumerable<UserEntity>> GetAll()
        {
            return Ok(usersService.GetAll());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("user", Name = "Get user")]
        public ActionResult<UserEntity> GetById()
        {
            var user = (UserEntity?)HttpContext.Items["User"];

            if (user is null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost(Name = "Create a user")]
        public ActionResult<UserEntity> Create([FromBody] UserEntity user)
        {
            return usersService.Add(user);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut(Name = "Update a user")]
        public ActionResult<UserEntity> Update([FromBody] UserUpdate userUpdate)
        {
            var userEntity = (UserEntity?)HttpContext.Items["User"];

            if (userEntity is null)
            {
                return NotFound();
            }

            var res = usersService.Update(userEntity, userUpdate);

            if (res == null)
            {
                return BadRequest(new { message = "Użytkownik o podanej nazwie już istnieje" });
            }

            return Ok(res);
        }
    }
}