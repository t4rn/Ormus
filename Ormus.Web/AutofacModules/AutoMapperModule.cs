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
                    .ForMember(um => um.UserRole, opt => opt.ResolveUsing(u => u.Role.Code))
                    .ForMember(um => um.RoleId, opt => opt.ResolveUsing(u => u.Role.Id))
                    .ReverseMap()
                    .ForMember(u => u.Password, opt => opt.ResolveUsing(um => Md5.CreateMd5(um.Password)))
                    .ForMember(u => u.Role, opt => opt.UseValue<UserModel>(null))//opt.ResolveUsing(um => new UserRole() { Id = um.RoleId }))
                    ;

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