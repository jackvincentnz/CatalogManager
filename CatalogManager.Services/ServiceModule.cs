using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Autofac;
using CatalogManager.Data;

namespace CatalogManager.Services
{
    public class ServiceModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.Load("CatalogManager.Services"))
                      .Where(t => t.Name.EndsWith("Service"))
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope();

            builder.RegisterType(typeof(CatalogManagerContext)).As(typeof(IDbContext)).InstancePerLifetimeScope();

        }
    }
}