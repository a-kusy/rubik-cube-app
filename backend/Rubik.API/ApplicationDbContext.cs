using Microsoft.EntityFrameworkCore;
using Rubik.API.Models;

namespace Rubik.API
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<UserEntity> Users { get; set; } = null!;

        public virtual DbSet<ScoreEntity> Scores { get; set; } = null!;

        public virtual DbSet<RankingEntity> Rankings { get; set; } = null!;

        public virtual DbSet<TutorialSectionEntity> TutorialSections { get; set; } = null!;

        public virtual DbSet<TutorialPageEntity> TutorialPages { get; set; } = null!;

        private readonly string? connectionString;

        public ApplicationDbContext(IConfiguration configuration)
        {
            if (Environment.GetEnvironmentVariable("IN_MEMORY_DATABASE") == "true")
            {
                return;
            }

            // if not using in-memory database, read connection string
            connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                               configuration.GetConnectionString("Default");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (connectionString is null)
            {
                optionsBuilder.UseInMemoryDatabase("Rubik");

                return;
            }
            
            optionsBuilder.UseMySQL(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(entity => entity.HasKey(e => e.Id));
            modelBuilder.Entity<ScoreEntity>().HasOne<UserEntity>().WithMany(x => x.Scores).HasForeignKey(x => x.UserId)
                .IsRequired();
            modelBuilder.Entity<TutorialSectionEntity>().HasOne<TutorialPageEntity>().WithMany(x => x.Sections)
                .HasForeignKey(x => x.PageId).IsRequired();
        }
    }
}