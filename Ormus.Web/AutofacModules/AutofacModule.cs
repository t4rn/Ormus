using System;
using Autofac;
using Ormus.AdoNet.Repositories;
using Ormus.Core.Repositories;
using Ormus.Dapper.Repositories;

namespace Ormus.Web.AutofacModules
{
    public class AutofacModule : Module
    {
        private readonly string _connStr;

        public AutofacModule(string connStr)
        {
            _connStr = connStr;
        }

        protected override void Load(ContainerBuilder builder)
        {
            //AdoNetRepositories(builder);
            DapperRepositories(builder);

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
    }
}