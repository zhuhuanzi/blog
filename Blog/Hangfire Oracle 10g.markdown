# Hangfire Oracle 10g 报错“名称已由现有对象使用”



###  结论

首先给出结论[Hangfire.Oracle.Core](https://github.com/akoylu/Hangfire.Oracle.Core)中的初始化数据库的脚本Install.sql中用了“LOB”（Oracle 11g支持），不支持Oracle 10g。

### 解决方案

提前用脚本创建Hangfire所需要的表结构，这是我修改好的初始化数据库的脚本。如下：

```sql
CREATE SEQUENCE HF_SEQUENCE START WITH 1 MAXVALUE 9999999999999999999999999999 MINVALUE 1 NOCYCLE CACHE 20 NOORDER;

CREATE SEQUENCE HF_JOB_ID_SEQ START WITH 1 MAXVALUE 9999999999999999999999999999 MINVALUE 1 NOCYCLE CACHE 20 NOORDER;

-- ----------------------------
-- Table structure for `Job`
-- ----------------------------

CREATE TABLE HF_JOB (ID                NUMBER (10)
                         ,STATE_ID          NUMBER (10)
                         ,STATE_NAME        NVARCHAR2 (20)
                         ,INVOCATION_DATA   NCLOB
                         ,ARGUMENTS         NCLOB
                         ,CREATED_AT        TIMESTAMP (4)
                         ,EXPIRE_AT         TIMESTAMP (4))
LOGGING
NOCOMPRESS
NOCACHE
NOPARALLEL
MONITORING;

ALTER TABLE HF_JOB ADD (
  PRIMARY KEY
  (ID)
  USING INDEX
  ENABLE VALIDATE);


-- ----------------------------
-- Table structure for `Counter`
-- ----------------------------

CREATE TABLE HF_COUNTER (ID          NUMBER (10)
                             ,KEY         NVARCHAR2 (255)
                             ,VALUE       NUMBER (10)
                             ,EXPIRE_AT   TIMESTAMP (4))
LOGGING
NOCOMPRESS
NOCACHE
NOPARALLEL
MONITORING;

ALTER TABLE HF_COUNTER ADD (
  PRIMARY KEY
  (ID)
  USING INDEX
  ENABLE VALIDATE);


CREATE TABLE HF_AGGREGATED_COUNTER (ID          NUMBER (10)
                                        ,KEY         NVARCHAR2 (255)
                                        ,VALUE       NUMBER (10)
                                        ,EXPIRE_AT   TIMESTAMP (4))
LOGGING
NOCOMPRESS
NOCACHE
NOPARALLEL
MONITORING;

ALTER TABLE HF_AGGREGATED_COUNTER ADD (
  PRIMARY KEY
  (ID)
  USING INDEX
  ENABLE VALIDATE,
  UNIQUE (KEY)
  USING INDEX
  ENABLE VALIDATE);


-- ----------------------------
-- Table structure for `DistributedLock`
-- ----------------------------

CREATE TABLE HF_DISTRIBUTED_LOCK ("RESOURCE" NVARCHAR2 (100), CREATED_AT TIMESTAMP (4))
LOGGING
NOCOMPRESS
NOCACHE
NOPARALLEL
MONITORING;


-- ----------------------------
-- Table structure for `Hash`
-- ----------------------------

CREATE TABLE HF_HASH (ID          NUMBER (10)
                          ,KEY         NVARCHAR2 (255)
                          ,VALUE       NCLOB
                          ,EXPIRE_AT   TIMESTAMP (4)
                          ,FIELD       NVARCHAR2 (40))
LOGGING
NOCOMPRESS
NOCACHE
NOPARALLEL
MONITORING;

ALTER TABLE HF_HASH ADD (
  PRIMARY KEY
  (ID)
  USING INDEX
  ENABLE VALIDATE,
  UNIQUE (KEY, FIELD)
  USING INDEX
  ENABLE VALIDATE);


-- ----------------------------
-- Table structure for `JobParameter`
-- ----------------------------

CREATE TABLE HF_JOB_PARAMETER (ID       NUMBER (10)
                                   ,NAME     NVARCHAR2 (40)
                                   ,VALUE    NCLOB
                                   ,JOB_ID   NUMBER (10))
LOGGING
NOCOMPRESS
NOCACHE
NOPARALLEL
MONITORING;

ALTER TABLE HF_JOB_PARAMETER ADD (
  PRIMARY KEY
  (ID)
  USING INDEX
  ENABLE VALIDATE);

ALTER TABLE HF_JOB_PARAMETER ADD (
  CONSTRAINT FK_JOB_PARAMETER_JOB
  FOREIGN KEY (JOB_ID)
  REFERENCES HF_JOB (ID)
  ON DELETE CASCADE ENABLE VALIDATE);


-- ----------------------------
-- Table structure for `JobQueue`
-- ----------------------------

CREATE TABLE HF_JOB_QUEUE (ID            NUMBER (10)
                               ,JOB_ID        NUMBER (10)
                               ,QUEUE         NVARCHAR2 (50)
                               ,FETCHED_AT    TIMESTAMP (4)
                               ,FETCH_TOKEN   NVARCHAR2 (36))
LOGGING
NOCOMPRESS
NOCACHE
NOPARALLEL
MONITORING;

ALTER TABLE HF_JOB_QUEUE ADD (
  PRIMARY KEY
  (ID)
  USING INDEX
  ENABLE VALIDATE);

ALTER TABLE HF_JOB_QUEUE ADD (
  CONSTRAINT FK_JOB_QUEUE_JOB
  FOREIGN KEY (JOB_ID)
  REFERENCES HF_JOB (ID)
  ON DELETE CASCADE ENABLE VALIDATE);


-- ----------------------------
-- Table structure for `JobState`
-- ----------------------------

CREATE TABLE HF_JOB_STATE (ID           NUMBER (10)
                               ,JOB_ID       NUMBER (10)
                               ,NAME         NVARCHAR2 (20)
                               ,REASON       NVARCHAR2 (100)
                               ,CREATED_AT   TIMESTAMP (4)
                               ,DATA         NCLOB)
LOGGING
NOCOMPRESS
NOCACHE
NOPARALLEL
MONITORING;

ALTER TABLE HF_JOB_STATE ADD (
  PRIMARY KEY
  (ID)
  USING INDEX
  ENABLE VALIDATE);

ALTER TABLE HF_JOB_STATE ADD (
  CONSTRAINT FK_JOB_STATE_JOB
  FOREIGN KEY (JOB_ID)
  REFERENCES HF_JOB (ID)
  ON DELETE CASCADE ENABLE VALIDATE);


-- ----------------------------
-- Table structure for `Server`
-- ----------------------------

CREATE TABLE HF_SERVER (ID NVARCHAR2 (100), DATA NCLOB, LAST_HEART_BEAT TIMESTAMP (4))
LOGGING
NOCOMPRESS
NOCACHE
NOPARALLEL
MONITORING;

ALTER TABLE HF_SERVER ADD (
  PRIMARY KEY
  (ID)
  USING INDEX
  ENABLE VALIDATE);


-- ----------------------------
-- Table structure for `Set`
-- ----------------------------

CREATE TABLE HF_SET (ID          NUMBER (10)
                         ,KEY         NVARCHAR2 (255)
                         ,VALUE       NVARCHAR2 (255)
                         ,SCORE       FLOAT (126)
                         ,EXPIRE_AT   TIMESTAMP (4))
LOGGING
NOCOMPRESS
NOCACHE
NOPARALLEL
MONITORING;

ALTER TABLE HF_SET ADD (
  PRIMARY KEY
  (ID)
  USING INDEX
  ENABLE VALIDATE,
  UNIQUE (KEY, VALUE)
  USING INDEX
  ENABLE VALIDATE);

CREATE TABLE HF_LIST (ID          NUMBER (10)
                          ,KEY         NVARCHAR2 (255)
                          ,VALUE       NCLOB
                          ,EXPIRE_AT   TIMESTAMP (4))
LOGGING
NOCOMPRESS
NOCACHE
NOPARALLEL
MONITORING;

ALTER TABLE HF_LIST ADD (
  PRIMARY KEY
  (ID)
  USING INDEX
  ENABLE VALIDATE);




```

### 排查过程

今天测试部署的新环境程序无法启动，并把程序启动日志发给了我，如下：

```C#
FATAL 2020-11-20 10:18:13,426 [1    ] Microsoft.AspNetCore.Hosting.WebHost     - Application startup exception
Oracle.ManagedDataAccess.Client.OracleException (0x80004005): ORA-00955: 名称已由现有对象使用
   at OracleInternal.ServiceObjects.OracleConnectionImpl.VerifyExecution(Int32& cursorId, Boolean bThrowArrayBindRelatedErrors, SqlStatementType sqlStatementType, Int32 arrayBindCount, OracleException& exceptionForArrayBindDML, Boolean& hasMoreRowsInDB, Boolean bFirstIterationDone)
   at OracleInternal.ServiceObjects.OracleCommandImpl.ExecuteNonQuery(String commandText, OracleParameterCollection paramColl, CommandType commandType, OracleConnectionImpl connectionImpl, Int32 longFetchSize, Int64 clientInitialLOBFS, OracleDependencyImpl orclDependencyImpl, Int64[]& scnFromExecution, OracleParameterCollection& bindByPositionParamColl, Boolean& bBindParamPresent, OracleException& exceptionForArrayBindDML, OracleConnection connection, OracleLogicalTransaction& oracleLogicalTransaction, Boolean isFromEF)
   at Oracle.ManagedDataAccess.Client.OracleCommand.ExecuteNonQuery()
   at Dapper.SqlMapper.ExecuteCommand(IDbConnection cnn, CommandDefinition& command, Action`2 paramReader) in C:\projects\dapper\Dapper\SqlMapper.cs:line 2797
   at Dapper.SqlMapper.ExecuteImpl(IDbConnection cnn, CommandDefinition& command) in C:\projects\dapper\Dapper\SqlMapper.cs:line 568
   at Dapper.SqlMapper.Execute(IDbConnection cnn, String sql, Object param, IDbTransaction transaction, Nullable`1 commandTimeout, Nullable`1 commandType) in C:\projects\dapper\Dapper\SqlMapper.cs:line 441
   at Hangfire.Oracle.Core.OracleObjectsInstaller.<>c__DisplayClass1_0.<Install>b__0(String s)
   at System.Collections.Generic.List`1.ForEach(Action`1 action)
   at Hangfire.Oracle.Core.OracleObjectsInstaller.Install(IDbConnection connection, String schemaName)
   at Hangfire.Oracle.Core.OracleStorage..ctor(String connectionString, OracleStorageOptions options)
   at MOONS.XST.Web.Startup.Startup.<ConfigureServices>b__4_8(IGlobalConfiguration config) in D:\5865\core3-tmp\aspnet-core\src\MOONS.XST.Web.Host\Startup\Startup.cs:line 200
   at Hangfire.HangfireServiceCollectionExtensions.<>c__DisplayClass0_0.<AddHangfire>b__0(IServiceProvider provider, IGlobalConfiguration config)
   at Hangfire.HangfireServiceCollectionExtensions.<>c__DisplayClass1_0.<AddHangfire>b__7(IServiceProvider serviceProvider)
   at Castle.Windsor.MsDependencyInjection.WindsorRegistrationHelper.<>c__DisplayClass5_0.<RegisterServiceDescriptor>b__0(IKernel c)
   at Castle.MicroKernel.Registration.ComponentRegistration`1.<>c__DisplayClass84_0`1.<UsingFactoryMethod>b__0(IKernel k, ComponentModel m, CreationContext c)
   at Castle.MicroKernel.ComponentActivator.FactoryMethodActivator`1.Instantiate(CreationContext context)
   at Castle.MicroKernel.ComponentActivator.DefaultComponentActivator.InternalCreate(CreationContext context)
   at Castle.MicroKernel.ComponentActivator.AbstractComponentActivator.Create(CreationContext context, Burden burden)
   at Castle.MicroKernel.Lifestyle.AbstractLifestyleManager.CreateInstance(CreationContext context, Boolean trackedExternally)
   at Castle.MicroKernel.Lifestyle.SingletonLifestyleManager.Resolve(CreationContext context, IReleasePolicy releasePolicy)
   at Castle.MicroKernel.Handlers.DefaultHandler.ResolveCore(CreationContext context, Boolean requiresDecommission, Boolean instanceRequired, Burden& burden)
   at Castle.MicroKernel.Handlers.DefaultHandler.Resolve(CreationContext context, Boolean instanceRequired)
   at Castle.MicroKernel.DefaultKernel.ResolveComponent(IHandler handler, Type service, Arguments additionalArguments, IReleasePolicy policy, Boolean ignoreParentContext)
   at Castle.MicroKernel.DefaultKernel.Castle.MicroKernel.IKernelInternal.Resolve(Type service, Arguments arguments, IReleasePolicy policy, Boolean ignoreParentContext)
   at Castle.MicroKernel.DefaultKernel.Resolve(Type service, Arguments arguments)
   at Castle.Windsor.WindsorContainer.Resolve(Type service)

````
看日志，得出关键字：“名称已由现有对象使用”,”Hangfire”、”Oracle“、“Dapper”。待会要根据这些关键字定位问题。

看了看数据库发现Hangfire相关的表没有生成，那就奇怪了，因为在我本地是可以创建表的，于是看测试服务器上配置的连接字符串，仔细核对发现也没什么问题。

然后我用Debug连接测试数据库进行代码调试，如下代码报错“名称已由现有对象使用”。又核对了一遍数据库连接字符串。

```C#
 var oracleStorage = new OracleStorage(connectionString,
                            new OracleStorageOptions
                            {
                                TransactionIsolationLevel = IsolationLevel.ReadCommitted,
                                QueuePollInterval = TimeSpan.FromSeconds(15),
                                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                                PrepareSchemaIfNecessary = true,
                                DashboardJobListLimit = 50000,
                                TransactionTimeout = TimeSpan.FromMinutes(1),
                                SchemaName = schemaName
                            });
                        config.UseStorage(oracleStorage);

```
恍然大悟，问了测试工程师，他装的环境是10g，而我们的开发环境是11g。

然后去百度、Google。搜索关键词：“Hanfire Oracle 名称已由现有对象使用”。发现也没有类似报错。

之后去GitHub找到了项目地址https://github.com/akoylu/Hangfire.Oracle.Core，就只能看看源码了，生成数据库的关键代码如下：判断了有没有“HF_”开头的表名，没有就去执行“Install.sql”。

我把“Install.sql”脚本单独用数据库可视化工具运行，果然报错“名称已由现有对象使用”。

分析“Install.sql”脚本，发现其中“LOB”不支持Oracle 10g，就只能修改这段脚本了，修改好的脚本在解决方案处。



```c#
public static class OracleObjectsInstaller
    {
        private static readonly ILog Log = LogProvider.GetLogger(typeof(OracleStorage));
        public static void Install(IDbConnection connection, string schemaName)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            if (TablesExists(connection, schemaName))
            {
                Log.Info("DB tables already exist. Exit install");
                return;
            }

            Log.Info("Start installing Hangfire SQL objects...");

            var script = GetStringResource("Hangfire.Oracle.Core.Install.sql");

            var sqlCommands = script.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            sqlCommands.ToList().ForEach(s => connection.Execute(s));

            Log.Info("Hangfire SQL objects installed.");
        }

        private static bool TablesExists(IDbConnection connection, string schemaName)
        {
            string tableExistsQuery;

            if (!string.IsNullOrEmpty(schemaName))
            {
                tableExistsQuery = $@"SELECT TABLE_NAME
					FROM all_tables
					WHERE OWNER = '{schemaName}' AND TABLE_NAME LIKE 'HF_%'
					ORDER BY TABLE_NAME";
            }
            else
            {
                tableExistsQuery = @"SELECT TABLE_NAME
					FROM all_tables
					WHERE TABLE_NAME LIKE 'HF_%'
					ORDER BY TABLE_NAME";
            }

            return connection.ExecuteScalar<string>(tableExistsQuery) != null;
        }

        private static string GetStringResource(string resourceName)
        {
			#if NET45
            var assembly = typeof(OracleObjectsInstaller).Assembly;
			#else
            var assembly = typeof(OracleObjectsInstaller).GetTypeInfo().Assembly;
			#endif

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Requested resource `{resourceName}` was not found 						in the assembly `{assembly}`.");
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }

```



### 总结

解决问题的思路及顺序：

1. 看报错日志
2. 确认环境（这一步我在解决此问题的时候漏掉了）
3. 分析代码（跑起来看一看）
4. 百度（解决不了去确认关键词百度）
5. 看源码（百度不了去看源码）
6. 提Issue（源码不会改提Issue）
7. 问大佬（Issue没人理大佬群里发个红包）
8. 换解决方案