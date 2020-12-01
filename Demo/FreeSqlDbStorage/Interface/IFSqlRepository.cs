using DbStorage.Interface;
using FreeSql;
using System;

namespace FreeSqlDbStorage.Interface
{
    public interface IFSqlRepository<TEntity, TPrimaryKey> : IDbRepository<TEntity, TPrimaryKey>
        where TEntity : class, IDbEntity<TPrimaryKey>
    {
        /// <summary>
        /// 获取FreeSql对象
        /// </summary>
        /// <returns></returns>
        IFreeSql GetCurrentFreeSql();

     

        /// <summary>
        /// 启动工作单元
        /// </summary>
        /// <param name="unitOfWorkOptions">事务配置函数</param>
        /// <returns></returns>
        IDbUnitOfWork BeginUnitOfWork(Action<IUnitOfWork> unitOfWorkOptions = null);


        /// <summary>
        /// 获取freesql查询器(分表)
        /// </summary>
        /// <param name="partitionId">分表键值</param>
        /// <returns></returns>
        ISelect<TEntity> GetSelect(object partitionId = null);


        /// <summary>
        /// 获取freesql查询器(时间范围分表)
        /// </summary>
        /// <param name="start">开始日期</param>
        /// <param name="end">结束日期</param>
        /// <returns></returns>
        ISelect<TEntity> GetSelect(DateTime start, DateTime end);
    }
}
