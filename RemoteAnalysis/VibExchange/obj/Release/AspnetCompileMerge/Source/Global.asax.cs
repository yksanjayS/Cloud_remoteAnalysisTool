using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using VibExchange.App_Start;
using VibExchange.Filters;
using VibExchange.Models;
namespace VibExchange
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //GlobalFilters.Filters.Add(new SessionExpireFilterAttribute());
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            //DBClass context = new DBClass();
            //Session["IsActiveSession"] = false;
            //Session["UserName"] = User.Identity.Name;
            //Session["UserRole"] = context.getUserRole(User.Identity.Name);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            Session.Abandon();
            //Session["IsActiveSession"] = false;
            //Session["UserName"] = null;
            //Session["UserRole"] = null;
            
        }
    }
}