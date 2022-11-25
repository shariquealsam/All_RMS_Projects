using ClosedXML.Excel;
using RMS_Fleet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS_Fleet.Controllers
{
    public class DashboardController : Controller
    {
        DashboardData DD = new DashboardData();

        public ActionResult Index()
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
            string Date = DateTime.Now.ToString("yyyy-MM-dd");

            string UserType = Session["UserType"].ToString();
            string RegionId = Session["RegionIds"].ToString();
            DataSet ds = new DataSet();

            if (UserType == "Admin" && RegionId == "0")
            {
                ds = DD.GetDashboardDetails(Date);
            }
            else if (UserType != "Admin" && RegionId != "0")
            {
                ds = DD.GetDashboardSelectedRegionDetails(Date, UserType, RegionId);
            }

            DataTable dt = new DataTable();

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //DataTable
                    dt = ds.Tables[0];
                }
            }
            catch (Exception)
            {
            }

            List<Dashboard> lstReport = new List<Dashboard>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Dashboard H = new Dashboard();

                H.RegionID = dt.Rows[i]["RegionId"].ToString();
                H.Region = dt.Rows[i]["RegionName"].ToString();
                H.NoOfVehicles = dt.Rows[i]["NoOfVechile"].ToString();
                H.NoOfOpenings = dt.Rows[i]["NoOfOpening"].ToString();
                H.NoOfClosings = dt.Rows[i]["NoOfClosing"].ToString();
                H.NoReportingToday = dt.Rows[i]["No Reporting Today"].ToString();
                H.NoReportingTillDate = dt.Rows[i]["NoReportingTillDate"].ToString();

                lstReport.Add(H);
            }

            ViewBag.Dashbard = lstReport;
            return View();
        }

        public ActionResult TotalVehicleOpeningClosing(string fromDte=null,string ToDate=null,string BranchId=null,string RegionIdd=null)
        {

            List<OpeningClosingDetails> report = DD.openCloseList(fromDte,ToDate,BranchId, RegionIdd);
            List<RegionMs> region = DD.allRegion();

            ViewBag.reportList = report;
            ViewBag.regionList = region;
            ViewBag.Branch = BranchId;
            ViewBag.Region = RegionIdd;
            ViewBag.Date1 = ToDate;
            ViewBag.Date2 = fromDte;

            return View();
        }

        public JsonResult TotalVehicleOpeningClosingSearch(string fromDte = null, string ToDate = null, string BranchId = null, string RegionIdd = null)
        {
            if(fromDte == "")
            {
                fromDte = null;
            }
            if (ToDate == "")
            {
                ToDate = null;
            }
            if(BranchId == "")
            {
                BranchId = null;
            }
            if(RegionIdd == "")
            {
                RegionIdd = null;
            }
            List<OpeningClosingDetails> report = DD.openCloseList(fromDte, ToDate, BranchId, RegionIdd);
            

            //ViewBag.reportList = report;
            //ViewBag.regionList = region;
            //ViewBag.Branch = BranchId;
            //ViewBag.Region = RegionIdd;
            //ViewBag.Date1 = ToDate;
            //ViewBag.Date2 = fromDte;
            return Json(report, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetBranch(int RegionId)
        {
            List<BranchMas> branch = DD.branchByRegion(RegionId);

            return Json(branch, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadReport(string fromDte = null, string ToDate = null, string RegionIdd = null, string BranchId = null)
        {
            if (fromDte == "")
            {
                fromDte = null;
            }
            if (ToDate == "")
            {
                ToDate = null;
            }
            if (BranchId == "")
            {
                BranchId = null;
            }
            if (RegionIdd == "")
            {
                RegionIdd = null;
            }
            DataTable dt = new DataTable();

            DataSet ds = DD.exportReport(fromDte, ToDate, BranchId, RegionIdd);
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable as Worksheet.
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
                Response.AddHeader("content-disposition", "attachment;filename=OpeningClosingReport.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }

            return RedirectToAction("TotalVehicleOpeningClosing");
        }
        public ActionResult RegionWiseMasterVehicle(int RegionId)
        {
            string Date = DateTime.Now.ToString("yyyy-MM-dd");

            DataSet ds = DD.GetDashboardRegionDetails(Date, RegionId);
            DataTable dt = new DataTable();

            try
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //DataTable
                    dt = ds.Tables[0];
                }
            }
            catch (Exception)
            {
            }

            List<Dashboard> lstReport = new List<Dashboard>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Dashboard H = new Dashboard();

                H.RegionID = dt.Rows[i]["RegionId"].ToString();
                H.BranchID = dt.Rows[i]["BranchId"].ToString();
                H.Branch = dt.Rows[i]["BranchName"].ToString();
                H.NoOfVehicles = dt.Rows[i]["NoOfVechile"].ToString();
                H.NoOfOpenings = dt.Rows[i]["NoOfOpening"].ToString();
                H.NoOfClosings = dt.Rows[i]["NoOfClosing"].ToString();
                H.NoReportingToday = dt.Rows[i]["NoReportingToday"].ToString();
                H.NoReportingTillDate = dt.Rows[i]["NoReportingTillDate"].ToString();

                lstReport.Add(H);
            }

            ViewBag.DashbardRegion = lstReport;
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
    }
}
