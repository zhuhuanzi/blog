namespace DbStorage.Interface
{
    /// <summary>
    /// 实体Id生成器（雪花算法）
    /// </summary>
    public interface IDbEntityIdGen
    {
        /// <summary>
        /// 获取下一个Id
        /// </summary>
        string Next { get; }
    }
}
