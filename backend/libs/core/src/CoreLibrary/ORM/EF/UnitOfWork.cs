using CoreLibrary.Exceptions;
using CoreLibrary.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace CoreLibrary.ORM.EF
{
    public class UnitOfWork(DbContext context) : IUnitOfWork
    {
        private const string msgErrorInvalidType = "UoW: Invalid type used";
        private readonly DbContext context = context;
        private readonly Hashtable repositories = [];

        public Task<int> Complete()
        {
            return this.context.SaveChangesAsync();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity).Name;

            if (!this.repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance =
                    Activator.CreateInstance(repositoryType
                        .MakeGenericType(typeof(TEntity)), this.context);

                this.repositories.Add(type, repositoryInstance);
            }

            if (this.repositories[type] is not IRepository<TEntity> repository)
                throw new TechnicalException(msgErrorInvalidType);

            return repository;
        }

        public void Dispose()
        {
            this.context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
