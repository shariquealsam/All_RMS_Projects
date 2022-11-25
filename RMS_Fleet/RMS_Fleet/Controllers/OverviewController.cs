using ClosedXML.Excel;
using RMS_Fleet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace RMS_Fleet.Controllers
{
    public class OverviewController : Controller
    {
        Overview Ov = new Overview();
        //
        // GET: /Overview/
        KPLData kpl = new KPLData();

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "Get_from_config_FleetApp" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Get_from_config_FleetApp", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
        }

        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public JsonResult GetAllRegions()
        {
            List<AllRegion> AllRegion = new List<Models.AllRegion>();
            try
            {
                string RegionId = Session["RegionIds"].ToString();

                DataSet dsRegion = Ov.GetRegionName(RegionId);
                DataTable dtRegion = dsRegion.Tables[0];

                for (int i = 0; i < dtRegion.Rows.Count; i++)
                {
                    AllRegion ar = new AllRegion();
                    ar.Region_id = dtRegion.Rows[i]["RegionId"].ToString();
                    ar.Region_Name = dtRegion.Rows[i]["RegionName"].ToString();
                    AllRegion.Add(ar);
                }
            }
            catch (Exception)
            { 
            
            }
           
            
            

            return Json(AllRegion, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBranch(string RegionId)
        {
            List<Branch> AllBranch = new List<Branch>();
            try
            {
                DataSet dsBranch = Ov.GetBranch(RegionId);
                DataTable dtBranch = new DataTable();
                if (dsBranch.Tables[0].Rows.Count > 0)
                {
                    dtBranch = dsBranch.Tables[0];
                }

                for (int i = 0; i <= dtBranch.Rows.Count - 1; i++)
                {
                    Branch br = new Branch();
                    br.BranchId = dtBranch.Rows[i]["BranchId"].ToString();
                    br.BranchName = dtBranch.Rows[i]["BranchName"].ToString();
                    AllBranch.Add(br);
                }
            }
            catch (Exception)
            { 
            
            }

            return Json(AllBranch, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetVechileList(string RegionId, string BranchId)
        {

            DataSet ds = Ov.GetVehicleMaster(RegionId, BranchId);
            DataTable dt = new DataTable();
            List<Kpi> lstVechileMaster = new List<Kpi>();
            //DataTable
            try
            {
                dt = ds.Tables[0];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    Kpi S = new Kpi();

                    S.VehicleNumber = dt.Rows[i]["VehicleNo"].ToString();

                    lstVechileMaster.Add(S);
                }
            }
            catch (Exception ex)
            {

            }
            return Json(lstVechileMaster, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Search_Repair_Maintenance_Overview(Search_Repair_Maintenance_Overview_Attributes obj)
        {
            List<OverviewDetails> lstOverview = new List<OverviewDetails>();
            try
            {
                if (Session["UserType"].ToString() == "User" || Session["UserType"].ToString() == "RH")
                {
                    obj.cmbRegion = Session["RegionIds"].ToString();
                }
                DataSet ds = Ov.Repair_Maintenance_Overview(obj);
                DataSet ds1 = Ov.Repair_Maintenance_Overview_Total_Vehicles(obj);
                DataTable dt = ds.Tables[0];

                
                if (dt.Rows.Count > 0)
                {
                    OverviewDetails od = new OverviewDetails();
                    od.TotalKM = dt.Rows[0]["TotalKM"].ToString();
                    od.TotalAmount = dt.Rows[0]["TotalAmount"].ToString();
                    od.PerKMCost = Math.Round(Convert.ToDecimal(dt.Rows[0]["PerKMCost"].ToString()), 2).ToString();
                    od.TotalExpenditureMaster = dt.Rows[0]["TotalExpenditureMaster"].ToString();
                    od.Difference = dt.Rows[0]["Difference"].ToString();
                    od.TotalVehicles = ds1.Tables[0].Rows[0]["TotalNoOfVehicle"].ToString();
                    lstOverview.Add(od);
                }
            }
            catch(Exception)
            {
            
            }
            return Json(lstOverview, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Search_KPL_Overview(Search_Repair_Maintenance_Overview_Attributes obj)
        {
            List<KPL_OverviewDetails> lstOverview = new List<KPL_OverviewDetails>();
            try
            {
                if (Session["UserType"].ToString() == "User" || Session["UserType"].ToString() == "RH")
                {
                    obj.cmbRegion = Session["RegionIds"].ToString();
                }

                DataSet ds = Ov.KPL_Overview(obj);
                DataSet ds1 = Ov.Repair_Maintenance_Overview_Total_Vehicles(obj);

                DataTable dt = ds.Tables[0];


                if (dt.Rows.Count > 0)
                {
                    KPL_OverviewDetails od = new KPL_OverviewDetails();
                    od.StdKmpl = dt.Rows[0]["StdKmpl"].ToString();
                    od.KMPL = dt.Rows[0]["KMPL"].ToString();
                    od.ExpectedFuleExpense = dt.Rows[0]["ExpectedFuelExpense"].ToString();
                    od.ActualFuelExpense = dt.Rows[0]["ActualFuelExpense"].ToString();
                    od.Difference = dt.Rows[0]["Difference"].ToString();
                    od.TotalKM = dt.Rows[0]["TotalKM"].ToString();
                    od.FuelRatePerltr = dt.Rows[0]["FuelRatePerltr"].ToString();
                    od.TotalVehicles = ds1.Tables[0].Rows[0]["TotalNoOfVehicle"].ToString();
                    lstOverview.Add(od);
                }
            }
            catch (Exception)
            {

            }
            return Json(lstOverview, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Download(Search_Repair_Maintenance_Overview_Attributes obj)
        {
            Random rndNumber = new Random();
            int iMaxNumber = 98772898;
            int iRandomNumberNew = rndNumber.Next(iMaxNumber);
            DataSet ds = new DataSet();

            string RegionId = Session["RegionIds"].ToString();

            Get_from_config();

            if (Session["UserType"].ToString() == "RH" || Session["UserType"].ToString() == "User")
            {
               ds= Ov.Repair_Maintenance_Overview_Report(obj, RegionId);
            }
            if (Session["UserType"].ToString() == "Admin")
            {
                ds = Ov.Repair_Maintenance_Overview_Report(obj);
            }
            DataTable dt = new DataTable();

            dt = ds.Tables[0];
            dt.TableName = "Sheet1";

            // Generate a new unique identifier against which the file can be stored
            string handle = Guid.NewGuid().ToString();

            using (XLWorkbook wb = new XLWorkbook())
            {
                foreach (DataTable dt1 in ds.Tables)
                {
                    //Add DataTable as Worksheet.
                    wb.Worksheets.Add(dt1);
                }

                //Export the Excel file.
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Report_" + iRandomNumberNew + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

            return RedirectToAction("Index");

        }


        [HttpPost]
        public ActionResult DownloadKPL(KPL_Overview_Attributes obj)
        {
            Random rndNumber = new Random();
            int iMaxNumber = 98772898;
            int iRandomNumberNew = rndNumber.Next(iMaxNumber);
            DataSet ds = new DataSet();

            string RegionId = Session["RegionIds"].ToString();

            Get_from_config();

            if (Session["UserType"].ToString() == "RH" || Session["UserType"].ToString() == "User")
            {
                ds = Ov.KPL_Overview_Report(obj, RegionId);
            }

            if (Session["UserType"].ToString() == "Admin")
            {
                ds = Ov.KPL_Overview_Report(obj);
            }
            DataTable dt = new DataTable();

            dt = ds.Tables[0];
            dt.TableName = "Sheet1";

            // Generate a new unique identifier against which the file can be stored
            string handle = Guid.NewGuid().ToString();

            using (XLWorkbook wb = new XLWorkbook())
            {
                foreach (DataTable dt1 in ds.Tables)
                {
                    //Add DataTable as Worksheet.
                    wb.Worksheets.Add(dt1);
                }

                //Export the Excel file.
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Report_" + iRandomNumberNew + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

            return RedirectToAction("Index");

        }

    }
}
