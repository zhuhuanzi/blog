using System;
using System.Collections.Generic;
using System.Text;

namespace DbStorage.Interface
{
    /// <summary>
    /// 用于分区表的描述信息
    /// </summary>
    public interface IPartitionTableInfo:IDbEntity<string>
    {
        /// <summary>
        /// 实际映射到的分表名
        /// </summary>
        string TableName { get; set; }
    }
}
