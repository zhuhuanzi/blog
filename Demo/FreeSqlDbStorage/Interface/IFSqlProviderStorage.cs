using DbStorage.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeSqlDbStorage.Interface
{
    /// <summary>
    /// 单例
    /// </summary>
    public interface IFSqlProviderStorage : IAnyStorage<IFSqlProvider>
    {

    }

    public class DefaultFSqlProviderStorage : IFSqlProviderStorage
    {
        public ConcurrentDictionary<string, IFSqlProvider> DataMap { get; private set; }


        public DefaultFSqlProviderStorage(IServiceProvider serviceProvider)
        {
            DataMap = new ConcurrentDictionary<string, IFSqlProvider>();

            var tmpDataMap = serviceProvider.GetServices<IFSqlProvider>()
                .ToDictionary(item => item.ProviderName);

            foreach (var item in tmpDataMap)
            {
                this.AddOrUpdate(item.Key, item.Value);
            }
        }


        public void AddOrUpdate(string name, IFSqlProvider val)
        {
            DataMap[name] = val;
        }

        public void Clear()
        {
            DataMap.Clear();
        }

        public IFSqlProvider GetByName(string name, string defaultName)
        {
            IFSqlProvider result = null;

            if (name == null)
            {
                if (!DataMap.TryGetValue(defaultName, out result))
                {
                    throw new Exception("Unregistered default configuration");
                }
                return result;
            }
            else if (DataMap.TryGetValue(name, out result))
            {
                return result;
            }

            throw new ArgumentException($"The Provider with the name {name} was not found");
        }

        public void Remove(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            this.DataMap.Remove(name, out IFSqlProvider result);
        }
    }
}

