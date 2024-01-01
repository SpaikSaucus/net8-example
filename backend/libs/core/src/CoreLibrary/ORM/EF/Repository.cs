using CoreLibrary.Specification;
using CoreLibrary.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CoreLibrary.ORM.EF
{
    public class Repository<TEntity>(DbContext context) : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _context = context;

        public void Add(TEntity entity)
        {
            this._context.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            this._context.Set<TEntity>().AddRange(entities);
        }

        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return this.Count(predicate) > 0;
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate != null)
                return this._context.Set<TEntity>().Where(predicate).Count();
            else
                return this._context.Set<TEntity>().Count();
        }

        public TEntity? FindById(object id)
        {
            return this._context.Set<TEntity>().Find(id);
        }

        public void Remove(TEntity entity)
        {
            this._context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            this._context.Set<TEntity>().RemoveRange(entities);
        }

        public void Update(TEntity entity)
        {
            this._context.Set<TEntity>().Attach(entity);
            this._context.Entry(entity).State = EntityState.Modified;
        }

        public IEnumerable<TEntity> Find(ISpecification<TEntity> specification)
        {
            return this.ApplySpecification(specification);
        }

        private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
        {
            var inputQuery = this._context.Set<TEntity>().AsQueryable();
            return SpecificationEvaluator<TEntity>.GetQuery(inputQuery, spec);
        }
    }
}
