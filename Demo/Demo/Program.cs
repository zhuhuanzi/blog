using System;
using System.Collections.Generic;
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

            CreateInitDemoData(freeSql);


            //编写一个查询语句

            
            Test(freeSql);

        }


        private static void Test(IFreeSql freeSql)
        {

            var data = freeSql.Select<DemoTable>()
                .Where(x => x.CreateTime.Date == DateTime.Now.Date).ToList();




        }


        /// <summary>
        /// 创建测试数据
        /// </summary>
        /// <param name="freeSql"></param>
        private static void CreateInitDemoData(IFreeSql freeSql)
        {
            var date = DateTime.Now.Date;

            var data = new List<DemoTable>();

            for (int i = 0; i < 24; i++)
            {
                for (int j = 0; j < 10000; j++)
                {
                    data.Add(new DemoTable()
                    {
                        CreateTime = date.AddHours(i),
                        FkId = j,
                        PartitionId = date.AddHours(i),
                        Remark = $"Demo程序演示表数据{i}{j}"
                    });
                }
            }

            freeSql.Insert(data).ExecuteAffrows();
        }
    }
}
