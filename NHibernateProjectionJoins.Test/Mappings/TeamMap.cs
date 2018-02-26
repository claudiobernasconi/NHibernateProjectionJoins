using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernateProjectionJoins.Entity;

namespace NHibernateProjectionJoins.Mappings
{
	public class TeamMap : ClassMapping<TeamEntity>
	{
		public TeamMap()
		{
			Table("DalTest_Team");
			Lazy(false);
			SelectBeforeUpdate(true);
			IdMapping();
			VersionMapping();

			Property(prop => prop.Shortname, Shortname);
		}

		private void IdMapping()
		{
			Id(p => p.Id,
				id =>
				{
					id.Access(Accessor.Property);
					id.Column("Team_Id");
					id.Generator(Generators.Assigned);
				});
		}

		private void Shortname(IPropertyMapper propertyMapper)
		{
			propertyMapper.Column("Shortname");
			propertyMapper.Type(NHibernateUtil.String);
			propertyMapper.Length(100);
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
