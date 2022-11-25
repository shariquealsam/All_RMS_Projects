using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using RMS_Fleet.Models;
using System.Xml;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace RMS_Fleet.Controllers
{
    public class MaintenanceController : Controller
    {
        //
        // GET: /Maintenance/
        SalesReportData Sr = new SalesReportData();
        Maintenance Maintenance = new Maintenance();
        String SessionTimeoutStatus = null;

        public ActionResult Index()
        {
            GetRegion();
            if (SessionTimeoutStatus == "Session Expired")
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        string strMySqlConnectionString = "";
        string strMySqlConnectionStringBulk = "";
        string strSqlConnectionString = "";
        string strSCOConnectionString = "";

        public void Get_from_config()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(@"C:\Rms_Bulk_Upload_Template\Cyclo_Connection_Details.xml");

                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/Configuration");

                foreach (XmlNode node in nodeList)
                {
                    strMySqlConnectionString = node.SelectSingleNode("MySqlConnectionString").InnerText;
                    strMySqlConnectionStringBulk = node.SelectSingleNode("MySqlConnectionStringBulk").InnerText;
                    strSqlConnectionString = node.SelectSingleNode("RMS_Connection_String").InnerText;
                    strSCOConnectionString = node.SelectSingleNode("SCO_Connection_String").InnerText;
                }

            }
            catch (Exception ex)
            {

            }
        }
        public JsonResult GetRegion()
        {
            List<Vechile> lstRegion = new List<Vechile>();
           
            try
            {
                string User = Session["UserType"].ToString();
                string RId = Session["RegionIds"].ToString();

                DataSet ds = new DataSet();

                if (User == "User")
                {
                    ds = Sr.GetRegionWithRegionDetails(RId);
                }
                else if (User == "Admin")
                {
                    ds = Sr.GetRegionDetails();
                }
                else if (User == "RH")
                {
                    ds = Sr.GetRegionWithRegionDetails(RId);
                }
                DataTable dt = new DataTable();

                //DataTable
                dt = ds.Tables[0];

           

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    Vechile H = new Vechile();

                    H.RegionId = Convert.ToInt32(dt.Rows[i]["RegionId"].ToString());
                    H.RegionName = dt.Rows[i]["RegionName"].ToString();

                    lstRegion.Add(H);
                }
                ViewBag.RegionDetails = lstRegion;
            }
            catch (Exception e)
            {
                SessionTimeoutStatus = "Session Expired";
            }
            return Json(lstRegion, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMakeModelChessisNumber(int RegionId, int BranchId, string VechileNumber)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
             List<MaintenanceAttributes> lstMakeModelChessisNo = new List<MaintenanceAttributes>();
             List<MaintenanceAttributes> lstExpiryDateDocument = new List<MaintenanceAttributes>();
            try
            {
                ds = Maintenance.GetMakeModelChessisNumber(RegionId, BranchId, VechileNumber);
                dt = ds.Tables[0];
                dt2 = ds.Tables[1];
               
                if (dt.Rows.Count > 0)
                {
                    MaintenanceAttributes ma = new MaintenanceAttributes();
                    ma.Make = dt.Rows[0]["Make"].ToString();
                    ma.Model= dt.Rows[0]["Model"].ToString();
                    ma.ChessisNumber = dt.Rows[0]["ChesisNo"].ToString();

                    lstMakeModelChessisNo.Add(ma);
                }
                if (dt2.Rows.Count > 0)
                {
                    MaintenanceAttributes ma2 = new MaintenanceAttributes();
                    ma2.ExpiryDate = Convert.ToDateTime(dt2.Rows[0]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");
                    ma2.Document = dt2.Rows[0]["Document"].ToString();

                    lstExpiryDateDocument.Add(ma2);
                }
                else {
                    MaintenanceAttributes ma2 = new MaintenanceAttributes();
                    ma2.ExpiryDate = "";
                    ma2.Document = "";

                    lstExpiryDateDocument.Add(ma2);
                }
            }
            
            catch (Exception ex)
            { 
                
            }
            var result = new { lstMakeModelChessisNo,lstExpiryDateDocument};
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveMaintenanceDetails(int RegionId, string RegionName, int BranchId, string BranchName,string VechileNo, string Make, 
                                                 string Model, string ChessisNumber, string ExpiryDate, string ReceivingDate, string selectDocument, string VehileNumber,
                                                 string ReceiptedAmount,string PenaltyAmount,string NonReceipteAmount,string TotalAmount,
                                                 string IssueDate,string PolicyNumber,string IDVValue,string NCBAmount,string PremiumAmount,
                                                 string Claim,string ClaimNumber,string TotalRepairAmt,string ClaimAmount)
        {
            Get_from_config();

            Session["VehicleNo"] = VechileNo;
            Session["Document"] = selectDocument;


            if (Session["EmailId"] == null)
            {
                Session["VehicleNo"] = null;
                Session["Document"] = null;

                return Json("Session Expired", JsonRequestBehavior.AllowGet);
            }

            int iReturn = 0;
            string Message = string.Empty;
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "Fleet_Maintenance_Tax_Details_Insert";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@RegionId", RegionId);
                cmd.Parameters.AddWithValue("@RegionName", RegionName);
                cmd.Parameters.AddWithValue("@BranchId", BranchId);
                cmd.Parameters.AddWithValue("@BranchName", BranchName);
                cmd.Parameters.AddWithValue("@VehicleNumber", VechileNo);
                cmd.Parameters.AddWithValue("@Make", Make);
                cmd.Parameters.AddWithValue("@Model", Model);
                cmd.Parameters.AddWithValue("@ChessisNumber", ChessisNumber);
                cmd.Parameters.AddWithValue("@ExpiryDate", ExpiryDate);
                cmd.Parameters.AddWithValue("@ReceivingDate", ReceivingDate);

                cmd.Parameters.AddWithValue("@Document", selectDocument);
                cmd.Parameters.AddWithValue("@ReceiptedAmount", ReceiptedAmount);
                cmd.Parameters.AddWithValue("@PenaltyAmount", PenaltyAmount);
                cmd.Parameters.AddWithValue("@NonReceipteAmount", NonReceipteAmount);
                cmd.Parameters.AddWithValue("@TotalAmount", TotalAmount);
                cmd.Parameters.AddWithValue("@IssueDate", IssueDate);
                cmd.Parameters.AddWithValue("@PolicyNumber", PolicyNumber);
                cmd.Parameters.AddWithValue("@IDVValue", IDVValue);
                cmd.Parameters.AddWithValue("@NCBAmount", NCBAmount);
                cmd.Parameters.AddWithValue("@PremiumAmount", PremiumAmount);
                cmd.Parameters.AddWithValue("@Claim", Claim);
                cmd.Parameters.AddWithValue("@ClaimNumber", ClaimNumber);
                cmd.Parameters.AddWithValue("@TotalRepairAmt", TotalRepairAmt);
                cmd.Parameters.AddWithValue("@ClaimAmount", ClaimAmount);
                cmd.Parameters.AddWithValue("@CreatedBy", Session["EmailId"]);

                con.Open();
                iReturn = cmd.ExecuteNonQuery();
                con.Close();

                if (iReturn > 0)
                {
                    Message = "Data has been inserted successfully";
                }
                else
                {
                    Message = "Failed to save the data";
                }
            }
            catch (Exception ex)
            {

            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDocumentExpiryDate(int RegionId, int BranchId,string VechileNo,string Document)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            
            ds=Maintenance.GetDocumentExpiryDate(RegionId, BranchId,VechileNo,Document);
            dt = ds.Tables[0];

            List<MaintenanceAttributes> lstDocumentExpiryDate = new List<MaintenanceAttributes>();
            if (dt.Rows.Count > 0)
            {
                MaintenanceAttributes a = new MaintenanceAttributes();
                a.ExpiryDate = dt.Rows[0]["ExpiryDate"].ToString();
                lstDocumentExpiryDate.Add(a);
            }
            else 
            {
                MaintenanceAttributes a = new MaintenanceAttributes();
                a.ExpiryDate = "";
                lstDocumentExpiryDate.Add(a);
            }

            return Json(lstDocumentExpiryDate, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetMaintenanceDocumentDate(string RegionId,string BranchId,string VechileNo,string document)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            ds = Maintenance.GetMaintenanceDocumentDate(RegionId, BranchId, VechileNo, document);
            dt = ds.Tables[0];

            List<MaintenanceAttributes> lstMaintenanceDocument = new List<MaintenanceAttributes>();
            if (dt.Rows.Count > 0)
            {
                MaintenanceAttributes MD = new MaintenanceAttributes();

                //Receiving Date check

                if (document == "NOC")
                {
                    MD.ReceivingDate = Convert.ToDateTime(dt.Rows[0]["ReceivingDate"].ToString()).ToString("yyyy-MM-dd");
                }

                MD.ExpiryDate = Convert.ToDateTime(dt.Rows[0]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");
                MD.OneYearCompleteOfDocument = Convert.ToDateTime(dt.Rows[0]["OneYearComplete"].ToString()).ToString("yyyy-MM-dd");
                MD.CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
                MD.ReceiptedAmount = dt.Rows[0]["ReceiptedAmount"].ToString();
                MD.PenaltyAmount = dt.Rows[0]["PenaltyAmount"].ToString();
                MD.NonReceipteAmount = dt.Rows[0]["NonReceipteAmount"].ToString();
                MD.TotalAmount = dt.Rows[0]["TotalAmount"].ToString();
                if (dt.Rows[0]["IssueDate"].ToString() == "" || dt.Rows[0]["IssueDate"].ToString() == null)
                {
                    MD.IssueDate = "";
                }
                else
                {
                    MD.IssueDate = Convert.ToDateTime(dt.Rows[0]["IssueDate"].ToString()).ToString("yyyy-MM-dd");
                }
                MD.PolicyNumber = dt.Rows[0]["PolicyNumber"].ToString();
                MD.IDVValue = dt.Rows[0]["IDVValue"].ToString();
                MD.NCBAmount = dt.Rows[0]["NCBAmount"].ToString();
                MD.PremiumAmount = dt.Rows[0]["PremiumAmount"].ToString();
                MD.Claim = dt.Rows[0]["Claim"].ToString();
                MD.ClaimNumber = dt.Rows[0]["ClaimNumber"].ToString();
                MD.TotalRepairAmt = dt.Rows[0]["TotalRepairAmt"].ToString();
                MD.ClaimAmount = dt.Rows[0]["ClaimAmount"].ToString();
                lstMaintenanceDocument.Add(MD);
            }
            else 
            {
                MaintenanceAttributes MD = new MaintenanceAttributes();
                MD.ReceivingDate = "";
                MD.ExpiryDate = "";
                lstMaintenanceDocument.Add(MD);
            }

            return Json(lstMaintenanceDocument, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult UploadFiles()
        {

            string VehicleNo = Session["VehicleNo"].ToString();
            string Document = Session["Document"].ToString();

            //Create Directory
            if (Directory.Exists("C:\\RMS_Fleet\\Tax\\" + VehicleNo + "_" + Document + "\\") == false)
            {
                Directory.CreateDirectory(("C:\\RMS_Fleet\\Tax\\" + VehicleNo + "_" + Document + "\\"));
            }

            string path = ("C:\\RMS_Fleet\\Tax\\" + VehicleNo + "_" + Document + "\\").ToString();

            HttpFileCollectionBase files = Request.Files;
            for (int i = 0; i < files.Count; i++)
            {
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

                if (System.IO.File.Exists(path + VehicleNo.ToString() + "_" + Document + "_" + files.AllKeys[i] + ".pdf") == true)
                {

                    System.IO.File.Delete(path + VehicleNo.ToString() + "_" + Document + "_" + files.AllKeys[i] + ".pdf");
                }
               
                HttpPostedFileBase file = files[i];
                file.SaveAs(path + VehicleNo.ToString() + "_" + Document + "_" + files.AllKeys[i] + ".pdf");
              
            }

            Session["RecId"] = null;
            return Json(files.Count + " Files Uploaded!");
        }

        [HttpPost]
        public JsonResult ExportExcel()
        {
            string VehicleNo = Session["VehicleNo"].ToString().Trim().TrimStart().TrimEnd();
            string DocumnetType = Session["Document"].ToString().Trim().TrimStart().TrimEnd();

            //// DataTable dt = DataService.GetData();
            var fileName = "" + VehicleNo + "_" + DocumnetType + ".pdf";

            var filePath = "C:\\RMS_Fleet\\Tax\\" + VehicleNo + "_" + DocumnetType + "\\" + VehicleNo + "_" + DocumnetType + "_Files" + ".pdf";
            var errorMessage = "";
            WebRequest webRequest = WebRequest.Create(filePath);
            webRequest.Timeout = 1200; // miliseconds
            webRequest.Method = "HEAD";

            try
            {
                webRequest.GetResponse();
                Session["fullPath"] = filePath;
            }
            catch
            {
                fileName = "";
                errorMessage = "No Details Found For The Seletect Type!";
            }

            //return the Excel file name
            return Json(new { fileName = fileName, errorMessage = errorMessage });
        }

        [HttpGet]
        public ActionResult Download(string file)
        {
            string fullPath = "";
            try
            {
                if (Session["fullPath"].ToString() != "")
                {
                    fullPath = Session["fullPath"].ToString();
                    Session["fullPath"] = null;
                    return File(fullPath, "application/pdf", file);
                }
            }
            catch
            {
                Session["fullPath"] = null;
            }

            return Json("Sucessfully Value Set", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetSessionValue(string VehicleNo, string Document)
        {
            Session["VehicleNo"] = VehicleNo;
            Session["Document"] = Document;

            return Json("Sucessfully Value Set", JsonRequestBehavior.AllowGet);

        }
    }
}
