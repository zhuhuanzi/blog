using DbStorage.Impl;
using FreeSqlDbStorage.Interface;
using System;
using FreeSql.Internal;

namespace FreeSqlDbStorage.Impl
{
    public class FSqlProvider : IFSqlProvider
    {

        private readonly IFSqlSetting _fSqlSetting;
        // ReSharper disable once InconsistentNaming
        protected readonly IFreeSql _freSql;


        public virtual string ProviderName { get => _fSqlSetting.Name; }

        public virtual IFreeSql FSql => this._freSql;


        public FSqlProvider(IFSqlSetting fSqlSetting)
        {
            this._fSqlSetting = fSqlSetting;
            // ReSharper disable once VirtualMemberCallInConstructor
            this._freSql = CreateFSql(this._fSqlSetting);
        }


        /// <summary>
        /// 创建实例fSql实例
        /// </summary>
        /// <param name="fSqlSetting"></param>
        /// <returns></returns>
        protected virtual IFreeSql CreateFSql(IFSqlSetting fSqlSetting)
        {
            var dbType = GetDbType(fSqlSetting.DatabaseType);

            var freeSqlBuilder = new FreeSql.FreeSqlBuilder();




            //var freeSqlBuilder = new FreeSql.FreeSqlBuilder()
            //         .UseConnectionString(dbType, fSqlSetting.ConnectionString)
            //         .UseAutoSyncStructure(false)  // 同步数据库表结构,不启用,使用EFCore code first
            //         .UseLazyLoading(false)         // 懒加载
            //                                        // 全部转大写,Oracle使用
            //         .UseNameConvert(NameConvertType.ToUpper)
            //         // 全部转小写,postgresql使用
            //         .UseSyncStructureToLower(dbType == FreeSql.DataType.PostgreSQL);

            freeSqlBuilder.UseConnectionString(dbType, fSqlSetting.ConnectionString);
            freeSqlBuilder.UseAutoSyncStructure(false);//同步数据库表结构,不启用,使用EFCore code first
            freeSqlBuilder.UseLazyLoading(false); //懒加载

            if (dbType == FreeSql.DataType.Oracle)
            {
                freeSqlBuilder.UseNameConvert(NameConvertType.ToUpper); //全部转大写,Oracle使用
            }

            if (dbType == FreeSql.DataType.PostgreSQL)
            {
                freeSqlBuilder.UseNameConvert(NameConvertType.ToLower); //全部转小写,PostgreSQL 使用
            }

            // 使用sql生成命令日志
            if (fSqlSetting.UseSqlExecuteLog && fSqlSetting.SqlExecuting != null)
            {

                freeSqlBuilder = freeSqlBuilder.UseMonitorCommand(
                    fSqlSetting.SqlExecuting,
                    fSqlSetting.SqlExecuted);
            }

            var fSql = freeSqlBuilder.Build();


            // Aop配置实体
            if (fSqlSetting.ConfigEntity != null)
            {
                fSql.Aop.ConfigEntity += new EventHandler<FreeSql.Aop.ConfigEntityEventArgs>(fSqlSetting.ConfigEntity);
            }


            // Aop配置实体属性
            if (fSqlSetting.ConfigEntityProperty != null)
            {
                fSql.Aop.ConfigEntityProperty += new EventHandler<FreeSql.Aop.ConfigEntityPropertyEventArgs>(fSqlSetting.ConfigEntityProperty);

            }


            return fSql;
        }

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <param name="databaseType"></param>
        /// <returns></returns>
        protected FreeSql.DataType GetDbType(DatabaseType databaseType)
        {
            switch (databaseType)
            {
                case DatabaseType.SqlServer:
                    return FreeSql.DataType.SqlServer;
                case DatabaseType.PostgreSQL:
                    return FreeSql.DataType.PostgreSQL;
                case DatabaseType.Oracle:
                    return FreeSql.DataType.Oracle;
                case DatabaseType.Sqlite:
                    return FreeSql.DataType.Sqlite;
            }

            throw new ArgumentException("Invalid database type");
        }


        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            this.FSql?.Dispose();
        }
    }
}
