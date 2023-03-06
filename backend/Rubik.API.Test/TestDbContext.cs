using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Rubik.API.Test
{
    public class TestDbContext : ApplicationDbContext
    {
        public TestDbContext(IConfiguration configuration) : base(configuration)
        {
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("Rubik");
        }
    }
}