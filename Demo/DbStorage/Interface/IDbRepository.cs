using System;
using System.Collections.Generic;
using System.Linq;

namespace DbStorage.Interface
{
    public interface IDbRepository<TEntity, TPrimaryKey> : IDisposable
    {
        /// <summary>
        /// 修改Provider
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IDisposable ChangeProvider(string name);


        /// <summary>
        /// 获取当前的工作单元对象
        /// </summary>
        /// <returns></returns>
        IDbUnitOfWork GetCurrentUnitOfWork();

        /// <summary>
        /// 启动工作单元
        /// </summary>
        /// <returns></returns>
        IDbUnitOfWork BeginUnitOfWork();


        /// <summary>
        /// 获取分表查询器
        /// </summary>
        /// <param name="partitionId">分表键值</param>
        /// <returns></returns>
        IQueryable<TEntity> GetQuery(object partitionId = null);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partitionId">分表键值</param>
        /// <returns></returns>
        TEntity Get(TPrimaryKey id, object partitionId = null);

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="partitionId">分表键值</param>
        /// <returns></returns>
        TEntity Insert(TEntity entity, object partitionId = null);

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="partitionId">分表键值</param>
        void Insert(List<TEntity> entitys, object partitionId = null);

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="partitionId">分表键值</param>
        /// <returns></returns>
        TEntity Update(TEntity entity, object partitionId = null);

        /// <summary>
        /// 批量更新数据
        /// </summary>
        /// <param name="entitys"></param>
        /// <param name="partitionId">分表键值</param>
        void Update(List<TEntity> entitys, object partitionId = null);

        /// <summary>
        /// 根据id删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partitionId">分表键值</param>
        void Delete(TPrimaryKey id, object partitionId = null);


        /// <summary>
        /// 根据id批量删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="partitionId">分表键值</param>
        void Delete(List<TPrimaryKey> ids, object partitionId = null);

        /// <summary>
        /// 获取表数据总数
        /// </summary>
        /// <param name="partitionId">分表键值</param>
        /// <returns>总数</returns>
        long Count(object partitionId = null);

        /// <summary>
        /// 提交更改
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// 回滚
        /// </summary>
        void Rollback();

        /// <summary>
        /// 批量插入,必须指定 partitionId
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="partitionId"></param>
        void BatchInsert(ICollection<TEntity> entities, object partitionId);
    }
}
