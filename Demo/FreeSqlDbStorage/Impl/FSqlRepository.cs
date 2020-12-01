using DbStorage;
using DbStorage.Interface;
using FreeSql;
using FreeSqlDbStorage.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FreeSqlDbStorage.Impl
{
    public class FSqlRepository<TEntity, TPrimaryKey> : IFSqlRepository<TEntity, TPrimaryKey>
        where TEntity : class, IDbEntity<TPrimaryKey>
    {

        // ReSharper disable once InconsistentNaming
        protected readonly string _entityName;
        // ReSharper disable once InconsistentNaming
        protected readonly FreeSqlRepositoryResolver _freeSqlRepositoryResolver;
        private readonly Dictionary<string, FreeSqlDbUnitOfWork> _unitOfWorkDict;

        public string ProviderName { get; private set; }
        public string OldProviderName { get; private set; }


        FreeSqlRepositoryResolver RepositoryResolver
            => this._freeSqlRepositoryResolver;
        IPartitionTableNameFactory PartitionTableNameFactory
            => this._freeSqlRepositoryResolver.PartitionTableNameFactory;
        IRelationalDatabaseProcessor RelationalDatabaseProcessor
            => this._freeSqlRepositoryResolver.RelationalDatabaseProcessorStorage.GetByName(ProviderName, FreeSqlDbStorageConsts.DefaultProviderName);


        public FSqlRepository(FreeSqlRepositoryResolver freeSqlRepositoryResolver)
        {
            _freeSqlRepositoryResolver = freeSqlRepositoryResolver;

            _entityName = typeof(TEntity).Name;
            _unitOfWorkDict = new Dictionary<string, FreeSqlDbUnitOfWork>();
        }



        #region 公开,修改数据库,获取database,启动事务,提交事务，获取当前事务

        /// <inheritdoc/>
        public IDisposable ChangeProvider(string name)
        {
            OldProviderName = ProviderName;
            ProviderName = name;

            return new DisposeAction(() =>
            {
                ProviderName = OldProviderName;
                OldProviderName = null;
            });
        }

        /// <inheritdoc/>
        public IFreeSql GetCurrentFreeSql()
        {
            return this.RepositoryResolver.GetFreeSql(this.ProviderName);
        }


        /// <inheritdoc/>
        public IDbUnitOfWork BeginUnitOfWork()
        {
            return this.BeginUnitOfWork(null);
        }


        /// <inheritdoc/>
        // ReSharper disable once MethodOverloadWithOptionalParameter
        public IDbUnitOfWork BeginUnitOfWork(Action<IUnitOfWork> unitOfWorkOptions = null)
        {
            var currentUnitOfWork = this.GetCurrentUnitOfWork();
            if (currentUnitOfWork != null)
            {
                return currentUnitOfWork;
            }


            var unitOfWorkKey = this.GetUnitOfWorkKey();

            var freeSql = this.GetCurrentFreeSql();
            var unitOfWork = freeSql.CreateUnitOfWork();

            unitOfWorkOptions?.Invoke(unitOfWork);


            var freeSqlDbUnitOfWork = new FreeSqlDbUnitOfWork(unitOfWork, () =>
            {
                this.RemoveUnitOfWork(unitOfWorkKey);
            });

            this._unitOfWorkDict[unitOfWorkKey] = freeSqlDbUnitOfWork;

            return freeSqlDbUnitOfWork;
        }


        /// <inheritdoc/>
        public IDbUnitOfWork GetCurrentUnitOfWork()
        {
            var unitOfWorkKey = this.GetUnitOfWorkKey();
            this._unitOfWorkDict.TryGetValue(unitOfWorkKey, out FreeSqlDbUnitOfWork unitOfWork);

            return unitOfWork;
        }

        /// <inheritdoc/>
        public void SaveChanges()
        {
            this.GetCurrentUnitOfWork()?.Commit();
        }

        public void Rollback()
        {
            this.GetCurrentUnitOfWork()?.Rollback();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            foreach (var item in this._unitOfWorkDict)
            {
                item.Value?.Dispose();
            }

            this._unitOfWorkDict.Clear();
        }

        #endregion


        #region 增删改查

        /// <inheritdoc/>
        public ISelect<TEntity> GetSelect(object partitionId = null)
        {
            var unitOfWork = this.GetCurrentFreeSqlUnitOfWork();

            return RepositoryResolver
                .GetRepository<TEntity, TPrimaryKey>(partitionId, this.ProviderName, unitOfWork)
                .Select;
        }

        /// <inheritdoc/>
        public ISelect<TEntity> GetSelect(DateTime start, DateTime end)
        {
            // 处理先后顺序
            if (end < start) (start, end) = (end,start);

            #region 开始/结束表名

            // 开始分表名称和结束分表名称
            var startTableName = this.PartitionTableNameFactory.GetTableName(this._entityName, start);
            var endTableName = this.PartitionTableNameFactory.GetTableName(this._entityName, end);

            // 获取实体的查询器
            var resultSelect = this.GetCurrentFreeSql().Select<TEntity>();

            // 开始和结束一致,则直接返回
            if (startTableName == endTableName)
            {
                return GetSelectPrivate(resultSelect, startTableName);
                //return this.GetSelect(start);
            }

            #endregion


            #region 组装范围查询

            // 添加开始
            resultSelect = GetSelectPrivate(resultSelect, startTableName);

            // 开始到结束范围表 临时存储变量
            var nextDateKey = start;

            // 获取 开始结束范围 中的表
            while (startTableName != endTableName)
            {
                nextDateKey = nextDateKey.AddMonths(1);
                startTableName = this.PartitionTableNameFactory.GetTableName(this._entityName, nextDateKey);
                resultSelect = GetSelectPrivate(resultSelect, startTableName);
            }

            #endregion


            // 如果当前存在事务，那么指定事务
            //var currentFreeUnitOfWork = this.GetCurrentFreeSqlUnitOfWork();
            //if (currentFreeUnitOfWork != null)
            //{
            //    return resultSelect.WithTransaction(currentFreeUnitOfWork.GetOrBeginTransaction());
            //}

            return resultSelect;
        }


        /// <inheritdoc/>
        public IQueryable<TEntity> GetQuery(object partitionId = null)
        {
            throw new NotImplementedException("The FreeSql implementation is not supported. Please use .GetSelect");
        }

        /// <inheritdoc/>
        public TEntity Get(TPrimaryKey id, object partitionId = null)
        {
            return this.GetSelect(partitionId).Where(o => o.Id.Equals(id)).First();
        }

        /// <inheritdoc/>
        public TEntity Insert(TEntity entity, object partitionId = null)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (entity is IPartitionEntity && partitionId != null)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                ((IPartitionEntity)entity).PartitionId = partitionId;
                var table = this.GetTable(partitionId);
                return table.Insert(entity);
            }
            // ReSharper disable once SuspiciousTypeConversion.Global
            else if (entity is IPartitionEntity)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                var table = this.GetTable(((IPartitionEntity)entity).PartitionId);
                return table.Insert(entity);
            }
            else
            {
                return this.GetTable().Insert(entity);
            }
        }

        /// <inheritdoc/>
        // ReSharper disable once IdentifierTypo
        public void Insert(List<TEntity> entitys, object partitionId = null)
        {
            foreach (var entity in entitys)
            {
                this.Insert(entity, partitionId);
            }

        }

        /// <inheritdoc/>
        public TEntity Update(TEntity entity, object partitionId = null)
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (entity is IPartitionEntity && partitionId != null)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                ((IPartitionEntity)entity).PartitionId = partitionId;
                var table = this.GetTable(partitionId);
                table.Update(entity);


            }
            // ReSharper disable once SuspiciousTypeConversion.Global
            else if (entity is IPartitionEntity)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                var table = this.GetTable(((IPartitionEntity)entity).PartitionId);
                table.Update(entity);
            }
            else
            {
                this.GetTable().Update(entity);
            }

            return entity;
        }

        /// <inheritdoc/>
        public void Update(List<TEntity> entitys, object partitionId = null)
        {
            foreach (var entity in entitys)
            {
                this.Update(entity, partitionId);
            }
        }

        /// <inheritdoc/>
        public void Delete(TPrimaryKey id, object partitionId = null)
        {
            this.GetTable(partitionId).Delete(id);
        }

        /// <inheritdoc/>
        public void Delete(List<TPrimaryKey> ids, object partitionId = null)
        {
            this.GetTable(partitionId).Delete(o => ids.Contains(o.Id));
        }

        /// <inheritdoc/>
        public long Count(object partitionId = null)
        {
            return this.GetSelect(partitionId).Count();
        }

        #endregion



        #region 辅助函数

        /// <summary>
        /// 调用AsTable方法，组合ISelect对象，形成类似union all的sql语句
        /// </summary>
        /// <param name="select"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private ISelect<TEntity> GetSelectPrivate(ISelect<TEntity> select, string tableName)
        {
            var newTableName = RelationalDatabaseProcessor.HandleString(tableName);
            return select.AsTable((type, oldName) => newTableName );
        }

        /// <summary>
        /// 获取表
        /// </summary>
        /// <param name="partitionId"></param>
        /// <returns></returns>
        private FSqlPartitionRepository<TEntity, TPrimaryKey> GetTable(object partitionId = null)
        {
            var currentUnitOfWork = this.GetCurrentFreeSqlUnitOfWork();


            var table = this.RepositoryResolver.GetRepository<TEntity, TPrimaryKey>(partitionId, this.ProviderName, currentUnitOfWork);

            var weakReference = new WeakReference(table);

            return weakReference.Target as FSqlPartitionRepository<TEntity, TPrimaryKey>;
        }



        private string GetUnitOfWorkKey(string name = null)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                return name;
            }

            return string.IsNullOrWhiteSpace(this.ProviderName) ? FreeSqlDbStorageConsts.DefaultProviderName : this.ProviderName;
        }


        /// <summary>
        /// 释放工作单元,默认是当前
        /// </summary>
        /// <param name="unitOfWorkKey"></param>
        private void RemoveUnitOfWork(string unitOfWorkKey = null)
        {
            unitOfWorkKey = unitOfWorkKey ?? this.GetUnitOfWorkKey();

            // 释放工作单元
            if (this._unitOfWorkDict.TryGetValue(unitOfWorkKey, out FreeSqlDbUnitOfWork unitOfWork))
            {
                unitOfWork?.Dispose();
                this._unitOfWorkDict.Remove(unitOfWorkKey);
            }
        }

        /// <summary>
        /// 获取 freesql 的工作单元
        /// </summary>
        /// <returns></returns>
        private IUnitOfWork GetCurrentFreeSqlUnitOfWork()
        {
            var dbUnitOfWork = this.GetCurrentUnitOfWork();

            if (dbUnitOfWork != null)
            {
                return (dbUnitOfWork as FreeSqlDbUnitOfWork)?.Outer;
            }

            return null;
        }

        public void BatchInsert(ICollection<TEntity> entities, object partitionId)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            if (partitionId == null)
            {
                throw new ArgumentNullException(nameof(partitionId));
            }

            // ReSharper disable once SuspiciousTypeConversion.Global
            if (entities.Any(o => !(o is IPartitionEntity)))
            {
                throw new ArgumentException($"The data entity does not implement the IPartitionEntity interface");
            }

            foreach (var entity in entities)
            {
                // ReSharper disable once PossibleNullReferenceException
                // ReSharper disable once SuspiciousTypeConversion.Global
                (entity as IPartitionEntity).PartitionId = partitionId;
            }

            this.GetTable(partitionId).Insert(entities);
        }

        #endregion
    }
}
