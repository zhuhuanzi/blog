using DbStorage.Interface;
using System;

namespace DbStorage.Impl
{
    public class GuidDbEntityIdGen : IDbEntityIdGen
    {
        public string Next
        {
            get => Guid.NewGuid().ToString().Replace("-", string.Empty);
        }
    }
}
