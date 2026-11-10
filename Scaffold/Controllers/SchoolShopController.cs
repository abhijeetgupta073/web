using AutoMapper;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Warwick_Academy.App_Code;
using Warwick_Academy.Models;
using Warwick_DataAccess;
using Warwick_Academy.Controllers;
using Newtonsoft.Json;
using AuthorizeNet.APICore;
using System.Collections;

namespace Warwick_Academy.Controllers
{
    public class SchoolShopController : Controller
    {
        WarwickAcademyEntities dbcontext = new WarwickAcademyEntities();
        // GET: SchoolShop
        public ActionResult SchoolShop(string EnclgnuserID)
        {
            var DecuserID = DataBaseCon.Decrypt(EnclgnuserID);
            ViewBag.EnclgnuserID = EnclgnuserID;

            if (ViewBag.EnclgnuserID != null && ViewBag.EnclgnuserID != "")
            {
                ViewBag.UserRole = "";
                ViewBag.UserCode = "";
                ViewBag.CheckUserCode = DataBaseCon.Decrypt(EnclgnuserID);
                var id = DataBaseCon.Decrypt(EnclgnuserID);
                if (Convert.ToInt32(id) < 17000)
                {
                   // var teacherdata = dbcontext.Teachers.Where(_ => _.TeacherCode == id).FirstOrDefault();
                  
                    var teacherdata = dbcontext.Teachers.Where(_ => _.TeacherCode == id && _.isArchived != true).FirstOrDefault();
                    ViewBag.UserRole = teacherdata.Role;
                    ViewBag.UserCode = teacherdata.TeacherCode;
                }
                else
                {
                    var Studentdata = dbcontext.Students.Where(_ => _.StudentCode == id && _.IsArchived == false).FirstOrDefault();
                    ViewBag.UserRole = "";
                    ViewBag.UserCode = Studentdata.StudentCode;
                }
                //ViewBag.DecUserID = DecuserID;
                return View();
            }
            else
            {
                //return RedirectToAction("Login", "Login");
                return View();

            }

        }
        public ActionResult GetShopData(string ProductCategory)
        {
            //var GetPrimaryShopData = dbcontext.Database.SqlQuery<Vw_TblShopProduct>("select * from Vw_TblShopProduct where ProductCategory ='" + ProductCategory + "'").ToList();
            
            //var GetPrimaryShopData = dbcontext.Database.SqlQuery<Vw_TblShopProduct>("select * from Vw_TblShopProduct where IsArchive != 1 and ProductCategory ='" + ProductCategory + "'").ToList();
            var GetPrimaryShopData = dbcontext.Database.SqlQuery<Vw_TblShopProduct>("SELECT ShopProductID, ProductName, Description1, Description2, ImagePath, ProductCategory, SizeCount, size,SalePrice,IsArchive FROM Vw_TblShopProduct where IsArchive != 1 and ProductCategory ='" + ProductCategory + "'").ToList();
            //var GetPrimaryShopData = dbcontext.TblShopProducts.Where(_=>_.ProductCategory == "Primary").ToList();
            //var response = new { GetPrimaryShopData = GetPrimaryShopData };
            return Json(GetPrimaryShopData, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSizeSelect(int ShopProductID)
        {
            //var GetSizeSelect = dbcontext.TblShopProductInventories.Where(_ => _.ShopProductID == ShopProductID).ToList();
            //var query = "Select * from Vw_TblShopProductInventory where ShopProductID=" + ShopProductID + "";
            var query = "Select * from Vw_TblShopProductInventory where IsArchive != 1    and ShopProductID=" + ShopProductID + "";
            var GetSizeSelect = dbcontext.Database.SqlQuery<Vw_TblShopProductInventory>(query).ToList();
            return Json(GetSizeSelect, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExistingOrderData(string studentCode , string StudentEmaiId)
        {
            string query = "";
            try
            {
                //var GetExistingOrderData = dbcontext.Vw_ShopProductCart.Where(_ => _.StudentCode == studentCode).ToList();
                var OrderStatusValue = "Pending";
                //var query = "Select * from Vw_ShopProductCart where StudentCode=" + studentCode + " and OrderStatus = '"+OrderStatusValue+"'"; // commented on 17-08-2023  for new admition 
                if (studentCode != "")
                {
                    query = "Select * from Vw_ShopProductCart where StudentCode=" + studentCode + " and OrderStatus = '" + OrderStatusValue + "'";
                }
                else
                {
                    query = "Select * from Vw_ShopProductCart where (StudentCode= '" + 0 + "' or  Email= '" + StudentEmaiId + "') and OrderStatus = '" + OrderStatusValue + "'";
                }
                var GetExistingOrderData=dbcontext.Database.SqlQuery<Vw_ShopProductCart>(query).ToList();
                return Json(GetExistingOrderData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ee)
            {
                return Json("");
            }
        }
        public ActionResult PrimaryAddToCart(int ShopProductID, decimal Quantity, string Size, decimal SalePrice, string studentCode, string StudentEmaiId)
        {
            StudentEmaiId = string.IsNullOrEmpty(StudentEmaiId) ? null : StudentEmaiId;

            var Result = "";
            var Message = "";
            int GetProductSizeId = 0;
            if ( Size != null)
            {
                var GetProductSize = dbcontext.TblShopProductInventories.Where(_ => _.ShopProductID == ShopProductID && _.Size == Size).FirstOrDefault();
                GetProductSizeId = GetProductSize.ShopProductInventoryID;
            }
            else
            {
                var GetProductSize = dbcontext.TblShopProductInventories.Where(_ => _.ShopProductID == ShopProductID).FirstOrDefault();
                GetProductSizeId = GetProductSize.ShopProductInventoryID;
            }

           

            //var GetCartdata = dbcontext.ShopProductCarts.Where(_ => _.StudentCode == studentCode && _.ShopProductInventoryID == GetProductSizeId).FirstOrDefault(); comment on  17-08-2023 for new admission
            //var GetCartdata = dbcontext.ShopProductCarts.Where(_ => (_.StudentCode == studentCode || _.Email == StudentEmaiId)  && _.ShopProductInventoryID == GetProductSizeId).FirstOrDefault();
            var GetCartdata = dbcontext.ShopProductCarts
            .Where(_ => (((_.StudentCode == (string.IsNullOrEmpty(studentCode) ? "0" : studentCode)) || _.Email == (string.IsNullOrEmpty(StudentEmaiId) ? "" : StudentEmaiId))) && _.ShopProductInventoryID == GetProductSizeId && _.OrderStatus == "Pending")
            .FirstOrDefault();

            if (GetCartdata != null)
            {
                if (GetCartdata.OrderStatus != "Success")
                {
                    GetCartdata.OrderQuantity = Quantity;
                    GetCartdata.ProductSalePrice = SalePrice;

                    GetCartdata.OrderStatus = "Pending";
                    GetCartdata.PaymentDate = null;
                    GetCartdata.PaymentMode = null;
                    GetCartdata.UpdatedOn = DateTime.Now;
                    dbcontext.SaveChanges();
                    Result = "Success";
                    Message = "Data Updated successfully";
                }
                else
                {
                    ShopProductCart cartData = new ShopProductCart();
                    cartData.ShopProductID = ShopProductID;
                    cartData.ShopProductInventoryID = GetProductSizeId;
                    cartData.OrderQuantity = Quantity;
                    cartData.ProductSize = Size;
                    //cartData.StudentCode = studentCode;// add  17-08-2023  for new addmission
                    cartData.StudentCode = string.IsNullOrEmpty(studentCode) ? null : studentCode;

                    cartData.Email = StudentEmaiId;// add  17-08-2023  for new addmission
                    cartData.ProductSalePrice = SalePrice;
                    cartData.OrderStatus = "Pending";
                    cartData.CreatedOn = DateTime.Now;
                    dbcontext.ShopProductCarts.Add(cartData);
                    dbcontext.SaveChanges();
                    Result = "Success";
                    Message = "Data Saved Successfully";
                }
            }
            else
            {
                ShopProductCart cartData = new ShopProductCart();
                cartData.ShopProductID = ShopProductID;
                cartData.ShopProductInventoryID = GetProductSizeId;
                cartData.OrderQuantity = Quantity;
                cartData.ProductSize = Size;
                //cartData.StudentCode = studentCode;// add  17-08-2023  for new addmission
                cartData.StudentCode = string.IsNullOrEmpty(studentCode) ? null : studentCode;

                cartData.Email = StudentEmaiId; // add  17-08-2023  for new addmission
                cartData.ProductSalePrice = SalePrice;
                cartData.OrderStatus = "Pending";
                cartData.CreatedOn = DateTime.Now;
                dbcontext.ShopProductCarts.Add(cartData);
                dbcontext.SaveChanges();
                Result = "Success";
                Message = "Data Saved Successfully";
            }
            var GetResponse = new { Result = Result, Message = Message };
            return Json(GetResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteCartProduct(int cartProductID)
        {
            var Result = "";
            var Message = "";
            var GetCartData = dbcontext.ShopProductCarts.Where(_ => _.CartProductID == cartProductID).FirstOrDefault();
            if (GetCartData != null)
            {
                DataBaseCon.RunCmd("Delete from ShopProductCart where CartProductID=" + cartProductID);
                Result = "Success";
                Message = "Data Deleted Successfully";
            }
            var GetResponse = new { Result = Result, Message = Message };
            return Json(GetResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetOrderSummaryData(string id , string StudentEmaiId)
        {
            try
            {
                //Comment By Prabhat 29Apr2023
                //var OrderSummaryData = dbcontext.Database.SqlQuery<TblOrderSummaryViewModel>("Select isnull(Sum(OrderQuantity),0) TotalItem, (Select isnull(Sum(ProductSalePrice),0) from Vw_ShopProductCart where ProductCategory='Primary' and StudentCode=" + id + ") PrimaryTotal,(Select isnull(Sum(ProductSalePrice),0) from Vw_ShopProductCart where ProductCategory='Secondary' and StudentCode=" + id + ") SecondaryTotal,(Select isnull(Sum(ProductSalePrice),0) from Vw_ShopProductCart where ProductCategory='Merchandise' and StudentCode=" + id + ") MerchandiseTotal from Vw_ShopProductCart  where StudentCode=" + id + "").FirstOrDefault();
                //string querytest = "Select isnull(Sum(OrderQuantity),0) TotalItem,isnull( Sum(case when ProductCategory='Primary' then   ProductSalePrice else 0 end),0) PrimaryTotal,isnull( Sum(case when ProductCategory='Secondary' then   ProductSalePrice else 0 end),0) SecondaryTotal,isnull( Sum(case when ProductCategory='Merchandise' then   ProductSalePrice else 0 end),0) MerchandiseTotal,  isnull(Sum(ProductSalePrice),0) OverallTotal from Vw_ShopProductCart  where StudentCode=" + id + " and OrderStatus='Pending'";

                //var OrderSummaryData = dbcontext.Database.SqlQuery<TblOrderSummaryViewModel>("Select isnull(Sum(OrderQuantity),0) TotalItem, (Select isnull(Sum(ProductSalePrice),0) from Vw_ShopProductCart where ProductCategory='Primary' and StudentCode=" + id + " and OrderStatus='Pending') PrimaryTotal,(Select isnull(Sum(ProductSalePrice),0) from Vw_ShopProductCart where ProductCategory='Secondary' and StudentCode=" + id + " and OrderStatus='Pending') SecondaryTotal,(Select isnull(Sum(ProductSalePrice),0) from Vw_ShopProductCart where ProductCategory='Merchandise' and StudentCode=" + id + " and OrderStatus='Pending') MerchandiseTotal from Vw_ShopProductCart  where StudentCode=" + id + " and OrderStatus='Pending'").FirstOrDefault();
                if(id == "")
                {
                    id = "0";
                }

                //var OrderSummaryData = dbcontext.Database.SqlQuery<TblOrderSummaryViewModel>("Select isnull(Sum(OrderQuantity),0) TotalItem,isnull( Sum(case when ProductCategory='Primary' then   ProductSalePrice else 0 end),0) PrimaryTotal,isnull( Sum(case when ProductCategory='Secondary' then   ProductSalePrice else 0 end),0) SecondaryTotal,isnull( Sum(case when ProductCategory='Merchandise' then   ProductSalePrice else 0 end),0) MerchandiseTotal,  isnull(Sum(ProductSalePrice),0) OverallTotal from Vw_ShopProductCart  where StudentCode=" + id + " and OrderStatus='Pending'").FirstOrDefault();
                string query = "Select isnull(Sum(OrderQuantity),0) TotalItem,isnull( Sum(case when ProductCategory='Primary' then   ProductSalePrice else 0 end),0) PrimaryTotal,isnull( Sum(case when ProductCategory='Secondary' then   ProductSalePrice else 0 end),0) SecondaryTotal,isnull( Sum(case when ProductCategory='Merchandise' then   ProductSalePrice else 0 end),0) MerchandiseTotal,  isnull(Sum(ProductSalePrice),0) OverallTotal from Vw_ShopProductCart  where (StudentCode=" + id + "  or Email = '" + StudentEmaiId + "' )and OrderStatus='Pending'";

                var OrderSummaryData = dbcontext.Database.SqlQuery<TblOrderSummaryViewModel>(query).FirstOrDefault();

                return Json(OrderSummaryData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ee)
            {
                return View();
            }

        }
        public ActionResult ExistingQtyUpdate(int CartProductID, decimal Quantity, string StudentCode, string StudentEmaiId)
        {
            var Result = "";
            var Message = "";
            var GetExistingCartData = dbcontext.ShopProductCarts.Where(_ => _.CartProductID == CartProductID).FirstOrDefault();
            var ShopProductInventoryData = dbcontext.Vw_TblShopProductInventory.Where(_ => _.ShopProductInventoryID == GetExistingCartData.ShopProductInventoryID).FirstOrDefault();

            if (GetExistingCartData != null)
            {
                GetExistingCartData.OrderQuantity = Quantity;
                GetExistingCartData.ProductSalePrice = Quantity * ShopProductInventoryData.SalePrice;
                GetExistingCartData.UpdatedOn = DateTime.Now;
                GetExistingCartData.UpdatedBy = StudentCode;
                dbcontext.SaveChanges();
                Result = "Success";
                Message = "Quantity Updated successfully";
            }
            var GetResponse = new { Result = Result, Message = Message };
            return Json(GetResponse, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetStudentDetails(string StudentCode, int CheckUserCode , string StudentEmaiId)
        {
            int id = Convert.ToInt32(StudentCode);
            var result = "";
            var checkHomeRomeyr = "";
            //ShopProductPaymentDetail CheckOrderPayment = new ShopProductPaymentDetail();
            StudentViewModel GetDetails = new StudentViewModel();
            TeacherViewModel GetTeacherDetail = new TeacherViewModel();

            //if (Convert.ToInt32(CheckUserCode) < 17000)
            if (Convert.ToInt32(StudentCode) < 17000)
            {
                //GetTeacherDetail = Mapper.Map<TeacherViewModel>(dbcontext.Teachers.Where(_ => _.TeacherCode == StudentCode).FirstOrDefault());
                GetTeacherDetail = Mapper.Map<TeacherViewModel>(dbcontext.Teachers.Where(_ => _.TeacherCode == StudentCode && _.isArchived != true && _.Role != "Visitor").FirstOrDefault());
                if (GetTeacherDetail != null)
                {
                    result = "Success";
                }
                else
                {
                    result = "Failed";
                }
            }
            else
            {
                GetDetails = Mapper.Map<StudentViewModel>(dbcontext.Students.Where(_ => _.StudentCode == StudentCode && _.IsArchived == false).FirstOrDefault());

                if (GetDetails != null)
                {
                    //CheckOrderPayment = dbcontext.ShopProductPaymentDetails.Where(_ => _.StudentID == id).ToList();
                    var getHomeRoom = dbcontext.Database.SqlQuery<HomeRoomYearViewModel>("select HomeroomYear from Homerooms where HomeroomID = " + GetDetails.StudentHomeroomID + "").FirstOrDefault();
                    if (getHomeRoom.HomeroomYear > 6)
                    {
                        checkHomeRomeyr = "GT";
                    }
                    else
                    {
                        checkHomeRomeyr = "LT";
                    }
                    result = "Success";
                }
                else
                {
                    result = "Failed";
                }
            }
            //if (GetDetails != null || GetTeacherDetail != null )
            //{
            //    CheckOrderPayment = dbcontext.OrderPaymentDetails.Where(_ => _.StudentID == id).FirstOrDefault();
            //    result = "Success";
            //}

            var data = new { Result = result, GetDetails = GetDetails, GetTeacherDetail = GetTeacherDetail, checkHomeRomeyr = checkHomeRomeyr };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult AddOrderPayment(ShopPaymentDetailsViewModel model, int[,]  ShopProductArr ,/* int[] ShopProductInventoryArr,*/ string UpdateValue, string lgnID)
        //{
        public ActionResult AddOrderPayment(ShopPaymentDetailsViewModel model, int[][] ShopProductArr,  string UpdateValue, string lgnID)
        {
            var result = "";
            var message = "";
            if (model != null)
            {
                if (ShopProductArr != null)
                {
                    for (var i = 0; i < ShopProductArr.Length; i++)
                    {
                        ShopProductPaymentDetail ShopProductPayment = new ShopProductPaymentDetail();
                        ShopProductPayment.CartProductID = ShopProductArr[i][0];

                        ShopProductPayment.ShopProductInventoryID = ShopProductArr[i][1];
                        ShopProductPayment.StudentID = model.StudentID;
                        ShopProductPayment.FullStudentName = model.FullStudentName;
                        ShopProductPayment.CCNumber = model.CCNumber;
                        ShopProductPayment.CCName = model.CCName;
                        ShopProductPayment.CCMonth = model.CCMonth;
                        ShopProductPayment.CCYear = model.CCYear;
                        ShopProductPayment.CCCVC = model.CCCVC;
                        ShopProductPayment.OrderStatus = "Success";
                        ShopProductPayment.HomeRoom = model.HomeRoom;
                        ShopProductPayment.ContactNumber = model.ContactNumber;
                        ShopProductPayment.EmailAddress = model.EmailAddress;
                        ShopProductPayment.Notes = model.Notes;
                        ShopProductPayment.PayCash = model.PayCash;
                        ShopProductPayment.PaySchoolAccount = model.PaySchoolAccount;
                        //ShopProductPayment.NumOfOrders = model.NumOfOrders;
                        ShopProductPayment.NumOfOrders = ShopProductArr[i][2];
                        ShopProductPayment.SalePrice = ShopProductArr[i][3];
                        if (ShopProductArr[i][4] == 1)
                        {
                            ShopProductPayment.pTotal = ShopProductArr[i][2] * ShopProductArr[i][3];
                        }
                        else if (ShopProductArr[i][4] == 2)
                        {
                            ShopProductPayment.sTotal = ShopProductArr[i][2] * ShopProductArr[i][3];
                        }
                        else
                        {
                            ShopProductPayment.mTotal = ShopProductArr[i][2] * ShopProductArr[i][3];
                        }
                        //ShopProductPayment.pTotal = model.pTotal;
                        //ShopProductPayment.sTotal = model.sTotal;
                        //ShopProductPayment.mTotal = ShopProductArr[i][2] * ShopProductArr[i][3];
                        ShopProductPayment.OrderDate = DateTime.Now;
                        dbcontext.ShopProductPaymentDetails.Add(ShopProductPayment);
                        dbcontext.SaveChanges();
                        //ShopProductCart cartData = new ShopProductCart();
                        //cartData.OrderStatus = "Success";
                        //dbcontext.SaveChanges();

                        //var CashcartData = dbcontext.Database.SqlQuery<ShopProductCartViewModel>("Select * from ShopProductCart where CartProductID=" + model.CartProductID + "").FirstOrDefault();
                        //if (CashcartData != null)
                        //{
                        if (model.PaySchoolAccount == true)
                        {
                            var OrderStatusValue = "Success";
                            var CurrentDateTime = DateTime.Now;
                            var PayByCash = "Pay By School Account";
                            DataBaseCon.RunCmd("update ShopProductCart Set OrderStatus = '" + OrderStatusValue + "',PaymentDate = '" + CurrentDateTime + "', PaymentMode = '" + PayByCash + "' where CartProductID=" + ShopProductArr[i][0]);
                        }
                        else
                        {
                            var OrderStatusValue = "Success";
                            var CurrentDateTime = DateTime.Now;
                            var PayByCash = "Pay By Cash";
                            DataBaseCon.RunCmd("update ShopProductCart Set OrderStatus = '" + OrderStatusValue + "',PaymentDate = '" + CurrentDateTime + "', PaymentMode = '" + PayByCash + "' where CartProductID=" + ShopProductArr[i][0]);
                        }
                            

                            
                        //}
                    }


                    result = "Success";
                    message = "Order Placed Successfully";

                }
            }
            else
            {
                result = "failed";
                message = "Please add Producd in your cart and try again";

            }
            var data = new { Result = result, Message = message };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ClearCartAfterSubmit(int[] ShopProductArr, string StudentID)
        {
            var result = "";
            var message = "";
            try
            {
                for (int i = 0; i < ShopProductArr.Length; i++)
                {
                    var cartData = dbcontext.Database.SqlQuery<ShopProductCart>("Select * from ShopProductCart where CartProductID=" + ShopProductArr[i] + "").FirstOrDefault();
                    //var cartData = dbcontext.ShopProductCarts.Where(_ => _.StudentCode == StudentID && _.CartProductID == ShopProductArr[i]).FirstOrDefault();
                    if (cartData != null)
                    {
                        DataBaseCon.RunCmd("Delete from ShopProductCart where CartProductID=" + ShopProductArr[i]);
                    }
                }
                result = "Success";
                message = "Deleted Successfully";
            }
            catch (Exception ex)
            {
                result = "Failed";
                message = "Internal error, Please Contact for details";
            }
            //var cartData=dbcontext.ShopProductCarts.Where(_=>_.CartProductID== ShopProductArr[i]&&_.StudentCode==id).FirstOrDefault();
            var data = new { Result = result, Message = message };
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //public ActionResult CreateAndAcceptPayment(String ApiLoginID, String ApiTransactionKey, decimal amount, string dataDescriptor, string dataValue, string studentCode)
        public ActionResult CreateAndAcceptPayment(AuthorizeNetRequestModel model)
        {

            //var EncuserID = DataBaseCon.Encrypt(model.studentCode);// COMENTED ON  19-08-2023 FOR REDIRECT ISSUE
            var EncuserID = model.EncuserID;
            
            //AuthorizeNetRequestModel model = new AuthorizeNetRequestModel();
            List<Items> items = new List<Items>();
            //Items item = new Items();
            //var Data = dbcontext.Vw_ShopProductCart.Where(_ => _.StudentCode == model.studentCode && _.OrderStatus == "Pending").ToList(); // commented on  18-08-2023 
            var studentCode = string.IsNullOrEmpty(model.studentCode) ? "0" : model.studentCode;
            var Data = dbcontext.Vw_ShopProductCart.Where(_ => (_.StudentCode == studentCode || _.Email == model.EmailAddress)&& _.OrderStatus == "Pending").ToList();

            if (Data != null && Data.Count > 0)
            {

                foreach (var itm in Data)
                {
                    Items item = new Items();

                    if (itm.OrderStatus != "Success")
                    {
                        item.itemId = itm.CartProductID;
                        item.name = itm.ProductName;
                        item.quantity = (decimal)itm.OrderQuantity;
                        item.unitPrice = (double)itm.ProductSalePrice;
                        item.SalePrice = (decimal)itm.SalePrice;
                        item.itemId = itm.CartProductID;
                        item.ShopProductInventoryID = itm.ShopProductInventoryID;
                        item.ProductCategory = itm.ProductCategory;
                        items.Add(item);
                    }
                }
                model.Items = items;
            }
            //model.ApiLoginID = ApiLoginID;
            //model.ApiTransactionKey = ApiTransactionKey;
            //model.amount = amount;
            //model.dataDescriptor = dataDescriptor;
            //model.dataValue = dataValue;
            //model.studentCode = studentCode;
            var response = PaymentTransactionController.Run(model);
           // var res = JsonConvert.DeserializeObject<transactionResponse>(response.ToString());
            //dynamic dynJson = JsonConvert.DeserializeObject(response.ToString());
            if (response.messages.resultCode == (AuthorizeNet.Api.Contracts.V1.messageTypeEnum)messageTypeEnum.Ok)
            {
                if (model.Items != null)
                {
                    for (var i = 0; i < model.Items.Count; i++)
                    {
                        ShopProductPaymentDetail ShopProductPayment = new ShopProductPaymentDetail();
                            ShopProductPayment.ShopProductInventoryID = model.Items[i].ShopProductInventoryID;
                            ShopProductPayment.CartProductID = model.Items[i].itemId;
                            //ShopProductPayment.StudentID = Convert.ToInt32(model.studentCode);
                        ShopProductPayment.StudentID = Convert.ToInt32(model.studentCode) == 0 ? (int?)null : Convert.ToInt32(model.studentCode);

                        ShopProductPayment.FullStudentName = model.fullName;
                            ShopProductPayment.CCNumber = model.CCNumber;
                            ShopProductPayment.CCName = model.CCName;
                            ShopProductPayment.CCMonth = model.CCMonth;
                            ShopProductPayment.CCYear = model.CCYear;
                            ShopProductPayment.CCCVC = model.CCCVC;
                            ShopProductPayment.OrderStatus = "Success";
                            ShopProductPayment.HomeRoom = model.HomeRoom;
                            ShopProductPayment.ContactNumber = model.ContactNumber;
                            ShopProductPayment.EmailAddress = model.EmailAddress;
                            ShopProductPayment.Notes = model.Notes;
                            ShopProductPayment.PayCash = false;
                            ShopProductPayment.PaySchoolAccount = false;
                            ShopProductPayment.NumOfOrders = Convert.ToInt32(model.Items[i].quantity);
                            ShopProductPayment.SalePrice = model.Items[i].SalePrice;
                            if (model.Items[i].ProductCategory == "Primary")
                            {
                                ShopProductPayment.pTotal = model.Items[i].unitPrice;
                            }
                            else if (model.Items[i].ProductCategory == "Secondary")
                            {
                                ShopProductPayment.sTotal = model.Items[i].unitPrice;
                            }
                            else if (model.Items[i].ProductCategory == "Merchandise")
                            {
                                ShopProductPayment.mTotal = model.Items[i].unitPrice;
                            }
                            ShopProductPayment.OrderDate = DateTime.Now;
                            dbcontext.ShopProductPaymentDetails.Add(ShopProductPayment);
                            dbcontext.SaveChanges();

                            var cartData = dbcontext.Database.SqlQuery<ShopProductCartViewModel>("Select * from ShopProductCart where CartProductID=" + model.Items[i].itemId + "").FirstOrDefault();
                            if (cartData != null)
                            {
                                var OrderStatusValue = "Success";
                                var CurrentDateTime = DateTime.Now;
                                var Creditcard = "Credit Card";
                                DataBaseCon.RunCmd("update ShopProductCart Set OrderStatus = '" + OrderStatusValue + "',PaymentDate = '" + CurrentDateTime + "', PaymentMode = '" + Creditcard + "' where CartProductID=" + model.Items[i].itemId);

                                //DataBaseCon.RunCmd("Delete from ShopProductCart where CartProductID=" + model.Items[i].itemId);
                            }
                    }
                }
                return RedirectToAction("SchoolShop", "SchoolShop", new { EnclgnuserID = EncuserID, sTs = "s" });
            }
            else
            {
                for (var i = 0; i < model.Items.Count; i++)
                {
                    var OrderStatusValue = "Failed";
                    var CurrentDateTime = DateTime.Now;
                    var Creditcard = "Credit Card";
                    DataBaseCon.RunCmd("update ShopProductCart Set OrderStatus = '" + OrderStatusValue + "',PaymentDate = '" + CurrentDateTime + "', PaymentMode = '" + Creditcard + "' where CartProductID=" + model.Items[i].itemId);
                }
                return RedirectToAction("SchoolShop", "SchoolShop", new { EnclgnuserID = EncuserID, sTs = "s" });
            }
        }
    }
    public class ShopProductCartViewModel
    {
        public string OrderStatus { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string PaymentMode { get; set; }
    }
}