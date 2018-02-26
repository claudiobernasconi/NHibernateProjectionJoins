namespace NHibernateProjectionJoins.Model
{
	public class PlayerModel
	{
		public long PlayerId { get; set; }
		public long TeamId { get; set; }
		public string Shortname { get; set; }
		public string Name { get; set; }
	}
}
