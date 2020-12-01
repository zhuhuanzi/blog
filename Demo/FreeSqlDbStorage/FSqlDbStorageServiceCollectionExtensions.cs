using DbStorage;
using DbStorage.Interface;
using FreeSqlDbStorage.Impl;
using FreeSqlDbStorage.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreeSqlDbStorage
{
    public static class FSqlDbStorageServiceCollectionExtensions
    {
        /// <summary>
        /// 添加FreeSql支持
        /// </summary>
        /// <param name="services"></param>
        /// <param name="defaultDbSetting"></param>
        /// <returns></returns>
        public static IServiceCollection AddFreeSqlDbStorage(this IServiceCollection services,
            IFSqlSetting defaultDbSetting)
        {

            if (defaultDbSetting == null)
            {
                throw new ArgumentNullException(nameof(defaultDbSetting));
            }

            services.AddFreeSqlDatabaseProvider(defaultDbSetting);

            services.AddTransient<FreeSqlRepositoryResolver>();
            services.TryAddTransient(typeof(IFSqlRepository<,>), typeof(FSqlRepository<,>));
            services.TryAddTransient(typeof(IDbRepository<,>), typeof(FSqlRepository<,>));

            services.TryAddSingleton<IFSqlProviderStorage, DefaultFSqlProviderStorage>();

            return services;
        }


        /// <summary>
        /// 添加新的 freeSql 数据库配置
        /// </summary>
        /// <param name="services">服务注册集合</param>
        /// <param name="dbSetting">数据库配置信息</param>
        /// <returns></returns>
        public static IServiceCollection AddFreeSqlDatabaseProvider(this IServiceCollection services, IFSqlSetting dbSetting)
        {
            if (dbSetting == null)
            {
                throw new ArgumentNullException(nameof(dbSetting));
            }


            services.AddSingleton<IFSqlProvider>((serviceProvider) =>
            {
                return new FSqlProvider(dbSetting);
            });

            // 添加对应的关系型数据库配置
            services.AddRelationalDatabaseProcessor(
                typeof(IFSqlProvider),
                dbSetting.DatabaseType,
                dbSetting.Name
            );

            return services;
        }

        public static IServiceProvider AddFreeSqlDatabaseProvider(this IServiceProvider serviceProvider, IFSqlSetting dbSetting)
        {
            if (dbSetting == null)
            {
                throw new ArgumentNullException(nameof(dbSetting));
            }

            var fSqlProviderStorage = serviceProvider.GetRequiredService<IFSqlProviderStorage>();

            fSqlProviderStorage.AddOrUpdate(dbSetting.Name, new FSqlProvider(dbSetting));



            serviceProvider.AddRelationalDatabaseProcessor(
                    typeof(IFSqlProvider),
                    dbSetting.DatabaseType,
                    dbSetting.Name
                );

            return serviceProvider;
        }



        /// <summary>
        /// 添加 freesql 默认仓储类型映射
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IDbRepositoryDict UseDefaultFreeSqlRepositoryMap(this IServiceProvider serviceProvider)
        {
            var dbRepositoryDict = serviceProvider.GetService<IDbRepositoryDict>();

            dbRepositoryDict.Add(FreeSqlDbStorageConsts.DefaultFSqlRepositoryName, typeof(IFSqlRepository<,>));

            return dbRepositoryDict;
        }

        /// <summary>
        /// 添加 freesql 默认仓储类型映射
        /// </summary>
        /// <param name="dbRepositoryDict"></param>
        /// <returns></returns>
        public static IDbRepositoryDict UseDefaultFreeSqlRepositoryMap(this IDbRepositoryDict dbRepositoryDict)
        {
            dbRepositoryDict.Add(FreeSqlDbStorageConsts.DefaultFSqlRepositoryName, typeof(IFSqlRepository<,>));

            return dbRepositoryDict;
        }

    }
}
