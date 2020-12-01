using System;
using System.Collections.Generic;
using System.Text;

namespace DbStorage.Interface
{
    public interface IPartitionEntity
    {
        object PartitionId { get; set; }
    }
}
