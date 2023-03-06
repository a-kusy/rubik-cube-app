using Rubik.API.Models;

namespace Rubik.API.Services
{
    public abstract class EntitiesService<T> : IEntitiesService<T> where T : BaseEntity
    {
        protected EntitiesService(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        protected ApplicationDbContext DbContext { get; }

        public T Add(T entity)
        {
            entity.CreatedDate = DateTime.Now;
            entity.ModifiedDate = DateTime.Now;
            entity = DbContext.Add(entity).Entity;
            DbContext.SaveChanges();

            return entity;
        }

        public void Delete(T entity)
        {
            DbContext.Remove(entity);
            DbContext.SaveChanges();
        }

        public abstract List<T> GetAll();

        public T? GetById(int id)
        {
            return DbContext.Find<T>(id);
        }
    }
}