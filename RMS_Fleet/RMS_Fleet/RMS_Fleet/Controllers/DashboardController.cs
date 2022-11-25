using RMS_Fleet.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
