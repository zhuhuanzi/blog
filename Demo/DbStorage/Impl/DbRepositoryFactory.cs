using DbStorage.Interface;
using System;

namespace DbStorage.Impl
{
    public class DbRepositoryFactory : IDbRepositoryFactory
    {
        // ReSharper disable once InconsistentNaming
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IDbRepositoryDict _dbRepositoryDict;

        public DbRepositoryFactory(IServiceProvider serviceProvider, IDbRepositoryDict repositoryDict)
        {
            this._serviceProvider = serviceProvider;
            this._dbRepositoryDict = repositoryDict;
        }


        public virtual IDbRepository<TEntity, TPrimaryKey> GetDbRepository<TEntity, TPrimaryKey>(string key)
            where TEntity : class, IDbEntity<TPrimaryKey>
        {
            if (!_dbRepositoryDict.TryGet(key, out Type repositoryType))
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("The repository-type  for the key value cannot be found");
            }

            var constructedRepositoryType = repositoryType.MakeGenericType(typeof(TEntity), typeof(TPrimaryKey));

            var result = _serviceProvider.GetService(constructedRepositoryType);

            return (IDbRepository<TEntity, TPrimaryKey>)result;
        }
    }
}
