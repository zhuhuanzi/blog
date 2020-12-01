using System;
using DbStorage;
using DbStorage.Impl;
using FreeSqlDbStorage;
using FreeSqlDbStorage.Impl;

namespace Demo
{
    class Program
    {
        // ReSharper disable once UnusedParameter.Local
        static void Main(string[] args)
        {
            var configuration = IocManager.Configuration;

            IocManager.Services.AddDbStorage();

            IocManager.Services.AddFreeSqlDbStorage(new FSqlSetting()
            {
                Name = configuration["DB"],
                ConnectionString = configuration["DB"],
                DatabaseType = DatabaseType.Sqlite,
                UseSqlExecuteLog = false,
                SqlExecuting = (a) =>
                {
                    Console.WriteLine(a.CommandText);
                }
            });


            var freeSql = new FreeSql.FreeSqlBuilder()
                .UseConnectionString(FreeSql.DataType.Sqlite,
                    configuration["DB"])
                .UseAutoSyncStructure(false) //自动同步实体结构到数据库
                .Build(); //请务必定义成 Singleton 单例模式

            freeSql.CodeFirst.SyncStructure<DemoTable>();

        }
    }
}
