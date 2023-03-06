using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rubik.API.Helpers;
using Rubik.API.Models;
using Rubik.API.Services;

namespace Rubik.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RankingsController : ControllerBase
    {
        private readonly IRankingsService rankingsService;
        private readonly IUsersService usersService;
        private readonly IScoresService scoresService;

        public RankingsController(IRankingsService rankingsService, IUsersService usersService, IScoresService scoresService)
        {
            this.rankingsService = rankingsService;
            this.usersService = usersService;
            this.scoresService = scoresService;
        }

        [HttpGet(Name = "Get all ranking")]
        public ActionResult<IEnumerable<RankingEntity>> GetAll()
        {
            return Ok(rankingsService.GetAll());
        }

        [HttpGet("{id:int}", Name = "Get ranking by ID")]
        public ActionResult<IEnumerable<RankingEntity>> GetById(int id)
        {
            return Ok(rankingsService.GetById(id));
        }

        [HttpGet("format", Name = "Get all ranking in format {pos, username, time, date}")]
        public ActionResult<IEnumerable<RankingEntityDto>> GetAllFormattedAndOrdered()
        {
            return Ok(Utilities.AllRankingsFormattedAndOrdered(rankingsService, usersService, scoresService));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("user", Name = "Get users ranking")]
        public ActionResult<RankingEntity> GetByUserId()
        {
            var user = (UserEntity?)HttpContext.Items["User"];

            if (user is null)
            {
                return Unauthorized();
            }
            
            var ranking = rankingsService.GetByUserId(user.Id);

            if (ranking is null)
            {
                return NotFound();
            }

            return ranking;
        }
    }
}