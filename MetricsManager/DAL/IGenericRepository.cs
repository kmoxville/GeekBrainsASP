namespace MetricsManager.DAL
{
    public interface IGenericRepository<TEntity> : IEnumerable<TEntity> 
        where TEntity : class
    {
        void Delete(object id);
        void Delete(TEntity entityToDelete);
        TEntity GetByID(object id);
        IEnumerable<TEntity> GetAll();
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
        void Save();
    }
}