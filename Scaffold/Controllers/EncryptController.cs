using Scaffold.App_Code;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class EncryptController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: Encrypt
        public ActionResult Encrypt(string UID, string UserRole, string id)
        {
            return View();
        }

        public ActionResult GetEncryptDecrypt(string Decrypt, string id)
        {
            // string query = "";
            // DataTable dt;
            //EncryptViewModel model = new EncryptViewModel();
            // query = "select userID,LoginID,Password from tbluser where Password IS NOT NULL and LoginID != '" + databaseCon.Decrypt(id) + "'";
            //dt = databaseCon.GetData(query);
            //if (Decrypt == "Decrypt")
            //{
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        string pass = databaseCon.Decrypt(dt.Rows[i]["Password"].ToString());
            //        dt.Rows[i]["Password"] = pass;
            //    }
            //}

            //var getData = dbContext.Database.SqlQuery<EncryptViewModel>(query).ToList();
            string query = "select userID, LoginID, Password from tbluser where Password IS NOT NULL and LoginID != '" + Session["UserEmail"].ToString() + "'";
            var getData = dbContext.Database.SqlQuery<EncryptViewModel>(query).ToList();
            if (getData != null) {
                if (Decrypt == "Decrypt")
                {
                    for (int i = 0; i < getData.Count; i++)
                    {
                        string pass = databaseCon.Decrypt(getData[i].Password.ToString());
                        getData[i].Password = pass;
                    }
                }
            }

            return Json(getData, JsonRequestBehavior.AllowGet);
        }

        public class EncryptViewModel 
        {
            public Nullable<int> userID { get; set; }
            public string LoginID { get; set; }
            public string Password { get; set; }
        }

    }
}