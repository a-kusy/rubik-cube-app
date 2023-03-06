using Rubik.API.Models;

namespace Rubik.API.Services
{
    public interface IScoresService : IEntitiesService<ScoreEntity>
    {
        List<ScoreEntity>? GetByUserId(int userId);
    }
}