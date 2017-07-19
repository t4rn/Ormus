using FluentNHibernate.Mapping;
using Ormus.Core.Domain;

namespace Ormus.Hibernate.Mappings
{
    public class UserRoleMap : ClassMap<UserRole>
    {
        public UserRoleMap()
        {
            Not.LazyLoad();

            Table("UsersRoles");

            Id(x => x.Id);
            Map(x => x.Code);
            Map(x => x.CreatedDate);
            Map(x => x.Description);
            Map(x => x.Ghost);
        }
    }
}
