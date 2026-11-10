using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class OffTestListController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: OffTestList
        public ActionResult OffTestList()
        {
            return View();
        }
    }
}