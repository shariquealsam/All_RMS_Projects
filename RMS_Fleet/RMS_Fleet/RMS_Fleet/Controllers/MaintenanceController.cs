using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using RMS_Fleet.Models;
using System.Xml;
using System.Data.SqlClient;

namespace RMS_Fleet.Controllers
{
    public class MaintenanceController : Controller
    {
        //
        // GET: /Maintenance/
        SalesReportData Sr = new SalesReportData();
        Maintenance Maintenance = new Maintenance();

        public ActionResult Index()
        {
            GetRegion();
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
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Vechile> lstRegion = new List<Vechile>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Vechile H = new Vechile();

                H.RegionId = Convert.ToInt32(dt.Rows[i]["RegionId"].ToString());
                H.RegionName = dt.Rows[i]["RegionName"].ToString();

                lstRegion.Add(H);
            }
            ViewBag.RegionDetails = lstRegion;
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
        public JsonResult SaveMaintenanceDetails(int RegionId, string RegionName, int BranchId, string BranchName,
                   string VechileNo, string Make, string Model, string ChessisNumber, string ExpiryDate
                    , string selectDocument, string VehileNumber)
        {
            Get_from_config();
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
                cmd.Parameters.AddWithValue("@Document", selectDocument);
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
                MD.ExpiryDate = Convert.ToDateTime(dt.Rows[0]["ExpiryDate"].ToString()).ToString("yyyy-MM-dd");
                MD.OneYearCompleteOfDocument = Convert.ToDateTime(dt.Rows[0]["OneYearComplete"].ToString()).ToString("yyyy-MM-dd");
                MD.CurrentDate = DateTime.Now.ToString("yyyy-MM-dd");
                lstMaintenanceDocument.Add(MD);
            }
            else 
            {
                MaintenanceAttributes MD = new MaintenanceAttributes();
                MD.ExpiryDate = "";
                lstMaintenanceDocument.Add(MD);
            }

            return Json(lstMaintenanceDocument, JsonRequestBehavior.AllowGet);

        }

    }
}
