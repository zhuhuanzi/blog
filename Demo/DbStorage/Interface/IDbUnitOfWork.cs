using System;
using System.Collections.Generic;
using System.Text;

namespace DbStorage.Interface
{
    public interface IDbUnitOfWork :IDisposable
    {
        /// <summary>
        /// Id
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// 是否已释放
        /// </summary>
        bool IsDispose { get; }

        /// <summary>
        /// 提交
        /// </summary>
        void Commit();

        /// <summary>
        /// 回滚
        /// </summary>
        void RollBack();
    }
}
