using Rubik.API.Models;

namespace Rubik.API.Services
{
    public class ScoresService : EntitiesService<ScoreEntity>, IScoresService
    {
        public ScoresService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public override List<ScoreEntity> GetAll()
        {
            return DbContext.Scores.ToList();
        }

        public List<ScoreEntity>? GetByUserId(int userId)
        {
            if (DbContext.Find<UserEntity>(userId) is null)
            {
                return null;
            }

            return DbContext.Scores.Where(s => s.UserId == userId).ToList();
        }
    }
}