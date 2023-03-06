using Microsoft.EntityFrameworkCore;
using Rubik.API.Models;

namespace Rubik.API.Services
{
    public class TutorialsService : EntitiesService<TutorialPageEntity>, ITutorialsService
    {
        public TutorialsService(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public TutorialSectionEntity AddSection(TutorialSectionEntity tutorialSectionEntity)
        {
            tutorialSectionEntity.ModifiedDate = DateTime.Now;
            tutorialSectionEntity.CreatedDate = DateTime.Now;
            var entity = DbContext.TutorialSections.Add(tutorialSectionEntity).Entity;
            DbContext.SaveChanges();

            return entity;
        }

        public override List<TutorialPageEntity> GetAll()
        {
            return DbContext.TutorialPages.Include(x => x.Sections).ToList();
        }
    }
}