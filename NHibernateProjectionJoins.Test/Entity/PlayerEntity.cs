using System;

namespace NHibernateProjectionJoins.Entity
{
	public class PlayerEntity
	{
		public virtual long Id { get; set; }
		public virtual string Name { get; set; }
		public virtual long TeamId { get; set; }

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
