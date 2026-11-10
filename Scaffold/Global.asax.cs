using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Scaffold
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        // ✅ Catches ALL unhandled errors globally across every page
        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            // ✅ Log the error privately to a file
            string logPath = Server.MapPath("~/Logs/errors.log");
            string logMessage = $"{DateTime.Now} | Error: {exception.Message} | Source: {exception.Source} | StackTrace: {exception.StackTrace}\n\n";

            // Create Logs folder if it doesn't exist
            System.IO.Directory.CreateDirectory(Server.MapPath("~/Logs"));
            System.IO.File.AppendAllText(logPath, logMessage);

            // ✅ Clear the error so it doesn't show on screen
            Server.ClearError();

            // ✅ Redirect user to safe error page
            Response.Redirect("~/Error");
        }
    }
}