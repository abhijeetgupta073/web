using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Services.Description;
using DocumentFormat.OpenXml.Office2010.Excel;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class TestOfflineController : Controller
    {
        ScaffoldEntities dbcontext = new ScaffoldEntities();
        // GET: TestOffline
        public ActionResult TestOffline()
        {
            string query = "select distinct UserId from tblUser where IsInspector='true' and userid= '" + databaseCon.Decrypt(Request.QueryString["UID"]) + "' and clientid='" + databaseCon.Decrypt(Request.QueryString["id"]) + "'";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["UserId"].ToString() != "0")
                {
                    ViewBag.ddlddlInspectedByadd = dt.Rows[0]["UserId"].ToString();
                }
            }
            return View();
        }
    }
}