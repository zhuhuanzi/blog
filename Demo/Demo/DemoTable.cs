using System;
using DbStorage.Interface;
using FreeSql.DataAnnotations;

namespace Demo
{
    public class DemoTable : IDbEntity<long>,IPartitionEntity
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public long Id { get; set; }
        public object PartitionId { get; set; }

        public string Remark { get; set; }

        public long FkId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}