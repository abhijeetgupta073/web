using Microsoft.WindowsAzure.Storage.Blob;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using Scaffold.App_Code;
using Scaffold_DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scaffold.Controllers
{
    public class StandingVsAvailablebyMaterialTypeController : Controller
    {
        ScaffoldEntities dbContext = new ScaffoldEntities();
        // GET: StandingVsAvailablebyMaterialType
        public ActionResult StandingVsAvailablebyMaterialType()
        {
            return View();
        }

        // Method to get material data for both graphs and datatables
        [HttpPost]
        public JsonResult GetMaterialData(int clientid, int Sitelocid, string Type, string MaterialType = "ALL")
        {
            List<MaterialData> result = new List<MaterialData>();
            string con = null;
            try
            {
                con = ConfigurationManager.ConnectionStrings["connectionstring"].ConnectionString;
                using (SqlConnection sqlCon = new SqlConnection(con))
                {
                    using (SqlCommand cmd = new SqlCommand("pro_StandingVsAvailablebyMatType", sqlCon))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@clientid", clientid);
                        cmd.Parameters.AddWithValue("@Sitelocid", Sitelocid);
                        cmd.Parameters.AddWithValue("@Type", Type);
                        cmd.Parameters.AddWithValue("@MaterialType", MaterialType);

                        sqlCon.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            if (Type == "Standing")
                            {
                                while (reader.Read())
                                {
                                    result.Add(new MaterialData
                                    {
                                        MaterialType = reader["MaterialType"].ToString(),
                                        TonnageStanding = Convert.ToDecimal(reader["TonnageStanding"])
                                    });
                                }
                            }
                            else if (Type == "Available")
                            {
                                while (reader.Read())
                                {
                                    result.Add(new MaterialData
                                    {
                                        MaterialType = reader["MaterialType"].ToString(),
                                        Tonnageavailable = Convert.ToDecimal(reader["Tonnageavailable"])
                                    });
                                }
                            }
                            else if (Type == "StandingInfo")
                            {
                                while (reader.Read())
                                {
                                    result.Add(new MaterialData
                                    {
                                        MaterialType = reader["MaterialType"].ToString(),
                                        ItemCode = reader["ItemCode"].ToString(),
                                        ItemDesc = reader["ItemDesc"].ToString(),
                                        AvailableQuantity = Convert.ToInt32(reader["AvailableQuantity"]),
                                        ItemWeight = Convert.ToDecimal(reader["ItemWeight"]),
                                        TonnageStanding = Convert.ToDecimal(reader["TonnageStanding"])
                                    });
                                }
                            }
                            else if (Type == "AvailableInfo")
                            {
                                while (reader.Read())
                                {
                                    result.Add(new MaterialData
                                    {
                                        MaterialType = reader["MaterialType"].ToString(),
                                        ItemCode = reader["ItemCode"].ToString(),
                                        ItemDesc = reader["ItemDesc"].ToString(),
                                        AvailableQuantity = Convert.ToInt32(reader["AvailableQuantity"]),
                                        ItemWeight = Convert.ToDecimal(reader["ItemWeight"]),
                                        Tonnageavailable = Convert.ToDecimal(reader["Tonnageavailable"])
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error
                return Json(new { success = false, message = ex.Message });
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
    // Material Data Model
    public class MaterialData
    {
        public string MaterialType { get; set; }
        public decimal TonnageStanding { get; set; }
        public decimal Tonnageavailable { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public int AvailableQuantity { get; set; }
        public decimal ItemWeight { get; set; }
    }
}
