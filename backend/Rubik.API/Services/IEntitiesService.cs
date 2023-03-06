namespace Rubik.API.Services
{
    public interface IEntitiesService<T>
    {
        public T? GetById(int id);

        public List<T> GetAll();

        public void Delete(T entity);

        public T Add(T entity);
    }
}