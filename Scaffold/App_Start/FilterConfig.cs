using System;
using System.Web;
using System.Web.Mvc;

namespace Scaffold
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // ✅ Handles errors at MVC controller/action level
            filters.Add(new HandleErrorAttribute());

            // ✅ Handles all other unhandled exceptions globally
            filters.Add(new GlobalExceptionFilter());
        }
    }

    // ✅ Global filter - automatically applies to EVERY controller and page
    public class GlobalExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                Exception exception = filterContext.Exception;

                // ✅ Log error privately
                string logPath = HttpContext.Current.Server.MapPath("~/Logs/errors.log");
                string logMessage = $"{DateTime.Now} | Error: {exception.Message} | Controller: {filterContext.RouteData.Values["controller"]} | Action: {filterContext.RouteData.Values["action"]} | StackTrace: {exception.StackTrace}\n\n";

                System.IO.Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~/Logs"));
                System.IO.File.AppendAllText(logPath, logMessage);

                // ✅ Mark as handled so it doesn't show on screen
                filterContext.ExceptionHandled = true;

                // ✅ Redirect to safe error page
                filterContext.Result = new RedirectResult("~/Error");
            }
        }
    }
}