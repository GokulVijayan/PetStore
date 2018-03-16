using Autofac;
using Autofac.Integration.Mvc;
using PetStoreDAL.DBContext;
using PetStoreDAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using PetStoreBL.Services;
namespace Common
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            HtmlHelper.ClientValidationEnabled = true;
            HtmlHelper.UnobtrusiveJavaScriptEnabled = true;
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterSource(new ViewRegistrationSource());
            builder.RegisterType<PetRepository>().As<IPetRepository>();
            builder.RegisterType<PetService>().As<IPetService>();
            builder.RegisterType<DiscountRepository>().As<IDiscountRepository>();
            builder.RegisterType<DiscountService>().As<IDiscountService>();
            builder.RegisterType<PetDbContext>();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

        }
    }
}
