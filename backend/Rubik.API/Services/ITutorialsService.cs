using Rubik.API.Models;

namespace Rubik.API.Services
{
    public interface ITutorialsService : IEntitiesService<TutorialPageEntity>
    {
        TutorialSectionEntity AddSection(TutorialSectionEntity tutorialSectionEntity);
    }
}