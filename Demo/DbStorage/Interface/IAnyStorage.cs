using System.Collections.Concurrent;

namespace DbStorage.Interface
{
    public interface IAnyStorage<T> where T:class
    {
        ConcurrentDictionary<string,T> DataMap { get; set; }

        T GetByName(string name, string defaultName);

        void AddOrUpdate(string name, T val);

        void Remove(string name);

        void Clear();
    }
}
