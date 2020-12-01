using DbStorage.Interface;
using System;
using System.Reflection;

namespace DbStorage.Impl
{
    public class DefaultRelationalDatabaseProcessor : IRelationalDatabaseProcessor
    {
        // ReSharper disable once InconsistentNaming
        public readonly string _dbStorageProviderName;
        // ReSharper disable once InconsistentNaming
        public readonly Type _dbStorageProviderType;
        // ReSharper disable once InconsistentNaming
        public readonly DatabaseType _dataBaseType;
        public string DbStorageProviderName => this._dbStorageProviderName;
        public Type DbStorageProviderType => this._dbStorageProviderType;


        public DefaultRelationalDatabaseProcessor(
            Type dbStorageProviderType,
            DatabaseType dataBaseType,
            string providerName)
        {
            _dbStorageProviderType = dbStorageProviderType;
            _dataBaseType = dataBaseType;
            _dbStorageProviderName = providerName;
        }

        public virtual string HandleConnectionString(string connectionString)
        {
            switch (_dataBaseType)
            {
                case DatabaseType.Oracle:
                    break;
            }

            return connectionString;
        }

        public virtual string HandleEntityName(Type entityType)
        {
            return HandleString(entityType.Name);
        }


        public string HandleEntityProperty(Type entityType, PropertyInfo propertyInfo)
        {
            return HandleString(propertyInfo.Name);
        }



        public string HandleString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            switch (_dataBaseType)
            {
                case DatabaseType.PostgreSQL:
                    return input.ToLower();
                case DatabaseType.Oracle:
                    return input.ToUpper();
            }

            return input;
        }
    }
}
