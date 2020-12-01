using FreeSql;
using System;
using System.Linq.Expressions;

namespace FreeSqlDbStorage.Impl
{
    public class FSqlPartitionRepository<TEntity, TKey> :
            BaseRepository<TEntity, TKey>
            where TEntity : class
    {
        public FSqlPartitionRepository(
            IFreeSql freeSql,
            Expression<Func<TEntity, bool>> filter,
            Func<string, string> asTable)
            : base(freeSql, filter, asTable)
        {

        }


        public FSqlPartitionRepository(
           IFreeSql freeSql,
           IUnitOfWork unitOfWork,
           Expression<Func<TEntity, bool>> filter,
           Func<string, string> asTable)
           : this(freeSql, filter, asTable)
        {
            this.UnitOfWork = unitOfWork;
        }
    }
}
