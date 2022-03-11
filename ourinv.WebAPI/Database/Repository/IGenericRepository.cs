namespace ourinv.WebAPI.Database.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        T Create(T entity);
        T GetById(int id);
        T Update(T entity);
        T Delete(T entity);
    }
}
