using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Scaffold.App_Code;
using Scaffold.Models;
using Scaffold_DataAccess;

namespace Scaffold.Controllers
{
    public class PreDeliveryInventoryController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: PreDeliveryInventory
        public ActionResult PreDeliveryInventory()
        {
            if (Session["Uid"] == null && Session["UserRole"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                var UID = Session["Uid"].ToString();
                var UserRole = Session["UserRole"].ToString();

                var id = Request.QueryString["id"];
                var checkid = Request.QueryString["checkid"];
                var FromId = Request.QueryString["FromId"];
                var ToId = Request.QueryString["ToId"];
                var fromtocheck = Request.QueryString["fromtocheck"];
                var from = Request.QueryString["from"];
                var To = Request.QueryString["To"];
                var DelStatus = Request.QueryString["DelStatus"];
                var sessionClientId = databaseCon.Decrypt(Session["ClientID"].ToString());
                var loginClientId = databaseCon.Decrypt(id.ToString());
                if (Session["ChckSecurityCode"] == null)  // For check security Code is generated or not, if yes, then we can't make user login into the system.
                {
                    if (sessionClientId == loginClientId)
                    {
                        Session["EmailClientMsg"] = null;
                        return RedirectToAction("DeliveryInventory", "DeliveryInventory", new { UID = databaseCon.Encrypt(UID), UserRole = databaseCon.Encrypt(UserRole), id = id, checkid = checkid, FromId = FromId, ToId = ToId, fromtocheck = fromtocheck, from = from, To = To, DelStatus = DelStatus });
                    }
                    else
                    {
                        string query1 = "select name,ClientId from tblclient where ClientId='" + databaseCon.Decrypt(id) + "'";
                        DataTable dt1 = databaseCon.GetData(query1);
                        var ClientName = dt1.Rows[0]["name"].ToString();
                        Session["EmailClientMsg"] = "The delivery is for client: " + ClientName + "";
                        Session["ClientID"] = null;
                        return RedirectToAction("Login", "Login");
                    }

                }
                else
                {
                    return RedirectToAction("Login", "Login");
                }
                
               
            }


        }

       

    }
}