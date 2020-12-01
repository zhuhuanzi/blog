using System;
using System.Collections.Generic;
using System.Text;

namespace FreeSqlDbStorage.Interface
{
    public interface IFSqlProvider : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// FreeSql实例
        /// </summary>
        IFreeSql FSql { get; }
    }
}
