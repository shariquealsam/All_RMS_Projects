using RMS_Fleet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace RMS_Fleet.Controllers
{
    public class ReportController : Controller
    {
        ReportData R = new ReportData();
        clsImportant ci = new clsImportant();

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "Get_from_config" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - Get_from_config", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
        }

        public ActionResult Index(string RegionId, string BranchId)
        {
            try
            {
                if (Session["UserName"].ToString() == "" || Session["UserName"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Login");
            }

           

            var now = DateTime.Now;
            //var startDate = new DateTime(now.Year, now.Month, 1);
            var Start = now.ToString("yyyy-MM-dd");

            ViewBag.Date = Start;

            Session["RegionId"] = RegionId;
            Session["BranchIds"] = BranchId;
            return View();
        }

        [HttpPost]
        public JsonResult ReportMaster(string FromDate, string ToDate, string RegId, string Branchid, string IsFilter, string IsSessionRegionID)
        {
            string Date = DateTime.Now.ToString("yyyy-MM-dd");
            string RegionId = "0";
            string BranchIds = "0";

            if (FromDate != null)
            {
                FromDate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd");
            }
            if (ToDate != null)
            {
                ToDate = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd");
            }

            try
            {
                if (Session["RegionId"].ToString() == "" || Session["RegionId"].ToString() == null)
                {
                    RegionId = "0";
                }
                else
                {
                    RegionId = Session["RegionId"].ToString();
                }
            }
            catch (Exception)
            {
                RegionId = "0";
            }

            try
            {
                if (Session["BranchIds"].ToString() == "" || Session["BranchIds"].ToString() == null)
                {
                    BranchIds = "0";
                }
                else
                {
                    BranchIds = Session["BranchIds"].ToString();
                }
            }
            catch (Exception)
            {
                BranchIds = "0";
            }

            IsSessionRegionID = Session["RegionIds"].ToString();
            DataSet ds = new DataSet();

            string UserType = "";

            if (Session["UserType"].ToString() == "User")
            {
                UserType = Session["UserType"].ToString();
            }
            else
            {
                UserType = Session["UserType"].ToString();
            }

            if (RegionId != "0" && BranchIds != "0")
            {
                ds = R.GetReportRegionVsBranchMaster(Date, RegionId, BranchIds);

            }

            else
            {
                ds = R.GetReportMaster(Date, RegionId, FromDate, ToDate, RegId, Branchid, IsFilter, IsSessionRegionID,UserType);
            }

            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Report> lstReport = new List<Report>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Report H = new Report();

                H.SlNumber = Convert.ToInt32(dt.Rows[i]["SrNo"].ToString());
                H.Date = Convert.ToDateTime(dt.Rows[i]["Report_Date"].ToString()).ToString("dd-MMM-yyyy");
                H.ServiceType = dt.Rows[i]["Service_Type"].ToString();
                H.RouteNo = dt.Rows[i]["RouteNo"].ToString();
                H.VechileNumber = dt.Rows[i]["VehicleNo"].ToString();
                H.Branch = dt.Rows[i]["BranchName"].ToString();
                H.OpeningKM = dt.Rows[i]["OpeningKM"].ToString();

                if (dt.Rows[i]["ClosingKM"].ToString() == "0")
                {
                    H.ClosingKM = "NOT SUBMITED";
                }
                else
                {
                    H.ClosingKM = dt.Rows[i]["ClosingKM"].ToString();
                }

                H.DistanceKM = dt.Rows[i]["Distance"].ToString();
                H.OCKey = Convert.ToInt32(dt.Rows[i]["OCKey"].ToString());
                H.CreatedDateTime = Convert.ToDateTime(dt.Rows[i]["Created_Datetime"].ToString()).ToString("yyyy-MM-dd");

                if (dt.Rows[i]["OpnTime"].ToString() != "")
                {
                    H.OpeningTime = Convert.ToDateTime(dt.Rows[i]["OpnTime"].ToString()).ToString("hh:mm:ss tt");
                }
                else
                {
                    H.OpeningTime = "";
                }
                H.OpeningRemarks = dt.Rows[i]["OpnRemarks"].ToString();

                if (dt.Rows[i]["ClosTime"].ToString() != "")
                {
                    H.ClosingTime = Convert.ToDateTime(dt.Rows[i]["ClosTime"].ToString()).ToString("hh:mm:ss tt");
                }
                else
                {
                    H.ClosingTime = "";
                }

                H.ClosingRemarks = dt.Rows[i]["ClosRemarks"].ToString();

                lstReport.Add(H);
            }

            return Json(lstReport, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOpnClosDetails(string OCKey, string VechileNumber)
        {

            if (OCKey == null || OCKey == "")
            {
                OCKey = "0";
            }

            if (VechileNumber == null || VechileNumber == "")
            {
                VechileNumber = "0";
            }

            Session["OCKey"] = OCKey;
            Session["VechileNumber"] = VechileNumber;


            return Json(1, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IGetOpnClosDetails()
        {
            return View();
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [HttpPost]
        public JsonResult ReportMasterOCKey()
        {
            string OCKey = "";
            string VechileNumber = "";

            try
            {
                OCKey = Session["OCKey"].ToString();
            }
            catch (Exception)
            {
                OCKey = "0";
            }

            try
            {
                VechileNumber = Session["VechileNumber"].ToString();
            }
            catch (Exception)
            {
                VechileNumber = "0";
            }

            DataSet ds = R.GetOpnClosImageDetails(OCKey, VechileNumber);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Report> lstReport = new List<Report>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Report H = new Report();

                H.SlNumber = Convert.ToInt32(dt.Rows[i]["SrNo"].ToString());
                H.Date = Convert.ToDateTime(dt.Rows[i]["OpeningDate"].ToString()).ToString("dd-MMM-yyyy");
                H.ServiceType = dt.Rows[i]["Service_Type"].ToString();
                H.RouteNo = dt.Rows[i]["RouteNo"].ToString();
                H.VechileNumber = dt.Rows[i]["VehicleNo"].ToString();
                H.Branch = dt.Rows[i]["BranchName"].ToString();
                H.OpeningKM = dt.Rows[i]["OpeningKM"].ToString();
                if (dt.Rows[i]["ClosingKM"].ToString() == "0" || dt.Rows[i]["ClosingKM"].ToString() == "")
                {
                    H.ClosingKM = "NOT SUBMITED";
                }
                else
                {
                    H.ClosingKM = dt.Rows[i]["ClosingKM"].ToString();
                }

                H.DistanceKM = dt.Rows[i]["Distance"].ToString();
                H.OCKey = Convert.ToInt32(dt.Rows[i]["OCKey"].ToString());
                H.CreatedDateTime = Convert.ToDateTime(dt.Rows[i]["Created_Datetime"].ToString()).ToString("yyyy-MM-dd");

                if(dt.Rows[i]["ClosingImage"].ToString() == "")
                {
                    H.ClosingImage = "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";
                }
                else
                {
                    H.ClosingImage = dt.Rows[i]["ClosingImage"].ToString();
                }

                if (dt.Rows[i]["OpeningImage"].ToString() == "")
                {
                    H.OpeningImage = "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";
                }
                else
                {
                    H.OpeningImage = dt.Rows[i]["OpeningImage"].ToString();
                }

                if (dt.Rows[i]["OpeningTime"].ToString() != "")
                {
                    H.OpeningTime = Convert.ToDateTime(dt.Rows[i]["OpeningTime"].ToString()).ToString("hh:mm tt");
                }
                else
                {
                    H.OpeningTime = dt.Rows[i]["OpeningTime"].ToString();
                }
                if (dt.Rows[i]["ClosingTime"].ToString() != "")
                {
                    H.ClosingTime = Convert.ToDateTime(dt.Rows[i]["ClosingTime"].ToString()).ToString("hh:mm tt");
                }
                else
                {
                    H.ClosingTime = dt.Rows[i]["ClosingTime"].ToString();
                }
                if(dt.Rows[i]["PreviousDayClosingKM"].ToString() == "")
                {
                    H.PreviousDayKM = "No Record Found";
                }
                else
                {
                  H.PreviousDayKM = dt.Rows[i]["PreviousDayClosingKM"].ToString();
                }

                try
                {
                    H.PreviousDayTime = Convert.ToDateTime(dt.Rows[i]["PreviousDayClosingTime"].ToString()).ToString("dd-MMM-yyyy");
                }
                catch(Exception)
                {
                    H.PreviousDayTime = "No Record Found";
                }

                lstReport.Add(H);
            }

            return Json(lstReport, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveRevisedEntries(string RevisedOpeningKM, string RevisedClosingKM, int OCKey)
        {
            string Message = "";
            int iReturn = 0;
            SqlTransaction trans = null;

            Get_from_config();

            try
            {
                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                string sSql = "Fleet_Update_Opening_Closing_KM";

                //Open Database Connection
                con.Open();

                using (trans = con.BeginTransaction())
                {
                    //Insert User Details
                    SqlCommand cmd = new SqlCommand(sSql, con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RevisedOpeningKM", RevisedOpeningKM);
                    cmd.Parameters.AddWithValue("@RevisedClosingKM", RevisedClosingKM);
                    cmd.Parameters.AddWithValue("@OCKey", OCKey);

                    iReturn = cmd.ExecuteNonQuery();

                    trans.Commit();

                    con.Close();
                }

                if (iReturn > 0)
                {
                    Message = "Sucessfully Updated  Opening Closing KM";
                }
                else
                {
                    Message = "Due to Some Network Issue,Data Not Updated Please Try Once Again !! ";
                }
            }
            catch
            {
                iReturn = 0;
                trans.Rollback();
                Message = "Due to Some Network Issue,Data Not Updated Please Try Once Again !! ";
            }

            return Json(Message, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBranch(int RegionId)
        {

            DataSet ds = R.GetBranchDetails(RegionId);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Vechile> lstBranch = new List<Vechile>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Vechile H = new Vechile();

                H.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                H.BranchName = dt.Rows[i]["BranchName"].ToString();

                lstBranch.Add(H);
            }

            return Json(lstBranch, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetRegion()
        {
            string User = Session["UserType"].ToString();
            string RId = Session["RegionIds"].ToString();

            DataSet ds = new DataSet();

            if (User == "User")
            {
                ds = R.GetRegionWithRegionDetails(RId);
            }
            else if (User == "Admin")
            {
                ds = R.GetRegionDetails();
            }
            else if (User == "RH")
            {
                ds = R.GetRegionWithRegionDetails(RId);
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

            return Json(lstRegion, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ViewHistoryVehicle(string VehicleNumber)
        {
            DataSet ds = new DataSet();

            ds = R.GetVehicleHistoryDetails(VehicleNumber);

            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Report> lstHistory = new List<Report>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Report H = new Report();
						
                H.HistorySrNo = dt.Rows[i]["SrNo"].ToString();
                H.HistoryVehicleNo = dt.Rows[i]["VehicleNo"].ToString();
                H.HistoryOpeningKM = dt.Rows[i]["OpeningKM"].ToString();
                H.HistoryClosingKM = dt.Rows[i]["ClosingKM"].ToString();
                H.HistoryDistance = dt.Rows[i]["Distance"].ToString();
                H.HistoryOpeningDate = dt.Rows[i]["OpeningDate"].ToString();
                H.HistoryClosingDate = dt.Rows[i]["ClosingDate"].ToString();

                lstHistory.Add(H);
            }
            return Json(lstHistory, JsonRequestBehavior.AllowGet);
        }

    }
}
