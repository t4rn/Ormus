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
            #region Repositories

            builder.Register(c => new AdoUserRepository(_connStr))
                .As<IUserRepository>().InstancePerRequest();


            // ADO.NET
            builder.Register(c => new AdoUserRoleRepository(_connStr))
                .As<IUserRoleRepository>().InstancePerRequest();

            // Dapper
            //builder.Register(c => new DapperUserRoleRepository(_connStr))
            //    .As<IUserRoleRepository>().InstancePerRequest();

            #endregion

            base.Load(builder);
        }
    }
}