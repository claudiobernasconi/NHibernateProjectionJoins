using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;

namespace NHibernateProjectionJoins
{
	[DeploymentItem("System.Data.SQLite.dll")]
	[DeploymentItem("x86\\SQLite.Interop.dll", "x86")]
	[DeploymentItem("x64\\SQLite.Interop.dll", "x64")]
	public abstract class InMemoryDatabaseTest
	{
		public InMemoryDatabaseTest()
		{
			DatabaseCreator = new InMemoryDatabaseCreator();
			SessionFactory = DatabaseCreator.SessionFactory;
		}

		protected InMemoryDatabaseCreator DatabaseCreator { get; }

		public ISessionFactory SessionFactory { get; }

		public TEntity FindBy<TEntity>(long id)
		{
			return InTransaction(session => session.Get<TEntity>(id));
		}

		public TEntity LoadBy<TEntity>(long id)
		{
			return InTransaction(session => session.Load<TEntity>(id));
		}

		public IReadOnlyList<TEntity> LoadAll<TEntity>()
		{
			return InTransaction(session => session.Query<TEntity>().ToList());
		}

		public void Save<TEntity>(TEntity entity)
		{
			InTransaction(session => session.Save(entity));
		}

		public ISession OpenSession()
		{
			return SessionFactory.OpenSession();
		}

		public void InTransaction(Action<ISession> action)
		{
			Func<ISession, object> wrappedAction = session =>
			{
				action(session);
				return null;
			};

			InTransaction(wrappedAction);
		}

		public TResult InTransaction<TResult>(Func<TResult> action)
		{
			return InTransaction(session => action());
		}

		public TResult InTransaction<TResult>(Func<ISession, TResult> action)
		{
			using (var session = OpenSession())
			{
				TResult result;
				using (var transaction = session.BeginTransaction())
				{
					result = action(session);
					transaction.Commit();
				}

				session.Close();

				return result;
			}
		}

		public virtual void Dispose()
		{
			DatabaseCreator.Dispose();
		}
	}
}
