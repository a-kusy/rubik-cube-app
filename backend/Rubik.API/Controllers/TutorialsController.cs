using Microsoft.AspNetCore.Mvc;
using Rubik.API.Models;
using Rubik.API.Services;

namespace Rubik.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TutorialsController : ControllerBase
    {
        private readonly ITutorialsService tutorialsService;

        public TutorialsController(ITutorialsService tutorialsService)
        {
            this.tutorialsService = tutorialsService;
        }

        [HttpGet(Name = "Get all tutorial pages")]
        public ActionResult<IEnumerable<TutorialPageEntity>> GetAll()
        {
            return Ok(tutorialsService.GetAll());
        }

        [HttpGet("{id:int}", Name = "Get tutorial page by ID")]
        public ActionResult<TutorialPageEntity> GetPageById(int id)
        {
            var score = tutorialsService.GetById(id);

            if (score is null)
            {
                return NotFound();
            }

            return score;
        }

        [HttpPost(Name = "Create a tutorial page")]
        public ActionResult<TutorialPageEntity> Create([FromBody] TutorialPageEntity tutorialPageEntity)
        {
            return tutorialsService.Add(tutorialPageEntity);
        }

        [HttpPost("section", Name = "Create tutorial section")]
        public ActionResult<TutorialSectionEntity> CreateSection([FromBody] TutorialSectionEntity tutorialSectionEntity)
        {
            return tutorialsService.AddSection(tutorialSectionEntity);
        }
    }
}