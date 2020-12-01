using DbStorage.Interface;
using FreeSql;
using System;

namespace FreeSqlDbStorage.Impl
{
    public class FreeSqlDbUnitOfWork : IDbUnitOfWork
    {
        public IUnitOfWork Outer { get; private set; }

        public Guid Id { get; private set; }

        public bool IsDispose { get; private set; }

        // ReSharper disable once InconsistentNaming
        protected Action _disposeAction;

        public FreeSqlDbUnitOfWork(IUnitOfWork unitOfWork, Action disposeAction)
        {
            Outer = unitOfWork;
            _disposeAction = disposeAction;

            this.Id = Guid.NewGuid();
        }

        public void Commit()
        {
            this.Outer?.Commit();
        }


        public void Rollback()
        {
            this.Outer?.Rollback();
        }

        public void Dispose()
        {
            if (!this.IsDispose)
            {
                this.IsDispose = true;
                this.Outer?.Dispose();
                this.Outer = null;
                this._disposeAction?.Invoke();
            }
        }

    }
}
