using Rubik.API.Models;
using Rubik.API.Services;

namespace Rubik.API.Helpers
{
    public static class Utilities
    {
        public static IEnumerable<RankingEntityDto> AllRankingsFormattedAndOrdered(IRankingsService rankingsService, IUsersService usersService, IScoresService scoresService)
        {
            var rankings = rankingsService.GetAll();
            var rankingsDto = new List<RankingEntityDto>();

            foreach (var r in rankings.ToList())
            {
                var username = usersService.GetById(r.UserId)!.Username;
                var score = scoresService.GetById(r.ScoreId)!;
                var newRankingEntityDto = new RankingEntityDto(r.Position, username, score.Time, score.Date);
                rankingsDto.Add(newRankingEntityDto);    
            }

            return rankingsDto.OrderBy(x => x.Position);
        }
    }
}