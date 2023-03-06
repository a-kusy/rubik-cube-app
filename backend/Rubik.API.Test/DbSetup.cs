using Rubik.API.Models;

namespace Rubik.API.Test
{
    public static class DbSetup
    {
        public static void DbInit(TestDbContext context)
        {
            context.Database.EnsureDeleted();

            var rankings = new List<RankingEntity> { new RankingEntity(1, 1, 4), new RankingEntity(2, 2, 1) };

            var users = new List<UserEntity>
            {
                new UserEntity("asdf", "1234", "idk@wp.pl"),
                new UserEntity("ghjk", "1234", "idk@wp.pl"),
                new UserEntity("qwerty", "1234", "idk@wp.pl"),
            };

            var scores = new List<ScoreEntity>
            {
                new ScoreEntity(200, DateTime.Now, 1),
                new ScoreEntity(320, DateTime.Now, 2),
                new ScoreEntity(420, DateTime.Now, 2),
                new ScoreEntity(153, DateTime.Now, 1),
            };

            var tutorials = new List<TutorialPageEntity>
            {
                new TutorialPageEntity(1, "tut1"), new TutorialPageEntity(2, "tut2"),
            };

            var tutorialSecs = new List<TutorialSectionEntity>
            {
                new TutorialSectionEntity("t1s1", "jakis content", 1),
                new TutorialSectionEntity("t1s2", "inny content", 1),
                new TutorialSectionEntity("t2s1", "jeszcze inny content", 2),
            };

            context.AddRange(scores);
            context.AddRange(users);
            context.AddRange(rankings);
            context.AddRange(tutorials);
            context.AddRange(tutorialSecs);

            context.SaveChanges();
        }
    }
}