using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NHibernateProjectionJoins
{
	[TestClass]
	public class QueryTest : InMemoryDatabaseTest
	{
		[TestMethod]
		public void Query_ProjectionInFromStatement_Compiles()
		{
			using (var session = OpenSession())
			{
				var query = from bezugseinheit in session.Query<BezugseinheitEntity>()
						.Select(bezugseinheit => new BezugseinheitModel
						{
							Id = bezugseinheit.Id,
							Steuerebene = bezugseinheit.Steuerebene
						})
							join bezug in session.Query<BezugEntity>()
								on bezugseinheit.Id equals bezug.BezugseinheitId
							select new BezugModel
							{
								BezugId = bezug.Id,
								BezugseinheitId = bezugseinheit.Id,
								Steuerebene = bezugseinheit.Steuerebene,
								Status = bezug.Status
							};

				query.ToList();
			}
		}

		[TestMethod]
		public void Query_ProjectionInJoinStatement_DoesNotCompile()
		{
			using (var session = OpenSession())
			{
				var query = from bezug in session.Query<BezugEntity>()
							join bezugseinheit in session.Query<BezugseinheitEntity>()
					.Select(bezugseinheit => new BezugseinheitModel
					{
						Id = bezugseinheit.Id,
						Steuerebene = bezugseinheit.Steuerebene
					})
						on bezug.BezugseinheitId equals bezugseinheit.Id
							select new BezugModel
							{
								BezugId = bezug.Id,
								BezugseinheitId = bezugseinheit.Id,
								Steuerebene = bezugseinheit.Steuerebene,
								Status = bezug.Status
							};

				query.ToList();
			}
		}
	}
}