using Rubik.API.Models;

namespace Rubik.API.Services
{
    public class RankingsService : EntitiesService<RankingEntity>, IRankingsService
    {
        public RankingsService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override List<RankingEntity> GetAll()
        {
            return DbContext.Rankings.ToList();
        }

        public RankingEntity? GetByUserId(int id)
        {
            return DbContext.Rankings.FirstOrDefault(x => x.UserId == id);
        }

        public void Update(RankingEntity ranking, RankingEntity newRanking)
        {
            ranking.Position = newRanking.Position;
            ranking.ScoreId = newRanking.ScoreId;
            ranking.ModifiedDate = DateTime.Now;

            DbContext.Update(ranking);
            DbContext.SaveChanges();
        }

        public void Update(RankingEntity ranking)
        {
            ranking.ModifiedDate = DateTime.Now;
            DbContext.Update(ranking);
            DbContext.SaveChanges();
        }
    }
}