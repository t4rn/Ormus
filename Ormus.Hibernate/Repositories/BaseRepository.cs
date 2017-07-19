using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using Ormus.Hibernate.Mappings;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Ormus.Hibernate.Repositories
{
    public class BaseRepository<T>
    {
        protected readonly string _connectionString;
        protected readonly ISessionFactory _sessionFactory;

        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
            _sessionFactory = CreateSessionFactory(_connectionString);
        }

        protected ISessionFactory CreateSessionFactory(string cs)
        {
            Console.SetOut(new CustomDebugWriter());

            return Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012
                    .ConnectionString(cs)
                    .ShowSql())
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserMap>())
                .BuildSessionFactory();
        }

        protected void SaveOrUpdate(T obj)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(obj);
                    transaction.Commit();
                }
            }
        }
    }

    public class CustomDebugWriter : TextWriter
    {
        public override void WriteLine(string value)
        {
            Debug.WriteLine(value);
            base.WriteLine(value);
        }

        public override void Write(string value)
        {
            Debug.Write(value);
            base.Write(value);
        }
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
