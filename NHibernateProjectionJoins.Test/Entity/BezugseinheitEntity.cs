using System;

namespace NHibernateProjectionJoins.Entity
{
	public class BezugseinheitEntity
	{
		public long Id { get; set; }
		public string Steuerebene { get; set; }

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
