namespace DbStorage.Interface
{
    public interface IDbRepositoryFactory
    {
        /// <summary>
        /// 获取仓储
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <typeparam name="TPrimaryKey"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        IDbRepository<TEntity, TPrimaryKey> GetDbRepository<TEntity, TPrimaryKey>(string key)
            where TEntity : class, IDbEntity<TPrimaryKey>;
    }
}
