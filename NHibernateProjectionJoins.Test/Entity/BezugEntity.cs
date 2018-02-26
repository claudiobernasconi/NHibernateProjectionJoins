using System;

namespace NHibernateProjectionJoins
{
	public class BezugEntity
	{
		public long Id { get; set; }
		public string Status { get; set; }
		public long BezugseinheitId { get; set; }

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
