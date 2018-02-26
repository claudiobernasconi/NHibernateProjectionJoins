using System;
using System.Data;
using System.IO;
using NHibernate;
using NHibernate.Bytecode;
using NHibernate.Cfg;
using NHibernate.Cfg.ConfigurationSchema;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Context;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Event;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NHConfig = NHibernate.Cfg;

namespace NHibernateProjectionJoins
{
	public class InMemoryDatabaseCreator : IDisposable
	{
		private const string InMemoryConnectionString = "FullUri=file:inmemorydatabase?mode=memory&cache=shared";
		private const string QuerySubstitutions = "true=1;false=0";

		private static readonly Configuration _staticConfiguration;
		private static readonly ISession _contextSession;
		private static readonly ISessionFactory _contextSessionFactory;
		private static readonly SchemaExport _schema;

		static InMemoryDatabaseCreator()
		{
			_staticConfiguration = BuildConfiguration();

			_contextSessionFactory = _staticConfiguration.BuildSessionFactory();
			_contextSession = _contextSessionFactory.OpenSession();

			_schema = new SchemaExport(_staticConfiguration);
		}

		public InMemoryDatabaseCreator()
		{
			_schema.Execute(false, true, false, _contextSession.Connection, TextWriter.Null);
		}

		public Configuration Configuration
		{
			get { return _staticConfiguration; }
		}

		public ISessionFactory SessionFactory
		{
			get { return _contextSessionFactory; }
		}

		public void Dispose()
		{
			_schema.Drop(false, true);
		}

		private static NHConfig.Configuration BuildConfiguration()
		{
			var configuration = new NHConfig.Configuration();
			configuration
				.SetProperty(NHConfig.Environment.ConnectionDriver, typeof(SQLite20Driver).AssemblyQualifiedName)
				.SetProperty(NHConfig.Environment.Dialect, typeof(SQLiteDialect).AssemblyQualifiedName)
				.SetProperty(NHConfig.Environment.QuerySubstitutions, QuerySubstitutions)
				.SetProperty(NHConfig.Environment.CurrentSessionContextClass, typeof(ThreadStaticSessionContext).AssemblyQualifiedName)
				.SetProperty(NHConfig.Environment.ProxyFactoryFactoryClass, typeof(DefaultProxyFactoryFactory).AssemblyQualifiedName)
				.SetProperty(NHConfig.Environment.Isolation, IsolationLevel.ReadCommitted.ToString())
				.SetProperty(NHConfig.Environment.ConnectionString, InMemoryConnectionString);

			var mappings = BuildMappings();
			configuration.AddMapping(mappings);

			OptimizePerformance(configuration);

			return configuration;
		}

		private static void OptimizePerformance(NHConfig.Configuration configuration)
		{
			configuration
				.SetProperty(NHConfig.Environment.FormatSql, bool.FalseString)
				.SetProperty(NHConfig.Environment.GenerateStatistics, bool.FalseString)
				.SetProperty(NHConfig.Environment.Hbm2ddlKeyWords, Hbm2DDLKeyWords.None.ToString())
				.SetProperty(NHConfig.Environment.PrepareSql, bool.TrueString)
				.SetProperty(NHConfig.Environment.PropertyBytecodeProvider, BytecodeProviderType.Lcg.ToString())
				.SetProperty(NHConfig.Environment.PropertyUseReflectionOptimizer, bool.TrueString)
				.SetProperty(NHConfig.Environment.QueryStartupChecking, bool.FalseString)
				.SetProperty(NHConfig.Environment.ShowSql, bool.FalseString)
				.SetProperty(NHConfig.Environment.UseProxyValidator, bool.FalseString)
				.SetProperty(NHConfig.Environment.UseSecondLevelCache, bool.FalseString)
				.SetProperty(NHConfig.Environment.UseSqlComments, bool.FalseString)
				.SetProperty(NHConfig.Environment.UseQueryCache, bool.FalseString)
				.SetProperty(NHConfig.Environment.WrapResultSets, bool.TrueString);

			// remove support for pre & post load event listeners to improve performance
			configuration.EventListeners.PostLoadEventListeners = new IPostLoadEventListener[0];
			configuration.EventListeners.PreLoadEventListeners = new IPreLoadEventListener[0];
		}

		private static HbmMapping BuildMappings()
		{
			var mappingAssembly = typeof(InMemoryDatabaseCreator).Assembly;

			var mapper = new ModelMapper();
			mapper.AddMappings(mappingAssembly.GetTypes());

			return mapper.CompileMappingForAllExplicitlyAddedEntities();
		}
	}
}
