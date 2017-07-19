using System;
using Autofac;
using Ormus.AdoNet.Repositories;
using Ormus.Core.Repositories;
using Ormus.Dapper.Repositories;
using Ormus.EntityFramework.Repositories;
using Ormus.Hibernate.Repositories;

namespace Ormus.Web.AutofacModules
{
    public class AutofacModule : Module
    {
        private readonly string _connStr;

        private enum OrmType
        {
            Ado, Dapper, EF, Hibernate
        }

        public AutofacModule(string connStr)
        {
            _connStr = connStr;
        }

        protected override void Load(ContainerBuilder builder)
        {
            OrmType ormType = OrmType.EF;

            switch (ormType)
            {
                case OrmType.Ado:
                    AdoNetRepositories(builder);
                    break;
                case OrmType.Dapper:
                    DapperRepositories(builder);
                    break;
                case OrmType.EF:
                    EntityFrameworkRepositories(builder);
                    break;
                case OrmType.Hibernate:
                    HiberanteRepositories(builder);
                    break;
                default:
                    break;
            }

            base.Load(builder);
        }

        private void AdoNetRepositories(ContainerBuilder builder)
        {
            builder.Register(c => new AdoUserRepository(_connStr))
                .As<IUserRepository>().InstancePerRequest();


            builder.Register(c => new AdoUserRoleRepository(_connStr))
                .As<IUserRoleRepository>().InstancePerRequest();
        }

        private void DapperRepositories(ContainerBuilder builder)
        {
            builder.Register(c => new DapperUserRepository(_connStr))
                .As<IUserRepository>().InstancePerRequest();

            builder.Register(c => new DapperUserRoleRepository(_connStr))
                .As<IUserRoleRepository>().InstancePerRequest();
        }

        private void EntityFrameworkRepositories(ContainerBuilder builder)
        {
            builder.Register(x => new EFUserRepository(new OrmusContext(_connStr)))
                .As<IUserRepository>().InstancePerRequest();

            builder.Register(x => new EFUserRoleRepository(new OrmusContext(_connStr)))
                .As<IUserRoleRepository>().InstancePerRequest();
        }
        private void HiberanteRepositories(ContainerBuilder builder)
        {
            builder.Register(c => new HibernateUserRepository(_connStr))
                .As<IUserRepository>().InstancePerRequest();

            builder.Register(c => new HibernateUserRoleRepository(_connStr))
                .As<IUserRoleRepository>().InstancePerRequest();
        }
    }
}