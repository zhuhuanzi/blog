using DbStorage.Impl;
using DbStorage.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace DbStorage
{
    public static class DbStorageServiceCollectionExtensions
    {

        /// <summary>
        /// 添加仓储服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configRepositoryDict">仓储类型映射字典</param>
        /// <returns></returns>
        public static IServiceCollection AddDbStorage(
            this IServiceCollection services,
            Func<IDbRepositoryDict> configRepositoryDict = null
            )
        {
            services.TryAddSingleton((serviceProvider) =>
            {
                if (configRepositoryDict != null)
                {
                    return configRepositoryDict.Invoke();
                }

                return new DbRepositoryDict();
            });


            services.TryAddSingleton<IPartitionTableNameFactory, PartitionTableNameFactory>();
            services.TryAddSingleton<IDbRepositoryFactory, DbRepositoryFactory>();
            services.TryAddSingleton<IDbEntityIdGen, GuidDbEntityIdGen>();
            services.AddSingleton<GuidDbEntityIdGen>();

            services.TryAddSingleton<IRelationalDatabaseProcessorStorage, Impl.DefaultRelationalDatabaseProcessorStorage>();

            return services;
        }


        /// <summary>
        /// 添加关系型数据库处理器
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dbStorageProviderType"></param>
        /// <param name="databaseType"></param>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static IServiceCollection AddRelationalDatabaseProcessor(
            this IServiceCollection services,
            Type dbStorageProviderType,
            DatabaseType databaseType,
            string providerName
            )
        {
            if (dbStorageProviderType == null)
            {
                throw new ArgumentNullException(nameof(dbStorageProviderType));
            }
            if (string.IsNullOrWhiteSpace(providerName))
            {
                throw new ArgumentNullException(nameof(providerName));
            }


            services.AddSingleton<IRelationalDatabaseProcessor>((serviceProvider) =>
            {
                return new DefaultRelationalDatabaseProcessor(dbStorageProviderType, databaseType, providerName);
            });

            return services;
        }


        /// <summary>
        /// 添加关系型数据库处理器
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="dbStorageProviderType"></param>
        /// <param name="databaseType"></param>
        /// <param name="providerName"></param>
        /// <param name="createFunc"></param>
        /// <returns></returns>
        public static IServiceProvider AddRelationalDatabaseProcessor(this IServiceProvider serviceProvider, Type dbStorageProviderType,
            DatabaseType databaseType,
            string providerName, Func<Type, DatabaseType, string, IRelationalDatabaseProcessor> createFunc = null)
        {
            if (dbStorageProviderType == null)
            {
                throw new ArgumentNullException(nameof(dbStorageProviderType));
            }
            if (string.IsNullOrWhiteSpace(providerName))
            {
                throw new ArgumentNullException(nameof(providerName));
            }

            var storage = serviceProvider.GetRequiredService<IRelationalDatabaseProcessorStorage>();

            if (createFunc == null)
            {
                var newProvider = new DefaultRelationalDatabaseProcessor(dbStorageProviderType, databaseType, providerName);
                storage.AddOrUpdate(providerName, newProvider);
            }
            else
            {
                storage.AddOrUpdate(providerName, createFunc.Invoke(dbStorageProviderType, databaseType, providerName));
            }


            return serviceProvider;
        }
    }
}