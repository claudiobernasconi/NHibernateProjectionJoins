using System;

namespace NHibernateProjectionJoins.Entity
{
	public class TeamEntity
	{
		public virtual long Id { get; set; }
		public virtual string Shortname { get; set; }

		private DateTime _modifiedDate = default;

		/// <summary>
		/// Zeitstempel der letzten Änderung
		/// </summary>
		public virtual DateTime ModifiedDate
		{
			get { return _modifiedDate; }
		}
	}
}
