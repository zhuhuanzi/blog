using System;
using FreeSql.Aop;
using System.Data.Common;
using DbStorage.Impl;

namespace FreeSqlDbStorage.Interface
{
    public interface IFSqlSetting
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        string Name { get; set; }


        /// <summary>
        /// 数据库类型
        /// </summary>
        DatabaseType DatabaseType { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        string ConnectionString { get; set; }


        /// <summary>
        /// 实体属性配置函数
        /// </summary>
        Action<object, ConfigEntityEventArgs> ConfigEntity { get; set; }

        /// <summary>
        /// 实体属性配置函数
        /// </summary>
        Action<object, ConfigEntityPropertyEventArgs> ConfigEntityProperty { get; set; }


        /// <summary>
        /// 使用Sql执行日志
        /// </summary>
        bool UseSqlExecuteLog { get; set; }

        /// <summary>
        /// Sql执行前触发回调函数 (Debug使用,发布使用影响性能)
        /// </summary>
        Action<DbCommand> SqlExecuting { get; set; }


        /// <summary>
        /// Sql执行完成触发回调函数 (Debug使用,发布使用影响性能)
        /// </summary>
        Action<DbCommand, string> SqlExecuted { get; set; }

    }
}
