using DbStorage.Interface;
using FreeSql;
using FreeSqlDbStorage.Impl;
using FreeSqlDbStorage.Interface;
using System;

namespace FreeSqlDbStorage
{
    public class FreeSqlRepositoryResolver
    {


        // ReSharper disable once InconsistentNaming
        protected readonly IServiceProvider _serviceProvider;

        // ReSharper disable once InconsistentNaming
        protected readonly IFSqlProviderStorage _freeSqlProviderStorage;

        public IRelationalDatabaseProcessorStorage RelationalDatabaseProcessorStorage { get; private set; }

        public IPartitionTableNameFactory PartitionTableNameFactory { get; private set; }


        public FreeSqlRepositoryResolver(IServiceProvider serviceProvider, IFSqlProviderStorage freeSqlProviderStorage, IRelationalDatabaseProcessorStorage relationalDatabaseProcessorStorage, IPartitionTableNameFactory partitionTableNameFactory)
        {
            _serviceProvider = serviceProvider;
            _freeSqlProviderStorage = freeSqlProviderStorage;
            RelationalDatabaseProcessorStorage = relationalDatabaseProcessorStorage;
            PartitionTableNameFactory = partitionTableNameFactory;
        }

        /// <summary>
        /// 获取FreeSql对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IFreeSql GetFreeSql(string name = null)
        {
            return this._freeSqlProviderStorage.GetByName(name,  FreeSqlDbStorageConsts.DefaultProviderName).FSql;
        }


        /// <summary>
        /// 获取仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="partitionId"></param>
        /// <param name="freeSqlName"></param>
        /// <returns></returns>
        public FSqlPartitionRepository<TEntity, TPrimaryKey> GetRepository<TEntity, TPrimaryKey>(object partitionId, string freeSqlName = null)
            where TEntity : class
        {
            var entityName = typeof(TEntity).Name;
            var tableName = this.PartitionTableNameFactory.GetTableName(entityName, partitionId);


            var freeSql = this.GetFreeSql(freeSqlName);

            var repository = new FSqlPartitionRepository<TEntity, TPrimaryKey>(freeSql, null, (oldName) =>
            {
                return this.RelationalDatabaseProcessorStorage.GetByName(freeSqlName, FreeSqlDbStorageConsts.DefaultProviderName).HandleString(tableName);
            });

            return repository;
        }

        /// <summary>
        /// 获取仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="partitionId"></param>
        /// <param name="freeSqlName"></param>
        /// <param name="unitOfWork"></param>
        /// <returns></returns>
        // ReSharper disable once MethodOverloadWithOptionalParameter
        public FSqlPartitionRepository<TEntity, TPrimaryKey> GetRepository<TEntity, TPrimaryKey>(object partitionId, string freeSqlName = null, IUnitOfWork unitOfWork = null)
              where TEntity : class
        {
            if (unitOfWork == null)
            {
                return this.GetRepository<TEntity, TPrimaryKey>(partitionId, freeSqlName);
            }


            var entityName = typeof(TEntity).Name;
            var tableName = this.PartitionTableNameFactory.GetTableName(entityName, partitionId);

            var freeSql = this.GetFreeSql(freeSqlName);

            var repository = new FSqlPartitionRepository<TEntity, TPrimaryKey>(freeSql, unitOfWork, null, (oldName) =>
            {
                return this.RelationalDatabaseProcessorStorage.GetByName(freeSqlName, FreeSqlDbStorageConsts.DefaultProviderName).HandleString(tableName);
            });

            return repository;
        }
    }

}
