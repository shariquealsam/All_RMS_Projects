using RMS_Fleet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RMS_Fleet.Controllers
{
    public class KpiController : Controller
    {
        KPLData kpl = new KPLData();

        public ActionResult Index()
        {
            string RegionId = Session["RegionIds"].ToString();

            DataSet ds = new DataSet();

            if (RegionId == "0")
            {
                ds = kpl.GetBranch();
            }
            else
            {
                ds = kpl.GetBranch(RegionId);
            }
            DataSet ds1 = kpl.GetRegionName(RegionId);

            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Kpi> lstBranch = new List<Kpi>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Kpi S = new Kpi();

                S.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                S.BranchName = dt.Rows[i]["BranchName"].ToString();

                lstBranch.Add(S);
            }

            ViewBag.BranchDetails = lstBranch;

            if (ds1.Tables[0].Rows.Count > 0)
            {
                ViewBag.RegionName = ds1.Tables[0].Rows[0]["RegionName"].ToString();
            }
            else
            {
                ViewBag.RegionName = "Admin";
            }

            return View();
        }

        [HttpPost]
        public JsonResult GetVechileList(string BranchId)
        {

            DataSet ds = kpl.GetVehicleMaster(BranchId);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Kpi> lstVechileMaster = new List<Kpi>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Kpi S = new Kpi();

                S.VehicleNumber = dt.Rows[i]["VehicleNo"].ToString();

                lstVechileMaster.Add(S);
            }

            return Json(lstVechileMaster, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetVechileDetails(string VehicleNumber)
        {
            DataSet ds = kpl.GetVehicleDetails(VehicleNumber);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Kpi> lstVechileDetails = new List<Kpi>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Kpi S = new Kpi();

                S.RegionName = dt.Rows[i]["RegionName"].ToString();
                S.BranchName = dt.Rows[i]["BranchName"].ToString();
                S.ChesisNo = dt.Rows[i]["ChesisNo"].ToString();
                S.VehicleNo = dt.Rows[i]["VehicleNo"].ToString();
                S.Make = dt.Rows[i]["Make"].ToString();
                S.Manufacturing_Year = dt.Rows[i]["Manufacturing_Year"].ToString();
                S.FuelType = dt.Rows[i]["FuelType"].ToString();
                S.PetroCardNumber = dt.Rows[i]["PetroCardNumber"].ToString();

                lstVechileDetails.Add(S);
            }

            return Json(lstVechileDetails, JsonRequestBehavior.AllowGet);
        }

    }
}
