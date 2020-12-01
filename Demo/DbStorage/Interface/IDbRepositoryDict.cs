using System;
using System.Collections.Generic;
using System.Text;

namespace DbStorage.Interface
{
    /// <summary>
    /// 仓储类型映射字典
    /// </summary>
    public interface IDbRepositoryDict
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Add(string key, Type value);

        /// <summary>
        /// 尝试获取
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGet(string key, out Type value);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Remove(string key);
    }
}
