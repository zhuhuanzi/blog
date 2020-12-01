using DbStorage.Impl;
using FreeSql.Aop;
using FreeSqlDbStorage.Interface;
using System;
using System.Data.Common;

namespace FreeSqlDbStorage.Impl
{
    public class FSqlSetting : IFSqlSetting
    {
        public virtual string Name { get; set; }
        public virtual DatabaseType DatabaseType { get; set; }

        public virtual string ConnectionString { get; set; }

        public virtual Action<object, ConfigEntityEventArgs> ConfigEntity { get; set; }

        public virtual Action<object, ConfigEntityPropertyEventArgs> ConfigEntityProperty { get; set; }

        public virtual bool UseSqlExecuteLog { get; set; }

        public virtual Action<DbCommand> SqlExecuting { get; set; }

        public virtual Action<DbCommand, string> SqlExecuted { get; set; }
    }
}
