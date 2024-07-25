using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using OutOfOffice.Components.Backend;
using OutOfOffice.Components.Repository.Interfaces;

namespace OutOfOffice.Components.Repository
{
    public class GenericRepositoryBase<TEntity> : IGenericRepositoryBase<TEntity> where TEntity : class, IEntity
    {

        protected readonly ApplicationDbContext dbContext;


        public GenericRepositoryBase(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> Create(TEntity entity)
        {
            EntityEntry<TEntity> entityEntry = await dbContext.AddAsync(entity);
            int created = await dbContext.SaveChangesAsync();
            return created>0;
        }

        public async Task<TEntity> GetById(long id)
        {
            TEntity entity = await dbContext.FindAsync<TEntity>(id);
            return entity;
        }

        public List<TEntity> GetAll()
        {
            return dbContext.Set<TEntity>().AsNoTracking().ToList();
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<bool> Remove(long id)
        {
            TEntity entity = await GetById(id);
            dbContext.Remove(entity);
            int removed = await dbContext.SaveChangesAsync();
            return removed > 0;
        }

        public async Task<bool> Update(TEntity entity)
        {
            this.dbContext.Update(entity);
            int updated = await dbContext.SaveChangesAsync();
            return updated > 0;
        }

        protected virtual IQueryable<TEntity> GetQuerable()
        {
            return dbContext.Set<TEntity>().AsNoTracking();
        }


        public virtual async Task<TEntity> FindSingleAsync(IQueryable<TEntity> querable)
        {
            return await querable.SingleOrDefaultAsync();
        }

        public virtual async Task<TEntity> FindSingleReadOnlyAsync(IQueryable<TEntity> querable)
        {
            querable = querable.AsNoTracking();
            return await querable.SingleOrDefaultAsync();
        }

        public virtual async Task<List<TEntity>> FindAsync(IQueryable<TEntity> querable)
        {
            return await querable.ToListAsync();
        }

        public virtual async Task<List<TEntity>> FindReadOnlyAsync(IQueryable<TEntity> querable)
        {
            querable = querable.AsNoTracking();
            return await querable.ToListAsync();
        }

        public virtual async Task<TEntity> FirstAsync(IQueryable<TEntity> querable)
        {
            return await querable.FirstOrDefaultAsync();
        }
    }
}
