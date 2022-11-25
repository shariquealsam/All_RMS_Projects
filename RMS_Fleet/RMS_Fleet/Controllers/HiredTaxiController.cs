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
    public class HiredTaxiController : Controller
    {
        clsImportant ci = new clsImportant();
        HiredTaxiDetails Ht = new HiredTaxiDetails();

        public ActionResult Index()
        {
            ViewBag.Date = DateTime.Now.ToString("dd-MM-yyyy");

            string UserType = Session["UserType"].ToString();
            string RegionId = Session["RegionIds"].ToString();
            int RegionID = 0;

            if (UserType == "Admin" && RegionId == "0")
            {
                RegionID = RegionID;
            }
            else
            {
                RegionID = Convert.ToInt32(RegionId);
                GetBranch(Convert.ToInt32(RegionId));

            }  

            DataSet dsRegion = Ht.GetRegionDetails(RegionID);
            DataTable dtRegion = new DataTable();

            //DataTable
            dtRegion = dsRegion.Tables[0];

            List<HiredTaxs> lstRegion = new List<HiredTaxs>();

            for (int i = 0; i <= dtRegion.Rows.Count - 1; i++)
            {
                HiredTaxs H = new HiredTaxs();

                H.RegionId = dtRegion.Rows[i]["RegionId"].ToString();
                H.RegionName = dtRegion.Rows[i]["RegionName"].ToString();

                lstRegion.Add(H);
            }


            ViewBag.RegionDetails = lstRegion;


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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "Get_from_config" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Get_from_config", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
        }

        public JsonResult SaveHiredTaxiDetails(string HiredDate, string Region, string Branch, string Location, string Customer, string TypeOfServices, string RouteNo, 
			  string RouteId, string VehicleNo, string Frequency, string CreatedBy)
        {
            Get_from_config();

            //if (Session["EmailId"] == null)
            //{
            //    Session["VehicleNo"] = null;
            //    Session["Document"] = null;

            //    return Json("Session Expired", JsonRequestBehavior.AllowGet);
            //}

            string HiredDates = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-dd");

            int iReturn = 0;
            string Message = string.Empty;
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "Fleet_Hired_Vehicle_Insert";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@HiredDate", HiredDates);
                cmd.Parameters.AddWithValue("@Region", Region);
                cmd.Parameters.AddWithValue("@Branch", Branch);
                cmd.Parameters.AddWithValue("@Location", Location);
                cmd.Parameters.AddWithValue("@Customer", Customer);
                cmd.Parameters.AddWithValue("@TypeOfServices", TypeOfServices);
                cmd.Parameters.AddWithValue("@RouteNo", RouteNo);
                cmd.Parameters.AddWithValue("@RouteId", RouteId);
                cmd.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmd.Parameters.AddWithValue("@Frequency", Frequency);
                cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);


                con.Open();
                iReturn = cmd.ExecuteNonQuery();
                con.Close();

                if (iReturn > 0)
                {
                    Message = "Vehicle details has been inserted successfully";
                }
                else
                {
                    Message = "Failed to insert vehicle details";
                }
            }
            catch (Exception ex)
            {

            }
            return Json(Message, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetBranch(int RegionId)
        {

            DataSet ds = Ht.GetBranchDetails(RegionId);
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

            ViewBag.BranchDetails = lstBranch;
            return Json(lstBranch, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetValidationCheck(string Date, string VehicleNo)
        {
            DataSet ds = Ht.GetValidationDetails(Convert.ToDateTime(Date).ToString("yyyy-MM-dd"), VehicleNo);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<HiredTaxs> lstdetails = new List<HiredTaxs>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    HiredTaxs H = new HiredTaxs();

                    H.Validation = dt.Rows[i]["Validation"].ToString();

                    lstdetails.Add(H);
                }
            }

            else
            {
                HiredTaxs H = new HiredTaxs();
                H.Validation = "0";
                lstdetails.Add(H);
            }
 
            return Json(lstdetails, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetValidation(string Date, string VehicleNo)
        {
            Date = Convert.ToDateTime(DateTime.Today).ToString("yyyy-MM-dd");

            DataSet ds = Ht.GetValidationDetails(Date, VehicleNo);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<HiredTaxs> lstdetails = new List<HiredTaxs>();

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    HiredTaxs H = new HiredTaxs();

                    H.Validation = dt.Rows[i]["Validation"].ToString();

                    lstdetails.Add(H);
                }
            }

            else
            {
                HiredTaxs H = new HiredTaxs();
                H.Validation = "0";
                lstdetails.Add(H);
            }

            return Json(lstdetails, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetHiredDetails(string Date, int RegionId, int BranchId)
        {
            DataSet ds = Ht.GetHiredVehicleMaster(Date, RegionId, BranchId);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<HiredTaxs> lstHiredVehicle = new List<HiredTaxs>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                HiredTaxs H = new HiredTaxs();

                H.Slno = dt.Rows[i]["SlNo"].ToString();
                H.Rec_id = dt.Rows[i]["Rec_id"].ToString();
                H.HiredDate = dt.Rows[i]["HiredDate"].ToString();
                H.Region = dt.Rows[i]["Region"].ToString();
                H.BranchName = dt.Rows[i]["Branch"].ToString();
                H.Location = dt.Rows[i]["Location"].ToString();
                H.Customer = dt.Rows[i]["Customer"].ToString();
                H.TypeOfServices = dt.Rows[i]["TypeOfServices"].ToString();
                H.RouteNo = dt.Rows[i]["RouteNo"].ToString();
                H.RouteId = dt.Rows[i]["RouteId"].ToString();
                H.VehicleNo = dt.Rows[i]["VehicleNo"].ToString();
                H.Frequency = dt.Rows[i]["Frequency"].ToString();

                lstHiredVehicle.Add(H);
            }

            return Json(lstHiredVehicle, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteRecord(int RecId)
        {
            Get_from_config();

            int iReturn = 0;
            string Message = string.Empty;
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "DELETE FROM [dbo].[Fleet_Hired_Taxi_Details] WHERE Rec_id = " + RecId + " ";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;

                con.Open();
                iReturn = cmd.ExecuteNonQuery();
                con.Close();

                if (iReturn > 0)
                {
                    Message = "Vehicle details has been successfully removed";
                }
                else
                {
                    Message = "Failed to remove vehicle details";
                }
            }
            catch (Exception ex)
            {

            }
            return Json(Message, JsonRequestBehavior.AllowGet);
        }
    }
}
