using ourinv.WebAPI.Models;

namespace ourinv.WebAPI.Database.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : EntityBase, new()
    {
        private readonly AppDbContext _context;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        public T Create(T entity)
        {
            var newEntity = _context.Set<T>().Add(entity);

            return newEntity.Entity;
        }

        public T Delete(T entity)
        {
            var entityToDelete = GetById(entity.Id);
            _context.Set<T>().Remove(entityToDelete);

            return entityToDelete;
        }

        public T GetById(int id)
        {
            var existingEntity = _context.Set<T>().Find(id);

            return existingEntity;
        }

        public T Update(T entity)
        {
            var updatedEntity = _context.Set<T>().Update(entity);

            return updatedEntity.Entity;
        }
    }
}
