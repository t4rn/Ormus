using Autofac;
using AutoMapper;
using Ormus.Common;
using Ormus.Core.Domain;
using Ormus.Web.Models;
using System.Collections.Generic;

namespace Ormus.Web.AutofacModules
{
    public class AutoMapperModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(AutoMapperModule).Assembly).As<Profile>();

            builder.Register(context => new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserRole, UserRoleModel>().ReverseMap();

                cfg.CreateMap<User, UserModel>()
                .ReverseMap()
                .ForMember(x => x.Password, opt => opt.ResolveUsing(y => Md5.CreateMd5(y.Password)));

                foreach (var profile in context.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }

            })).AsSelf().SingleInstance();

            builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
                .As<IMapper>()
                .InstancePerLifetimeScope();
        }
    }
}