using System;
using System.Collections.Generic;
using System.Text;

namespace DbStorage.Interface
{
    public interface IPartitionTableNameFactory
    {
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="basicTable">分表键值</param>
        /// <param name="partitionId">表名</param>
        /// <returns></returns>
        string GetTableName(string basicTable, object partitionId);
    }
}
