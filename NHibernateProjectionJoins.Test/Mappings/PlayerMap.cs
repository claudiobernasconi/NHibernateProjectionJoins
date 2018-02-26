using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernateProjectionJoins.Entity;

namespace NHibernateProjectionJoins.Mappings
{
	public class PlayerMap : ClassMapping<PlayerEntity>
	{
		public PlayerMap()
		{
			Table("DalTest_Player");
			Lazy(false);
			SelectBeforeUpdate(true);
			IdMapping();
			VersionMapping();

			Property(prop => prop.Name, Name);
			Property(prop => prop.TeamId, TeamId);
		}

		private void IdMapping()
		{
			Id(p => p.Id,
				id =>
				{
					id.Access(Accessor.Property);
					id.Column("Player_Id");
					id.Generator(Generators.Assigned);
				});
		}

		private void Name(IPropertyMapper propertyMapper)
		{
			propertyMapper.Column("Name");
			propertyMapper.Type(NHibernateUtil.String);
			propertyMapper.Length(100);
			propertyMapper.NotNullable(false);
		}

		private void TeamId(IPropertyMapper propertyMapper)
		{
			propertyMapper.Column("Team_Id");
			propertyMapper.Type(NHibernateUtil.Int32);
			propertyMapper.NotNullable(false);
		}

		private void VersionMapping()
		{
			Version(p => p.ModifiedDate, v =>
			{
				v.Access(Accessor.NoSetter);
				v.Type(new NHibernate.Type.DbTimestampType());
				v.Generated(VersionGeneration.Never);
			});
		}
	}
}
