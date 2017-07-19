using FluentNHibernate.Mapping;
using Ormus.Core.Domain;

namespace Ormus.Hibernate.Mappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Not.LazyLoad();

            Table("Users");

            Id(x => x.Id);
            Map(x => x.CreatedDate);
            Map(x => x.Email);
            Map(x => x.FirstName);
            Map(x => x.Ghost);
            Map(x => x.LastName);
            Map(x => x.Login);
            Map(x => x.Password);
            References(x => x.Role, "RoleId");
            Map(x => x.UpdatedDate);
        }
    }
}
