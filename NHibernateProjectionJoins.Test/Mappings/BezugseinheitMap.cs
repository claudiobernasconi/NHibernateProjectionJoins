using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernateProjectionJoins.Entity;

namespace NHibernateProjectionJoins.Mappings
{
	public class BezugMap : ClassMapping<BezugEntity>
	{
		public BezugMap()
		{
			Table("DalTest_Bezug");
			Lazy(false);
			SelectBeforeUpdate(true);
			IdMapping();
			VersionMapping();

			Property(prop => prop.Status, Status);
			Property(prop => prop.BezugseinheitId, BezugseinheitId);
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

		private void Status(IPropertyMapper propertyMapper)
		{
			propertyMapper.Column("Status");
			propertyMapper.Type(NHibernateUtil.String);
			propertyMapper.Length(100);
			propertyMapper.NotNullable(false);
		}

		private void BezugseinheitId(IPropertyMapper propertyMapper)
		{
			propertyMapper.Column("Bezugseinheit_Id");
			propertyMapper.Type(NHibernateUtil.Int32);
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
