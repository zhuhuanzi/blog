using System;
using System.Collections.Generic;
using System.Text;

namespace DbStorage.Interface
{
    public interface IDbRepositoryFactory
    {
        IDbRepository<TEntity, TPrimaryKey> GetDbRepository<TEntity, TPrimaryKey>(string key)
            where TEntity : class, IDbEntity<TPrimaryKey>;
    }
}
