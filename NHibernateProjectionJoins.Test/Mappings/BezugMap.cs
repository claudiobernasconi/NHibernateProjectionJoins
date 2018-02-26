using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernateProjectionJoins.Entity;

namespace NHibernateProjectionJoins.Mappings
{
	public class BezugseinheitMap : ClassMapping<BezugseinheitEntity>
	{
		public BezugseinheitMap()
		{
			Table("DalTest_Bezug");
			Lazy(false);
			SelectBeforeUpdate(true);
			IdMapping();
			VersionMapping();

			Property(prop => prop.Steuerebene, Steuerebene);
		}

		private void IdMapping()
		{
			Id(p => p.Id,
				id =>
				{
					id.Access(Accessor.Field);
					id.Column("Bezug_Id");
					id.Generator(Generators.Assigned);
				});
		}

		private void Steuerebene(IPropertyMapper propertyMapper)
		{
			propertyMapper.Column("Steuerebene");
			propertyMapper.Type(NHibernateUtil.String);
			propertyMapper.Length(100);
			propertyMapper.NotNullable(false);
		}

		private void VersionMapping()
		{
			Version(p => p.ModifiedDate, v =>
			{
				v.Access(Accessor.NoSetter);
				v.Type(new global::NHibernate.Type.DbTimestampType());
				v.Generated(VersionGeneration.Never);
			});
		}
	}
}
