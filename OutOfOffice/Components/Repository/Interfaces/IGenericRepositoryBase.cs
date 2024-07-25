using OutOfOffice.Components.Backend;

namespace OutOfOffice.Components.Repository.Interfaces
{
    public interface IGenericRepositoryBase<TEntity> where TEntity : IEntity
    {
        Task<TEntity> GetById(long id);

        List<TEntity> GetAll();

        Task<List<TEntity>> GetAllAsync();

        Task<bool> Update(TEntity entity);

        Task<bool> Create(TEntity entity);

        Task<bool> Remove(long id);

    }
}
