using CoreLibrary.Specification;
using System.Linq.Expressions;

namespace CoreLibrary.UnitOfWork
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity? FindById(object id);
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        bool Contains(Expression<Func<TEntity, bool>> predicate);
        int Count(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> Find(ISpecification<TEntity> specification);
    }
}
