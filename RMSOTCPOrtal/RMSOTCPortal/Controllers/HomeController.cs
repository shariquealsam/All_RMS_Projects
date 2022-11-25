using Newtonsoft.Json;
using RMSOTCPortal.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

// Current Running portal
namespace RMSOTCPortal.Controllers
{
    public class HomeController : Controller
    {
        HomeData HD = new HomeData();

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

            

            var now = DateTime.Now;
            var Now2 = DateTime.Now;
            var Today = now.ToString("yyyy-MM-dd");
            var TodayCheck = Now2.ToString("yyyy-MM-dd");

            ViewBag.Date = Today;
            ViewBag.DateCheck = TodayCheck;

            var Userid = Session["UserId"].ToString();

            TotalCountValue(Today, Userid);
            return View();
        }

        [HttpPost]
        public JsonResult AutoComplete(string prefix)
        {
            DataSet ds = HD.GetBranchDetails(prefix);
            DataTable dt = new DataTable();
            
            //DataTable
            dt = ds.Tables[0];

            List<Home> lstBranch = new List<Home>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Home AD = new Home();

                AD.ATMId = dt.Rows[i]["ClientATMID"].ToString();

                lstBranch.Add(AD);
            }

            return Json(lstBranch);
        }

    [HttpPost]
    public JsonResult ScoDetails(string FromDate, string ToDate, string ATMID)
    {
      string routeNo = string.Empty;
      string auditorName = string.Empty;
      string auditorId = string.Empty;
      string branchId = string.Empty;

      // this crews details only for Audit
      string Custodian1 = string.Empty;
      string CustodianMobile1 = string.Empty;
      string Custodian1RegNo = string.Empty;
      string Custodian2 = string.Empty;
      string CustodianMobile2 = string.Empty;
      string Custodian2RegNo = string.Empty;
      //end Here
      DataSet ds = new DataSet();
      DataSet ds1 = new DataSet();
      DataSet dsRMS = new DataSet(); ///rms dataset
      DataSet dsFLMSiteTime = new DataSet();

      DataSet dsAuditRMS = new DataSet();// audit detaset
      DataSet dsAudti1 = new DataSet();// audit dataset
      DataSet dsAuditDtls = new DataSet();// Audit SCO Details Dataset

      DataSet dsFLM = new DataSet(); ///Otc dataset 
      DataSet dsOther = new DataSet();

      DataTable dt = new DataTable();
      DataTable dtFlm = new DataTable();
      DataTable dtRMS = new DataTable();
      DataTable dtRMSFlm = new DataTable();
      DataTable dtOther = new DataTable();
      DataTable dtFLMSiteTime = new DataTable();

      DataTable dtAudit = new DataTable(); // audit datatable
      DataTable dtScoAuditDtls = new DataTable(); //audit SCO Databale
      DataTable dtAuditRms = new DataTable();// RMS SCO audit Datable


      dsRMS = HD.GetRMSScoDetails(FromDate, ToDate, ATMID);
      ds = HD.GetScoDetails(FromDate, ToDate, ATMID);


      // Data Tables
      try
      {
        if (dsRMS.Tables[0].Rows.Count > 0)
        {
          dtRMS = dsRMS.Tables[0];
        }
      }
      catch
      {
      }

      try
      {
        if (ds.Tables[0].Rows.Count > 0)
        {
          dt = ds.Tables[0];
        }
      }
      catch
      {
      }

      List<Home> lstDetails = new List<Home>();
      List<Home> lstFlms = new List<Home>();
      FlmSiteCls flmSite = new FlmSiteCls();
      List<Home> lstOtherRms = new List<Home>();
      List<Home> AuditDtls = new List<Home>();

      for (int i = 0; i <= dtRMS.Rows.Count - 1; i++)
      {
        Home AD = new Home();

        AD.ATMId = dtRMS.Rows[i]["ATMID"].ToString();
        AD.Date = Convert.ToDateTime(dtRMS.Rows[i]["Date"].ToString()).ToString("yyyy-MM-dd");
        //AD.Date = Convert.ToDateTime(dtRMS.Rows[i]["Date"].ToString()).ToString("dd-MMM-yyyy");
        AD.REGION = dtRMS.Rows[i]["Region"].ToString();
        AD.BRANCH = dtRMS.Rows[i]["Branch"].ToString();
        AD.CITY = dtRMS.Rows[i]["City"].ToString();
        AD.MSP = dtRMS.Rows[i]["Msp"].ToString();
        AD.BANK = dtRMS.Rows[i]["Bank"].ToString();
        AD.Route = dtRMS.Rows[i]["RouteNo"].ToString();
        AD.Activity = dtRMS.Rows[i]["Activity"].ToString();
        AD.Status = dtRMS.Rows[i]["Status"].ToString();
        AD.Custodian1Name = dtRMS.Rows[i]["Custodian1Name"].ToString();
        AD.Custodian1RegNo = dtRMS.Rows[i]["Custodian1RegNo"].ToString();
        AD.Custodian1Mobile = dtRMS.Rows[i]["KeyHolderMobile"].ToString();
        AD.Custodian2Name = dtRMS.Rows[i]["Custodian2Name"].ToString();
        AD.Custodian2RegNo = dtRMS.Rows[i]["Custodian2RegNo"].ToString();
        AD.Custodian2Mobile = dtRMS.Rows[i]["CallerNumber"].ToString();
        AD.RouteKeyName = dtRMS.Rows[i]["RouteKeyName"].ToString();
        AD.CombinationNo = dtRMS.Rows[i]["CombinationNo"].ToString();
        if (!string.IsNullOrEmpty(AD.CombinationNo))
        {
          AD.RemarksOne = dtRMS.Rows[i]["RemarksOne"].ToString();
          AD.RemarksTwo = dtRMS.Rows[i]["RemarksTwo"].ToString();
        }
        else
        {
          AD.RemarksOne = "";
          AD.RemarksTwo = "";
        }
        AD.CurrentDatePlusOneDay = Convert.ToDateTime(dtRMS.Rows[i]["Date"].ToString()).AddDays(1).ToString("yyyy-MM-dd");
        AD.item_route_sheet_id = dtRMS.Rows[i]["Item_Route_Sheet_Id"].ToString();
        AD.ActualTime = Convert.ToDateTime(dtRMS.Rows[i]["Actual_Date_Time"].ToString()).ToString("HH:mm");
        AD.UserName = dtRMS.Rows[i]["UserName"].ToString();
        AD.TypeFlag = "RMS";
        lstDetails.Add(AD);
      }


      if (dtRMS.Rows.Count > 0)
      {
        for (int i = 0; i <= dt.Rows.Count - 1; i++)
        {
          //ScoDetails 
          for (int j = 0; j <= dtRMS.Rows.Count - 1; j++)
          {
            if ((dtRMS.Rows[j]["ATMID"].ToString()) != (dt.Rows[i]["ATM_ID"].ToString()) && Convert.ToDateTime(dtRMS.Rows[j]["Date"].ToString()).ToString("yyyy-MM-dd") != Convert.ToDateTime(dt.Rows[i]["SERVICE_DATE"].ToString()).ToString("yyyy-MM-dd"))
            {
              Home AD = new Home();

              AD.ATMId = dt.Rows[i]["ATM_ID"].ToString();
              AD.Date = Convert.ToDateTime(dt.Rows[i]["SERVICE_DATE"].ToString()).ToString("yyyy-MM-dd");
              //AD.Date = Convert.ToDateTime(dt.Rows[i]["SERVICE_DATE"].ToString()).ToString("dd-MMM-yyyy");
              AD.REGION = dt.Rows[i]["Region"].ToString();
              AD.BRANCH = dt.Rows[i]["Branch_Name"].ToString();
              AD.CITY = dt.Rows[i]["City"].ToString();
              AD.MSP = dt.Rows[i]["MSP_Name"].ToString();
              AD.BANK = dt.Rows[i]["Bank_Name"].ToString();
              AD.Route = dt.Rows[i]["route_no"].ToString();
              AD.Activity = dt.Rows[i]["Activity"].ToString();
              AD.Status = dt.Rows[i]["Status"].ToString();
              AD.Custodian1Name = dt.Rows[i]["Custodian1"].ToString();
              AD.Custodian1RegNo = dt.Rows[i]["Custodian1_RegNo"].ToString();
              AD.Custodian1Mobile = dt.Rows[i]["Custodian1Mobile"].ToString();
              AD.Custodian2Name = dt.Rows[i]["Custodian2"].ToString();
              AD.Custodian2RegNo = dt.Rows[i]["Custodian2_RegNo"].ToString();
              AD.Custodian2Mobile = dt.Rows[i]["Custodian2Mobile"].ToString();
              AD.CurrentDatePlusOneDay = Convert.ToDateTime(dt.Rows[i]["SERVICE_DATE"].ToString()).AddDays(1).ToString("yyyy-MM-dd");
              AD.item_route_sheet_id = dt.Rows[i]["item_route_sheet_id"].ToString();
              AD.RemarksOne = "";
              AD.RemarksTwo = "";
              AD.ActualTime = "";
              AD.UserName = "";
              AD.TypeFlag = "SCO";
              AD.Indent_Uploaded = "";
              lstDetails.Add(AD);
            }

          }
        }
      }
      else
      {
        for (int k = 0; k <= dt.Rows.Count - 1; k++)
        {
          Home AD = new Home();

          AD.ATMId = dt.Rows[k]["ATM_ID"].ToString();
          AD.Date = Convert.ToDateTime(dt.Rows[k]["SERVICE_DATE"].ToString()).ToString("yyyy-MM-dd");
          //AD.Date = Convert.ToDateTime(dt.Rows[k]["SERVICE_DATE"].ToString()).ToString("dd-MMM-yyyy");
          AD.REGION = dt.Rows[k]["Region"].ToString();
          AD.BRANCH = dt.Rows[k]["Branch_Name"].ToString();
          AD.CITY = dt.Rows[k]["City"].ToString();
          AD.MSP = dt.Rows[k]["MSP_Name"].ToString();
          AD.BANK = dt.Rows[k]["Bank_Name"].ToString();
          AD.Route = dt.Rows[k]["route_no"].ToString();
          AD.Activity = dt.Rows[k]["Activity"].ToString();
          AD.Status = dt.Rows[k]["Status"].ToString();
          AD.Custodian1Name = dt.Rows[k]["Custodian1"].ToString();
          AD.Custodian1RegNo = dt.Rows[k]["Custodian1_RegNo"].ToString();
          AD.Custodian1Mobile = dt.Rows[k]["Custodian1Mobile"].ToString();
          AD.Custodian2Name = dt.Rows[k]["Custodian2"].ToString();
          AD.Custodian2RegNo = dt.Rows[k]["Custodian2_RegNo"].ToString();
          AD.Custodian2Mobile = dt.Rows[k]["Custodian2Mobile"].ToString();
          AD.CurrentDatePlusOneDay = Convert.ToDateTime(dt.Rows[k]["SERVICE_DATE"].ToString()).AddDays(1).ToString("yyyy-MM-dd");
          AD.item_route_sheet_id = dt.Rows[k]["item_route_sheet_id"].ToString();
          AD.RemarksOne = "";
          AD.RemarksTwo = "";
          AD.ActualTime = "";
          AD.UserName = "";
          AD.TypeFlag = "SCO";
          AD.Indent_Uploaded = Convert.ToDateTime(dt.Rows[k]["Indent_Uploaded"].ToString()).ToString("dd-MM-yyyy hh:mm tt");
          lstDetails.Add(AD);
        }
      }

      // TESTING START
      if (dtRMS.Rows.Count > 0 && dt.Rows.Count > 0)
      {
        for (int i = 0; i <= dt.Rows.Count - 1; i++) //2
        {
          string strItemRouteSheetId = dt.Rows[i]["item_route_sheet_id"].ToString();
          bool isAvaialble = false;

          for (int j = 0; j <= dtRMS.Rows.Count - 1; j++) //3
          {
            if (strItemRouteSheetId == dtRMS.Rows[j]["Item_Route_Sheet_Id"].ToString())
            {
              isAvaialble = true;
            }
          }

          //SCO New Data should be add if found
          if (isAvaialble == false)
          {
            Home AD = new Home();

            AD.ATMId = dt.Rows[i]["ATM_ID"].ToString();
            AD.Date = Convert.ToDateTime(dt.Rows[i]["SERVICE_DATE"].ToString()).ToString("yyyy-MM-dd");
            //AD.Date = Convert.ToDateTime(dt.Rows[i]["SERVICE_DATE"].ToString()).ToString("dd-MMM-yyyy");
            AD.REGION = dt.Rows[i]["Region"].ToString();
            AD.BRANCH = dt.Rows[i]["Branch_Name"].ToString();
            AD.CITY = dt.Rows[i]["City"].ToString();
            AD.MSP = dt.Rows[i]["MSP_Name"].ToString();
            AD.BANK = dt.Rows[i]["Bank_Name"].ToString();
            AD.Route = dt.Rows[i]["route_no"].ToString();
            AD.Activity = dt.Rows[i]["Activity"].ToString();
            AD.Status = dt.Rows[i]["Status"].ToString();
            AD.Custodian1Name = dt.Rows[i]["Custodian1"].ToString();
            AD.Custodian1RegNo = dt.Rows[i]["Custodian1_RegNo"].ToString();
            AD.Custodian1Mobile = dt.Rows[i]["Custodian1Mobile"].ToString();
            AD.Custodian2Name = dt.Rows[i]["Custodian2"].ToString();
            AD.Custodian2RegNo = dt.Rows[i]["Custodian2_RegNo"].ToString();
            AD.Custodian2Mobile = dt.Rows[i]["Custodian2Mobile"].ToString();
            AD.CurrentDatePlusOneDay = Convert.ToDateTime(dt.Rows[i]["SERVICE_DATE"].ToString()).AddDays(1).ToString("yyyy-MM-dd");
            AD.item_route_sheet_id = dt.Rows[i]["item_route_sheet_id"].ToString();
            AD.RemarksOne = "";
            AD.RemarksTwo = "";
            AD.ActualTime = "";
            AD.UserName = "";
            AD.TypeFlag = "BothRMSSCO";
            AD.Indent_Uploaded = "";
            lstDetails.Add(AD);
          }
        }
      }

      // Audit Start

      //Check if ATM have Audit
      dsAudti1 = HD.getAuditATM(FromDate, ToDate, ATMID);
      if(dsAudti1.Tables[0].Rows.Count > 0)
      {
        
        dtAudit = dsAudti1.Tables[0];
        routeNo = dtAudit.Rows[0]["RouteNo"].ToString();
        auditorName = dtAudit.Rows[0]["UserName"].ToString();
        auditorId = dtAudit.Rows[0]["UserId"].ToString();
        branchId = dtAudit.Rows[0]["Branch_Id"].ToString();

        // get Crew details for audit if atm has only Audit
      DataSet dsCrew = new DataSet();
      DataTable dtCrew = new DataTable();
      DataSet dsRMSOnlyAudit = new DataSet();
      DataTable dtRMSOnlyAudit = new DataTable();
        //get Audit SCO RMS
      dsAuditRMS = HD.GetAuditRMSScoDetails(FromDate, ToDate, ATMID);
        // Data Tables
        try
        {
            if (dsAuditRMS.Tables[0].Rows.Count > 0)
            {
                dtAuditRms = dsAuditRMS.Tables[0];
            }
        }
        catch (Exception e)
        {
        }
      if(dsAuditRMS.Tables[0].Rows.Count> 0)
      {
        for (int x = 0; x <= dtAuditRms.Rows.Count - 1; x++)
        {
            Home AD = new Home();
            AD.ATMId = dtAuditRms.Rows[x]["ATMID"].ToString();
            AD.Date = Convert.ToDateTime(dtAuditRms.Rows[x]["Date"].ToString()).ToString("yyyy-MM-dd");
            //AD.Date = Convert.ToDateTime(dtRMS.Rows[i]["Date"].ToString()).ToString("dd-MMM-yyyy");
            AD.REGION = dtAuditRms.Rows[x]["Region"].ToString();
            AD.BRANCH = dtAuditRms.Rows[x]["Branch"].ToString();
            AD.CITY = dtAuditRms.Rows[x]["City"].ToString();
            AD.MSP = dtAuditRms.Rows[x]["Msp"].ToString();
            AD.BANK = dtAuditRms.Rows[x]["Bank"].ToString();
            AD.Route = dtAuditRms.Rows[x]["RouteNo"].ToString();
            AD.Activity = dtAuditRms.Rows[x]["Activity"].ToString();
            AD.Status = dtAuditRms.Rows[x]["Status"].ToString();
            AD.Custodian1Name = dtAuditRms.Rows[x]["Custodian1Name"].ToString();
            AD.Custodian1RegNo = dtAuditRms.Rows[x]["Custodian1RegNo"].ToString();
            AD.Custodian1Mobile = dtAuditRms.Rows[x]["KeyHolderMobile"].ToString();
            AD.Custodian2Name = dtAuditRms.Rows[x]["Custodian2Name"].ToString();
            AD.Custodian2RegNo = dtAuditRms.Rows[x]["Custodian2RegNo"].ToString();
            AD.Custodian2Mobile = dtAuditRms.Rows[x]["CallerNumber"].ToString();
            AD.RouteKeyName = dtAuditRms.Rows[x]["RouteKeyName"].ToString();
            AD.CombinationNo = dtAuditRms.Rows[x]["CombinationNo"].ToString();
            if (!string.IsNullOrEmpty(AD.CombinationNo))
            {
                AD.RemarksOne = dtAuditRms.Rows[x]["RemarksOne"].ToString();
                AD.RemarksTwo = dtAuditRms.Rows[x]["RemarksTwo"].ToString();
            }
            else
            {
                AD.RemarksOne = "";
                AD.RemarksTwo = "";
            }
            AD.CurrentDatePlusOneDay = Convert.ToDateTime(dtAuditRms.Rows[x]["Date"].ToString()).AddDays(1).ToString("yyyy-MM-dd");
            AD.item_route_sheet_id = dtAuditRms.Rows[x]["Item_Route_Sheet_Id"].ToString();
            AD.ActualTime = Convert.ToDateTime(dtAuditRms.Rows[x]["Actual_Date_Time"].ToString()).ToString("HH:mm");
            AD.UserName = dtAuditRms.Rows[x]["UserName"].ToString();
            AD.Auditor = auditorName;
            AD.AuditorId = auditorId;
            AD.TypeFlag = "RMS";
            AuditDtls.Add(AD);
        }
      }
      else {
        if (ds.Tables[0].Rows.Count == 0)
        {
            dsCrew = HD.GetAuditScoRouteCrewDetails(FromDate, ToDate, routeNo, branchId);
            try
            {
                if (dsCrew.Tables[0].Rows.Count > 0)
                {
                    dtCrew = dsCrew.Tables[0];
                    Custodian1 = dtCrew.Rows[0]["Custodian1"].ToString();
                    Custodian1RegNo = dtCrew.Rows[0]["Custodian1_RegNo"].ToString();
                    CustodianMobile1 = dtCrew.Rows[0]["Custodian1Mobile"].ToString();
                    Custodian2 = dtCrew.Rows[0]["Custodian2"].ToString();
                    Custodian2RegNo = dtCrew.Rows[0]["Custodian2_RegNo"].ToString();
                    CustodianMobile2 = dtCrew.Rows[0]["Custodian2Mobile"].ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        dsRMSOnlyAudit = HD.GetRMSAuditDetails(FromDate, ToDate, ATMID);
        try
        {
            if (dsRMSOnlyAudit.Tables[0].Rows.Count > 0)
            {
                dtRMSOnlyAudit = dsRMSOnlyAudit.Tables[0];
            }
        }
        catch
        {
        }


        //if ATM has audit and loading both
        if (ds.Tables[0].Rows.Count > 0 && dsRMSOnlyAudit.Tables[0].Rows.Count > 0)
        {
            for (int k = 0; k <= dt.Rows.Count - 1; k++)
            {
                for(int y=0;y<= dtRMSOnlyAudit.Rows.Count - 1; y++)
                {
                    if(Convert.ToDateTime(dt.Rows[k]["SERVICE_DATE"].ToString()).ToString("yyyy-MM-dd") == Convert.ToDateTime(dtRMSOnlyAudit.Rows[y]["Date"].ToString()).ToString("yyyy-MM-dd"))
                                {
                            Home AD = new Home();

                            AD.ATMId = dt.Rows[k]["ATM_ID"].ToString();
                            AD.Date = Convert.ToDateTime(dt.Rows[k]["SERVICE_DATE"].ToString()).ToString("yyyy-MM-dd");
                            //AD.Date = Convert.ToDateTime(dt.Rows[k]["SERVICE_DATE"].ToString()).ToString("dd-MMM-yyyy");
                            AD.REGION = dt.Rows[k]["Region"].ToString();
                            AD.BRANCH = dt.Rows[k]["Branch_Name"].ToString();
                            AD.CITY = dt.Rows[k]["City"].ToString();
                            AD.MSP = dt.Rows[k]["MSP_Name"].ToString();
                            AD.BANK = dt.Rows[k]["Bank_Name"].ToString();
                            AD.Route = dt.Rows[k]["route_no"].ToString();
                            AD.Activity = "Audit";
                            AD.Status = "Pending";
                            AD.Custodian1Name = dt.Rows[k]["Custodian1"].ToString();
                            AD.Custodian1RegNo = dt.Rows[k]["Custodian1_RegNo"].ToString();
                            AD.Custodian1Mobile = dt.Rows[k]["Custodian1Mobile"].ToString();
                            AD.Custodian2Name = dt.Rows[k]["Custodian2"].ToString();
                            AD.Custodian2RegNo = dt.Rows[k]["Custodian2_RegNo"].ToString();
                            AD.Custodian2Mobile = dt.Rows[k]["Custodian2Mobile"].ToString();
                            AD.CurrentDatePlusOneDay = Convert.ToDateTime(dt.Rows[k]["SERVICE_DATE"].ToString()).AddDays(1).ToString("yyyy-MM-dd");
                            AD.item_route_sheet_id = dt.Rows[k]["item_route_sheet_id"].ToString();
                            AD.RemarksOne = "";
                            AD.RemarksTwo = "";
                            AD.ActualTime = "";
                            AD.UserName = "";
                            AD.TypeFlag = "SCO";
                            AD.Auditor = auditorName;
                            AD.AuditorId = auditorId;
                            AD.Indent_Uploaded = Convert.ToDateTime(dt.Rows[k]["Indent_Uploaded"].ToString()).ToString("dd-MM-yyyy hh:mm tt");
                            AuditDtls.Add(AD);
                        }
                    
                }
                
            }
            if (dtAuditRms.Rows.Count > 0 && dt.Rows.Count > 0)
            {
                for (int i = 0; i <= dt.Rows.Count - 1; i++) //2
                {
                    string strItemRouteSheetId = dt.Rows[i]["item_route_sheet_id"].ToString();
                    bool isAvaialble = false;

                    for (int j = 0; j <= dtAuditRms.Rows.Count - 1; j++) //3
                    {
                        if (strItemRouteSheetId == dtAuditRms.Rows[j]["Item_Route_Sheet_Id"].ToString())
                        {
                            isAvaialble = true;
                        }
                    }

                    //SCO New Data should be add if found
                    if (isAvaialble == false)
                    {
                        Home AD = new Home();

                        AD.ATMId = dt.Rows[i]["ATM_ID"].ToString();
                        AD.Date = Convert.ToDateTime(dt.Rows[i]["SERVICE_DATE"].ToString()).ToString("yyyy-MM-dd");
                        //AD.Date = Convert.ToDateTime(dt.Rows[i]["SERVICE_DATE"].ToString()).ToString("dd-MMM-yyyy");
                        AD.REGION = dt.Rows[i]["Region"].ToString();
                        AD.BRANCH = dt.Rows[i]["Branch_Name"].ToString();
                        AD.CITY = dt.Rows[i]["City"].ToString();
                        AD.MSP = dt.Rows[i]["MSP_Name"].ToString();
                        AD.BANK = dt.Rows[i]["Bank_Name"].ToString();
                        AD.Route = dt.Rows[i]["route_no"].ToString();
                        AD.Activity = dt.Rows[i]["Activity"].ToString();
                        AD.Status = dt.Rows[i]["Status"].ToString();
                        AD.Custodian1Name = dt.Rows[i]["Custodian1"].ToString();
                        AD.Custodian1RegNo = dt.Rows[i]["Custodian1_RegNo"].ToString();
                        AD.Custodian1Mobile = dt.Rows[i]["Custodian1Mobile"].ToString();
                        AD.Custodian2Name = dt.Rows[i]["Custodian2"].ToString();
                        AD.Custodian2RegNo = dt.Rows[i]["Custodian2_RegNo"].ToString();
                        AD.Custodian2Mobile = dt.Rows[i]["Custodian2Mobile"].ToString();
                        AD.CurrentDatePlusOneDay = Convert.ToDateTime(dt.Rows[i]["SERVICE_DATE"].ToString()).AddDays(1).ToString("yyyy-MM-dd");
                        AD.item_route_sheet_id = dt.Rows[i]["item_route_sheet_id"].ToString();
                        AD.RemarksOne = "";
                        AD.RemarksTwo = "";
                        AD.ActualTime = "";
                        AD.UserName = "";
                        AD.TypeFlag = "BothRMSSCO";
                        AD.Auditor = auditorName;
                        AD.AuditorId = auditorId;
                        AuditDtls.Add(AD);
                    }
                }
            }
        }
        else if (dsCrew.Tables[0].Rows.Count > 0 && dsAudti1.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count == 0 && dsRMSOnlyAudit.Tables[0].Rows.Count > 0 && dsAuditRMS.Tables[0].Rows.Count == 0)
        {
            for (int k = 0; k <= dtRMSOnlyAudit.Rows.Count - 1; k++)
            {
                Home AD = new Home();

                AD.ATMId = dtRMSOnlyAudit.Rows[k]["ATMID"].ToString();
                AD.Date = Convert.ToDateTime(dtRMSOnlyAudit.Rows[k]["Date"].ToString()).ToString("yyyy-MM-dd");
                //AD.Date = Convert.ToDateTime(dt.Rows[k]["SERVICE_DATE"].ToString()).ToString("dd-MMM-yyyy");
                AD.REGION = dtRMSOnlyAudit.Rows[k]["Region"].ToString();
                AD.BRANCH = dtRMSOnlyAudit.Rows[k]["Branch"].ToString();
                AD.CITY = dtRMSOnlyAudit.Rows[k]["Branch"].ToString();
                AD.MSP = dtRMSOnlyAudit.Rows[k]["MSP"].ToString();
                AD.BANK = dtRMSOnlyAudit.Rows[k]["Bank"].ToString();
                AD.Route = dtRMSOnlyAudit.Rows[k]["Route_No"].ToString();
                AD.Activity = dtRMSOnlyAudit.Rows[k]["Activity"].ToString();
                AD.Status = "Pending";
                AD.Custodian1Name = Custodian1;
                AD.Custodian1RegNo = Custodian1RegNo;
                AD.Custodian1Mobile = CustodianMobile1;
                AD.Custodian2Name = Custodian2;
                AD.Custodian2RegNo = Custodian2RegNo;
                AD.Custodian2Mobile = CustodianMobile2;
                AD.CurrentDatePlusOneDay = Convert.ToDateTime(dtRMSOnlyAudit.Rows[k]["Date"].ToString()).AddDays(1).ToString("yyyy-MM-dd");
                AD.item_route_sheet_id = "";
                AD.RemarksOne = "";
                AD.RemarksTwo = "";
                AD.ActualTime = "";
                AD.UserName = "";
                AD.TypeFlag = "";
                AD.Auditor = auditorName;
                AD.AuditorId = auditorId;
                AuditDtls.Add(AD);
            }
        }
        else if (dsCrew.Tables[0].Rows.Count == 0 && dsAudti1.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows.Count == 0 && dsRMSOnlyAudit.Tables[0].Rows.Count > 0 && dsAuditRMS.Tables[0].Rows.Count == 0)
        {
            for (int k = 0; k <= dtRMSOnlyAudit.Rows.Count - 1; k++)
            {
                Home AD = new Home();

                AD.ATMId = dtRMSOnlyAudit.Rows[k]["ATMID"].ToString();
                AD.Date = Convert.ToDateTime(dtRMSOnlyAudit.Rows[k]["Date"].ToString()).ToString("yyyy-MM-dd");
                //AD.Date = Convert.ToDateTime(dt.Rows[k]["SERVICE_DATE"].ToString()).ToString("dd-MMM-yyyy");
                AD.REGION = dtRMSOnlyAudit.Rows[k]["Region"].ToString();
                AD.BRANCH = dtRMSOnlyAudit.Rows[k]["Branch"].ToString();
                AD.CITY = dtRMSOnlyAudit.Rows[k]["Branch"].ToString();
                AD.MSP = dtRMSOnlyAudit.Rows[k]["MSP"].ToString();
                AD.BANK = dtRMSOnlyAudit.Rows[k]["Bank"].ToString();
                AD.Route = dtRMSOnlyAudit.Rows[k]["Route_No"].ToString();
                AD.Activity = dtRMSOnlyAudit.Rows[k]["Activity"].ToString();
                AD.Status = "Pending";
                AD.Custodian1Name = "";
                AD.Custodian1RegNo = "";
                AD.Custodian1Mobile = "";
                AD.Custodian2Name = "";
                AD.Custodian2RegNo = "";
                AD.Custodian2Mobile = "";
                AD.CurrentDatePlusOneDay = Convert.ToDateTime(dtRMSOnlyAudit.Rows[k]["Date"].ToString()).AddDays(1).ToString("yyyy-MM-dd");
                AD.item_route_sheet_id = "";
                AD.RemarksOne = "";
                AD.RemarksTwo = "";
                AD.ActualTime = "";
                AD.UserName = "";
                AD.TypeFlag = "";
                AD.Auditor = auditorName;
                AD.AuditorId = auditorId;
                AuditDtls.Add(AD);
            }
        }
       }
     }

            // end all details from SCO
            //if details not found in SCO

            //}
            //End Audit

            // TESTING END

            //FLM segment starts
            //This data comes when we save the data from portal while giving otc
            dsFLM = HD.GetRMSFlmDetails(FromDate, ToDate, ATMID);
            //dtRMSFlm = dsFLM.Tables[0];

            try
            {
                if (dsFLM.Tables[0].Rows.Count > 0)
                {
                    //Otc data 
                    dtRMSFlm = dsFLM.Tables[0];
                }
            }
            catch
            {
            }

            for (int i = 0; i <= dtRMSFlm.Rows.Count - 1; i++)
            {
                Home AD = new Home();

                AD.FlMATMId = dtRMSFlm.Rows[i]["ATMID"].ToString();
                AD.DocketNo = dtRMSFlm.Rows[i]["DOCKETNUMBER"].ToString();
                AD.Route_Id = dtRMSFlm.Rows[i]["CallType"].ToString(); //Here CallType is RouteNo because table design is old
                AD.Remarks = dtRMSFlm.Rows[i]["Remarks"].ToString();
                AD.OpenClose = dtRMSFlm.Rows[i]["OpenClose"].ToString();
                AD.OPENDATE = Convert.ToDateTime(dtRMSFlm.Rows[i]["Date"]).ToString("yyyy-MM-dd HH:mm:ss");
                //AD.OPENDATE = Convert.ToDateTime(dtRMSFlm.Rows[i]["Date"]).ToString("dd-MMM-yyyy");
                AD.FLMRouteKeyName = dtRMSFlm.Rows[i]["RouteKeyName"].ToString();
                AD.FLMCombinationNo = dtRMSFlm.Rows[i]["CombinationNo"].ToString();
                AD.FLMRemarksOne = dtRMSFlm.Rows[i]["RemarksOne"].ToString();
                AD.FLMRemarksTwo = dtRMSFlm.Rows[i]["RemarksTwo"].ToString();
                AD.FLMCallerNumber = dtRMSFlm.Rows[i]["CallerNumber"].ToString();
                AD.ActualTime = Convert.ToDateTime(dtRMSFlm.Rows[i]["ActualDateTime"].ToString()).ToString("HH:mm");
                AD.UserName = dtRMSFlm.Rows[i]["UserName"].ToString();
                AD.FlmRemarks = dtRMSFlm.Rows[i]["FlmRemarks"].ToString();
                AD.TypeFlag = "RMSFLM";
                lstFlms.Add(AD);
            }

            ds1 = HD.GetFlmDetails(FromDate, ToDate, ATMID);

            try
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    dtFlm = ds1.Tables[0];
                }
            }
            catch
            {
            }

            if (dtRMSFlm.Rows.Count > 0)
            {
                for (int i = 0; i <= dtFlm.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= dtRMSFlm.Rows.Count - 1; j++)
                    {
                        if ((dtRMSFlm.Rows[j]["ATMID"].ToString()) != (dtFlm.Rows[i]["ATMID"].ToString()) && Convert.ToDateTime(dtRMSFlm.Rows[j]["Date"].ToString()).ToString("yyyy-MM-dd") != Convert.ToDateTime(dtFlm.Rows[i]["OPENDATE"].ToString()).ToString("yyyy-MM-dd") && (dtRMSFlm.Rows[j]["DOCKETNUMBER"].ToString()) != (dtFlm.Rows[i]["DOCKETNUMBER"].ToString()))
                        {
                            Home AD = new Home();

                            //FLMDetails 
                            AD.FlMATMId = dtFlm.Rows[i]["ATMID"].ToString();
                            AD.DocketNo = dtFlm.Rows[i]["DOCKETNUMBER"].ToString();
                            //AD.Calltype = dtFlm.Rows[i]["CallType"].ToString(); ////Old
                            AD.Route_Id = dtFlm.Rows[i]["route_id"].ToString();
                            AD.Branch = dtFlm.Rows[i]["Branch_Id"].ToString();
                            AD.Remarks = dtFlm.Rows[i]["Remark"].ToString();
                            AD.OpenClose = dtFlm.Rows[i]["CallStatus"].ToString();
                            AD.OPENDATE = Convert.ToDateTime(dtFlm.Rows[i]["OPENDATE"]).ToString("yyyy-MM-dd");
                            //AD.OPENDATE = Convert.ToDateTime(dtFlm.Rows[i]["OPENDATE"]).ToString("dd-MMM-yyyy");
                            AD.ActualTime = "";
                            AD.UserName = "";
                            AD.TypeFlag = "FLM";
                            AD.FlmRemarks = dtFlm.Rows[i]["comment"].ToString();
                            AD.Route_Id = dtFlm.Rows[i]["route_id"].ToString();
                            AD.BranchName = dtFlm.Rows[i]["BRANCHNAME"].ToString();
                            lstFlms.Add(AD);
                        }
                    }
                }
            }
            else
            {
                for (int j = 0; j <= dtFlm.Rows.Count - 1; j++)
                {
                    Home AD = new Home();

                    AD.FlMATMId = dtFlm.Rows[j]["ATMID"].ToString();
                    AD.DocketNo = dtFlm.Rows[j]["DOCKETNUMBER"].ToString();
                    //AD.Calltype = dtFlm.Rows[j]["CallType"].ToString(); Old
                    AD.Route_Id = dtFlm.Rows[j]["route_id"].ToString();
                    AD.Branch = dtFlm.Rows[j]["Branch_Id"].ToString();
                    AD.Remarks = dtFlm.Rows[j]["CallType"].ToString();
                    AD.OpenClose = dtFlm.Rows[j]["CallStatus"].ToString();
                    AD.OPENDATE = Convert.ToDateTime(dtFlm.Rows[j]["OPENDATE"]).ToString("yyyy-MM-dd HH:mm:ss");
                    //AD.OPENDATE = Convert.ToDateTime(dtFlm.Rows[j]["OPENDATE"]).ToString("dd-MMM-yyyy");
                    AD.ActualTime = "";
                    AD.UserName = "";
                    AD.TypeFlag = "FLM";
                    AD.FlmRemarks = dtFlm.Rows[j]["comment"].ToString();
                    AD.Route_Id = dtFlm.Rows[j]["route_id"].ToString();
                    AD.BranchName = dtFlm.Rows[j]["BRANCHNAME"].ToString();
                    lstFlms.Add(AD);
                }
            }

            if (dtRMSFlm.Rows.Count > 0 && dtFlm.Rows.Count > 0)
            {
                for (int i = 0; i <= dtFlm.Rows.Count - 1; i++) //2
                {
                    string strFLMDocketNo = dtFlm.Rows[i]["DOCKETNUMBER"].ToString();
                    bool isAvaialble = false;

                    for (int j = 0; j <= dtRMSFlm.Rows.Count - 1; j++) //3
                    {
                        if (strFLMDocketNo == dtRMSFlm.Rows[j]["DOCKETNUMBER"].ToString())
                        {
                            isAvaialble = true;
                        }
                    }

                    //FLM New Data should be add if found
                    if (isAvaialble == false)
                    {
                        Home AD = new Home();

                        //FLMDetails 
                        AD.FlMATMId = dtFlm.Rows[i]["ATMID"].ToString();
                        AD.DocketNo = dtFlm.Rows[i]["DOCKETNUMBER"].ToString();
                        //AD.Calltype = dtFlm.Rows[i]["CallType"].ToString(); Old
                        AD.Route_Id = dtFlm.Rows[i]["route_id"].ToString();
                        AD.Branch = dtFlm.Rows[i]["Branch_Id"].ToString();
                        AD.Remarks = dtFlm.Rows[i]["CallType"].ToString();
                        AD.OpenClose = dtFlm.Rows[i]["CallStatus"].ToString();
                        AD.OPENDATE = Convert.ToDateTime(dtFlm.Rows[i]["OPENDATE"]).ToString("yyyy-MM-dd HH:mm:ss");
                        //AD.OPENDATE = Convert.ToDateTime(dtFlm.Rows[i]["OPENDATE"]).ToString("dd-MMM-yyyy");
                        AD.ActualTime = "";
                        AD.UserName = "";
                        AD.TypeFlag = "BothRMSFLMData";
                        AD.FlmRemarks = dtFlm.Rows[i]["comment"].ToString();
                        AD.Route_Id = dtFlm.Rows[i]["route_id"].ToString();
                        AD.BranchName = dtFlm.Rows[i]["BRANCHNAME"].ToString();

                        lstFlms.Add(AD);
                    }
                }
            }

            //End of the FLM Data

            dsOther = HD.GetOtherDetails(ATMID, FromDate, ToDate);

            try
            {
                if (dsOther.Tables[0].Rows.Count > 0)
                {
                    dtOther = dsOther.Tables[0];
                }
            }
            catch
            {
            }

            for (int j = 0; j <= dtOther.Rows.Count - 1; j++)
            {
                Home AD = new Home();

                AD.OtherATMID = dtOther.Rows[j]["ATMID"].ToString();
                AD.OtherDate = Convert.ToDateTime(dtOther.Rows[j]["Date"].ToString()).ToString("dd-MMM-yyyy");
                AD.OtherActivity = dtOther.Rows[j]["Activity"].ToString();
                AD.OtherRouteKeyName = dtOther.Rows[j]["RouteKeyName"].ToString();
                AD.OtherCombinationNo = dtOther.Rows[j]["CombinationNo"].ToString();
                AD.OtherRemarksOne = dtOther.Rows[j]["RemarksOne"].ToString();
                AD.OtherRemarksTwo = dtOther.Rows[j]["RemarksTwo"].ToString();
                AD.OtherCallerNumber = dtOther.Rows[j]["CallerNumber"].ToString();
                AD.OtherCombinationIssuedBy = dtOther.Rows[j]["UserName"].ToString();
                AD.OtherCombinationIssuedTime = Convert.ToDateTime(dtOther.Rows[j]["ActualDateTime"].ToString()).ToString("HH:mm");
                AD.TypeFlag = "OtherRMS";
                lstOtherRms.Add(AD);
            }

            dsFLMSiteTime = HD.GetSiteDetailsDetails(ATMID);
            //dtFLMSiteTime = dsFLMSiteTime.Tables[0];

            try
            {
                if (dsFLMSiteTime.Tables[0].Rows.Count > 0)
                {
                    dtFLMSiteTime = dsFLMSiteTime.Tables[0];
                }
            }
            catch
            {
            }

            if (dtFLMSiteTime.Rows.Count > 0)
            {
                //for (int l = 0; l <= dtFLMSiteTime.Rows.Count - 1; l++)
                //{
                //    Home AD = new Home();

                //    AD.SiteAccessTime = dtFLMSiteTime.Rows[l]["AccessTime"].ToString();
                //    AD.OperationAccessTime = dtFLMSiteTime.Rows[l]["OperationHour"].ToString();
                //    lstDetails.Add(AD);
                //}
                flmSite = dtFLMSiteTime.AsEnumerable().Select(r => new FlmSiteCls
                {
                    SiteAccessTime = r.Field<string>("AccessTime"),
                    OperationAccessTime = r.Field<string>("OperationHour")
                }).FirstOrDefault();
            }
            //else
            //{
            //Home AD = new Home();

            //AD.SiteAccessTime = "";
            //AD.OperationAccessTime = "";

            //lstDetails.Add(AD);

            //}
            //get route status detail
            RouteStatus routeStatus = GetOTCRouteStatus(FromDate, ToDate, ATMID).FirstOrDefault();
            if (routeStatus!=null && !string.IsNullOrEmpty(routeStatus.AtmId))
            {
                routeStatus.LastOtcTaken = Convert.ToDateTime(routeStatus.LastOtcTaken).ToString("dd-MM-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                DateTime lasttaken = Convert.ToDateTime(routeStatus.LastOtcTaken);
                DateTime currdate = DateTime.Now;
                TimeSpan ts = currdate.Subtract(lasttaken);
                int timeInMinute = Convert.ToInt32(Math.Floor(ts.TotalMinutes));
                routeStatus.timefromCurrent = timeInMinute;
            }

            return Json(new { lstDetails, flmSite, routeStatus, lstFlms, lstOtherRms, AuditDtls });
        }

        [HttpPost]
        public JsonResult SaveScoDetails(string ATMId, string Date, string REGION, string BRANCH, string CITY, string MSP, string BANK, string Route, string Activity,
                                         string Status, string Custodian1Name, string Custodian1RegNo, string Custodian2Name, string Custodian2RegNo, string CallerNumber,
                                         string RouteKeyName, string CombinationNo, string RemarksOne, string RemarksTwo, string ItemRouteSheetId,string Custodian1Mobile)
        {
            int return1 = 0;
            string UserID = Session["UserId"].ToString();
            var resultSuccess = new { Message = "Successfully Inserted" };
            var resultError = new { Message = "Error Please Check" };

            return1 = HD.ScoDetailsInsert(ATMId, Date, REGION, BRANCH, CITY, MSP, BANK, Route, Activity, Status, Custodian1Name, Custodian1RegNo, Custodian2Name,
                                          Custodian2RegNo, CallerNumber, RouteKeyName, CombinationNo, RemarksOne, RemarksTwo, UserID, ItemRouteSheetId, Custodian1Mobile);

            if (return1 > 0)
            {
                return Json(resultSuccess, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(resultError, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public JsonResult SaveAuditScoDetails(string ATMId, string Date, string REGION, string BRANCH, string CITY, string MSP, string BANK, string Route, string Activity,
                                         string Status, string Custodian1Name, string Custodian1RegNo, string Custodian2Name, string Custodian2RegNo, string CallerNumber,
                                         string RouteKeyName, string CombinationNo, string RemarksOne, string RemarksTwo, string ItemRouteSheetId, string Custodian1Mobile,string AuditorId)
        {
            int return1 = 0;
            string UserID = Session["UserId"].ToString();
            var resultSuccess = new { Message = "Successfully Inserted" };
            var resultError = new { Message = "Error Please Check" };

            return1 = HD.AuditScoDetailsInsert(ATMId, Date, REGION, BRANCH, CITY, MSP, BANK, Route, Activity, Status, Custodian1Name, Custodian1RegNo, Custodian2Name,
                                          Custodian2RegNo, CallerNumber, RouteKeyName, CombinationNo, RemarksOne, RemarksTwo, UserID, ItemRouteSheetId, Custodian1Mobile, AuditorId);

            if (return1 > 0)
            {
                return Json(resultSuccess, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(resultError, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult SaveFlmDetails(string ATMId, string Date, string DocketNumber, string CallType, string Remarks, string OpenClose, string CallerNumber,
                                         string RouteKeyName, string CombinationNo, string RemarksOne, string RemarksTwo, string FlmRemarks)
        {
            int return1 = 0;
            string UserID = Session["UserId"].ToString();
            var resultSuccess = new { Message = "Successfully Inserted" };
            var resultError = new { Message = "Error Please Check" };

            return1 = HD.FlmDetailsInsert(ATMId, Date, DocketNumber, CallType, Remarks, OpenClose, CallerNumber,
                                          RouteKeyName, CombinationNo, RemarksOne, RemarksTwo, UserID, FlmRemarks);

            if (return1 > 0)
            {
                return Json(resultSuccess, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(resultError, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult ManualDetailsSave(string Date, string Activity, string ATMID, string CallerNumber, string RouteKeyName, string CombinationNo, string RemarksOne, string RemarksTwo, string ItemRouteSheetId)
        {
            int return1 = 0;
            string UserID = Session["UserId"].ToString();
            var resultSuccess = new { Message = "Successfully Inserted" };
            var resultError = new { Message = "Error Please Check" };

            DataSet ds = HD.GetMasterDetails(ATMID);
            var now = DateTime.Now;
            var Today = now.ToString("yyyy-MM-dd hh:mm:ss");

            if (ItemRouteSheetId == null)
            {
                ItemRouteSheetId = "";
            }

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Activity == "Cash Loading" || Activity == "EOD")
                {
                    return1 = HD.ScoDetailsInsert(ATMID, Today, ds.Tables[0].Rows[0]["Region"].ToString(), ds.Tables[0].Rows[0]["Branch"].ToString(), ds.Tables[0].Rows[0]["City"].ToString(), ds.Tables[0].Rows[0]["MSP"].ToString(), ds.Tables[0].Rows[0]["Bank"].ToString(), ds.Tables[0].Rows[0]["Route_No"].ToString(), Activity, "", "", "", "", "", CallerNumber, RouteKeyName, CombinationNo, RemarksOne, RemarksTwo, UserID, ItemRouteSheetId, "");
                }
                else if (Activity == "FLM" || Activity == "SLM" || Activity == "Fake Closer FLM Visit")
                {
                    return1 = HD.FlmDetailsInsert(ATMID, Today, "", "", Activity, "", CallerNumber, RouteKeyName, CombinationNo, RemarksOne, RemarksTwo, UserID, "");
                }
                else
                {
                    return1 = HD.OtherDetailsInsert(ATMID, Today, ds.Tables[0].Rows[0]["Region"].ToString(), ds.Tables[0].Rows[0]["Branch"].ToString(), ds.Tables[0].Rows[0]["City"].ToString(), ds.Tables[0].Rows[0]["MSP"].ToString(), ds.Tables[0].Rows[0]["Bank"].ToString(), ds.Tables[0].Rows[0]["Route_No"].ToString(), Activity, CallerNumber, RouteKeyName, CombinationNo, RemarksOne, RemarksTwo, UserID);
                }
            }
            else
            {
                if (Activity == "Cash Loading" || Activity == "EOD")
                {
                    return1 = HD.ScoDetailsInsert(ATMID, Today, "", "", "", "", "", "", Activity, "", "", "", "", "", CallerNumber, RouteKeyName, CombinationNo, RemarksOne, RemarksTwo, UserID, "", "");
                }
                else if (Activity == "FLM" || Activity == "SLM" || Activity == "Fake Closer FLM Visit")
                {
                    return1 = HD.FlmDetailsInsert(ATMID, Today, "", "", Activity, "", CallerNumber, RouteKeyName, CombinationNo, RemarksOne, RemarksTwo, UserID, "");
                }
                else
                {
                    return1 = HD.OtherDetailsInsert(ATMID, Today, "", "", "", "", "", "", Activity, CallerNumber, RouteKeyName, CombinationNo, RemarksOne, RemarksTwo, UserID);
                }
            }
            if (return1 > 0)
            {
                return Json(resultSuccess, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(resultError, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult TotalCountValue(string Date, string UserId)
        {
            DataSet ds = HD.GetTotalCountDetails(Date,UserId);
            DataTable dt = new DataTable();

            dt = ds.Tables[0];

            ViewBag.TotalCountValue = ds.Tables[0].Rows[0]["Value"].ToString();

            List<Home> lstDetails = new List<Home>();
            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Home AD = new Home();
                AD.Value = dt.Rows[i]["Value"].ToString();
                lstDetails.Add(AD);
            }
            return Json(lstDetails);
        }

        [HttpPost]
        public JsonResult SaveMessageDetails(string Message, string UserId, string Isinsert)
        {
            int return1 = 0;

            var resultSuccess = new { Message = "Successfully Inserted" };
            var resultError = new { Message = "Error Please Check" };

            return1 = HD.MessageDetailsInsert(Message, UserId, Isinsert);

            if (return1 > 0)
            {
                return Json(resultSuccess, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(resultError, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SearchMessageDetails()
        {
            string Todate = DateTime.Now.ToString("yyyy-MM-dd");
            DataSet ds = HD.SearchMessageDetails(Todate);
            DataTable dt = new DataTable();

            dt = ds.Tables[0];

            List<Home> lstDetails = new List<Home>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Home AD = new Home();
                AD.MessageBox = dt.Rows[i]["MessageBox"].ToString();
                AD.RecId = dt.Rows[i]["Rec_Id"].ToString();
                lstDetails.Add(AD);
            }

            return Json(lstDetails);
        }

        [HttpPost]
        public JsonResult CountMessageNotification()
        {
            DataSet ds = HD.CountMessageNotification();
            DataTable dt = new DataTable();

            dt = ds.Tables[0];
            string count;
            count = dt.Rows[0][0].ToString();
            return Json(count);
        }

        [HttpPost]
        public JsonResult RandomQuestion(string CustodianIdOne, string CustodianIdTwo, string Branch)
        {
            if (CustodianIdOne == "" || CustodianIdOne == null)
            {
                CustodianIdOne = "1";
            }
            if (CustodianIdTwo == "" || CustodianIdTwo == null)
            {
                CustodianIdTwo = "2";
            }
            List<Home> lstDetails = new List<Home>();
            Home AD = new Home();
            try
            {
                DataSet ds = HD.GetRandomQuestionDetails(CustodianIdOne, CustodianIdTwo, Branch);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    AD.CustodianOneQ1 = ds.Tables[0].Rows[0]["Question"].ToString();
                    AD.CustodianOneQ2 = ds.Tables[0].Rows[1]["Question"].ToString();
                    AD.CustodianTwoQ1 = ds.Tables[1].Rows[0]["Question"].ToString();
                    AD.CustodianTwoQ2 = ds.Tables[1].Rows[1]["Question"].ToString();

                    lstDetails.Add(AD);
                }
                else
                {
                    AD.CustodianOneQ1 = "No Information Found";
                    AD.CustodianOneQ2 = "No Information Found";
                    AD.CustodianTwoQ1 = "No Information Found";
                    AD.CustodianTwoQ2 = "No Information Found";

                    lstDetails.Add(AD);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                AD.CustodianOneQ1 = "No Information Found";
                AD.CustodianOneQ2 = "No Information Found";
                AD.CustodianTwoQ1 = "No Information Found";
                AD.CustodianTwoQ2 = "No Information Found";

                lstDetails.Add(AD);
            }
            return Json(lstDetails);
        }

        [HttpPost]
        public JsonResult SaveRandomQuestions(string QuestionOne, string QuestionTwo, string QuestionThree, string QuestionFour, string ATMId, string Date, string RouteNo,
                                              string CustodianOneQuestionOne, string CustodianOneQuestionTwo, string CustodianTwoQuestionOne, string CustodianTwoQuestionTwo, string Itefolrot)
        {
            string UserID = Session["UserId"].ToString();
            int return1 = 0;

            var resultSuccess = new { Message = "Successfully Inserted" };
            var resultError = new { Message = "Error Please Check" };

            return1 = HD.SaveRandomQuestionsInsert(QuestionOne, QuestionTwo, QuestionThree, QuestionFour, ATMId, Date, RouteNo,
                (!string.IsNullOrWhiteSpace(CustodianOneQuestionOne) ? CustodianOneQuestionOne : ""),
                (!string.IsNullOrWhiteSpace(CustodianOneQuestionTwo) ? CustodianOneQuestionTwo : ""),
                (!string.IsNullOrWhiteSpace(CustodianTwoQuestionOne) ? CustodianTwoQuestionOne : ""),
                (!string.IsNullOrWhiteSpace(CustodianTwoQuestionTwo) ? CustodianTwoQuestionTwo : ""), 
                (!string.IsNullOrWhiteSpace(Itefolrot) ? Itefolrot : ""), UserID);

            if (return1 > 0)
            {
                return Json(resultSuccess, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(resultError, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult RandomQuestionAudit(string CustodianIdOne, string CustodianIdTwo,string AuditorId, string Branch)
        {
            if (CustodianIdOne == "" || CustodianIdOne == null)
            {
                CustodianIdOne = "1";
            }
            if (CustodianIdTwo == "" || CustodianIdTwo == null)
            {
                CustodianIdTwo = "2";
            }
            if(AuditorId == "" || AuditorId == null)
            {
                AuditorId = "3";
            }
            List<Home> lstDetails = new List<Home>();
            Home AD = new Home();
            try
            {
                DataSet ds = HD.GetAuditRandomQuestionDetails(CustodianIdOne, CustodianIdTwo, AuditorId, Branch);
            if (CustodianIdOne != "1" && CustodianIdTwo != "2")
                {
                if (ds.Tables[0].Rows.Count > 0)
                {
                  AD.CustodianOneQ1 = ds.Tables[0].Rows[0]["Question"].ToString();
                  AD.CustodianOneQ2 = ds.Tables[0].Rows[1]["Question"].ToString();
                  AD.CustodianTwoQ1 = ds.Tables[1].Rows[0]["Question"].ToString();
                  AD.CustodianTwoQ2 = ds.Tables[1].Rows[1]["Question"].ToString();
                  AD.AuditorQ1 = ds.Tables[2].Rows[0]["Question"].ToString();
                  AD.AuditorQ2 = ds.Tables[2].Rows[1]["Question"].ToString();

                  lstDetails.Add(AD);
                }
                else
                {
                  AD.CustodianOneQ1 = "No Information Found";
                  AD.CustodianOneQ2 = "No Information Found";
                  AD.CustodianTwoQ1 = "No Information Found";
                  AD.CustodianTwoQ2 = "No Information Found";

                  lstDetails.Add(AD);
                }
              }

              if (CustodianIdOne == "1" || CustodianIdTwo == "2" && AuditorId !="3")
              {
                if (ds.Tables[0].Rows.Count > 0)
                {
                  AD.AuditorQ1 = ds.Tables[0].Rows[0]["Question"].ToString();
                  AD.AuditorQ2 = ds.Tables[0].Rows[1]["Question"].ToString();

                  lstDetails.Add(AD);
                }
                else
                {
                AD.AuditorQ1 = "No Information Found";
                AD.AuditorQ2 = "No Information Found";

                    lstDetails.Add(AD);
                    }
                  }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                AD.CustodianOneQ1 = "No Information Found";
                AD.CustodianOneQ2 = "No Information Found";
                AD.CustodianTwoQ1 = "No Information Found";
                AD.CustodianTwoQ2 = "No Information Found";
                AD.AuditorQ1 = "No Information Found";
                AD.AuditorQ2 = "No Information Found";

                lstDetails.Add(AD);
            }
            return Json(lstDetails);
        }

        [HttpPost]
        public JsonResult FLMRandomQuestion(string CustodianPhoneNumber1, string CustodianPhoneNumber2)
        {

            DataSet ds = HD.GetFLMRandomQuestionDetails(CustodianPhoneNumber1, CustodianPhoneNumber2);

            List<Home> lstDetails = new List<Home>();
            Home AD = new Home();

            if (ds.Tables[0].Rows.Count > 0)
            {
                AD.CustodianOneQ1 = ds.Tables[0].Rows[0]["Question"].ToString();
                AD.CustodianOneQ2 = ds.Tables[0].Rows[1]["Question"].ToString();
                AD.CustodianOneId = ds.Tables[0].Rows[1]["RegNoErp"].ToString();
                AD.CustodianOneName = ds.Tables[0].Rows[1]["EmployeeName"].ToString();
                AD.CustodianTwoQ1 = ds.Tables[1].Rows[0]["Question"].ToString();
                AD.CustodianTwoQ2 = ds.Tables[1].Rows[1]["Question"].ToString();
                AD.CustodianTwoId = ds.Tables[1].Rows[1]["RegNoErp"].ToString();
                AD.CustodianTwoName = ds.Tables[1].Rows[1]["EmployeeName"].ToString();

                lstDetails.Add(AD);
            }
            else
            {
                AD.CustodianOneQ1 = "No Information Found";
                AD.CustodianOneQ2 = "No Information Found";
                AD.CustodianOneId = "No Information Found";
                AD.CustodianOneName = "No Information Found";
                AD.CustodianTwoQ1 = "No Information Found";
                AD.CustodianTwoQ2 = "No Information Found";
                AD.CustodianTwoId = "No Information Found";
                AD.CustodianTwoName = "No Information Found";

                lstDetails.Add(AD);
            }

            return Json(lstDetails);
        }

        [HttpPost]
        public JsonResult SaveFLMRandomQuestions(string QuestionOne, string QuestionTwo, string QuestionThree, string QuestionFour,
                                                 string ATMId, string DocketNumber, string FLMDate,
                                                 string CustodianOneQuestionOne, string CustodianOneQuestionTwo, string CustodianOneId, string CustodianOneName,
                                                 string CustodianOnePhone,string CustodianTwoQuestionOne,string CustodianTwoQuestionTwo,string CustodianTwoId,
                                                 string CustodianTwoName, string CustodianTwoPhone)
        {
            int return1 = 0;
            string UserID = Session["UserId"].ToString();
            var resultSuccess = new { Message = "Successfully Inserted" };
            var resultError = new { Message = "Error Please Check" };

            return1 = HD.SaveRandomQuestionsFLMInsert(QuestionOne, QuestionTwo, QuestionThree, QuestionFour,ATMId, DocketNumber, FLMDate,
                                                     CustodianOneQuestionOne, CustodianOneQuestionTwo, CustodianOneId, CustodianOneName,
                                                     CustodianOnePhone,CustodianTwoQuestionOne,CustodianTwoQuestionTwo,CustodianTwoId,
                                                     CustodianTwoName, CustodianTwoPhone, UserID);

            if (return1 > 0)
            {
                return Json(resultSuccess, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(resultError, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SearchLast3OTCDetails(string Date, string Route, string Branch)
        {
            DataSet ds = HD.GetLast3OTCDetails(Date, Route, Branch);

            List<Home> lstLast3Details = new List<Home>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    Home AD = new Home();

                    AD.ATMID = ds.Tables[0].Rows[i]["ATMID"].ToString();
                    AD.Branch = ds.Tables[0].Rows[i]["Branch"].ToString();
                    AD.RouteNo = ds.Tables[0].Rows[i]["RouteNo"].ToString();
                    AD.Activity = ds.Tables[0].Rows[i]["Activity"].ToString();
                    AD.Crew1Name = ds.Tables[0].Rows[i]["Custodian1Name"].ToString();
                    AD.Crew1Regno = ds.Tables[0].Rows[i]["Custodian1RegNo"].ToString();
                    AD.Crew2Name = ds.Tables[0].Rows[i]["Custodian2Name"].ToString();
                    AD.Crew2Regno = ds.Tables[0].Rows[i]["Custodian2RegNo"].ToString();
                    AD.LastOTCTaken = ds.Tables[0].Rows[i]["lastOtc"].ToString();
                    AD.CallerNumber = ds.Tables[0].Rows[i]["CallerNumber"].ToString();
                    AD.RouteKeyName = ds.Tables[0].Rows[i]["RouteKeyName"].ToString();
                    AD.CombinationNo = ds.Tables[0].Rows[i]["CombinationNo"].ToString();
                    AD.RemarksOne = ds.Tables[0].Rows[i]["RemarksOne"].ToString();
                    AD.RemarksTwo = ds.Tables[0].Rows[i]["RemarksTwo"].ToString();
                    AD.Slno = ds.Tables[0].Rows[i]["Slno"].ToString();

                    lstLast3Details.Add(AD);
                }

            }

            return Json(lstLast3Details);
        }

        //Last 3 Audit OTC Details
        [HttpPost]
        public JsonResult SearchLast3AuditOTCDetails(string Date, string Route, string Branch)
        {
            DataSet ds = HD.GetLast3AuditOTCDetails(Date, Route, Branch);

            List<Home> lstLast3Details = new List<Home>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    Home AD = new Home();

                    AD.ATMID = ds.Tables[0].Rows[i]["ATMID"].ToString();
                    AD.Branch = ds.Tables[0].Rows[i]["Branch"].ToString();
                    AD.RouteNo = ds.Tables[0].Rows[i]["RouteNo"].ToString();
                    AD.Activity = ds.Tables[0].Rows[i]["Activity"].ToString();
                    AD.Crew1Name = ds.Tables[0].Rows[i]["Custodian1Name"].ToString();
                    AD.Crew1Regno = ds.Tables[0].Rows[i]["Custodian1RegNo"].ToString();
                    AD.Crew2Name = ds.Tables[0].Rows[i]["Custodian2Name"].ToString();
                    AD.Crew2Regno = ds.Tables[0].Rows[i]["Custodian2RegNo"].ToString();
                    AD.LastOTCTaken = ds.Tables[0].Rows[i]["lastOtc"].ToString();
                    AD.CallerNumber = ds.Tables[0].Rows[i]["CallerNumber"].ToString();
                    AD.RouteKeyName = ds.Tables[0].Rows[i]["RouteKeyName"].ToString();
                    AD.CombinationNo = ds.Tables[0].Rows[i]["CombinationNo"].ToString();
                    AD.RemarksOne = ds.Tables[0].Rows[i]["RemarksOne"].ToString();
                    AD.RemarksTwo = ds.Tables[0].Rows[i]["RemarksTwo"].ToString();
                    AD.Slno = ds.Tables[0].Rows[i]["Slno"].ToString();

                    lstLast3Details.Add(AD);
                }

            }

            return Json(lstLast3Details);
        }
        [HttpPost]
        public JsonResult SearchLast3FLMOTCDetails(string Date, string Route, string Branch)
        {
            DataSet ds = HD.GetLast3FLMOTCDetails(Date, Route, Branch);

            List<Home> lstLast3Details = new List<Home>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    Home AD = new Home();

                    AD.ATMID = ds.Tables[0].Rows[i]["ATMID"].ToString();
                    AD.Branch = ds.Tables[0].Rows[i]["Branch"].ToString();
                    AD.RouteNo = ds.Tables[0].Rows[i]["RouteNo"].ToString();
                    AD.Activity = ds.Tables[0].Rows[i]["Activity"].ToString();
                    AD.Crew1Name = ds.Tables[0].Rows[i]["Custodian1Name"].ToString();
                    AD.Crew1Regno = ds.Tables[0].Rows[i]["Custodian1RegNo"].ToString();
                    AD.Crew2Name = ds.Tables[0].Rows[i]["Custodian2Name"].ToString();
                    AD.Crew2Regno = ds.Tables[0].Rows[i]["Custodian2RegNo"].ToString();
                    AD.LastOTCTaken = ds.Tables[0].Rows[i]["lastOtc"].ToString();
                    AD.CallerNumber = ds.Tables[0].Rows[i]["CallerNumber"].ToString();
                    AD.RouteKeyName = ds.Tables[0].Rows[i]["RouteKeyName"].ToString();
                    AD.CombinationNo = ds.Tables[0].Rows[i]["CombinationNo"].ToString();
                    AD.RemarksOne = ds.Tables[0].Rows[i]["RemarksOne"].ToString();
                    AD.RemarksTwo = ds.Tables[0].Rows[i]["RemarksTwo"].ToString();
                    AD.Slno = ds.Tables[0].Rows[i]["Slno"].ToString();

                    lstLast3Details.Add(AD);
                }

            }

            return Json(lstLast3Details);
        }

        string strRouteKeyName = "https://rms.sisprosegur.com/ReconVerifierWebApi/api/OTC/RouteKeyIdDetails";
        string strGenerateOTC = "https://rms.sisprosegur.com/ReconVerifierWebApi/api/OTC/GenerateOTC";
        //string strRouteKeyName = "http://localhost:1098/api/OTC/RouteKeyIdDetails";
        //string strGenerateOTC = "http://localhost:1098/api/OTC/GenerateOTC";

        string strBasicAuthentication = "ReconVerifier:RVerfierPortal@6197";

        public JsonResult RetriveRouteKeyName(string Company, string prefix)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(strRouteKeyName);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", strBasicAuthentication);

            Routekey R = new Routekey();
            R.Company = Company;
            //HTTP POST
            var postTask = client.PostAsJsonAsync(client.BaseAddress.ToString(), R);

            var result = postTask.Result;

            List<RoutekeyNameDetails> details = new List<RoutekeyNameDetails>();
            List<RoutekeyNameDetails> details2 = new List<RoutekeyNameDetails>();
            DataTable dt2 = new DataTable();

            if (result.IsSuccessStatusCode)
            {
                var strResult = postTask.Result.Content.ReadAsStringAsync().Result.ToString();

                details = JsonConvert.DeserializeObject<List<RoutekeyNameDetails>>(strResult);

                DataTable dt = JsonStringToDataTable(strResult);
                DataRow[] dr = dt.Select("RouteKeyId like '%" + prefix + "%'");
                dt2.Columns.Add("RouteKeyName");

                for (int i = 0; i <= dr.Length - 1; i++)
                {

                    dt2.Rows.Add(dr[i].ItemArray[0]);

                    RoutekeyNameDetails rnd = new RoutekeyNameDetails();
                    rnd.RouteKeyId = dr[i].ItemArray[0].ToString();
                    details2.Add(rnd);
                }
            }
            else
            {
            }

            return Json(new { Success = details2 }, JsonRequestBehavior.AllowGet);

        }

        public DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dt.Columns.Add(AddColumnName);
            }
            foreach (string jSA in jsonStringArray)
            {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                        nr[RowColumns] = RowDataString;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }

        [HttpPost]
        public JsonResult GetCompany(string ATMID)
        {
            DataSet ds = HD.GetCompanyDetails(ATMID);

            List<Home> lstCompany = new List<Home>();

            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
                {
                    Home AD = new Home();

                    AD.Company = ds.Tables[0].Rows[i]["COMPANY"].ToString();

                    lstCompany.Add(AD);
                }
            }

            return Json(lstCompany);
        }

        [HttpPost]
        public JsonResult RetriveATMLock(string ATMID, string RouteKeyName, string TimeBlock, string LockStatus)
        {
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(strGenerateOTC);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", strBasicAuthentication);

            DataSet ds = HD.GetCompanyDetails(ATMID);

            Generatekey gnk = new Generatekey();
            gnk.AtmId = ATMID;
            gnk.OTCRouteKeyId = RouteKeyName;
            gnk.Company = ds.Tables[0].Rows[0]["COMPANY"].ToString();
            gnk.OTCUserId = Session["UserId"].ToString();
            gnk.OTCUserPWD = Session["UserPass"].ToString();
            gnk.TimeBlock = TimeBlock;
            gnk.LockStatus = LockStatus;

            OTCReturnData otcreturn = GenerateOTC(gnk);
            ////client.Timeout = 120000;
            ////HTTP POST
            //var postTask = client.PostAsJsonAsync(client.BaseAddress.ToString(), gnk);

            //var result = postTask.Result;

            ResponseGeneratekey details = new ResponseGeneratekey();
            details.OTC = otcreturn.OTC;
            details.CallerNo = otcreturn.CallerNo;
            details.Msg = otcreturn.Msg;
            //if (result.IsSuccessStatusCode)
            //{
            //    var strResult = postTask.Result.Content.ReadAsStringAsync().Result.ToString();

            //    details = JsonConvert.DeserializeObject<ResponseGeneratekey>(strResult);

            //}
            //else
            //{
            //}

            return Json(details, JsonRequestBehavior.AllowGet);

        }
        public List<RouteStatus> GetOTCRouteStatus(string FromDate, string ToDate, string ATMID)
        {
            //DataSet dsPendingRouteStatus = new DataSet();
            DataSet dsPendingRouteStatusRMS = new DataSet();

            //DataTable dtPendingRouteStatus = new DataTable();
            DataTable dtPendingRouteStatusRMS = new DataTable();

            List<RouteStatus> lstRouteDetails = new List<RouteStatus>();

            dsPendingRouteStatusRMS = HD.GetPendingStatusRMS(FromDate, ToDate, ATMID);

            if (dsPendingRouteStatusRMS.Tables[0].Rows.Count > 0)
            {
                dtPendingRouteStatusRMS = dsPendingRouteStatusRMS.Tables[0];
                if (dtPendingRouteStatusRMS.Rows.Count > 0)
                {
                    lstRouteDetails = dtPendingRouteStatusRMS.AsEnumerable().Select(r => new RouteStatus
                    {
                        AtmId = r.Field<string>("ATMID"),
                        RouteNo = r.Field<string>("RouteNo"),
                        Status = r.Field<string>("Status"),//dtPendingRouteStatus.Rows[0]["ITEM_SITUATION_NAME"].ToString(),//get status from SCO
                        LastOtcTaken = r.Field<DateTime>("Actual_Date_Time").ToString()
                    }).ToList();
                }
                //dsPendingRouteStatus = HD.GetPendingStatus(FromDate, ToDate, dsPendingRouteStatusRMS.Tables[0].Rows[0]["ATMID"].ToString().Trim());
                //if (dsPendingRouteStatus.Tables[0].Rows.Count > 0)
                //{
                //    dtPendingRouteStatus = dsPendingRouteStatus.Tables[0];
                //    if (dtPendingRouteStatusRMS.Rows[0]["ATMID"].ToString().Trim() == dtPendingRouteStatus.Rows[0]["CLIENT_ATM_ID"].ToString().Trim())
                //    {


                //    }
                //}
            }
            return lstRouteDetails;
        }
        //commented this logic after discussed with pradeep --01-12-2021
        //public List<RouteStatus> GetOTCRouteStatus(string FromDate,string ToDate,string ATMID)
        //{
        //    DataSet dsPendingRouteStatus = new DataSet();
        //    DataSet dsPendingRouteStatusRMS = new DataSet();

        //    DataTable dtPendingRouteStatus = new DataTable();
        //    DataTable dtPendingRouteStatusRMS = new DataTable();

        //    List<RouteStatus> lstRouteDetails = new List<RouteStatus>();

        //    dsPendingRouteStatusRMS = HD.GetPendingStatusRMS(FromDate, ToDate, ATMID);

        //    if (dsPendingRouteStatusRMS.Tables[0].Rows.Count > 0)
        //    {
        //        dtPendingRouteStatusRMS = dsPendingRouteStatusRMS.Tables[0];
        //        dsPendingRouteStatus = HD.GetPendingStatus(FromDate, ToDate, dsPendingRouteStatusRMS.Tables[0].Rows[0]["ATMID"].ToString().Trim());
        //        if (dsPendingRouteStatus.Tables[0].Rows.Count > 0)
        //        {
        //            dtPendingRouteStatus = dsPendingRouteStatus.Tables[0];
        //            if (dtPendingRouteStatusRMS.Rows[0]["ATMID"].ToString().Trim() == dtPendingRouteStatus.Rows[0]["CLIENT_ATM_ID"].ToString().Trim())
        //            {

        //                if (dtPendingRouteStatusRMS.Rows.Count > 0)
        //                {
        //                    lstRouteDetails = dtPendingRouteStatusRMS.AsEnumerable().Select(r => new RouteStatus
        //                    {
        //                        AtmId = r.Field<string>("ATMID"),
        //                        RouteNo = r.Field<string>("RouteNo"),
        //                        Status = dtPendingRouteStatus.Rows[0]["ITEM_SITUATION_NAME"].ToString(),//get status from SCO
        //                        LastOtcTaken = r.Field<DateTime>("Actual_Date_Time").ToString()
        //                    }).ToList();
        //                }
        //            }
        //        }
        //    }
        //    return lstRouteDetails;
        //}
        public JsonResult GetRouteKeyName(string prefix, string AtmID)
        {

            List<Generatekey> routKeyList = new List<Generatekey>();
            DataSet ds = HD.GetCompanyDetails(AtmID);
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = HD.GetRouteKeyName(prefix, ds.Tables[0].Rows[0]["COMPANY"].ToString());
                if (dt.Rows.Count > 0)
                {
                    routKeyList = dt.AsEnumerable().Select(r => new Generatekey
                    {
                        OTCRouteKeyId = r.Field<string>("FirstName")
                    }).ToList();
                }
                else
                    routKeyList = new List<Generatekey>();
            }
            

            return Json(routKeyList);
        }

        public OTCReturnData GenerateOTC(Generatekey otc)
        {
            OTCReturnData objOTC = new OTCReturnData();
            bool isError = false;
            string strOperationCode = "";
            string strOTC = "";
            string AuthCode = "";
            string AccessToken = "";

            //Get UserId and TouchKeyId from OTC Db using RouteKey Id
            string strUserId = "";
            string TouchKeyID = "";
            string PagerNo = "";

            DataSet ds = HD.OTCGetUserIdTouchKeyId(otc.OTCRouteKeyId, otc.Company);

            if (ds.Tables[0].Rows.Count > 0)
            {
                strUserId = ds.Tables[0].Rows[0]["UserId"].ToString();
                TouchKeyID = ds.Tables[0].Rows[0]["TouchKeyID"].ToString();
                PagerNo = ds.Tables[0].Rows[0]["PagerNo"].ToString();
                try
                {
                    using (var client = new HttpClient())
                    {
                        OTCAuthCode obj = new OTCAuthCode();
                        if (otc.Company.ToString().ToUpper() == "SIS")
                        {
                            client.BaseAddress = new Uri("http://10.61.0.44:1111/login/auth_code");
                            obj.ClientId = clsSISCredentials.ClientId.ToString();
                            obj.ClientSecret = clsSISCredentials.ClientSecret.ToString();
                            obj.Username = otc.OTCUserId;
                            obj.Password = otc.OTCUserPWD;
                        }
                        else if (otc.Company.ToString().ToUpper() == "SISCO")
                        {
                            client.BaseAddress = new Uri("http://10.61.0.44:1112/login/auth_code");
                            obj.ClientId = clsSISCOCredentials.ClientId.ToString();
                            obj.ClientSecret = clsSISCOCredentials.ClientSecret.ToString();
                            obj.Username = otc.OTCUserId;
                            obj.Password = otc.OTCUserPWD;
                        }

                        //HTTP POST
                        var postTask = client.PostAsJsonAsync<OTCAuthCode>("auth_code", obj);

                        postTask.Wait();

                        var result = postTask.Result;

                        if (result.IsSuccessStatusCode)
                        {
                            var strResult = postTask.Result.Content.ReadAsStringAsync().Result.ToString();

                            dynamic dynObj = JsonConvert.DeserializeObject(strResult);

                            string status = dynObj.Header.Status;

                            if (status.ToString().ToUpper() == "OK")
                            {
                                AuthCode = dynObj.Body.AuthCode;

                                //Get AccessToken using AuthCode
                                if (AuthCode != "" && strOTC == "")
                                {
                                    using (var authcodeclient = new HttpClient())
                                    {
                                        OTCAuthCodeToken objauth = new OTCAuthCodeToken();

                                        if (otc.Company.ToString().ToUpper() == "SIS")
                                        {
                                            authcodeclient.BaseAddress = new Uri("http://10.61.0.44:1111/login/access_token");
                                            objauth.ClientId = clsSISCredentials.ClientId.ToString();
                                            objauth.ClientSecret = clsSISCredentials.ClientSecret.ToString();
                                            objauth.AuthCode = AuthCode;
                                        }
                                        else if (otc.Company.ToString().ToUpper() == "SISCO")
                                        {
                                            authcodeclient.BaseAddress = new Uri("http://10.61.0.44:1112/login/access_token");
                                            objauth.ClientId = clsSISCOCredentials.ClientId.ToString();
                                            objauth.ClientSecret = clsSISCOCredentials.ClientSecret.ToString();
                                            objauth.AuthCode = AuthCode;
                                        }

                                        //HTTP POST
                                        var postTaskauth = client.PostAsJsonAsync<OTCAuthCodeToken>("access_token", objauth);

                                        postTaskauth.Wait();

                                        var resultauth = postTaskauth.Result;

                                        if (resultauth.IsSuccessStatusCode)
                                        {
                                            var strResultauth = postTaskauth.Result.Content.ReadAsStringAsync().Result.ToString();

                                            dynamic dynObjauth = JsonConvert.DeserializeObject(strResultauth);

                                            string statusauth = dynObjauth.Header.Status;

                                            if (statusauth.ToString().ToUpper() == "OK")
                                            {
                                                AccessToken = dynObjauth.Body.AccessToken;

                                                if (AccessToken != "" && strOTC == "" && isError == false)
                                                {
                                                    using (var clienttoken = new HttpClient())
                                                    {
                                                        if (otc.Company.ToString().ToUpper() == "SIS")
                                                        {
                                                            clienttoken.BaseAddress = new Uri("http://10.61.0.44:1111/OPEN_LOCK_A");
                                                        }
                                                        else if (otc.Company.ToString().ToUpper() == "SISCO")
                                                        {
                                                            clienttoken.BaseAddress = new Uri("http://10.61.0.44:1112/OPEN_LOCK_A");
                                                        }

                                                        clienttoken.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                                        clienttoken.DefaultRequestHeaders.Add("Authorization", AccessToken);

                                                        OTC_Open_LOCK_A objlock = new OTC_Open_LOCK_A();

                                                        objlock.AtmId = otc.AtmId;
                                                        objlock.UserId = strUserId;
                                                        //obj.UserId = otc.OTCUserId;
                                                        objlock.TouchKeyId = TouchKeyID;
                                                        objlock.Date = DateTime.Now.Date.ToString("dd/MM/yyyy");
                                                        //objlock.Hour = DateTime.Now.Hour;
                                                        //ashish logic for reducing time block-16/02/2021------------------
                                                        int hrsforReduce = (Convert.ToInt32(otc.TimeBlock) - 1);
                                                        DateTime reduce_time = DateTime.Now.AddHours(-hrsforReduce);
                                                        objlock.Hour = reduce_time.Hour;
                                                        if (DateTime.Now.Minute >= 30)
                                                        {
                                                            objlock.Hour = reduce_time.AddHours(1).Hour;
                                                        }
                                                        //--------------------------------------------------
                                                        objlock.TimeBlock = Convert.ToInt32(otc.TimeBlock);
                                                        objlock.LockStatus = Convert.ToInt32(otc.LockStatus);

                                                        //HTTP POST
                                                        var postTasklock = clienttoken.PostAsJsonAsync<OTC_Open_LOCK_A>("OPEN_LOCK_A", objlock);

                                                        postTasklock.Wait();

                                                        var resultlock = postTasklock.Result;

                                                        if (resultlock.IsSuccessStatusCode)
                                                        {
                                                            var strResultlock = postTasklock.Result.Content.ReadAsStringAsync().Result.ToString();

                                                            dynamic dynObjlock = JsonConvert.DeserializeObject(strResultlock);

                                                            string statuslock = dynObjlock.Header.Status;

                                                            if (statuslock.ToString().ToUpper() == "OK")
                                                            {
                                                                if (dynObjlock.Body.OperationCode.ToString().Length > 0 && dynObjlock.Body.OperationCode.ToString().Length < 8)
                                                                {
                                                                    strOTC = (dynObjlock.Body.OperationCode).ToString().PadLeft(8, '0');
                                                                    strOperationCode = dynObjlock.Body.OperationCode;
                                                                }
                                                                else
                                                                {
                                                                    strOTC = dynObjlock.Body.OperationCode;
                                                                    strOperationCode = dynObjlock.Body.OperationCode;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                strOTC = dynObjlock.Header.Message;
                                                                isError = true;
                                                            }
                                                        }

                                                    }
                                                }
                                            }
                                            else
                                            {
                                                strOTC = dynObjauth.Header.Message;
                                                isError = true;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                strOTC = dynObj.Header.Status;
                                isError = true;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    strOTC = ex.Message.ToString();
                    isError = true;
                }

                //Log Before Return             
                //int i = crv.OTCInsertLog(otc.Company, otc.AtmId, otc.OTCRouteKeyId, TouchKeyID, otc.OTCUserId, strOperationCode, strOTC);

               

                objOTC.OTC = strOTC;
                objOTC.CallerNo = PagerNo;

                if (isError == true)
                {
                    objOTC.Msg = "Error";
                }

                if (isError == false)
                {
                    objOTC.Msg = "OK";
                }
            }
            else
            {
                strOTC = "Please check your RouteKey Id details";
                objOTC.Msg = "Error";
                objOTC.OTC = strOTC;
                objOTC.CallerNo = PagerNo;
            }
            return objOTC;
        }
    }
}
