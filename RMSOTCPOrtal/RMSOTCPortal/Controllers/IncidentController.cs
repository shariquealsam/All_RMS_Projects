using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using RMSOTCPortal.Models;

namespace RMSOTCPortal.Controllers
{
    public class IncidentController : Controller
    {
        //
        // GET: /Incident/
        Incident incc = new Incident();
        public ActionResult Index()
        {
            
            DataSet ds = incc.RegionList();
            DataTable dt = new DataTable();
            //DataTable
            dt = ds.Tables[0];

            List<RegionDetails> lstRegion = new List<RegionDetails>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                RegionDetails R = new RegionDetails();

                R.Region = dt.Rows[i]["Region"].ToString();

                lstRegion.Add(R);
            }
            var now = DateTime.Now;
            var Today = now.ToString("yyyy-MM-dd");
            ViewBag.Date = Today;
            ViewBag.RegionDelts = lstRegion;

            DataSet ds1 = incc.allIncidenceList();
            DataTable dt1 = new DataTable();
            dt1= ds1.Tables[0];
            List<incideDetails> incList = new List<incideDetails>();
            for(int i=0;i<=dt1.Rows.Count -1;i++)
            {
                incideDetails inc = new incideDetails();
                inc.Rec_Id = Convert.ToInt64(dt1.Rows[i]["Rec_Id"]);
                inc.OTC_Date = dt1.Rows[i]["OTC_Date"].ToString();
                inc.Company = dt1.Rows[i]["Company"].ToString();
                inc.Region = dt1.Rows[i]["Region"].ToString();
                inc.Branch = dt1.Rows[i]["Branch"].ToString();
                inc.RoutNo = Convert.ToInt32(dt1.Rows[i]["RoutNo"]);
                inc.KeyName = dt1.Rows[i]["KeyName"].ToString();
                inc.TouchKey = dt1.Rows[i]["TouchKey"].ToString();
                inc.KeyType = dt1.Rows[i]["KeyType"].ToString();
                inc.ATM_ID = dt1.Rows[i]["ATM_ID"].ToString();
                inc.Purpose = dt1.Rows[i]["Purpose"].ToString();
                inc.KeyCorrupt = dt1.Rows[i]["KeyCorrupt"].ToString();
                inc.ConfigureDate = dt1.Rows[i]["ConfigureDate"].ToString();
                inc.SendDateIst = dt1.Rows[i]["SendDateIst"].ToString();
                //inc.SendDateIInd= Convert.ToDateTime(dt1.Rows[i]["SendDateIInd"].ToString()).ToString("yyyy-MM-dd");
                inc.SendDateIInd = dt1.Rows[i]["SendDateIInd"].ToString();
                inc.ResolutionDate = dt1.Rows[i]["ResolutionDate"].ToString();
                inc.BatteryStatus = dt1.Rows[i]["BatteryStatus"].ToString();
                inc.Status = dt1.Rows[i]["Status"].ToString();
                inc.Remarks = dt1.Rows[i]["Remarks"].ToString();
                incList.Add(inc);
            }
            ViewBag.incDetais = incList;
            return View();
        }

        [HttpPost]
        public JsonResult BranchDetails(string Region)
        {
            DataSet ds = incc.GetBranchDetails(Region);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<BranchDtls> lstBranch = new List<BranchDtls>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                BranchDtls R = new BranchDtls();

                R.Branch_Id = dt.Rows[i]["Branch_ID"].ToString();
                R.Branch = dt.Rows[i]["Branch"].ToString();

                lstBranch.Add(R);
            }
            ViewBag.BranchDelts = lstBranch;
            return Json(lstBranch);
        }
        [HttpPost]
        public JsonResult RouteDetails(string BranchId)
        {
            DataSet ds = incc.GetRouteDetails(BranchId);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<RouteDtls> lstRoute = new List<RouteDtls>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                RouteDtls R = new RouteDtls();

                R.Route_No = dt.Rows[i]["Route_No"].ToString();
                R.Route_Name = dt.Rows[i]["Route_Name"].ToString();

                lstRoute.Add(R);
            }
            ViewBag.RouteDelts = lstRoute;
            return Json(lstRoute);
        }
        [HttpPost]
        public ActionResult InsertIncidentReport(incideDetails inc)
        {
            string ConfiguredBy = Session["UserId"].ToString();
            int i = incc.insertIncident(inc,ConfiguredBy);
            if (i > 0)
            {
                TempData["Success"] = "Added Successfully!";
            }
            else
            {
                TempData["error"] = "Something went wrong!";
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult incidentByRec_Id(int Rec_Id)
        {
            DataSet ds1 = incc.incidenceDetailsByRecId(Rec_Id);
            DataTable dt1 = new DataTable();
            dt1 = ds1.Tables[0];
            List<incideDetails> incList = new List<incideDetails>();
            for (int i = 0; i <= dt1.Rows.Count - 1; i++)
            {
                incideDetails inc = new incideDetails();
                inc.Rec_Id = Convert.ToInt64(dt1.Rows[i]["Rec_Id"]);
                inc.OTC_Date = dt1.Rows[i]["OTC_Date"].ToString();
                inc.Company = dt1.Rows[i]["Company"].ToString();
                inc.Region = dt1.Rows[i]["Region"].ToString();
                inc.Branch = dt1.Rows[i]["Branch"].ToString();
                inc.RoutNo = Convert.ToInt32(dt1.Rows[i]["RoutNo"]);
                inc.KeyName = dt1.Rows[i]["KeyName"].ToString();
                inc.TouchKey = dt1.Rows[i]["TouchKey"].ToString();
                inc.KeyType = dt1.Rows[i]["KeyType"].ToString();
                inc.ATM_ID = dt1.Rows[i]["ATM_ID"].ToString();
                inc.Purpose = dt1.Rows[i]["Purpose"].ToString();
                inc.KeyCorrupt = dt1.Rows[i]["KeyCorrupt"].ToString();
                inc.ConfigureDate = dt1.Rows[i]["ConfigureDate"].ToString();
                inc.SendDateIst = dt1.Rows[i]["SendDateIst"].ToString();
                //inc.SendDateIInd= Convert.ToDateTime(dt1.Rows[i]["SendDateIInd"].ToString()).ToString("yyyy-MM-dd");
                inc.SendDateIInd = dt1.Rows[i]["SendDateIInd"].ToString();
                inc.ResolutionDate = dt1.Rows[i]["ResolutionDate"].ToString();
                inc.BatteryStatus = dt1.Rows[i]["BatteryStatus"].ToString();
                inc.Status = dt1.Rows[i]["Status"].ToString();
                inc.Remarks = dt1.Rows[i]["Remarks"].ToString();
                incList.Add(inc);
            }
            
            return Json(incList);
        }


    }
}
