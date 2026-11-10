using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Scaffold.App_Code;

namespace Scaffold.Controllers
{
    public class SecurityCodeController : Controller
    {
        // GET: SecurityCode
        string query;
        string IPAddress = "";
        string strReturnVal = "";
        public ActionResult SecurityCode(string UID,string UserRole,string id,string SecurityCode)
        {
            ViewBag.UID = UID;
            ViewBag.UserRole = UserRole;
            ViewBag.id = id;
            ViewBag.SecurityCode = databaseCon.Decrypt(SecurityCode);

          return View();
        }
        public string GetIPAddress()
        {
            System.Web.HttpRequest currentRequest = System.Web.HttpContext.Current.Request;
            //  HttpRequest currentRequest = HttpContext.Current.Request;
            IPAddress = currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];

            IPAddress = currentRequest.ServerVariables["REMOTE_ADDR"];

            // string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            // IPAddress = Dns.GetHostByName(hostName).AddressList[0].ToString();  // Get Ip From Current System IP

            if (IPAddress == "::1")  // For Local Condation Check
            {
                IPAddress = "115.110.119.138";
            }

            Session["IpAddress"] = IPAddress;
            return IPAddress;

        }

        // GetCountryByIP  

        public string GetCountryByIP(string IPAddress)
        {
            string ipResponse = IPRequestHelper("http://ip-api.com/xml/" + IPAddress);
            //return ipResponse;
            XmlDocument ipInfoXML = new XmlDocument();
            ipInfoXML.LoadXml(ipResponse);
            XmlNodeList responseXML = ipInfoXML.GetElementsByTagName("query");

            NameValueCollection dataXML = new NameValueCollection();

            dataXML.Add(responseXML.Item(0).ChildNodes[2].InnerText, responseXML.Item(0).ChildNodes[2].Value);

            strReturnVal = responseXML.Item(0).ChildNodes[1].InnerText.ToString(); // Contry
            strReturnVal += "(" + responseXML.Item(0).ChildNodes[2].InnerText.ToString() + ")";  // Contry Code 

            return strReturnVal;
        }

        public string IPRequestHelper(string url)
        {

            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();

            StreamReader responseStream = new StreamReader(objResponse.GetResponseStream());
            string responseRead = responseStream.ReadToEnd();

            responseStream.Close();
            responseStream.Dispose();

            return responseRead;
        }

        public ActionResult LogOutClick()
        {
            if (Session["Uid"] != null && Session["UserRole"] != null)
            {
                Session["Uid"] = null;
                Session["UserRole"] = null;
                Session["ClientID"] = null;
                Session["EmailClientMsg"] = null;
            }
            return Json(new { Result = "SessionOut", Message = "Session Out" }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult btnSubmit_Click(string SecurityCode,string txtSecurytiCode,string hdnTimeOffSet)
        {
            GetIPAddress();
            GetCountryByIP(IPAddress);

            string ipResponse = IPRequestHelper("http://ip-api.com/xml/" + IPAddress);
            //return ipResponse;
            XmlDocument ipInfoXML = new XmlDocument();
            ipInfoXML.LoadXml(ipResponse);
            XmlNodeList responseXML = ipInfoXML.GetElementsByTagName("query");

            NameValueCollection dataXML = new NameValueCollection();

            dataXML.Add(responseXML.Item(0).ChildNodes[2].InnerText, responseXML.Item(0).ChildNodes[2].Value);

            strReturnVal = responseXML.Item(0).ChildNodes[1].InnerText.ToString(); // Contry
            strReturnVal += "(" + responseXML.Item(0).ChildNodes[2].InnerText.ToString() + ")";  // Contry Code 
            var region = responseXML.Item(0).ChildNodes[3].InnerText.ToString();
            var regionName = responseXML.Item(0).ChildNodes[4].InnerText.ToString();
            var city = responseXML.Item(0).ChildNodes[5].InnerText.ToString();
            var zip = responseXML.Item(0).ChildNodes[6].InnerText.ToString();
            var latitute = responseXML.Item(0).ChildNodes[7].InnerText.ToString();
            var longitude = responseXML.Item(0).ChildNodes[8].InnerText.ToString();
            var timezone = responseXML.Item(0).ChildNodes[9].InnerText.ToString();

            var LoginCountry = strReturnVal;
           var Result = "";
           var Message = "";
          
            DateTime LoginDateTime = DateTime.Now;   // Utc DateTime Save 
            bool LoginAttempt = false;

            string query = "select EmailID,Password,ClientID,Role,UserID,isnull(Accept,0) as Accept,Status,Rolenumber  from tbluser join tbluserrole on tblUser.Role = tbluserrole.Rolename  where EmailID='" + Session["UserEmail"] + "' and Role != 'Resource' ";
            DataTable dt = databaseCon.GetData(query);
            if (dt.Rows.Count > 0)
            {
                var userID = Convert.ToInt32(dt.Rows[0]["UserID"]);
                var EmailID = (dt.Rows[0]["EmailID"]).ToString();
                var Password = (dt.Rows[0]["Password"]).ToString();
                var clientID = Convert.ToInt32(dt.Rows[0]["ClientID"]);
                if (txtSecurytiCode == SecurityCode)
                {
                    Session["ChckSecurityCode"] = null; 
                    if (Convert.ToBoolean(dt.Rows[0]["Accept"]) == true)
                    {
                        LoginAttempt = true;
                        query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + userID + "," + clientID + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + LoginAttempt + "','" + EmailID + "','" + Password + "','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";

                        int result = databaseCon.RunCmd(query);

                        if (Convert.ToString(dt.Rows[0]["RoleNumber"]).ToUpper() == "55")
                        {
                            Result = "Success";
                            Message = "RedirectToInventoryManagement";
                        }
                        else{

                            Result = "Success";
                            Message = "RedirectToRegisterList";
                        }

                        //  return RedirectToAction("RegisterList", "RegisterList", new { UID = UID, UserRole = UserRole, id =id });

                    }
                    else
                    {
                        // Code Add  On 22 July 2019 For new Requirment For insert TblAuditLogin
                        LoginAttempt = true;
                        query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + userID + "," + clientID + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + LoginAttempt + "','" + EmailID + "','" + Password + "','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";
                        int result = databaseCon.RunCmd(query);
                        Result = "Success";
                        Message = "RedirectToTermsNcondition";
                      //  return RedirectToAction("TermsNcondition", "TermsNcondition", new { UID = UID, UserRole = UserRole, id = id,From ="" });
                    }
                }

                else
                {
                    // Code Add  On 22 July 2019 For new Requirment For insert TblAuditLogin
                    query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginFailureReason,LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + userID + "," + clientID + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + LoginAttempt + "','" + EmailID + "','" + Password + "','Your Security Code Invalid.','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";
                    int result = databaseCon.RunCmd(query);

                    //art1.Attributes.CssStyle.Add("display", "block");
                    //art1.Attributes["class"] = " w3-section w3-card-12 fade in w3-pale-red w3-padding";
                    //alertlabel.Text = "<strong>Alert !</strong> Your Security Code Invalid.";

                    Result = "Failed";
                    Message = "Your Security Code Invalid.";
                }
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }
    }
}