using Ormus.Core.Domain;
using Ormus.Core.Repositories;
using System;
using System.Collections.Generic;

namespace Ormus.Hibernate.Repositories
{
    public class HibernateUserRepository : BaseRepository<User>, IUserRepository
    {
        public HibernateUserRepository(string connectionString) : base(connectionString)
        {
        }

        public void Add(User user)
        {
            // for automapper conflict with EF
            if (user.Role == null && user.RoleId > 0)
            {
                user.Role = new UserRole() { Id = user.RoleId };
            }

            SaveOrUpdate(user);
        }

        public int Delete(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.Delete(new User() { Id = id });
                    transaction.Commit();
                }
            }

            return 1;
        }

        public User Get(int id)
        {
            User user;
            using (var session = _sessionFactory.OpenSession())
            {
                user = session.QueryOver<User>()
                    .Where(x => x.Id == id)
                    .SingleOrDefault();
            }

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            IList<User> users;
            using (var session = _sessionFactory.OpenSession())
            {
                users = session.QueryOver<User>().List();
            }

            return users;
        }

        public int Update(User user)
        {
            // for automapper conflict with EF
            if (user.Role == null && user.RoleId > 0)
            {
                user.Role = new UserRole() { Id = user.RoleId };
            }

            SaveOrUpdate(user);

            return 1;
        }
    }
}
