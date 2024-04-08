using Microsoft.EntityFrameworkCore;
using Supplier.Business.Interfaces;
using Supplier.Business.Models;
using Supplier.Data.Context;
using System.Linq.Expressions;

namespace Supplier.Data.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity 
    {
        protected readonly MeuDbContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(MeuDbContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }

        public virtual async Task<TEntity> ObterPorId(Guid id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task<List<TEntity>> ObterTodos()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public virtual async Task Adicionar(TEntity entity)
        {
            DbSet.Add(entity);
            await Db.SaveChangesAsync();            
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            DbSet.Update(entity);
            await Db.SaveChangesAsync();
        }

        public void Dispose()
        {
            Db.Dispose();
        }
    }
}
