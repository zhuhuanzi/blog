using DbStorage.Interface;

namespace DbStorage.Impl
{
    /// <summary>
    /// 用于分区表的描述信息
    /// </summary>
    public class PartitionTableInfo : IPartitionTableInfo
    {
        public string Id { get; set; }

        /// <summary>
        /// 实际映射到的分区表名
        /// </summary>
        public string TableName { get; set; }

    }
}
