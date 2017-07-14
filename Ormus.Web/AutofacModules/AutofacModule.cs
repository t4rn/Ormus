using Autofac;
using Ormus.AdoNet.Repositories;
using Ormus.Core.Repositories;

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


            builder.Register(c => new AdoUserRoleRepository(_connStr))
                .As<IUserRoleRepository>().InstancePerRequest();

            #endregion

            //#region Services

            //builder.RegisterType<LogService>().As<IAppLogService>().InstancePerRequest();

            //#endregion

            base.Load(builder);
        }
    }
}