using Rubik.API.Models;

namespace Rubik.API.Services
{
    public interface IRankingsService : IEntitiesService<RankingEntity>
    {
        RankingEntity? GetByUserId(int id);
        void Update(RankingEntity ranking, RankingEntity newRanking);
        void Update(RankingEntity ranking);
    }
}