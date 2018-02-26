using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernateProjectionJoins.Entity;
using NHibernateProjectionJoins.Model;
using NHibernateProjectionJoins.Util;

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
				var query = from team in session.Query<TeamEntity>()
						.Select(team => new TeamModel
						{
							TeamId = team.Id,
							Shortname = team.Shortname
						})
							join player in session.Query<PlayerEntity>()
								on team.TeamId equals player.TeamId
							select new PlayerModel
							{
								PlayerId = player.Id,
								TeamId = team.TeamId,
								Shortname = team.Shortname,
								Name = player.Name
							};

				query.ToList();
			}
		}

		[TestMethod]
		public void Query_ProjectionInJoinStatement_DoesNotCompile()
		{
			using (var session = OpenSession())
			{
				var query = from player in session.Query<PlayerEntity>()
							join team in session.Query<TeamEntity>()
					.Select(team => new TeamModel
					{
						TeamId = team.Id,
						Shortname = team.Shortname
					})
						on player.TeamId equals team.TeamId
							select new PlayerModel
							{
								PlayerId = player.Id,
								TeamId = team.TeamId,
								Shortname = team.Shortname,
								Name = player.Name
							};

				query.ToList();
			}
		}
	}
}