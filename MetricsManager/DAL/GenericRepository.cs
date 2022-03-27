using MetricsManager.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace MetricsManager.DAL
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> 
        where TEntity : class
    {
        private readonly MetricsDbContext _context;
        internal DbSet<TEntity> _dbSet = null!;

        public GenericRepository(MetricsDbContext context)
        {
            this._context = context;
        }

        public virtual TEntity GetByID(object id)
        {
            return _dbSet.Find(id)!;
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id)!;
            Delete(entityToDelete);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _dbSet.AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
