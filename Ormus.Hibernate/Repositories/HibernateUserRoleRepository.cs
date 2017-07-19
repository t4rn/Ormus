using Ormus.Core.Domain;
using Ormus.Core.Repositories;
using System;
using System.Collections.Generic;

namespace Ormus.Hibernate.Repositories
{
    public class HibernateUserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public HibernateUserRoleRepository(string connectionString) : base(connectionString)
        {
        }

        public void Add(UserRole userRole)
        {
            SaveOrUpdate(userRole);
        }

        public int Delete(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(new UserRole() { Id = id });
                    transaction.Commit();
                }
            }

            return 1;
        }

        public UserRole Get(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.QueryOver<UserRole>()
                    .Where(x => x.Id == id)
                    .SingleOrDefault();
            }
        }

        public IEnumerable<UserRole> GetAll()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                return session.QueryOver<UserRole>().List();
            }
        }

        public int Update(UserRole userRole)
        {
            SaveOrUpdate(userRole);

            return 1;
        }
    }
}
