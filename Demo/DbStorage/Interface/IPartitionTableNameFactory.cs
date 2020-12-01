namespace DbStorage.Interface
{
    public interface IPartitionTableNameFactory
    {
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="basicTableName">数据库名</param>
        /// <param name="partitionId">分表键值</param>
        /// <returns>表名</returns>
        string GetTableName(string basicTableName, object partitionId);
    }
}
