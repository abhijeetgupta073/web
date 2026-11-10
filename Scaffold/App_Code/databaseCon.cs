using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Scaffold.App_Code
{
    public class databaseCon
    {
        public static string siteimage = ConfigurationManager.AppSettings["siteimage"].ToString();
        public static string registerdocument = ConfigurationManager.AppSettings["registerdocument"].ToString();
        public static string clientimage = ConfigurationManager.AppSettings["clientimage"].ToString();
        public string baseURL = ConfigurationManager.AppSettings["storedBlobURL"].ToString();


        // Prod Blob Refrence
        //public const string registerdocumentlink = "https://scaffolddocuments.blob.core.windows.net/registerdocument/";
        //public const string siteimagelink = "https://scaffolddocuments.blob.core.windows.net/siteimage/";
        //public const string clientimageLink = "https://scaffolddocuments.blob.core.windows.net/clientimage/";


        // Test Blob Refrence
        public const string siteimagelink = "https://scaffolddocumentstest.blob.core.windows.net/siteimage/";
        public const string registerdocumentlink = "https://scaffolddocumentstest.blob.core.windows.net/registerdocument/";
        public const string clientimageLink = "https://scaffolddocumentstest.blob.core.windows.net/clientimage/";
        //public const string logoimage = "/images/logo.png";
        public const string logoimage = "/images/logo.png?v=2";
        //public const string ClientLogoLink = "~/Images/Logo2.png";
        //public const string ClientLogoLink2 = "~/images/Logo2.png";




        static string query, querypro = string.Empty;
        static SqlConnection con = null;
        static SqlCommand cmd = null;
        static SqlDataAdapter sda = null;
        static DataTable dt = null;
        static DataSet ds = null;
        string msg = "";
        public static Object thisLock = new Object();
        static databaseCon()
        {

            //  con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);


        }
        public static SqlConnection conM()
        {

            return con;

        }


        //this method fetch data according to given query when we pass procedure,view and selectQuery....

        public static DateTime ToTimeZoneTime(DateTime time, string timeZoneId = "Australian Western Standard Time")
        //public static DateTime ToTimeZoneTime(DateTime time, string timeZoneId = "Central Standard Time")
        //public static DateTime ToTimeZoneTime(DateTime time, string Clientid, string timeZoneId = "Central Standard Time")
        {
            //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            //return TimeZoneInfo.ConvertTimeFromUtc(time, tzi);
            decimal hours = databaseCon.getCell("select [Hours] from tblDaylightShift");
            //decimal hours = databaseCon.getCell("select [Hours] from tblDaylightShift where Country=(select Country from tblClient where ClientId='"+ Clientid + "')");
            return time.AddHours(Convert.ToInt32(hours));

        }

        //Added on 10-06-2023
        //public static string ShowControl(string PageId, string UserRole)
        //{
        //    string querysql = "exec Pro_tblShowControl_GET '" + PageId + "','" + UserRole + "'";
        //    string Buttonidfind = "";
        //    DataTable GetPageName = databaseCon.GetData(querysql);
        //    if (GetPageName.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < GetPageName.Rows.Count; i++)
        //        {
        //            if (GetPageName.Rows[i]["ButtonType"].ToString() == "Button")
        //            {
        //                if (GetPageName.Rows[i]["ButtonId"].ToString() != "")
        //                {
        //                    if (GetPageName.Rows.Count == 1)
        //                    {
        //                        Buttonidfind += GetPageName.Rows[i]["ButtonId"].ToString() + " btntype";
        //                    }
        //                    else
        //                    {
        //                        Buttonidfind += GetPageName.Rows[i]["ButtonId"].ToString() + " btntype" + ",";
        //                    }
        //                }
        //            }
        //            if (GetPageName.Rows[i]["ButtonType"].ToString() == "LinkButton")
        //            {
        //                if (GetPageName.Rows[i]["ButtonId"].ToString() != "")
        //                {
        //                    if (GetPageName.Rows.Count == 1)
        //                    {
        //                        Buttonidfind += GetPageName.Rows[i]["ButtonId"].ToString() + " linkbtntype";
        //                    }
        //                    else
        //                    {
        //                        Buttonidfind += GetPageName.Rows[i]["ButtonId"].ToString() + " linkbtntype" + ",";
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return Buttonidfind;


        //    //string a = "";
        //    // string getUserrole = "";
        //    // DataTable showControl = databaseCon.GetData("select HideFromRole from tblHideShow where ControlType='"+ ControlType + "' and ControlName='"+ ControlName + "'");
        //    // DataTable userRole = databaseCon.GetData("select Role from tblUser where  ClientId= '" + Clientid + "' and UserId='" + UserId + "' ");
        //    // string getHiderole = "";
        //    // if(showControl.Rows.Count>0)
        //    // {
        //    //     getHiderole = Convert.ToString(showControl.Rows[0]["HideFromRole"]);
        //    // }
        //    // if(userRole.Rows.Count>0)
        //    // {
        //    //     getUserrole = Convert.ToString(userRole.Rows[0]["Role"]);
        //    // }
        //    // else
        //    // {
        //    //     getUserrole ="blank";
        //    // }
        //    //// string getUserrole = "User";
        //    // string getrolename = "";
        //    // if(getHiderole.Contains(getUserrole))
        //    //  {
        //    //     getrolename = "none";
        //    // }
        //    // else
        //    // {
        //    //     getrolename = "block";
        //    // }
        //    //// return user;
        //    // return getrolename;
        //}
        public static DataTable GetData(string dataString, bool OverrideTimeout = false, int Timeout = 0)
        {
            lock (thisLock)
            {

                try
                {

                    query = dataString;
                    sda = new SqlDataAdapter(query, con);
                    //sda.SelectCommand.CommandTimeout = 0; // April 4, 2017
                    sda.SelectCommand.CommandTimeout = 900;
                    if (OverrideTimeout)
                    {
                        sda.SelectCommand.CommandTimeout = Timeout; // Jan 11, 2022
                    }

                    ds = new DataSet();
                    sda.Fill(ds);
                    //dt = ds.Tables[0];
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                    else
                    {
                        dt = new DataTable();
                    }

                    return dt;
                }
                catch (Exception ob)
                {
                    int? uid = null;
                    string pagePath = null;

                    HttpContext context = HttpContext.Current;
                    if (context != null)
                    {
                        string encryptedUID = context.Request.QueryString["UID"];
                        if (!string.IsNullOrEmpty(encryptedUID))
                        {
                            string decryptedUID = Decrypt(encryptedUID);
                            if (!string.IsNullOrEmpty(decryptedUID) && decryptedUID != "0")
                            {
                                uid = Convert.ToInt32(decryptedUID);
                            }
                        }
                        pagePath = context.Request.Url.AbsolutePath;
                    }

                    using (var logCon = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString))
                    using (var cmd = new SqlCommand("INSERT INTO tblErrorLog (ErrorMessage, SqlQuery, UserId, PagePath, CreatedOn) VALUES (@err, @sql, @uid, @page, @created)", logCon))
                    {
                        cmd.Parameters.AddWithValue("@err", ob.Message ?? "");
                        cmd.Parameters.AddWithValue("@sql", dataString ?? "");
                        cmd.Parameters.AddWithValue("@uid", uid ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@page", pagePath ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@created", DateTime.Now);
                        logCon.Open();
                        cmd.ExecuteNonQuery();
                    }
                    // ✅ Redirect user to login page
                    if (context != null)
                    {
                        context.Response.Redirect("~/Login");
                    }
                    dt = new DataTable();
                    return dt;
                }
            }
        }

        public static DataSet GetDataSet(string dataString)
        {
            lock (thisLock)
            {
                try
                {

                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    dt = new DataTable();
                    query = dataString;
                    sda = new SqlDataAdapter(query, con);
                    ds = new DataSet();

                    sda.Fill(ds);

                }
                catch
                {

                }
                finally { con.Close(); }
            }
            return ds;
        }

        public static decimal getCell(string Query)
        {

            // con.Open();   // Comment On 24 Oct 2019
            decimal de = 0;
            try
            {
                // Add on 24 Oct 2019 Check If Connection Is Not Open Then Connection Open
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand com = new SqlCommand(Query, con);
                de = Convert.ToDecimal((object)com.ExecuteScalar());
            }
            catch (Exception ex) { }
            finally { con.Close(); }
            return de;
        }
        public static String getStringCell(string Query)
        {

            // con.Open();  // Comment On 24 Oct 2019
            string de = "0";
            try
            {
                // Add on 24 Oct 2019 Check If Connection Is Not Open Then Connection Open 
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }
                SqlCommand com = new SqlCommand(Query, con);
                de = Convert.ToString((object)com.ExecuteScalar());
            }
            catch (Exception ex) { }
            finally { con.Close(); }
            return de;
        }
        public static DateTime getDate(string Query)
        {

            // con.Open(); // Comment On 24 Oct 2019
            DateTime de = Convert.ToDateTime("01/01/1900");
            try
            {    // Add on 24 Oct 2019 Check If Connection Is Not Open Then Connection Open
                if (con.State != ConnectionState.Open)
                {
                    con.Open();
                }

                SqlCommand com = new SqlCommand(Query, con);
                de = Convert.ToDateTime((object)com.ExecuteScalar());
            }
            catch (Exception ex) { }
            finally { con.Close(); }
            return de;
        }
        public static int RunCmd(string Query)
        {
            int d = 0;

            lock (thisLock)
            {
                try
                {

                    con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }
                    SqlCommand com = new SqlCommand(Query, con);
                    com.CommandTimeout = 0;
                    d = Convert.ToInt32((object)com.ExecuteNonQuery());

                }

                catch (Exception ex)
                {
                    return d;
                }
                finally
                {
                    con.Close();
                }
            }
            return d;

        }

        public static string CheckDecimal(string TextBoxvalues)
        {
            string value = "0";
            if (TextBoxvalues != "")
            {
                return TextBoxvalues;
            }
            else
            {
                return value;
            }
        }
        public static string ReturnNull(string TextBoxvalues)
        {
            string value = "NULL";
            if (TextBoxvalues != "")
            {
                return TextBoxvalues;
            }
            else
            {
                return value;
            }
        }
        public static int RunCmdID(string Query)
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString);
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            int d = 0;

            try
            {
                SqlCommand com = new SqlCommand(Query, con);
                d = Convert.ToInt32((object)com.ExecuteNonQuery());
                if (d > 0)
                {
                    object i = "0";
                    SqlCommand cmd1 = new SqlCommand("SELECT SCOPE_IDENTITY()", con);
                    SqlDataReader sdr = cmd1.ExecuteReader();
                    while (sdr.Read())
                    {
                        i = sdr.GetValue(0);
                        d = Convert.ToInt32(i.ToString());

                    }



                }
                return d;
            }
            catch (Exception ex)
            {
                return d;
            }
            finally
            {
                con.Close();
            }

        }
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "HSE8TIPS4PBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            try
            {
                string EncryptionKey = "HSE8TIPS4PBNI99212";
                if (cipherText != null)
                {
                    cipherText = cipherText.Replace(" ", "+");
                }
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch (Exception ex)
            {
                return "0";
            }
        }
        public static bool SendEmailWithName(string FromEmailID, string To, string Sub, string Msg, string FromEmailName)
        {
            bool Status = false;
            //string From = "azure_0df29fac9bf524c9c35a58c87ad05d0b@azure.com";
            //string Password = "tBmetVs7N2DpnEa";
            //NetworkCredential Credential = new NetworkCredential(From, Password);
            //SmtpClient Client = new SmtpClient();
            //Client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //Client.EnableSsl = true;
            //Client.Host = "smtp.sendgrid.net";
            //Client.Port = 25;
            //Client.UseDefaultCredentials = false;
            //Client.Credentials = Credential;

            // Init SmtpClient and send
            SmtpClient Client = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(25));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("apikey", ConfigurationManager.AppSettings["SendGridApiKey"]);
            Client.Credentials = credentials;

            MailMessage MailMsg = new MailMessage();
            MailMsg.From = new MailAddress(FromEmailID, FromEmailName);
            MailMsg.To.Add(new MailAddress(To));
            MailMsg.Subject = Sub;
            MailMsg.Body = Msg;
            MailMsg.IsBodyHtml = true;
            try
            {
                //Client.Send(MailMsg);
                Status = true;
            }
            catch
            {
                Status = false;
            }
            return Status;
        }

        public static int GetInspectionTamplate(int TypeID)
        {
            int countt = 0;
            query = "select templateID from defInspectionType where InspectionTypeID=" + TypeID;
            DataTable dtn = databaseCon.GetData(query);
            if (dtn.Rows.Count > 0)
            {
                countt = Convert.ToInt32(dtn.Rows[0]["templateID"]);
            }
            return countt;
        }

        public static string SaveDate(string Date)
        {
            string Value = "";
            if (Date != "")
            {
                Value = "'" + Date + "'";
            }
            else
            {
                Value = "NULL";
            }
            return Value;
        }
        public static bool SendEmailfordemo(string FromEmailID, string To, string Sub, string Msg)
        {
            bool Status = false;

            string FromEmailName = "HSC-TIPS";
            string cc = "";
            //NetworkCredential Credential = new NetworkCredential(From, Password);
            //SmtpClient Client = new SmtpClient();
            //Client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //Client.EnableSsl = true;
            //Client.Host = "smtp.sendgrid.net";
            //Client.Port = 25;
            //Client.UseDefaultCredentials = false;
            //Client.Credentials = Credential;

            // Init SmtpClient and send
            SmtpClient Client = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(25));
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("apikey", ConfigurationManager.AppSettings["SendGridApiKey"]);
            Client.Credentials = credentials;


            MailMessage MailMsg = new MailMessage();
            MailMsg.From = new MailAddress(FromEmailID, FromEmailName);
            MailMsg.To.Add(new MailAddress(To));
            MailMsg.CC.Add(new MailAddress(cc));
            MailMsg.Subject = Sub;
            MailMsg.Body = Msg;
            MailMsg.IsBodyHtml = true;
            try
            {
                //             Client.Send(MailMsg);
                Status = true;
            }
            catch
            {
                Status = false;
            }
            return Status;
        }
        public static void savefileonblob(string FileName, string StorageType, string Path, string ContentType)
        {
            CloudStorageAccount storageaccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobclient = storageaccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobclient.GetContainerReference(siteimage);
            if (StorageType == "Site-images")
            {
                container = blobclient.GetContainerReference(siteimage);
            }
            if (StorageType == "Inspection-Document")
            {
                container = blobclient.GetContainerReference(registerdocument);
            }
            if (StorageType == "Client-img")
            {
                //int query = databaseCon.RunCmd("insert into tblerror (Descr, CreatedOn) values(' "+ clientimage + "'+'clientimage','" + DateTime.Now+"')");
                container = blobclient.GetContainerReference(clientimage);

            }
            //int query1 = databaseCon.RunCmd("insert into tblerror (Descr, CreatedOn) values('" + FileName + "'+'after client-img','" + DateTime.Now + "')");
            CloudBlockBlob blockblob = container.GetBlockBlobReference(FileName);
            blockblob.Properties.ContentType = ContentType;
            //int query2 = databaseCon.RunCmd("insert into tblerror (Descr, CreatedOn) values('" + Path + "'+'in Path','" + DateTime.Now + "')");
            using (var fileStream = System.IO.File.OpenRead(Path))
            {
                blockblob.UploadFromStream(fileStream);

            }
        }
        public static void deletefileonblob(string filename, string foldername = "")
        {
            CloudStorageAccount storageaccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            CloudBlobClient blobclient = storageaccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobclient.GetContainerReference(foldername);
            CloudBlockBlob blockblob = container.GetBlockBlobReference(filename);
            blockblob.DeleteIfExists();
        }
        public static bool SendEmail(string To, string cc, string Sub, string Msg, string FileName)
        {
            bool Status = false;
            // string From, Password;
            //From = "azure_0df29fac9bf524c9c35a58c87ad05d0b@azure.com";
            //Password = "tBmetVs7N2DpnEa";
            //NetworkCredential Credential = new NetworkCredential(From, Password);
            //SmtpClient Client = new SmtpClient();
            //Client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //Client.EnableSsl = true;
            //Client.Host = "smtp.sendgrid.net";
            //Client.Port = 25;// 587;
            //Client.UseDefaultCredentials = false;
            //Client.Credentials = Credential;

            // Init SmtpClient and send
            SmtpClient Client = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(25));
            // SendGrid credentials are supplied through configuration.
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("apikey", ConfigurationManager.AppSettings["SendGridApiKey"]); Client.Credentials = credentials;

            MailMessage MailMsg = new MailMessage();
            MailMsg.From = new MailAddress("admin@scafpac.com.au", "Scaffold");

            string tomail = To.Replace(":", ",").Replace(";", ",");
            string[] multi = tomail.Split(',');
            foreach (string multiemail in multi)
            {
                if (multiemail != "")
                {
                    MailMsg.To.Add(new MailAddress(multiemail));
                }
            }

            if (cc != "")
            {
                MailMsg.CC.Add(new MailAddress(cc));
            }
            MailMsg.CC.Add("scafpacinspection@gmail.com");

            MailMsg.Subject = Sub;// "Thankyou for Sign Up";

            MailMsg.Body = Msg.Replace("\n", "<br/>");// 
            MailMsg.IsBodyHtml = true;
            if (FileName != "")
                try
                {
                    MailMsg.Attachments.Add(new Attachment(FileName));
                }
                catch { }
            try
            {

                //Client.Send(MailMsg);
                Status = true;
                MailMsg.Dispose();
                if (FileName.Contains("Template") || FileName.Contains("Report"))
                {
                    //do not delete file

                }
                else
                {
                    //if (File.Exists(FileName))
                    //File.Delete(FileName);
                }
            }
            catch (Exception ex)
            {
                MailMsg.Dispose();
                Status = false;
                HttpContext.Current.Response.Write(ex.Message);
            }

            return Status;
        }

        public static string InspectionPdfReportCreate(int ClientID, int InspectionID)
        {
            string FileName, FileName1, PdfFileFullPath;
            string query;
            // string queryClientLogo = "select Clientimage,isnull(InspectionCheckListColor,0) as InspectionCheckListColor  from tblclient where clientid = '" + databaseCon.Decrypt(Request.QueryString["ID"]) + "'";

            string queryClientLogo = "select Clientimage,isnull(InspectionCheckListColor,0) as InspectionCheckListColor  from tblclient where clientid = '" + ClientID + "'";

            //query = "select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,REPLACE(CONVERT(VARCHAR(11),InspectionDate,106), ' ','-') as InspectionDate,InspectionDate as InspectionDateTime ,InspectedById,CompleteInsp,InspStatus,tblInspection.InspectedBy,tblinspection.CreatedBy,tblinspection.CreatedOn,OnlineInsp,Acknowledgment,CompleteInsp,tblinspection.Lat,tblinspection.Long,GPSAddress,Inspimage,InspAudio,Inspvedio,InspSign,InspComments,InspStatus,MapImage,case when AckDate='1900-01-01' then null else REPLACE(CONVERT(VARCHAR(11),AckDate,106), ' ','-') end as AckDate,LicenceNo,Right('000000'+Convert(varchar,ScaffRef),6) as ScaffRef,DesignType,VW_TblRegister.SiteLocation as SiteName,tblSiteLocation.OwnerName as OwnerName from tblinspection  join VW_TblRegister on tblInspection.RegisterID=VW_TblRegister.RegisterId join tblSiteLocation on tblSiteLocation.SiteLocID=VW_TblRegister.SiteLocId where InspectionID ='" + InspectionID + "'";

            // Query Change On 10 Jan 2020 For Show AckDate In Data And Time
            //query = "select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,REPLACE(CONVERT(VARCHAR(11),InspectionDate,106), ' ','-') as InspectionDate,InspectionDate as InspectionDateTime ,InspectedById,CompleteInsp,InspStatus,firstName+LastName as InspectedBy,tblinspection.CreatedBy,tblinspection.CreatedOn,OnlineInsp,Acknowledgment,CompleteInsp,tblinspection.Lat,tblinspection.Long,GPSAddress,Inspimage,InspAudio,Inspvedio,InspSign,InspComments,InspStatus,MapImage,case when AckDate='1900-01-01' then null else REPLACE(CONVERT(VARCHAR(11),AckDate,106), ' ','-') end as AckDate,tblinspection.LicenceNo,Right('000000'+Convert(varchar,ScaffRef),6) as ScaffRef,DesignType,VW_TblRegister.SiteLocation as SiteName,tblSiteLocation.OwnerName as OwnerName from tblinspection  join VW_TblRegister on tblInspection.RegisterID=VW_TblRegister.RegisterId join tblSiteLocation on tblSiteLocation.SiteLocID=VW_TblRegister.SiteLocId join tblUser on tblInspection.InspectedById=tblUser.UserId  where InspectionID ='" + InspectionID + "'";

            //query = "select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,REPLACE(CONVERT(VARCHAR(11),InspectionDate,106), ' ','-') as InspectionDate,InspectionDate as InspectionDateTime ,InspectedById,CompleteInsp,InspStatus,firstName+LastName as InspectedBy,tblinspection.CreatedBy,tblinspection.CreatedOn,OnlineInsp,Acknowledgment,CompleteInsp,tblinspection.Lat,tblinspection.Long,GPSAddress,Inspimage,InspAudio,Inspvedio,InspSign,InspComments,InspStatus,MapImage,case when AckDate='1900-01-01' then null else AckDate end as AckDate,tblinspection.LicenceNo,Right('000000'+Convert(varchar,ScaffRef),6) as ScaffRef,DesignType,VW_TblRegister.SiteLocation as SiteName,tblSiteLocation.OwnerName as OwnerName from tblinspection  join VW_TblRegister on tblInspection.RegisterID=VW_TblRegister.RegisterId join tblSiteLocation on tblSiteLocation.SiteLocID=VW_TblRegister.SiteLocId join tblUser on tblInspection.InspectedById=tblUser.UserId  where InspectionID ='" + InspectionID + "'";

            // by baans 01/22/2020 Add PhotoAngle 
            // query = "select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,REPLACE(CONVERT(VARCHAR(11),InspectionDate,106), ' ','-') as InspectionDate,InspectionDate as InspectionDateTime ,InspectedById,CompleteInsp,InspStatus,firstName+LastName as InspectedBy,tblinspection.CreatedBy,tblinspection.CreatedOn,OnlineInsp,Acknowledgment,CompleteInsp,tblinspection.Lat,tblinspection.Long,GPSAddress,Inspimage,tblInspection.PhotoAngle,InspAudio,Inspvedio,InspSign,InspComments,InspStatus,MapImage,case when AckDate='1900-01-01' then null else AckDate end as AckDate,tblinspection.LicenceNo,Right('000000'+Convert(varchar,ScaffRef),6) as ScaffRef,DesignType,VW_TblRegister.SiteLocation as SiteName,tblSiteLocation.OwnerName as OwnerName from tblinspection  join VW_TblRegister on tblInspection.RegisterID=VW_TblRegister.RegisterId join tblSiteLocation on tblSiteLocation.SiteLocID=VW_TblRegister.SiteLocId join tblUser on tblInspection.InspectedById=tblUser.UserId  where InspectionID ='" + InspectionID + "'";
            //end

            // Query Change Add  Join On ViewTblInspection Query on 24 Jan 2020
            // query = "select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,REPLACE(CONVERT(VARCHAR(11),InspectionDate,106), ' ','-') as InspectionDate,InspectionDate as InspectionDateTime ,InspectedById,CompleteInsp,InspStatus,firstName+LastName as InspectedBy,vw_TblInspection.CreatedBy,vw_TblInspection.CreatedOn,OnlineInsp,Acknowledgment,CompleteInsp,vw_TblInspection.Lat,vw_TblInspection.Long,GPSAddress,Inspimage,vw_TblInspection.PhotoAngle,InspAudio,Inspvedio,InspSign,InspComments,InspStatus,MapImage,case when AckDate='1900-01-01' then null else AckDate end as AckDate,vw_TblInspection.LicenceNo,Right('000000'+Convert(varchar,ScaffRef),6) as ScaffRef,DesignType,VW_TblRegister.SiteLocation as SiteName,tblSiteLocation.OwnerName as OwnerName from vw_TblInspection  join VW_TblRegister on vw_TblInspection.RegisterID=VW_TblRegister.RegisterId join tblSiteLocation on tblSiteLocation.SiteLocID=VW_TblRegister.SiteLocId join tblUser on vw_TblInspection.InspectedById=tblUser.UserId  where InspectionID ='" + InspectionID + "'";

            // Query Change On 27 Jan 2020
            // query = "select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,InspectionTime,REPLACE(CONVERT(VARCHAR(11),InspectionDate,106), ' ','-') as InspectionDate,InspectionDate as InspectionDateTime ,InspectedById,CompleteInsp,InspStatus,firstName+LastName as InspectedBy,vw_TblInspection.CreatedBy,vw_TblInspection.CreatedOn,OnlineInsp,Acknowledgment,CompleteInsp,vw_TblInspection.Lat,vw_TblInspection.Long,GPSAddress,Inspimage,Vw_TblInspection.PhotoAngle,InspAudio,Inspvedio,InspSign,InspComments,InspStatus,MapImage,case when AckDate='1900-01-01' then null else AckDate end as AckDate,AckDateTime,vw_TblInspection.LicenceNo,Right('000000'+Convert(varchar,ScaffRef),6) as ScaffRef,DesignType,VW_TblRegister.SiteLocation as SiteName,tblSiteLocation.OwnerName as OwnerName from vw_TblInspection  join VW_TblRegister on vw_TblInspection.RegisterID=VW_TblRegister.RegisterId join tblSiteLocation on tblSiteLocation.SiteLocID=VW_TblRegister.SiteLocId join tblUser on vw_TblInspection.InspectedById=tblUser.UserId  where InspectionID ='" + InspectionID + "'";

            // Query Change On 12 August 2020 Sum PhotoAngle+SafariPhotoangle=PhotoAngle Only For Pdf
            query = "select REPLACE(CONVERT(VARCHAR(11),getdate(),106), ' ','-') as CurrentDate,InspectionTime,REPLACE(CONVERT(VARCHAR(11),InspectionDate,106), ' ','-') as InspectionDate,InspectionDate as InspectionDateTime ,InspectedById,CompleteInsp,InspStatus,firstName + ' ' + LastName as InspectedBy,vw_TblInspection.CreatedBy,vw_TblInspection.CreatedOn,OnlineInsp,Acknowledgment,CompleteInsp,vw_TblInspection.Lat,vw_TblInspection.Long,GPSAddress,Inspimage,Inspimage2,Convert(int,isnull(Vw_TblInspection.PhotoAngle,0))+ Convert(int,isnull(Vw_TblInspection.SafariPhotoAngle,0)) as PhotoAngle,Convert(int,isnull(Vw_TblInspection.PhotoAngle1,0))+ Convert(int,isnull(Vw_TblInspection.SafariPhotoAngle1,0)) as PhotoAngle1,InspAudio,Inspvedio,InspSign,InspComments,InspStatus,MapImage,case when AckDate='1900-01-01' then null else AckDate end as AckDate,AckDateTime,vw_TblInspection.LicenceNo,Right('000000'+Convert(varchar,ScaffRef),6) as ScaffRef,DesignType,VW_TblRegister.SiteLocation as SiteName,tblSiteLocation.OwnerName as OwnerName from vw_TblInspection  join VW_TblRegister on vw_TblInspection.RegisterID=VW_TblRegister.RegisterId join tblSiteLocation on tblSiteLocation.SiteLocID=VW_TblRegister.SiteLocId join tblUser on vw_TblInspection.InspectedById=tblUser.UserId  where InspectionID ='" + InspectionID + "'";


            // Query Comment On 20 Jan 2020

            // string query1 = "select InspectionListID, tblInspectionList.CheckListID,tblInspectionList.ContentID,CheckListContent ,Y,N,NA,ActionRequired ,case when checklistItem IS not NULL then 'Question' else 'Category' end as CategoryType  from tblInspectionList join tblChecklistContent on tblInspectionList.ContentID = tblChecklistContent.ContentID where InspectionID = '" + InspectionID + "' order by isnull(ChecklistItem, tblInspectionList.ContentId),CheckListItem";


            // Query Change On 20 Jan 2020 Add Order By Item In Header And Ouestion


            string query1 = "Select * from (select InspectionListID, tblInspectionList.CheckListID, tblInspectionList.ContentID, CheckListContent, Y, N,Case when isnull(IsNA,0)= 0 then Null else NA end as NA,ActionRequired ,case when checklistItem IS not NULL then 'Question' else 'Category' end as CategoryType ,case when checklistItem is null and subChildItem is null then Right('00' + CONVERT(varchar, itemorder), 2)+'.0' Else case when checklistItem is not null and subChildItem is not null then Convert(varchar, (select Right('00' + Convert(varchar, itemOrder), 2) from tblChecklistContent where ContentID = a.checklistItem) )+'.' + (select Right('00' + Convert(varchar, itemOrder), 2) from tblChecklistContent where ContentID = a.subChildItem)+'.' + Right('000' + CONVERT(varchar, itemorder), 3) Else Right('00' + Convert(varchar, (select Convert(varchar, itemOrder) from tblChecklistContent where ContentID = a.checklistItem)), 2)+'.' + Right('000' + CONVERT(varchar, itemorder), 3) End End as itemorder from tblInspectionList join tblChecklistContent a on tblInspectionList.ContentID = a.ContentID where InspectionID =  '" + InspectionID + "')AllData order by AllData.ItemOrder";

            DataTable clientLogodt = databaseCon.GetData(queryClientLogo);

            DataTable Inspdt = databaseCon.GetData(query);

            DataTable checkdt = databaseCon.GetData(query1);

            // string path = Server.MapPath("~/Report/Inspection_" + Inspdt.Rows[0]["InspectionDate"] + "_" + InspectionID + ".pdf");
            string path = System.Web.HttpContext.Current.Server.MapPath("~/Report/Inspection_" + Inspdt.Rows[0]["InspectionDate"] + "_" + InspectionID + ".pdf");


            // txttempfile.Text = "Inspection_" + Inspdt.Rows[0]["InspectionDate"] + "_" + InspectionID + ".pdf";
            // txtFile.Text = path;
            FileName = "Inspection_" + Inspdt.Rows[0]["InspectionDate"] + "_" + InspectionID + ".pdf";
            PdfFileFullPath = path;

            iTextSharp.text.Document doc = new iTextSharp.text.Document(PageSize.A4, -50f, -50f, 20f, 20f);   //Page Size set top ,left,margin
                                                                                                              //doc.SetPageSize(iTextSharp.text.PageSize.A4);

            //PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
            // Response.ContentType = ("application/pdf");
            // var contentfontCheck = FontFactory.GetFont((Server.MapPath("~/Assets/fonts/wingding.ttf")), BaseFont.CP1252, true, 14);
            // var contentfontCheckYes = FontFactory.GetFont((Server.MapPath("~/Assets/fonts/wingding.ttf")), BaseFont.CP1252, true, 14);
            var contentfontCheck = FontFactory.GetFont((System.Web.HttpContext.Current.Server.MapPath("~/Content/fonts/wingding.ttf")), BaseFont.CP1252, true, 14);
            var contentfontCheckYes = FontFactory.GetFont((System.Web.HttpContext.Current.Server.MapPath("~/Content/fonts/wingding.ttf")), BaseFont.CP1252, true, 14);

            //  contentfontCheckYes.SetColor(0, 154, 0); // Comment On 10 Dec 2019 For Conditipon Base Trick In Checkbox

            // var contentfontCheckNo = FontFactory.GetFont((System.Web.HttpContext.Current.Server.MapPath("~/Assets/fonts/wingding.ttf")), BaseFont.CP1252, true, 14);
            var contentfontCheckNo = FontFactory.GetFont((System.Web.HttpContext.Current.Server.MapPath("~/Content/fonts/wingding.ttf")), BaseFont.CP1252, true, 14);
            // contentfontCheckNo.SetColor(214, 0, 0);  // Comment On 10 Dec 2019 For Conditipon Base Trick In Checkbox

            if (Convert.ToBoolean(clientLogodt.Rows[0]["InspectionCheckListColor"]) == false)
            {
                contentfontCheckYes.SetColor(0, 154, 0);
                contentfontCheckNo.SetColor(214, 0, 0);
            }

            //pageSize.setBackgroundColor(new BaseColor(84, 141, 212));
            // var contentfontUncheck = FontFactory.GetFont((Server.MapPath("~/Assets/fonts/wingding.ttf")), BaseFont.CP1252, true, 14);
            var contentfontUncheck = FontFactory.GetFont((System.Web.HttpContext.Current.Server.MapPath("~/Content/fonts/wingding.ttf")), BaseFont.CP1252, true, 14);
            PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
            doc.Open();
            var Heading787 = FontFactory.GetFont("Arial", 20f, iTextSharp.text.Font.BOLD, BaseColor.BLUE);
            var Heading786 = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            var Heading788 = FontFactory.GetFont("Arial", 12f, iTextSharp.text.Font.BOLD, BaseColor.BLUE);
            var DataFont = FontFactory.GetFont("Arial", 10f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            Phrase phrase;
            PdfPCell cell;

            PdfPTable headertable1 = new PdfPTable(5);
            headertable1.SetWidths(new int[] { 25, 9, 10, 9, 9 });

            //Table Start
            PdfPTable headertable2 = new PdfPTable(6);
            headertable2.SetWidths(new int[] { 10, 9, 25, 9, 9, 9 });


            //  if (Convert.ToString(Inspdt.Rows[0]["InspStatus"]) == "Confirmed")
            if ((Convert.ToString(Inspdt.Rows[0]["InspStatus"]) == "Confirmed") || (Convert.ToString(Inspdt.Rows[0]["InspStatus"]) == "Out of Service"))
            {
                string Unchecked = System.Web.HttpContext.Current.Server.MapPath("~/images/lock.png");
                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(Unchecked), true);
                cell.FixedHeight = 20f;

                cell.PaddingTop = -10f;
                cell.Border = 0;
                cell.Colspan = 6;
                cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                headertable2.AddCell(cell);
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 6;
                cell.FixedHeight = 20f;
                cell.PaddingTop = -10f;
                cell.Border = 0;
                headertable2.AddCell(cell);
            }


            if (!string.IsNullOrEmpty(Convert.ToString(clientLogodt.Rows[0]["ClientImage"])))
            {
                //  var uri = new Uri(databaseCon.clientimageLink);
                //  var container = new CloudBlobContainer(uri);
                //  var blob = container.GetBlockBlobReference(clientLogodt.Rows[0]["ClientImage"].ToString());
                // var noimage = Server.MapPath("Images/Logo2.png");
                var noimage = System.Web.HttpContext.Current.Server.MapPath("~/Images/Logo2.png");

                //  var exists = blob.Exists();
                if (clientLogodt.Rows[0]["ClientImage"].ToString() == "")
                {
                    // string nophoto = Server.MapPath("images/Logo2.png");
                    string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                    cell.Colspan = 2;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.FixedHeight = 50f;
                    cell.PaddingTop = -10f;

                }
                else
                {
                    //iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(Server.MapPath("images/clientlogo/" + clientLogodt.Rows[0]["ClientImage"].ToString()));
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(System.Web.HttpContext.Current.Server.MapPath("~/images/clientlogo/" + clientLogodt.Rows[0]["ClientImage"].ToString()));
                    img.ScaleAbsolute(180f, 133f);
                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);

                    cell.Colspan = 2;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.FixedHeight = 50f;
                    cell.PaddingTop = -10f;

                }
            }
            else
            {
                //string nophoto = Server.MapPath("images/Logo2.png");
                string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/Logo2.png");

                cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
                cell.Colspan = 2;
            }
            cell.Border = 0;
            cell.Colspan = 2;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.FixedHeight = 50f;
            cell.PaddingTop = -10f;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Scaffold Inspection", Heading787));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            cell.PaddingTop = -10f;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            headertable2.AddCell(cell);

            // Remove Date No Need To Show as per Client Requirment 15 Jan 2020

            phrase = new Phrase();
            // phrase.Add(new Chunk("Date: ", Heading786));
            // phrase.Add(new Chunk("" + DateTime.Now.ToString("dd-MMM-yyy") + "", DataFont));
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 2;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            headertable2.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);


            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 6;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable2.AddCell(cell);

            // Add On 28 June
            phrase = new Phrase();
            phrase.Add(new Chunk("Site", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.FixedHeight = 30f;
            cell.Border = 0;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);

            if (Inspdt.Rows.Count > 0)
            {
                for (int i = 0; i < Inspdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(Inspdt.Rows[i]["SiteName"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.PaddingBottom = 6F;
                cell.BorderColor = BaseColor.GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }


            phrase = new Phrase();
            phrase.Add(new Chunk("Owner Name", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.FixedHeight = 30f;
            cell.Border = 0;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);
            if (Inspdt.Rows.Count > 0)
            {
                for (int i = 0; i < Inspdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(Inspdt.Rows[i]["OwnerName"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.PaddingBottom = 6F;
                cell.BorderColor = BaseColor.GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }


            phrase = new Phrase();
            phrase.Add(new Chunk("Scafpac Tag #", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.FixedHeight = 30f;
            cell.Border = 0;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);

            if (Inspdt.Rows.Count > 0)
            {
                for (int i = 0; i < Inspdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(Inspdt.Rows[i]["ScaffRef"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.PaddingBottom = 6F;
                cell.BorderColor = BaseColor.GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }


            phrase = new Phrase();
            phrase.Add(new Chunk("Design Type", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.FixedHeight = 30f;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (Inspdt.Rows.Count > 0)
            {
                for (int i = 0; i < Inspdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(Inspdt.Rows[i]["DesignType"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;

                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }



            phrase = new Phrase();
            // phrase.Add(new Chunk("Inspection Date", Heading786));
            // Change On 15 Jan 2020
            phrase.Add(new Chunk("Scheduled Inspection Date", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.BorderColor = BaseColor.GRAY;
            cell.FixedHeight = 30f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);

            if (Inspdt.Rows.Count > 0)
            {
                for (int i = 0; i < Inspdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    // Add Check On 27 Jan 2020 When Inspection Time is 12 Am then Show only DAte otherwise DateTime
                    if (Convert.ToInt32(Inspdt.Rows[i]["InspectionTime"]) == 0)
                    {
                        phrase.Add(new Chunk(string.Format("{0:dd-MMM-yyyy}", Inspdt.Rows[i]["InspectionDateTime"]), DataFont));
                    }
                    else
                    {
                        phrase.Add(new Chunk(string.Format("{0:dd-MMM-yyyy hh:mm tt}", Inspdt.Rows[i]["InspectionDateTime"]), DataFont));
                    }
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            // Code Changes On 1 August 2019

            //phrase = new Phrase();
            //phrase.Add(new Chunk("GPS Location", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.BorderColor = BaseColor.GRAY; ;
            //cell.BackgroundColor = new iTextSharp.text.Color(220, 220, 220);
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //headertable1.AddCell(cell);

            //if (!string.IsNullOrEmpty(Convert.ToString(Inspdt.Rows[0]["MapImage"])))
            //{
            //    var uri = new Uri(databaseCon.siteimagelink);
            //    var container = new CloudBlobContainer(uri);
            //    var blob = container.GetBlockBlobReference(Inspdt.Rows[0]["MapImage"].ToString());
            //    var noimage = Server.MapPath("images/noimg.jpg");
            //    var exists = blob.Exists();
            //    if (exists == false)
            //    {
            //        string nophoto = Server.MapPath("images/noimg.jpg");
            //        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(noimage), true);
            //        cell.PaddingLeft = 10f;
            //        cell.PaddingBottom = 10f;
            //        cell.PaddingRight = 150f;

            //        cell.PaddingTop = 10f;
            //        cell.BorderColor = Color.LIGHT_GRAY;
            //        cell.Colspan = 4;
            //    }
            //    else
            //    {
            //        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + Inspdt.Rows[0]["MapImage"].ToString());
            //        img.ScaleAbsolute(180f, 133f);

            //        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
            //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cell.BorderColor = BaseColor.GRAY;
            //        cell.PaddingLeft = 1f;
            //        cell.PaddingBottom = 1f;
            //        cell.PaddingRight = 100f;

            //        cell.PaddingTop = 1f;
            //        cell.Colspan = 4;
            //        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    }
            //}
            //else
            //{
            //    string nophoto = Server.MapPath("images/noimg.jpg");
            //    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);
            //    cell.Colspan = 4;
            //    cell.PaddingLeft = 10f;
            //    cell.PaddingBottom = 10f;
            //    cell.PaddingRight = 150f;

            //    cell.PaddingTop = 10f;
            //    cell.BorderColor = BaseColor.GRAY;
            //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //}
            //cell.Border = 0;
            //cell.Colspan = 4;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.BorderColor = BaseColor.GRAY;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //headertable1.AddCell(cell);




            phrase = new Phrase();
            phrase.Add(new Chunk("Photo1", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BorderColor = BaseColor.GRAY;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);

            if (!string.IsNullOrEmpty(Convert.ToString(Inspdt.Rows[0]["Inspimage"])))
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(Inspdt.Rows[0]["Inspimage"].ToString());
                // var noimage = Server.MapPath("images/noimg.jpg");
                var noimage = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");

                var exists = blob.Exists();
                if (exists == false)
                {
                    //string nophoto = Server.MapPath("images/noimg.jpg");
                    string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");
                    // cell = new PdfPCell(iTextSharp.text.Image.GetInstance(noimage), true);
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", Heading786));
                    cell = new PdfPCell(phrase);
                    cell.FixedHeight = 30f;

                    cell.PaddingLeft = 10f;
                    cell.PaddingBottom = 10f;
                    cell.PaddingRight = 150f;

                    cell.PaddingTop = -20f;
                    cell.BorderColor = BaseColor.LIGHT_GRAY;
                    cell.Colspan = 4;
                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + Inspdt.Rows[0]["Inspimage"].ToString());
                    //added on 28-July-2022 start here
                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(databaseCon.siteimagelink + Inspdt.Rows[0]["Inspimage"].ToString());
                    MemoryStream ms = new MemoryStream(bytes);
                    System.Drawing.Image img1 = System.Drawing.Image.FromStream(ms);
                    //added by 28-July-2022 end here 
                    // var GetImageRotation = img.GetImageRotation();

                    // by baans 01/22/2020
                    var imagephotoangle = "";
                    if (Inspdt.Rows[0]["PhotoAngle"].ToString() != "")
                    {

                        imagephotoangle = Inspdt.Rows[0]["PhotoAngle"].ToString();
                        if (imagephotoangle == "-90")
                        {
                            imagephotoangle = "-270";
                        }
                        else if (imagephotoangle == "-270")
                        {
                            imagephotoangle = "-90";
                        }

                        else if (imagephotoangle == "90")
                        {
                            imagephotoangle = "270";
                        }

                        else if (imagephotoangle == "270")
                        {
                            imagephotoangle = "90";
                        }
                        else
                        {
                            imagephotoangle = Inspdt.Rows[0]["PhotoAngle"].ToString();
                        }

                    }
                    else
                    {
                        imagephotoangle = "0";
                    }

                    img.ScaleAbsolute(180f, 133f);
                    img.Rotation = (float)Math.PI / 2;

                    var imagesangle = Convert.ToString(imagephotoangle);
                    img.RotationDegrees = Convert.ToSingle(imagephotoangle);

                    //img.setRotationDegrees(90);
                    // end
                    // Added on 28-July-2022 start here
                    if (img1.PropertyIdList.Contains(0x0112))
                    {
                        PropertyItem propOrientation = img1.GetPropertyItem(0x0112);
                        short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
                        if (orientation == 6)
                        {
                            img1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        }
                        else if (orientation == 8)
                        {
                            img1.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        }
                        else if (orientation == 3)
                        {
                            img1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        }
                        else
                        {
                            // Do nothing
                        }

                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img1, BaseColor.WHITE), true);
                    }
                    else
                    {
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    }
                    // Added on 28-July-2022 end here
                    //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);//Commented on 28-July-2022
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.PaddingLeft = 1f;
                    cell.PaddingBottom = 1f;
                    cell.PaddingRight = 100f;
                    cell.PaddingTop = 1f;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                }
            }
            else
            {
                //string nophoto = Server.MapPath("images/noimg.jpg");
                string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");

                // cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.FixedHeight = 30f;

                cell.Colspan = 4;
                cell.PaddingLeft = 10f;
                cell.PaddingBottom = 10f;
                cell.PaddingRight = 150f;
                cell.PaddingTop = 10f;
                cell.BorderColor = BaseColor.GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            }
            cell.Border = 0;
            cell.Colspan = 4;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderColor = BaseColor.GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            //New Inspimages Start
            phrase = new Phrase();
            phrase.Add(new Chunk("Photo2", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BorderColor = BaseColor.GRAY;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);

            if (!string.IsNullOrEmpty(Convert.ToString(Inspdt.Rows[0]["Inspimage2"])))
            {
                var uri1 = new Uri(databaseCon.siteimagelink);
                var container1 = new CloudBlobContainer(uri1);
                var blob1 = container1.GetBlockBlobReference(Inspdt.Rows[0]["Inspimage2"].ToString());
                // var noimage = Server.MapPath("images/noimg.jpg");
                var noimage = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");

                var exists1 = blob1.Exists();
                if (exists1 == false)
                {
                    //string nophoto = Server.MapPath("images/noimg.jpg");
                    string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");
                    // cell = new PdfPCell(iTextSharp.text.Image.GetInstance(noimage), true);
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", Heading786));
                    cell = new PdfPCell(phrase);
                    cell.FixedHeight = 30f;

                    cell.PaddingLeft = 10f;
                    cell.PaddingBottom = 10f;
                    cell.PaddingRight = 150f;

                    cell.PaddingTop = -20f;
                    cell.BorderColor = BaseColor.LIGHT_GRAY;
                    cell.Colspan = 4;
                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + Inspdt.Rows[0]["Inspimage2"].ToString());

                    //added on 28-July-2022 start here
                    WebClient wc = new WebClient();
                    byte[] bytes = wc.DownloadData(databaseCon.siteimagelink + Inspdt.Rows[0]["Inspimage2"].ToString());
                    MemoryStream ms = new MemoryStream(bytes);
                    System.Drawing.Image img1 = System.Drawing.Image.FromStream(ms);
                    //added by 28-July-2022 end here 

                    // var GetImageRotation = img.GetImageRotation();

                    // by baans 01/22/2020
                    var imagephotoangle = "";
                    if (Inspdt.Rows[0]["PhotoAngle1"].ToString() != "")
                    {

                        imagephotoangle = Inspdt.Rows[0]["PhotoAngle1"].ToString();
                        if (imagephotoangle == "-90")
                        {
                            imagephotoangle = "-270";
                        }
                        else if (imagephotoangle == "-270")
                        {
                            imagephotoangle = "-90";
                        }

                        else if (imagephotoangle == "90")
                        {
                            imagephotoangle = "270";
                        }

                        else if (imagephotoangle == "270")
                        {
                            imagephotoangle = "90";
                        }
                        else
                        {
                            imagephotoangle = Inspdt.Rows[0]["PhotoAngle1"].ToString();
                        }

                    }
                    else
                    {
                        imagephotoangle = "0";
                    }

                    img.ScaleAbsolute(180f, 133f);
                    img.Rotation = (float)Math.PI / 2;

                    var imagesangle = Convert.ToString(imagephotoangle);
                    img.RotationDegrees = Convert.ToSingle(imagephotoangle);

                    //img.setRotationDegrees(90);
                    // end

                    // Added on 28-July-2022 start here
                    if (img1.PropertyIdList.Contains(0x0112))
                    {
                        PropertyItem propOrientation = img1.GetPropertyItem(0x0112);
                        short orientation = BitConverter.ToInt16(propOrientation.Value, 0);
                        if (orientation == 6)
                        {
                            img1.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        }
                        else if (orientation == 8)
                        {
                            img1.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        }
                        else if (orientation == 3)
                        {
                            img1.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        }
                        else
                        {
                            // Do nothing
                        }

                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img1, BaseColor.WHITE), true);
                    }
                    else
                    {
                        cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    }
                    // Added on 28-July-2022 end here

                    //cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true); //Commented on (28-July-2022)
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.PaddingLeft = 1f;
                    cell.PaddingBottom = 1f;
                    cell.PaddingRight = 100f;
                    cell.PaddingTop = 1f;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                }
            }
            else
            {
                //string nophoto = Server.MapPath("images/noimg.jpg");
                string nophoto = System.Web.HttpContext.Current.Server.MapPath("~/images/noimg.jpg");

                // cell = new PdfPCell(iTextSharp.text.Image.GetInstance(nophoto), true);

                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.FixedHeight = 30f;

                cell.Colspan = 4;
                cell.PaddingLeft = 10f;
                cell.PaddingBottom = 10f;
                cell.PaddingRight = 150f;
                cell.PaddingTop = 10f;
                cell.BorderColor = BaseColor.GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            }
            cell.Border = 0;
            cell.Colspan = 4;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.BorderColor = BaseColor.GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            //End New 

            //Image img1 = Image.GetInstance(databaseCon.siteimagelink + Inspdt.Rows[0]["Inspimage"].ToString());
            //img1.ScaleAbsolute(159f, 159f);

            //PdfPTable table = new PdfPTable(1);
            //table.AddCell(img1);
            //doc.Add(table);


            // Code Changes On 1 August 2019  

            //phrase = new Phrase();
            //phrase.Add(new Chunk("Dictation", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.FixedHeight = 30f;
            //cell.BorderColor = BaseColor.GRAY;
            //cell.BackgroundColor = new iTextSharp.text.Color(220, 220, 220);
            //cell.Border = 0;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //headertable1.AddCell(cell);

            //if (Inspdt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < Inspdt.Rows.Count; i++)
            //    {
            //        if (!string.IsNullOrEmpty(Convert.ToString(Inspdt.Rows[i]["InspAudio"].ToString())))
            //        {
            //            phrase = new Phrase();
            //            phrase.Add(new Chunk("Yes", Heading786));
            //            cell = new PdfPCell(phrase);
            //        }
            //        else
            //        {
            //            phrase = new Phrase();
            //            phrase.Add(new Chunk("No", Heading786));
            //            cell = new PdfPCell(phrase);
            //        }
            //        cell.Border = 0;
            //        cell.BorderColor = BaseColor.GRAY;
            //        cell.FixedHeight = 1f;
            //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //        cell.Colspan = 4;
            //        headertable1.AddCell(cell);


            //    }
            //}
            //else
            //{
            //    phrase = new Phrase();
            //    phrase.Add(new Chunk("", DataFont));
            //    cell = new PdfPCell(phrase);
            //    cell.Colspan = 4;
            //    cell.BorderColor = BaseColor.GRAY;
            //    cell.PaddingBottom = 6F;
            //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    headertable1.AddCell(cell);
            //}


            //phrase = new Phrase();
            //phrase.Add(new Chunk("Video", Heading786));
            //cell = new PdfPCell(phrase);
            //cell.Colspan = 1;
            //cell.FixedHeight = 30f;
            //cell.BackgroundColor = new iTextSharp.text.Color(220, 220, 220);
            //cell.Border = 0;
            //cell.BorderColor = BaseColor.GRAY;
            //cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //headertable1.AddCell(cell);


            //if (Inspdt.Rows.Count > 0)
            //{
            //    for (int i = 0; i < Inspdt.Rows.Count; i++)
            //    {

            //        if (!string.IsNullOrEmpty(Convert.ToString(Inspdt.Rows[i]["Inspvedio"].ToString())))
            //        {
            //            phrase = new Phrase();
            //            phrase.Add(new Chunk("Yes", Heading786));
            //            cell = new PdfPCell(phrase);
            //        }
            //        else
            //        {
            //            phrase = new Phrase();
            //            phrase.Add(new Chunk("No", Heading786));
            //            cell = new PdfPCell(phrase);
            //        }
            //        cell.Border = 0;
            //        cell.BorderColor = BaseColor.GRAY;
            //        cell.FixedHeight = 1f;
            //        cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //        cell.Colspan = 4;
            //        headertable1.AddCell(cell);


            //    }
            //}
            //else
            //{
            //    phrase = new Phrase();
            //    phrase.Add(new Chunk("", DataFont));
            //    cell = new PdfPCell(phrase);
            //    cell.Colspan = 4;
            //    cell.PaddingBottom = 6F;
            //    cell.BorderColor = BaseColor.GRAY;
            //    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
            //    headertable1.AddCell(cell);
            //}

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.Border = 0;
            cell.FixedHeight = 30f;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Inspection List", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Yes", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.FixedHeight = 20f;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.BorderColor = BaseColor.GRAY;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("No", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.BorderColor = BaseColor.GRAY;
            cell.FixedHeight = 20f;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("NA", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Action", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.FixedHeight = 20f;
            cell.BorderColor = BaseColor.GRAY;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);

            if (checkdt.Rows.Count > 0)
            {
                for (int i = 0; i < checkdt.Rows.Count; i++)
                {
                    if (checkdt.Rows[i]["CategoryType"].ToString() == "Category")
                    {
                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(checkdt.Rows[i]["CheckListContent"].ToString()), Heading788));
                        cell = new PdfPCell(phrase);
                        cell.PaddingTop = 15f;
                        cell.PaddingBottom = 5f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.BackgroundColor = BaseColor.WHITE;
                        cell.Border = 1;
                        cell.Colspan = 5;
                        cell.BorderColor = BaseColor.GRAY;
                        cell.Border = PdfPCell.BOTTOM_BORDER;
                        headertable1.AddCell(cell);
                    }

                    else
                    {

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(checkdt.Rows[i]["CheckListContent"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220); ;
                        cell.Border = 0;
                        cell.Colspan = 1;
                        cell.BorderColor = BaseColor.GRAY;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);


                        if (string.IsNullOrEmpty(Convert.ToString(checkdt.Rows[i]["Y"])))
                        {
                            phrase = new Phrase();
                            phrase.Add(new Chunk("", contentfontCheck));
                            cell = new PdfPCell(phrase);
                        }
                        else
                        {
                            if (Convert.ToBoolean(checkdt.Rows[i]["Y"]) == true)
                            {
                                phrase = new Phrase();
                                phrase.Add(new Chunk("\xFE", contentfontCheckYes));
                                cell = new PdfPCell(phrase);

                            }
                            else
                            {
                                phrase = new Phrase();
                                phrase.Add(new Chunk("\x6F", contentfontUncheck));
                                cell = new PdfPCell(phrase);
                            }
                        }
                        cell.Border = 0;
                        cell.BorderColor = BaseColor.GRAY;
                        cell.FixedHeight = 1f;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell.Colspan = 1;
                        headertable1.AddCell(cell);

                        if (string.IsNullOrEmpty(Convert.ToString(checkdt.Rows[i]["N"])))
                        {
                            phrase = new Phrase();
                            phrase.Add(new Chunk("", contentfontCheck));
                            cell = new PdfPCell(phrase);

                        }
                        else
                        {
                            if (Convert.ToBoolean(checkdt.Rows[i]["N"]) == true)
                            {
                                phrase = new Phrase();
                                phrase.Add(new Chunk("\xFE", contentfontCheckNo));
                                cell = new PdfPCell(phrase);
                            }
                            else
                            {
                                phrase = new Phrase();
                                phrase.Add(new Chunk("\x6F", contentfontUncheck));
                                cell = new PdfPCell(phrase);
                            }
                        }
                        cell.Border = 0;
                        cell.BorderColor = BaseColor.GRAY;
                        cell.FixedHeight = 1f;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell.Colspan = 1;
                        headertable1.AddCell(cell);

                        if (string.IsNullOrEmpty(Convert.ToString(checkdt.Rows[i]["NA"])))
                        {
                            phrase = new Phrase();
                            phrase.Add(new Chunk("", contentfontCheck));
                            cell = new PdfPCell(phrase);
                        }
                        else
                        {
                            if (Convert.ToBoolean(checkdt.Rows[i]["NA"]) == true)
                            {
                                phrase = new Phrase();
                                phrase.Add(new Chunk("\xFE", contentfontCheck));
                                cell = new PdfPCell(phrase);
                            }
                            else
                            {
                                phrase = new Phrase();
                                phrase.Add(new Chunk("\x6F", contentfontUncheck));
                                cell = new PdfPCell(phrase);
                            }
                        }
                        cell.Border = 0;
                        cell.BorderColor = BaseColor.GRAY;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.FixedHeight = 1f;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER;
                        cell.Colspan = 1;
                        headertable1.AddCell(cell);

                        phrase = new Phrase();
                        phrase.Add(new Chunk(Convert.ToString(checkdt.Rows[i]["ActionRequired"].ToString()), DataFont));
                        cell = new PdfPCell(phrase);
                        cell.PaddingBottom = 15f;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Border = 1;
                        cell.Colspan = 1;
                        cell.BorderColor = BaseColor.GRAY;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        headertable1.AddCell(cell);
                    }
                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 1;
                cell.Border = 5;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);

            }

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Sign Off", Heading788));
            cell = new PdfPCell(phrase);
            cell.Colspan = 5;
            cell.FixedHeight = 20f;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            headertable1.AddCell(cell);

            phrase = new Phrase();
            phrase.Add(new Chunk("Comments", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.FixedHeight = 30f;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);

            if (Inspdt.Rows.Count > 0)
            {
                for (int i = 0; i < Inspdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(Inspdt.Rows[i]["InspComments"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            //phrase.Add(new Chunk("Acknowledgment", Heading786));
            phrase.Add(new Chunk("Acknowledgement", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.FixedHeight = 30f;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);

            if (Inspdt.Rows.Count > 0)
            {
                for (int i = 0; i < Inspdt.Rows.Count; i++)
                {

                    if (!string.IsNullOrEmpty(Convert.ToString(Inspdt.Rows[i]["Acknowledgment"].ToString())))
                    {
                        phrase = new Phrase();
                        phrase.Add(new Chunk("This scaffold has been inspected.", Heading786));
                        phrase.Add(Environment.NewLine);
                        phrase.Add(Environment.NewLine);
                        phrase.Add(new Chunk("I acknowledge that the above information is complete and accurate.", Heading786));
                        phrase.Add(Environment.NewLine);
                        phrase.Add(Environment.NewLine);

                        phrase.Add(new Chunk("\xFE " + "", contentfontCheck));
                        phrase.Add(new Chunk(" I Agree", Heading786));
                        cell = new PdfPCell(phrase);
                        cell.Colspan = 4;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        cell.BorderColor = BaseColor.GRAY;
                        cell.BackgroundColor = new iTextSharp.text.BaseColor(176, 255, 175);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        headertable1.AddCell(cell);
                    }
                    else
                    {
                        phrase = new Phrase();
                        phrase.Add(new Chunk("This scaffold has been inspected.", Heading786));
                        phrase.Add(Environment.NewLine);
                        phrase.Add(Environment.NewLine);
                        phrase.Add(new Chunk("I acknowledge that the above information is complete and accurate.", Heading786));
                        phrase.Add(Environment.NewLine);
                        phrase.Add(Environment.NewLine);
                        phrase.Add(new Chunk("\x6F" + " ", contentfontUncheck));
                        phrase.Add(new Chunk("I Agree", Heading786));
                        cell = new PdfPCell(phrase);
                        cell.Colspan = 4;
                        cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                        cell.BorderColor = BaseColor.GRAY;
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        headertable1.AddCell(cell);
                    }
                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.PaddingBottom = 6F;
                cell.BorderColor = BaseColor.GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            //phrase.Add(new Chunk("Acknowledgment Date", Heading786));
            phrase.Add(new Chunk("Acknowledgement Date", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.FixedHeight = 30f;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);

            if (Inspdt.Rows.Count > 0)
            {
                for (int i = 0; i < Inspdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    // phrase.Add(new Chunk(Convert.ToString(Inspdt.Rows[i]["AckDate"].ToString()), DataFont));
                    //  phrase.Add(new Chunk(string.Format("{0:dd-MMM-yyyy}", Inspdt.Rows[i]["AckDate"]), DataFont));
                    // Add Check On 27 Jan 2020 When Inspection Time is 12 Am then Show only DAte otherwiuse DateTime
                    if (Convert.ToInt32(Inspdt.Rows[i]["AckDateTime"]) == 0)
                    {
                        phrase.Add(new Chunk(string.Format("{0:dd-MMM-yyyy}", Inspdt.Rows[i]["AckDate"]), DataFont));
                    }
                    else
                    {
                        phrase.Add(new Chunk(string.Format("{0:dd-MMM-yyyy hh:mm tt}", Inspdt.Rows[i]["AckDate"]), DataFont));
                    }
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.PaddingBottom = 6F;
                cell.BorderColor = BaseColor.GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Inspector Name", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.FixedHeight = 30f;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (Inspdt.Rows.Count > 0)
            {
                for (int i = 0; i < Inspdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(Inspdt.Rows[i]["InspectedBy"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.BorderColor = BaseColor.GRAY;
                cell.PaddingBottom = 6F;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Licence to Perform High Risk Work #", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.FixedHeight = 30f;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.BorderColor = BaseColor.GRAY;
            cell.Border = 0;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);

            if (Inspdt.Rows.Count > 0)
            {
                for (int i = 0; i < Inspdt.Rows.Count; i++)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(Convert.ToString(Inspdt.Rows[i]["LicenceNo"].ToString()), DataFont));
                    cell = new PdfPCell(phrase);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                    headertable1.AddCell(cell);

                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", DataFont));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.PaddingBottom = 6F;
                cell.BorderColor = BaseColor.GRAY;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                headertable1.AddCell(cell);
            }

            phrase = new Phrase();
            phrase.Add(new Chunk("Inspector Signature", Heading786));
            cell = new PdfPCell(phrase);
            cell.Colspan = 1;
            cell.FixedHeight = 30f;
            cell.BackgroundColor = new iTextSharp.text.BaseColor(220, 220, 220);
            cell.Border = 0;
            cell.BorderColor = BaseColor.GRAY;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            if (!string.IsNullOrEmpty(Convert.ToString(Inspdt.Rows[0]["InspSign"])))
            {
                var uri = new Uri(databaseCon.siteimagelink);
                var container = new CloudBlobContainer(uri);
                var blob = container.GetBlockBlobReference(Inspdt.Rows[0]["InspSign"].ToString());
                //var noimage = Server.MapPath("images/noimg.jpg");
                var exists = blob.Exists();
                if (exists == false)
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("", Heading786));
                    cell = new PdfPCell(phrase);
                    cell.Colspan = 4;
                    cell.FixedHeight = 150f;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;

                }
                else
                {
                    iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(databaseCon.siteimagelink + Inspdt.Rows[0]["InspSign"].ToString());
                    img.ScaleAbsolute(180f, 133f);

                    cell = new PdfPCell(iTextSharp.text.Image.GetInstance(img), true);
                    cell.HorizontalAlignment = Element.ALIGN_LEFT;
                    cell.BorderColor = BaseColor.GRAY;
                    cell.PaddingLeft = 1f;
                    cell.PaddingBottom = 1f;
                    cell.PaddingRight = 100f;
                    cell.PaddingTop = 1f;
                    cell.Colspan = 4;
                    cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
                }
            }
            else
            {
                phrase = new Phrase();
                phrase.Add(new Chunk("", Heading786));
                cell = new PdfPCell(phrase);
                cell.Colspan = 4;
                cell.FixedHeight = 150f;
                cell.BorderColor = BaseColor.GRAY;
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;

            }
            cell.Border = 0;
            cell.Colspan = 4;
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.FixedHeight = 150f;
            cell.BorderColor = BaseColor.GRAY;
            cell.Border = PdfPCell.TOP_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER;
            headertable1.AddCell(cell);


            doc.Add(headertable2);
            doc.Add(headertable1);
            doc.Close();

            return FileName + '|' + PdfFileFullPath;
        }

        //Commented on 20-05-2023 We added a common function for redirecting pages and this function was throwing error. It does not have any significance as of now so we commented it.
        //internal static string Decrypt(object clientId)
        //{
        //    throw new NotImplementedException();
        //}
        public static string RedirectingCommonLink(int id, string userID, string RoleNumber, bool? HideMainDepotInventory = false)
        {
            var ClientID = id;
            var url = "";
            string Cid = databaseCon.Decrypt((ClientID).ToString());

            if (databaseCon.Decrypt(RoleNumber) == "10")
            {
                url = "DashboardHere/DashboardHere";
                return url;

                // Response.Redirect("/DashboardHere.aspx?UID=" + databaseCon.Encrypt(userID) + "&UserRole=" + databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()));
            }
            else if ((HideMainDepotInventory == true) && (databaseCon.Decrypt(RoleNumber) == "55"))
            {
                url = "MainDepotInventory/MainDepotInventory";
                return url;
            }
            else if (databaseCon.Decrypt(RoleNumber) == "55")
            {
                //return RedirectToAction("MainDepotInventory", "MainDepotInventory", new { UID = databaseCon.Encrypt(userID), UserRole = databaseCon.Encrypt(dt.Rows[0]["Rolenumber"].ToString()), id = databaseCon.Encrypt(ClientID.ToString()) });

                url = "SiteInventory/SiteInventory";
                return url;
            }
            else if (databaseCon.Decrypt(RoleNumber) == "95")
            {
                url = "OffSystemInspection/OffSystemInspection";
                return url;
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
                url = "RegisterList/RegisterList";
                return url;

                //}
            }

        }

        public static bool CopyAndRenameFileOnBlob(string sourceFilename, string newFileName, string storageName)
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = null;

                if (storageName == "Site-images")
                {
                    container = blobClient.GetContainerReference("siteimage");
                }
                else if (storageName == "Inspection-Document")
                {
                    container = blobClient.GetContainerReference("registerdocument");
                }
                else if (storageName == "Client-img")
                {
                    container = blobClient.GetContainerReference("clientimage");
                }
                else
                {
                    // Handle invalid storage name
                    Console.WriteLine("Invalid storage name.");
                    return false;
                }

                // Get the reference to the source blob
                CloudBlockBlob sourceBlob = container.GetBlockBlobReference(sourceFilename);

                // Check if the source blob exists
                if (!sourceBlob.Exists())
                {
                    Console.WriteLine("Source file not found in the container.");
                    return false;
                }



                // Get the reference to the destination blob
                CloudBlockBlob destinationBlob = container.GetBlockBlobReference(newFileName);

                // Copy the source blob to the destination blob
                destinationBlob.StartCopy(sourceBlob.Uri);
                destinationBlob.FetchAttributes(); // Fetch the attributes to ensure the copy is completed

                // Optionally, you can delete the original blob
                // sourceBlob.Delete();

                Console.WriteLine("File copied and renamed successfully. Copied Blob URI: " + destinationBlob.Uri);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return false;
            }
        }

        public static bool BlobFileExists(string fileName, string containerName)
        {
            CloudStorageAccount storageaccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            CloudBlobClient blobclient = storageaccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobclient.GetContainerReference(containerName);
            CloudBlockBlob blockblob = container.GetBlockBlobReference(fileName);

            return blockblob.Exists();
        }

        //public static void SetClientLogoPaths(this Controller controller)
        //{
        //    string clientId = null;

        //    // Get client ID from query string
        //    if (controller.Request.QueryString["id"] != null)
        //    {
        //        clientId = Decrypt(controller.Request.QueryString["id"]);
        //    }

        //    // If no client ID, use default logo
        //    if (string.IsNullOrEmpty(clientId))
        //    {
        //        controller.ViewBag.ClientLogo = "/images/logo.png?v=2";
        //        return;
        //    }

        //    // Fetch client image from database
        //    string query = "SELECT ClientImage FROM tblClient WHERE ClientId = '" + clientId + "'";
        //    DataTable dt = GetData(query);

        //    if (dt.Rows.Count > 0 && !string.IsNullOrEmpty(dt.Rows[0]["ClientImage"].ToString()))
        //    {
        //        string logoPath = "/images/clientlogo/" + dt.Rows[0]["ClientImage"].ToString();
        //        controller.ViewBag.ClientLogo = logoPath + "?v=2";
        //    }
        //    else
        //    {
        //        controller.ViewBag.ClientLogo = "/images/logo.png?v=2";
        //    }
        //}
    }
}