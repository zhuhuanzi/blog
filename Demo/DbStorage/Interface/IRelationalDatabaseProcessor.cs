using System;
using System.Reflection;

namespace DbStorage.Interface
{
    /// <summary>
    /// 关系型数据库参数处理器
    /// </summary>
    public interface IRelationalDatabaseProcessor
    {
        /// <summary>
        /// 提供者名称
        /// </summary>
        string DbStorageProviderName { get; }

        /// <summary>
        /// 提供者类型
        /// </summary>
        Type DbStorageProviderType { get; }

        /// <summary>
        /// 处理实体名称
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        string HandleEntityName(Type entityType);

        /// <summary>
        /// 处理实体字段
        /// </summary>
        /// <param name="entityType">实体类型</param>
        /// <param name="propertyInfo">属性类型</param>
        /// <returns></returns>
        string HandleEntityProperty(Type entityType, PropertyInfo propertyInfo);


        /// <summary>
        /// 处理字符串,根据数据库类型决定大写，小写，不变
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string HandleString(string input);


        /// <summary>
        /// 处理连接字符串
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        string HandleConnectionString(string connectionString);
    }
}
