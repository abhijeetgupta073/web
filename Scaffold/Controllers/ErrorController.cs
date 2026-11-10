using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

 namespace Scaffold.Controllers
 {
        public class ErrorController : Controller
        {
            // General error - ~/Error
            public ActionResult Index()
            {
                Response.StatusCode = 500;
                return View();
            }

            // Server error - ~/Error/ServerError
            public ActionResult ServerError()
            {
                Response.StatusCode = 500;
                return View("Index");
            }

            // Not found - ~/Error/NotFound
            public ActionResult NotFound()
            {
                Response.StatusCode = 404;
                return View();
            }

            // Bad request - ~/Error/BadRequest
            public ActionResult BadRequest()
            {
                Response.StatusCode = 400;
                return View("Index");
            }

            // Unauthorized - ~/Error/Unauthorized
            public ActionResult Unauthorized()
            {
                Response.StatusCode = 401;
                return View("Index");
            }

            // Forbidden - ~/Error/Forbidden
            public ActionResult Forbidden()
            {
                Response.StatusCode = 403;
                return View("Index");
            }
        }
    
 }