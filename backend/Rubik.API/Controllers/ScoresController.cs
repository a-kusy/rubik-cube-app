using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rubik.API.Helpers;
using Rubik.API.Models;
using Rubik.API.Services;

namespace Rubik.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScoresController : ControllerBase
    {
        private readonly IScoresService scoresService;
        private readonly IRankingsService rankingsService;
        private readonly IUsersService usersService;

        public ScoresController(IScoresService scoresService, IRankingsService rankingsService, IUsersService usersService)
        {
            this.scoresService = scoresService;
            this.rankingsService = rankingsService;
            this.usersService = usersService;
        }

        [HttpGet(Name = "Get all scores")]
        public ActionResult<IEnumerable<ScoreEntity>> GetAll()
        {
            return Ok(scoresService.GetAll());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id:int}", Name = "Get score by ID")]
        public ActionResult<ScoreEntity> GetById(int id)
        {
            var score = scoresService.GetById(id);

            if (score is null)
            {
                return NotFound();
            }

            return score;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("user", Name = "Get users score")]
        public ActionResult<IEnumerable<ScoreEntity>> GetByUserId()
        {
            var user = (UserEntity)HttpContext.Items["User"]!;
            var scores = scoresService.GetByUserId(user.Id);

            if (scores is null)
            {
                return NotFound();
            }

            return Ok(scores);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(Name = "Create a score")]
        public ActionResult<ScoreEntity> Create([FromBody] ScoreEntity score)
        {
            var user = (UserEntity)HttpContext.Items["User"]!;
            score.UserId = user.Id;
            score = scoresService.Add(score);
            UpdateRankingOnNewScore(score);

            return score;
        }
        
        /// <returns>
        ///     True if ranking was updated, false if the score wasn't a personal highscore.
        /// </returns>
        private void UpdateRankingOnNewScore(ScoreEntity score)
        {
            var ranking = rankingsService.GetByUserId(score.UserId);
            
            if (!IsPersonalHighscore(score, ranking))
            {
                return;
            }

            var oldPosition = ranking?.Position;
            var newPosition = GetPositionByTime(score.Time);

            if (score.Id == 0)
            {
                throw new ArgumentException("Score must have a set ID to use it in Ranking");
            }

            ShiftRankingsDown(newPosition, oldPosition);

            if (ranking is null)
            {
                ranking = new RankingEntity(newPosition, score.UserId, score.Id);
                rankingsService.Add(ranking);

                return;
            }            
            
            ranking.ScoreId = score.Id;
            ranking.Position = newPosition;
            
            rankingsService.Update(ranking);
        }

        private int GetPositionByTime(int timeMs)
        {
            var allFormattedAndOrdered = Utilities.AllRankingsFormattedAndOrdered(rankingsService, usersService, scoresService).ToList();
            var firstWorseRanking = allFormattedAndOrdered.FirstOrDefault(x => x.Time > timeMs);

            if (firstWorseRanking is null) // the worst ranking ever
            {
                return allFormattedAndOrdered.Last().Position + 1;
            }

            return firstWorseRanking.Position;
        }

        private bool IsPersonalHighscore(ScoreEntity newScore, RankingEntity? currentRanking)
        {
            if (currentRanking is null)
            {
                return true;
            }

            var currentHighscore = scoresService.GetById(currentRanking.ScoreId)!;

            return newScore.Time < currentHighscore.Time;
        }

        /// <summary>
        ///     Shifts rankings with <see cref="RankingEntity.Position"/>s
        ///     from <paramref name="top"/> inclusive to <paramref name="bottom"/> exclusive.
        /// </summary>
        /// <param name="top">Inclusive</param>
        /// <param name="bottom">Exclusive</param>
        private void ShiftRankingsDown(int top, int? bottom)
        {
            var rankingsToUpdate = rankingsService.GetAll()
                .Where(x => x.Position >= top);

            if (bottom is not null)
            {
                rankingsToUpdate = rankingsToUpdate.Where(x => x.Position < bottom);
            }

            foreach (var rankingEntity in rankingsToUpdate)
            {
                rankingEntity.Position += 1;
                rankingsService.Update(rankingEntity);
            }
        }
    }
}