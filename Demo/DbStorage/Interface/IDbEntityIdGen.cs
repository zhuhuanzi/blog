namespace DbStorage.Interface
{
    /// <summary>
    /// 实体Id生成器.
    /// 默认实现:  GUID 去除 -
    /// </summary>
    public interface IDbEntityIdGen
    {

        /// <summary>
        /// 获取下一个Id
        /// </summary>
        string Next { get; }
    }
}
