using DbStorage.Interface;
using System;
using System.Collections.Generic;

namespace DbStorage.Impl
{
    public class DbRepositoryDict : IDbRepositoryDict
    {
        public static Dictionary<string, Type> RepositoryMap { get; private set; }

        public DbRepositoryDict()
        {
            RepositoryMap = new Dictionary<string, Type>();
        }

        public virtual void Add(string key, Type value)
        {
            if (RepositoryMap.ContainsKey(key))
            {
                return;
            }
            RepositoryMap.Add(key, value);
        }

        public virtual bool TryGet(string key, out Type value)
        {
            return RepositoryMap.TryGetValue(key, out value);
        }

        public virtual bool Remove(string key)
        {
            return RepositoryMap.Remove(key);
        }
    }
}
