using Microsoft.Extensions.Configuration;
using Rubik.API.Models;
using Rubik.API.Services;

namespace Rubik.API.Test
{
    public class ServiceTests
    {
        private readonly RankingsService rankingsService;
        private readonly UsersService usersService;
        private readonly ScoresService scoresService;
        private readonly TutorialsService tutorialsService;

        public ServiceTests()
        {
            var configurationManager = new ConfigurationManager();
            var dbContext = new TestDbContext(configurationManager);
            rankingsService = new RankingsService(dbContext);
            usersService = new UsersService(dbContext, configurationManager);
            scoresService = new ScoresService(dbContext);
            tutorialsService = new TutorialsService(dbContext);
            DbSetup.DbInit(dbContext);
        }

        [Fact]
        public void GetRankingsByUserIdTest()
        {
            var data = rankingsService.GetByUserId(1)!;
            var expected = new RankingEntity(1, 1, 4);

            Assert.Equal(expected.Position, data.Position);
            Assert.Equal(expected.UserId, data.UserId);
            Assert.Equal(expected.ScoreId, data.ScoreId);
        }

        [Fact]
        public void GetAllRankingsCountTest()
        {
            Assert.Equal(2, rankingsService.GetAll().Count());
        }

        [Fact]
        public void UpdateRankingsTest()
        {
            var toUpdate = rankingsService.GetById(1)!;
            var updated = new RankingEntity(1, 1, 3);
            rankingsService.Update(toUpdate, updated);

            var afterUpdate = rankingsService.GetById(1)!;

            Assert.Equal(3, afterUpdate.ScoreId);
        }

        [Fact]
        public void GetAllUsersCountTest()
        {
            Assert.Equal(3, usersService.GetAll().Count());
        }

        [Fact]
        public void GetUserByIdTest()
        {
            Assert.Equal("ghjk", usersService.GetById(2)?.Username);
        }

        [Fact]
        public void AddUserTest()
        {
            var newUser = new UserEntity("Jan", "www", "jakistam@nwm.pl");
            usersService.Add(newUser);

            Assert.Equal(4, usersService.GetAll().Count());
            Assert.Equal("Jan", usersService.GetById(4)?.Username);
        }

        [Fact]
        public void GetAllScoresTest()
        {
            var scores = scoresService.GetAll();
            Assert.Equal(4, scores.Count());
        }

        [Fact]
        public void AddScoreTest()
        {
            var toAdd = new ScoreEntity(32, DateTime.Now, 3);
            scoresService.Add(toAdd);

            Assert.Equal(5, scoresService.GetAll().Count());
            Assert.Equal(32, scoresService.GetById(5)?.Time);
        }

        [Fact]
        public void GetAllTutorialsTest()
        {
            var tutorials = tutorialsService.GetAll();
            var tutorialPageEntities = tutorials.ToList();
            Assert.Equal(2, tutorialPageEntities.Count);
            Assert.Equal("tut1", tutorialPageEntities.ElementAt(0).Name);
            Assert.Equal("t1s1", tutorialPageEntities.ElementAt(0).Sections.ElementAt(0).Name);
        }

        [Fact]
        public void AddTutorialSectionTest()
        {
            var toAdd = new TutorialSectionEntity("t2s2", "nawet jeszcze inny content", 2);
            tutorialsService.AddSection(toAdd);
            Assert.Equal("t2s2", tutorialsService.GetById(2)?.Sections.ElementAt(1).Name);
            Assert.Equal("nawet jeszcze inny content", tutorialsService.GetById(2)?.Sections.ElementAt(1).Content);
        }
    }
}