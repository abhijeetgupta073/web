﻿using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Scaffold.App_Code;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml;
using Azure.Core;
namespace Scaffold.Controllers
{
    public class LoginController : Controller
    {
        DataTable dt;
        string query = "";
        // string IPAddress = "68.188.100.46";  //US IP ADDRESS
        //string IPAddress = "5.101.108.255";  //Netherland IP ADDRESS
        //  string IPAddress= "106.223.41.48";   // India IP Address
        // string IPAddress = "115.110.119.138";    // India IP Address
        //  string IPAddress = "59.94.239.96";
        // string IPAddress = "14.203.176.136";  // Australia(AU)
        // string IPAddress = "119.92.244.146";   // Philippines(PH)
        string IPAddress = "";
        string strReturnVal = "";

        // GET: Login
        ScaffoldEntities dbContext = new ScaffoldEntities();
        public ActionResult Login()
        {
            try
            {
                if (Session["googlemapapi"] == null || string.IsNullOrEmpty(Session["googlemapapi"].ToString()))
                {
                    var keyVaultUrl = "https://scafpackeyvault.vault.azure.net/";
                    var secretName = "googlemapapikey";

                    Azure.Core.TokenCredential credential;

                    credential = new DefaultAzureCredential();

                    var client = new SecretClient(new Uri(keyVaultUrl), credential);

                    var secret = client.GetSecret(secretName);

                    var googlemapapi = secret.Value.Value;
                    // Session["googlemapapi"] = googlemapapi;

                    //For Local 
                    Session["googlemapapi"] = "AIzaSyArS08FWLDMcjHRfSW8f7YisfUHJ6pIh4U";
                }
            }
            catch (Exception ex)
            {
                //Session["googlemapapi"] = null;

                //For Local 
                Session["googlemapapi"] = "AIzaSyArS08FWLDMcjHRfSW8f7YisfUHJ6pIh4U";
            }
            if (Session["EmailClientMsg"] != null && Session["EmailClientMsg"].ToString() != "")
            {
                var EmailClientMsg = Session["EmailClientMsg"].ToString();
                ViewBag.EmailClientMsg = EmailClientMsg;
            }
            else
            {
                ViewBag.EmailClientMsg = "";
            }
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

            if (IPAddress == "::1")  // for local condation check
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

        private LoginLocationInfo GetLocationFromIP(string IPAddress)
        {
            try
            {
                string ipResponse = IPRequestHelper("http://ip-api.com/xml/" + IPAddress);

                XmlDocument ipInfoXML = new XmlDocument();
                ipInfoXML.LoadXml(ipResponse);

                XmlNodeList responseXML = ipInfoXML.GetElementsByTagName("query");

                if (responseXML == null || responseXML.Count == 0)
                {
                    return GetBackupLocation();
                }

                return new LoginLocationInfo
                {
                    Country = responseXML.Item(0).ChildNodes[1].InnerText + "(" + responseXML.Item(0).ChildNodes[2].InnerText + ")",
                    Region = responseXML.Item(0).ChildNodes[3].InnerText,
                    RegionName = responseXML.Item(0).ChildNodes[4].InnerText,
                    City = responseXML.Item(0).ChildNodes[5].InnerText,
                    Zip = responseXML.Item(0).ChildNodes[6].InnerText,
                    Latitude = responseXML.Item(0).ChildNodes[7].InnerText,
                    Longitude = responseXML.Item(0).ChildNodes[8].InnerText,
                    Timezone = responseXML.Item(0).ChildNodes[9].InnerText
                };
            }
            catch
            {
                return GetBackupLocation();
            }
        }

        private LoginLocationInfo GetBackupLocation()
        {
            return new LoginLocationInfo
            {
                Country = "Australia(AU)",
                Region = "",
                RegionName = "",
                City = "",
                Zip = "",
                Latitude = "",
                Longitude = "",
                Timezone = ""
            };
        }

        public class LoginLocationInfo
        {
            public string Country { get; set; }
            public string Region { get; set; }
            public string RegionName { get; set; }
            public string City { get; set; }
            public string Zip { get; set; }
            public string Latitude { get; set; }
            public string Longitude { get; set; }
            public string Timezone { get; set; }
        }

        [HttpPost]
        //  public ActionResult Login(string UserEmail, string Password)
        public ActionResult Login(string UserEmail, string Password, string hdnTimeOffSetVal, string chkRem)
        {

            var txtUserName = UserEmail;
            var txtPassword = Password;
            var hdnTimeOffSet = hdnTimeOffSetVal;
            var Result = "";
            var Message = "";


            GetIPAddress();

            LoginLocationInfo location = GetLocationFromIP(IPAddress);

            strReturnVal = location.Country;

            var region = location.Region;
            var regionName = location.RegionName;
            var city = location.City;
            var zip = location.Zip;
            var latitute = location.Latitude;
            var longitude = location.Longitude;
            var timezone = location.Timezone;

            var LoginCountry = strReturnVal;


            //  string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            //  string GetCurrentSystemIpAddress = Dns.GetHostByName(hostName).AddressList[0].ToString();  // Get Ip From Current System IP
            string GetCurrentSystemIpAddress = IPAddress;

            // Comment On 28 Jan 2020
            //DateTime LoginDateTime = databaseCon.ToTimeZoneTime(DateTime.Now);   // Utc Time

            DateTime LoginDateTime = DateTime.Now;   // Utc DateTime Save 

            bool LoginAttempt = false;
            // Comment On 08 Jan 2020 
            // if (strReturnVal == ("United States(US)")) //Check if Country Code US Not Allow To Login 

            //Check if Country Code Australia(AU) Or India(IN) or ("Philippines(PH)") Then  Allow To Login  OtherWise Not Allow To Login
            //if ((strReturnVal != ("Australia(AU)")) && (strReturnVal != ("India(IN)")) && (strReturnVal != ("Philippines(PH)")))

            //Added on 25-04-2023 Client also want people to have access from UK   
            //if ((strReturnVal != ("Australia(AU)")) && (strReturnVal != ("India(IN)")) && (strReturnVal != ("United Kingdom(GB)")) && (strReturnVal != ("Philippines(PH)")))
            //Added on 18-05-2023 Client want to remove access from UK Now 
            // if ((strReturnVal != ("Australia(AU)")) && (strReturnVal != ("India(IN)")) && (strReturnVal != ("Philippines(PH)")))
            //Added United States for new client BHP
            //Commented on (12-07-2023) Client want access from Bali.
            //if ((strReturnVal != ("Australia(AU)")) && (strReturnVal != ("India(IN)")) && (strReturnVal != ("Philippines(PH)")) && (strReturnVal != ("United States(US)")))
            //Added on (12-07-2023) Client want access from Bali.
            //Commented on (16-04-2024) client want access only for australia and india.
            //if ((strReturnVal != ("Australia(AU)")) && (strReturnVal != ("India(IN)")) && (strReturnVal != ("Philippines(PH)")) && (strReturnVal != ("United States(US)")) && (strReturnVal != ("Indonesia(ID)")))
            //Added on (16-04-2024) client want access only for australia and india.
            //if ((strReturnVal != ("Australia(AU)")) && (strReturnVal != ("India(IN)")))
            //Added on (17-02-2025) client want access  for Singapore(SG).
            //if ((strReturnVal != ("Australia(AU)")) && (strReturnVal != ("India(IN)")) && (strReturnVal != ("Singapore(SG)")))
            //Added on (14-08-2025) client want access  for New Zealand(NZ).

            if ((strReturnVal != ("Australia(AU)")) && (strReturnVal != ("India(IN)")) && (strReturnVal != ("Singapore(SG)") && strReturnVal != ("New Zealand(NZ)")))

            //Added on (16-01-2026) client want access from UK .
            //if ((strReturnVal != ("Australia(AU)")) && (strReturnVal != ("India(IN)")) && (strReturnVal != ("Singapore(SG)") && strReturnVal != ("New Zealand(NZ)")) && (strReturnVal != ("United Kingdom(GB)")))
            {
                // Code Add  On 22 July 2019 For insert TblAuditLogin New Table Create
                query = "select EmailID,ClientID,UserID from tbluser where EmailID='" + txtUserName.Trim().Replace("'", "''") + "'";
                dt = databaseCon.GetData(query);
                // SAFE CHECK: Check rows and check if the column exists in the current memory state
                if (dt.Rows.Count > 0 && dt.Columns.Contains("UserID"))
                {
                    query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginFailureReason,LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + Convert.ToInt32(dt.Rows[0]["UserID"]) + "," + Convert.ToInt32(dt.Rows[0]["ClientID"]) + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + LoginAttempt + "','" + txtUserName.Trim().Replace("'", "''") + "','" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "','You are not Authorised.','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";
                    int result = databaseCon.RunCmd(query);

                    Result = "Failed";
                    Message = "You are not Authorised.";
                }
                else
                {

                    query = "insert into TblAuditLogin (IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginFailureReason,LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values ('" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + LoginAttempt + "','" + txtUserName.Trim().Replace("'", "''") + "','" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "',' You are not Authorised. and UserName or password invalid.','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";

                    int result = databaseCon.RunCmd(query);

                    Result = "Failed";
                    Message = "You are not Authorised.";
                }
            }
            else
            {

                if (chkRem == "on")
                {
                    Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
                }
                else
                {
                    Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);

                }
                Response.Cookies["UserName"].Value = txtUserName.Trim();
                Response.Cookies["Password"].Value = txtPassword.Trim();

                //  query = "select EmailID,Password,ClientID,Role,UserID,isnull(Accept,0) as Accept,Status,Rolenumber  from tbluser join tbluserrole on tblUser.Role = tbluserrole.Rolename  where EmailID='" + txtUserName.Trim().Replace("'", "''") + "' and Password='" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "' and Role != 'Resource' ";
                query = "select EmailID,Password,ClientID,Role,UserID,isnull(Accept,0) as Accept,Status,Rolenumber,IPAddress,isnull(IPAuthentication,0) as IPAuthentication,isnull(EmailAuthentication,0) as EmailAuthentication  from tbluser join tbluserrole on tblUser.Role = tbluserrole.Rolename  where EmailID='" + txtUserName.Trim().Replace("'", "''") + "' and Password='" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "' and Role != 'Resource' ";

                dt = databaseCon.GetData(query);
                // SAFE CHECK: Ensure the table has rows and contains the required UserID column
                if (dt.Rows.Count > 0 && dt.Columns.Contains("UserID"))
                {
                    int sesnid = 0;
                    Session["UserEmail"] = txtUserName.Trim().Replace("'", "''");
                    string userID = dt.Rows[0]["UserID"].ToString();
                    int ClientID = Convert.ToInt32(dt.Rows[0]["ClientID"]);
                    Session["ChckSecurityCode"] = null;
                    Session["EmailClientMsg"] = null;
                    Session["Uid"] = userID;
                    Session["UserRole"] = dt.Rows[0]["Rolenumber"].ToString();
                    Session["ClientID"] = databaseCon.Encrypt(Convert.ToString(ClientID));
                    Session["LoginDateTime"] = LoginDateTime;

                    //string query2 = "Update tblUser set SessionId='" + System.Web.HttpContext.Current.Session.SessionID + "' where UserId=" + userID;
                    //24-05-204 we update the session id based on email id due to managerall userrole chnage 
                    string query2 = "Update tblUser set SessionId='" + System.Web.HttpContext.Current.Session.SessionID + "' where Emailid ='" + txtUserName + "'";
                    sesnid = databaseCon.RunCmdID(query2);
                    string IP = dt.Rows[0]["IPAddress"].ToString();
                    var IParray = IP.Split(',');
                    var ISIPValidation = false;
                    var ISEmailValidation = false;
                    string IPAuthentication = dt.Rows[0]["IPAuthentication"].ToString();
                    string EmailAuthentication = dt.Rows[0]["EmailAuthentication"].ToString();
                    string EmailID = dt.Rows[0]["EmailID"].ToString();
                    if (IPAuthentication == "True")
                    {
                        for (int i = 0; i < IParray.Length; i++)
                        {
                            var IpAddress = IParray[i];
                            if (IParray[i] == GetCurrentSystemIpAddress)
                            {
                                ISIPValidation = true;
                            }

                            // else if(IpAddress.Substring(IpAddress.Length - 2)== GetCurrentSystemIpAddress.Substring(GetCurrentSystemIpAddress.Length - 2))
                            else if (IpAddress.Remove(IpAddress.Length - 2) == GetCurrentSystemIpAddress.Remove(GetCurrentSystemIpAddress.Length - 2))
                            {
                                ISIPValidation = true;
                            }
                        }

                    }
                    else
                    {
                        ISIPValidation = true;
                        if (EmailAuthentication == "True")
                        {
                            ISEmailValidation = true;
                        }
                    }
                    if (ISEmailValidation == true)
                    {

                        if (sesnid != 0)
                        {
                            if (dt.Rows[0]["Status"].ToString() == "InActive")
                            {

                                query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginFailureReason,LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + userID + "," + ClientID + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + LoginAttempt + "','" + txtUserName.Trim().Replace("'", "''") + "','" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "','Your Account is inactive please contact administrator.','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";

                                int result = databaseCon.RunCmd(query);

                                Result = "Failed";
                                Message = " Your Account is inactive please contact administrator.";
                            }
                            else if (Convert.ToBoolean(dt.Rows[0]["Accept"]) == true)
                            {

                                LoginAttempt = true;

                                query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + userID + "," + ClientID + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + LoginAttempt + "','" + txtUserName.Trim().Replace("'", "''") + "','" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";

                                int result = databaseCon.RunCmd(query);

                                if (Convert.ToString(dt.Rows[0]["RoleNumber"]).ToUpper() == "10")
                                {
                                    string query1 = "select distinct(Name+' '+'('+status+')') as name,ClientId from tblclient where ClientId!='1' Order by name";
                                    DataTable dt1 = databaseCon.GetData(query1);
                                    string id = databaseCon.Encrypt(dt1.Rows[0]["ClientId"].ToString());
                                    Session["ClientID"] = id;
                                    return RedirectToAction("DashboardHere", "DashboardHere", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = id });
                                }
                                else
                                {
                                    var SecurityCode = securitycodeNumber("Email", EmailID);
                                    Session["ChckSecurityCode"] = SecurityCode;
                                    return RedirectToAction("SecurityCode", "SecurityCode", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()), SecurityCode = databaseCon.Encrypt(SecurityCode) });

                                }
                            }
                            else
                            {

                                LoginAttempt = true;
                                string From = "null";

                                query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + userID + "," + ClientID + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + txtUserName.Trim().Replace("'", "''") + "','" + LoginAttempt + "','" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";

                                int result = databaseCon.RunCmd(query);

                                if (Convert.ToString(dt.Rows[0]["RoleNumber"]).ToUpper() == "10")
                                {
                                    string query1 = "select distinct(Name+' '+'('+status+')') as name,ClientId from tblclient where ClientId!='1' Order by name";
                                    DataTable dt1 = databaseCon.GetData(query1);
                                    string id = databaseCon.Encrypt(dt1.Rows[0]["ClientId"].ToString());
                                    Session["ClientID"] = id;
                                    return RedirectToAction("TermsNcondition", "TermsNcondition", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = id, From = From });
                                }
                                else
                                {

                                    var SecurityCode = securitycodeNumber("Email", EmailID);
                                    Session["ChckSecurityCode"] = SecurityCode;
                                    return RedirectToAction("SecurityCode", "SecurityCode", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()), SecurityCode = databaseCon.Encrypt(SecurityCode) });

                                }
                            }

                        }

                    }
                    else if (ISIPValidation == true)
                    {
                        if (sesnid != 0)
                        {
                            if (dt.Rows[0]["Status"].ToString() == "InActive")
                            {
                                // Code Add  On 22 July 2019 For new Requirment For insert TblAuditLogin

                                query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginFailureReason,LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + userID + "," + ClientID + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + LoginAttempt + "','" + txtUserName.Trim().Replace("'", "''") + "','" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "','Your Account is inactive please contact administrator.','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";

                                int result = databaseCon.RunCmd(query);

                                Result = "Failed";
                                Message = " Your Account is inactive please contact administrator.";
                            }
                            else if (Convert.ToBoolean(dt.Rows[0]["Accept"]) == true)
                            {
                                // Code Add  On 22 July 2019 For insert TblAuditLogin New Table Create
                                LoginAttempt = true;

                                query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + userID + "," + ClientID + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + LoginAttempt + "','" + txtUserName.Trim().Replace("'", "''") + "','" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";

                                int result = databaseCon.RunCmd(query);
                                //Added on 11-Dec-2021 Start
                                DataTable dtMainDepot = databaseCon.GetData("select isnull(HideMainDepotInventory,0) as HideMainDepotInventory from tblClient where clientid='" + ClientID + "'");
                                HideMainDepotInventoryViewModel GetHideMainDepotInventory = new HideMainDepotInventoryViewModel();
                                if ((dtMainDepot.Rows[0]["HideMainDepotInventory"]).ToString() != "True")
                                {
                                    GetHideMainDepotInventory.HideMainDepotInventory = false;
                                }
                                else
                                {
                                    GetHideMainDepotInventory.HideMainDepotInventory = true;
                                }
                                //Added on 11-Dec-2021 End

                                //Added on 22-05-2023 To Use Common link to redirect pages Start
                                string EncryptClientID = "";
                                var Rolenumber = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString());
                                var TempURL = "";
                                if (Convert.ToString(dt.Rows[0]["RoleNumber"]).ToUpper() == "10")
                                {
                                    string query1 = "select distinct(Name+' '+'('+status+')') as name,ClientId from tblclient where ClientId!='1' Order by name";
                                    DataTable dt1 = databaseCon.GetData(query1);
                                    string id = databaseCon.Encrypt(dt1.Rows[0]["ClientId"].ToString());
                                    Session["ClientID"] = id;
                                    EncryptClientID = databaseCon.Encrypt(dt1.Rows[0]["ClientId"].ToString());
                                    id = databaseCon.Decrypt(id);
                                    int clientid = Convert.ToInt32(id);

                                    TempURL = databaseCon.RedirectingCommonLink(clientid, userID, Rolenumber, GetHideMainDepotInventory.HideMainDepotInventory);

                                }
                                else
                                {
                                    EncryptClientID = databaseCon.Encrypt(Convert.ToInt32(ClientID).ToString());
                                    TempURL = databaseCon.RedirectingCommonLink(ClientID, userID, Rolenumber, GetHideMainDepotInventory.HideMainDepotInventory);

                                }

                                Response.Redirect("/" + TempURL + "?UID=" + databaseCon.Encrypt(userID) + "&UserRole=" + Rolenumber + "&id=" + EncryptClientID);
                                //Added on 22-05-2023 To Use Common link to redirect pages end here

                                //Commented on 20-05-2023 To Use Common link to redirect pages Start here. We have removed Cid condition in our new code as we dicussed we dont have any client id (36 or 44)
                                //if (Convert.ToString(dt.Rows[0]["RoleNumber"]).ToUpper() == "10")
                                //{
                                //    string query1 = "select distinct(Name+' '+'('+status+')') as name,ClientId from tblclient where ClientId!='1' Order by name";
                                //    DataTable dt1 = databaseCon.GetData(query1);
                                //    string id = databaseCon.Encrypt(dt1.Rows[0]["ClientId"].ToString());
                                //    Session["ClientID"] = id;
                                //    string Cid = databaseCon.Decrypt(id);
                                //    //return RedirectToAction("DashboardHere", "DashboardHere", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = id });
                                //    if (Cid == "44" || Cid == "36")
                                //        {
                                //        return RedirectToAction("RegisterList", "RegisterList", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = id });
                                //        }
                                //    else {
                                //        return RedirectToAction("DashboardHere", "DashboardHere", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = id });
                                //        }


                                //    // Response.Redirect("/DashboardHere.aspx?UID=" + databaseCon.Encrypt(userID) + "&UserRole=" + databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()));
                                //}
                                //else if ((GetHideMainDepotInventory.HideMainDepotInventory == true)&&(Convert.ToString(dt.Rows[0]["RoleNumber"]).ToUpper() == "55"))
                                //{
                                //    return RedirectToAction("MainDepotInventory", "MainDepotInventory", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()) });
                                //}
                                //else if (Convert.ToString(dt.Rows[0]["RoleNumber"]).ToUpper() == "55")
                                //{
                                //    //return RedirectToAction("MainDepotInventory", "MainDepotInventory", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()) });
                                //    return RedirectToAction("SiteInventory", "SiteInventory", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()) });
                                //}
                                //else if ((Convert.ToString(dt.Rows[0]["RoleNumber"]).ToUpper() == "95"))
                                //{
                                //    return RedirectToAction("OffSystemInspection", "OffSystemInspection", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()) });
                                //}
                                //else
                                //{
                                //    //if (EmailAuthentication == "True")
                                //    //{

                                //    //    var SecurityCode = securitycodeNumber("Email", EmailID);
                                //    //    return RedirectToAction("SecurityCode", "SecurityCode", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()), SecurityCode = databaseCon.Encrypt(SecurityCode) });

                                //    //}
                                //    //else
                                //    //{
                                //        return RedirectToAction("RegisterList", "RegisterList", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()) });

                                //    //}
                                //}
                                //Commented on 20-05-2023 To Use Common link to redirect pages End here

                            }
                            else
                            {
                                // Code Add  On 22 July 2019 For insert TblAuditLogin New Table Create
                                LoginAttempt = true;
                                string From = "null";

                                query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + userID + "," + ClientID + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + txtUserName.Trim().Replace("'", "''") + "','" + LoginAttempt + "','" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";

                                int result = databaseCon.RunCmd(query);

                                if (Convert.ToString(dt.Rows[0]["RoleNumber"]).ToUpper() == "10")
                                {
                                    string query1 = "select distinct(Name+' '+'('+status+')') as name,ClientId from tblclient where ClientId!='1' Order by name";
                                    DataTable dt1 = databaseCon.GetData(query1);
                                    string id = databaseCon.Encrypt(dt1.Rows[0]["ClientId"].ToString());
                                    Session["ClientID"] = id;
                                    return RedirectToAction("TermsNcondition", "TermsNcondition", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = id, From = From });

                                    // Response.Redirect("/TermsNcondition.aspx?UID=" + databaseCon.Encrypt(userID) + "&UserRole=" + databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()) + "&From=");
                                }
                                else
                                {
                                    //if (EmailAuthentication == "True")
                                    //{

                                    //    var SecurityCode = securitycodeNumber("Email", EmailID);
                                    //    return RedirectToAction("SecurityCode", "SecurityCode", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()), SecurityCode = databaseCon.Encrypt(SecurityCode) });

                                    //}
                                    //else
                                    //{
                                    return RedirectToAction("TermsNcondition", "TermsNcondition", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()), From = From });
                                    // Response.Redirect("~/TermsNcondition.aspx?UID=" + databaseCon.Encrypt(userID) + "&UserRole=" + databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()) + "&id=" + databaseCon.Encrypt(ClientID.ToString()) + "&From=");
                                    // }
                                }
                            }
                        }

                    }

                    else
                    {

                        query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginFailureReason,LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + Convert.ToInt32(dt.Rows[0]["UserID"]) + "," + Convert.ToInt32(dt.Rows[0]["ClientID"]) + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + LoginAttempt + "','" + txtUserName.Trim().Replace("'", "''") + "','" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "','Your device is not registered. Please contact Scafpac Administrator at: admin@scafpac.com.au for your Registerd IP. Current System IP : " + GetCurrentSystemIpAddress + "','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";
                        int result = databaseCon.RunCmd(query);


                        Result = "Failed";
                        //Message = "Your device is not registered. Please contact Scafpac Administrator at: admin@scafpac.com.au";
                        Message = "Your device is not registered. Please contact Scafpac Administrator at: admin@scafpac.com.au for your Registerd IP.";

                        if (ISIPValidation == false && EmailAuthentication == "False")
                        {
                            Session["Uid"] = null;
                            Session["UserRole"] = null;
                            Session["ClientID"] = null;
                        }

                        if (EmailAuthentication == "True")
                        {
                            if (sesnid != 0)
                            {
                                if (dt.Rows[0]["Status"].ToString() == "InActive")
                                {
                                    // Code Add  On 22 July 2019 For new Requirment For insert TblAuditLogin

                                    query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginFailureReason,LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + userID + "," + ClientID + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + LoginAttempt + "','" + txtUserName.Trim().Replace("'", "''") + "','" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "','Your Account is inactive please contact administrator.','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";

                                    result = databaseCon.RunCmd(query);

                                    Result = "Failed";
                                    Message = " Your Account is inactive please contact administrator.";
                                }
                                else if (Convert.ToBoolean(dt.Rows[0]["Accept"]) == true)
                                {
                                    // Code Add  On 22 July 2019 For insert TblAuditLogin New Table Create
                                    LoginAttempt = true;

                                    query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + userID + "," + ClientID + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + LoginAttempt + "','" + txtUserName.Trim().Replace("'", "''") + "','" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";

                                    result = databaseCon.RunCmd(query);

                                    if (Convert.ToString(dt.Rows[0]["RoleNumber"]).ToUpper() == "10")
                                    {
                                        string query1 = "select distinct(Name+' '+'('+status+')') as name,ClientId from tblclient where ClientId!='1' Order by name";
                                        DataTable dt1 = databaseCon.GetData(query1);
                                        string id = databaseCon.Encrypt(dt1.Rows[0]["ClientId"].ToString());
                                        Session["ClientID"] = id;
                                        string Cid = databaseCon.Decrypt(id);
                                        // return RedirectToAction("DashboardHere", "DashboardHere", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = id });
                                        if (Cid == "44" || Cid == "36")
                                        {
                                            return RedirectToAction("RegisterList", "RegisterList", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = id });
                                        }
                                        else
                                        {
                                            return RedirectToAction("DashboardHere", "DashboardHere", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = id });
                                        }
                                    }
                                    else
                                    {
                                        //if (EmailAuthentication == "True")
                                        //{

                                        var SecurityCode = securitycodeNumber("Email", EmailID);
                                        Session["ChckSecurityCode"] = SecurityCode;
                                        return RedirectToAction("SecurityCode", "SecurityCode", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()), SecurityCode = databaseCon.Encrypt(SecurityCode) });

                                        //}
                                        //else
                                        //{
                                        //    return RedirectToAction("RegisterList", "RegisterList", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()) });

                                        //}
                                    }
                                }
                                else
                                {
                                    // Code Add  On 22 July 2019 For insert TblAuditLogin New Table Create
                                    LoginAttempt = true;
                                    string From = "null";

                                    query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + userID + "," + ClientID + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + txtUserName.Trim().Replace("'", "''") + "','" + LoginAttempt + "','" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";

                                    result = databaseCon.RunCmd(query);

                                    if (Convert.ToString(dt.Rows[0]["RoleNumber"]).ToUpper() == "10")
                                    {
                                        string query1 = "select distinct(Name+' '+'('+status+')') as name,ClientId from tblclient where ClientId!='1' Order by name";
                                        DataTable dt1 = databaseCon.GetData(query1);
                                        string id = databaseCon.Encrypt(dt1.Rows[0]["ClientId"].ToString());
                                        Session["ClientID"] = id;
                                        return RedirectToAction("TermsNcondition", "TermsNcondition", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = id, From = From });
                                    }
                                    else
                                    {
                                        //if (EmailAuthentication == "True")
                                        //{

                                        var SecurityCode = securitycodeNumber("Email", EmailID);
                                        Session["ChckSecurityCode"] = SecurityCode;
                                        return RedirectToAction("SecurityCode", "SecurityCode", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()), SecurityCode = databaseCon.Encrypt(SecurityCode) });

                                        //}
                                        //else
                                        //{
                                        //    return RedirectToAction("TermsNcondition", "TermsNcondition", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()), From = From });
                                        //    // Response.Redirect("~/TermsNcondition.aspx?UID=" + databaseCon.Encrypt(userID) + "&UserRole=" + databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()) + "&id=" + databaseCon.Encrypt(ClientID.ToString()) + "&From=");
                                        //}
                                    }
                                }
                            }// end mine

                        }



                    }

                }

                else
                {

                    // Code Add  On 22 July 2019 For insert TblAuditLogin New Table Create
                    query = "select EmailID,ClientID,UserID from tbluser where EmailID='" + txtUserName.Trim().Replace("'", "''") + "'";
                    dt = databaseCon.GetData(query);
                    // SAFE CHECK: Ensure the login failure audit log also checks for the correct columns
                    if (dt.Rows.Count > 0 && dt.Columns.Contains("UserID"))
                    {

                        query = "insert into TblAuditLogin (UserId,ClientID,IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginFailureReason,LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values (" + Convert.ToInt32(dt.Rows[0]["UserID"]) + "," + Convert.ToInt32(dt.Rows[0]["ClientID"]) + ",'" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + LoginAttempt + "','" + txtUserName.Trim().Replace("'", "''") + "','" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "','UserName or password invalid.','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";
                        int result = databaseCon.RunCmd(query);

                        Result = "Failed";
                        Message = "UserName or password invalid.";

                    }
                    else
                    {
                        query = "insert into TblAuditLogin (IPAddress,Country,UTCLoginTime,SessionId,LoginAttemptSuccess,UserName,[Password],LoginFailureReason,LoginTimeZone,regionName,city,zip,latitute,longitude,timezone) values ('" + IPAddress + "','" + LoginCountry + "','" + LoginDateTime + "','" + System.Web.HttpContext.Current.Session.SessionID + "','" + LoginAttempt + "','" + txtUserName.Trim().Replace("'", "''") + "','" + databaseCon.Encrypt(txtPassword.Trim().Replace("'", "''")) + "','UserName or password invalid.','" + Convert.ToDouble((hdnTimeOffSet)) * (-1) + "','" + regionName + "','" + city + "','" + zip + "','" + latitute + "','" + longitude + "','" + timezone + "')";

                        int result = databaseCon.RunCmd(query);

                        Result = "Failed";
                        Message = "UserName or password invalid.";

                    }
                }
            }  //else Block Close

            // return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);

            //ModelState.AddModelError("Username", Message);

            //return View(ModelState);

            ViewBag.Result = Result;
            ViewBag.Message = Message;
            return View();
        }

        string securitycodeNumber(string authType, string EmailID)
        {
            Random generator = new Random();
            String SecurityCode = generator.Next(0, 999999).ToString("D6");
            string strText = "Your Security Code is:- " + SecurityCode;
            if (authType == "Email")
            {
                databaseCon.SendEmail(EmailID, "", "Security Code", strText, "");
            }
            return SecurityCode;
        }

        public ActionResult btnSubmit_Click(string textforgetEmail)
        {
            var Result = "";
            var Message = "";
            string strusername = textforgetEmail.ToString().Replace("'", "''").Trim();
            string query = "select EmailID from tbluser where tbluser.EmailID='" + strusername + "'";
            DataTable dtfLogin = databaseCon.GetData(query);
            if (dtfLogin.Rows.Count > 0)
            {
                if (dtfLogin.Rows[0]["EmailID"].ToString() != "")
                {
                    string From = "azure_0df29fac9bf524c9c35a58c87ad05d0b@azure.com";
                    string Password = "tBmetVs7N2DpnEa";
                    NetworkCredential Credential = new NetworkCredential(From, Password);
                    SmtpClient Client = new SmtpClient();
                    Client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    Client.EnableSsl = true;
                    Client.Host = "smtp.sendgrid.net";// "smtp.gmail.com";
                    Client.Port = 25;// 587;
                    Client.UseDefaultCredentials = false;
                    Client.Credentials = Credential;
                    MailMessage MailMsg = new MailMessage();
                    MailMsg.From = new MailAddress("admin@scafpac.com.au", "Test [Scaffold]");
                    MailMsg.To.Add(new MailAddress(dtfLogin.Rows[0]["EmailID"].ToString()));
                    StringBuilder strBody = new StringBuilder();
                    string emailbody = "Dear Scaffold User \n\n\n You are receiving this email because we received a password reset request for your account.\n\n";
                    // emailbody += "Reset Your Password: " + "http://" + HttpContext.Current.Request.Url.Authority + "/ChangePassword?ID=" + databaseCon.Encrypt(dtfLogin.Rows[0]["EmailID"].ToString()) + "\n\n";
                    emailbody += "Reset Your Password: " + "http://" + System.Web.HttpContext.Current.Request.Url.Authority + "/ChangePassword/ChangePassword?ID=" + databaseCon.Encrypt(dtfLogin.Rows[0]["EmailID"].ToString()) + "\n\n";
                    emailbody += " As always ,\n Thank you for using Scaffold system.";
                    // txtemailbody.Text = emailbody.ToString().Replace("\n", "<br>");
                    MailMsg.Subject = "Reset Scaffold Login Password ";
                    // MailMsg.Body = txtemailbody.Text;
                    MailMsg.Body = emailbody.ToString().Replace("\n", "<br>");
                    MailMsg.IsBodyHtml = true;
                    Client.Send(MailMsg);

                    //art1.Attributes["class"] = "alert w3-green w3-card-4";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "Reset password link has been sent on your EmailID";
                    //txtemailbody.Text = "";
                    Result = "Success";
                    Message = "Reset password link has been sent on your EmailID";
                }
                else
                {
                    //art1.Attributes["class"] = "alert w3-red w3-card-4";
                    //art1.Style.Add("display", "block");
                    //alertlabel.Text = "<strong>Opps! </strong>There is no user  registered for " + dtfLogin.Rows[0]["EmailID"].ToString() + ",Please contact the administrator.";
                    Result = "Failed";
                    Message = "There is no user  registered for " + dtfLogin.Rows[0]["EmailID"].ToString() + ",Please contact the administrator.";
                }
            }
            else
            {
                //art1.Attributes["class"] = "alert w3-red w3-card-4";
                //art1.Style.Add("display", "block");
                //alertlabel.Text = "<strong>Opps! </strong>The User Name you entered does not exists.";
                Result = "Failed";
                Message = "The User Name you entered does not exist.";
            }
            return Json(new { Result = Result, Message = Message }, JsonRequestBehavior.AllowGet);
        }

    }
}

public class HideMainDepotInventoryViewModel
{
    public Nullable<bool> HideMainDepotInventory { get; set; }

}