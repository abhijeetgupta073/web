using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Scaffold_DataAccess;
using System.Text.RegularExpressions;

namespace Scaffold.Controllers
    {
    public class ComparisonDashboardReportController : Controller
        {
        // GET: ComparisonDashboardReport
        public ActionResult Index()
            {
            return View();
            }


        public static System.Drawing.Image Base64ToImage(string base64String)
            {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
                {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);

                return image;
                }
            }

        public void ComparisonDashboardReport()
            {
            try
                {
                string Page1Src = Session["Page1base64"].ToString();
                string Page2Src = Session["Page2base64"].ToString();
                
                Session["Page1base64"] = null;
                Session["Page2base64"] = null;



                string base64Data = "";
                //Create .NET Image object, please make sure it is not iTextSharp Image object
                System.Drawing.Image Page1Image = null;
                System.Drawing.Image Page2Image = null;
                
                
                if ((!string.IsNullOrEmpty(Page1Src)) && (Page1Src != "undefined"))
                    {
                    base64Data = Regex.Match(Page1Src, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                    Page1Image = Base64ToImage(base64Data);
                    }

                if ((!string.IsNullOrEmpty(Page2Src)) && (Page2Src != "undefined"))
                    {
                    base64Data = Regex.Match(Page2Src, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                    Page2Image = Base64ToImage(base64Data);
                    }


                Document doc = new Document(iTextSharp.text.PageSize.A4, -40f, -40f, 10f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
                doc.Open();
                Response.ContentType = ("Application/pdf");
                
                
                iTextSharp.text.Image Page1Img = iTextSharp.text.Image.GetInstance(Page1Image, System.Drawing.Imaging.ImageFormat.Jpeg);
                Page1Img.SetAbsolutePosition(0, 0);
                Page1Img.ScaleAbsolute(iTextSharp.text.PageSize.A4.Width-5f, iTextSharp.text.PageSize.A4.Height);

                iTextSharp.text.Image Page2Img = iTextSharp.text.Image.GetInstance(Page2Image, System.Drawing.Imaging.ImageFormat.Jpeg);
                Page2Img.SetAbsolutePosition(0, 0);
                Page2Img.ScaleAbsolute(iTextSharp.text.PageSize.A4.Width-5f, iTextSharp.text.PageSize.A4.Height);

                doc.Add(Page1Img);
                doc.NewPage();
                doc.Add(Page2Img);
                
                doc.Close();
                }
            catch (Exception ex)
                {

                }
            

            }


        }
    }