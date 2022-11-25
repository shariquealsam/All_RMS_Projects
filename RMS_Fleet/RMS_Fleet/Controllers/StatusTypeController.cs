using RMS_Fleet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace RMS_Fleet.Controllers
{
    public class StatusTypeController : Controller
    {
        StatusType st = new StatusType();
        clsImportant ci = new clsImportant();


        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetApprovedDetails(string FromDate, string ToDate, string status)
        {
            string Name = Session["UserType"].ToString();
            string UName = Session["UserName"].ToString();
            List<Sales> lstVehicle = new List<Sales>();

            #region Admin condition
            if (Name == "Admin" && UName != "Sanjay Pandey")
            {
                DataSet ds = st.ApprovedVehicleDetails( FromDate, ToDate, status, Name, UName);
                DataTable dt = new DataTable();

                //DataTable
                dt = ds.Tables[0];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {

                    Sales H = new Sales();
                    H.Status = dt.Rows[i]["Rec_Id"].ToString();
                    if (H.Status != "")
                    {
                        if (dt.Rows[i]["Type"].ToString() != "" && Convert.ToInt32(dt.Rows[i]["TotalAmount"].ToString()) != 0)
                        {
                            H.RecId = dt.Rows[i]["Rec_Id"].ToString();
                            H.VehicleNumber = dt.Rows[i]["VechileNumber"].ToString();
                            //H.SalesDate = dt.Rows[i]["SalesDate"].ToString();
                           // H.RouteNo = dt.Rows[i]["RouteNumber"].ToString();
                            H.RegionId = dt.Rows[i]["RegionId"].ToString();
                            H.RegionName = dt.Rows[i]["RegionName"].ToString();
                            H.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                            H.BranchName = dt.Rows[i]["BranchName"].ToString();
                            H.ActualDateTime = dt.Rows[i]["Actual_Date_Time"].ToString();
                            DateTime date = Convert.ToDateTime(H.ActualDateTime);
                            H.ActualDateTime = date.ToString("dd-MM-yyyy");
                            H.TotalAmount = dt.Rows[i]["TotalAmount"].ToString();

                            string[] a = new string[10];

                            string str = dt.Rows[i]["Type"].ToString();
                            int x = str.IndexOf("[");
                            int y = str.LastIndexOf("]");
                            int z = y - x;
                            str = str.Substring(x + 1, z - 1);



                            if (str.Contains("]["))
                            {
                                str = str.Replace("][", ",");
                                a = str.Split(',');
                            }
                            else
                            {
                                a[0] = str;
                            }

                            string[] b = new string[a.Length];

                            int k = 0;
                            for (int j = 0; j < a.Length; j++)
                            {
                                if (a[j] == null)
                                {
                                    continue;
                                }
                                if (a[j] == ("TyreAmount") && dt.Columns.Contains("TyreStatus_Admin"))
                                {
                                    if (dt.Rows[i]["TyreStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }                                   
                                }
                                 if (a[j] == ("BatteryAmount") && dt.Columns.Contains("BatteryStatus_Admin"))
                                {
                                    if (dt.Rows[i]["BatteryStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }                                   
                                }
                                if(a[j] == ("DentingAmount") &&  dt.Columns.Contains("DentingStatus_Admin"))
                                {
                                    if (dt.Rows[i]["DentingStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                     if(a[j] == ("MinorAmount") &&  dt.Columns.Contains("MinorStatus_Admin"))
                                {
                                    if (dt.Rows[i]["MinorStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                 if(a[j] == ("RoutineAmount") &&  dt.Columns.Contains("RoutineStatus_Admin"))
                                {
                                    if (dt.Rows[i]["RoutineStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                 if(a[j] == ("SeatAmount") &&  dt.Columns.Contains("SeatStatus_Admin"))
                                {
                                    if (dt.Rows[i]["SeatStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("SelfAmount") &&  dt.Columns.Contains("SelfStatus_Admin"))
                                {
                                    if (dt.Rows[i]["SelfStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("ElectricalAmount") &&  dt.Columns.Contains("ElectricalStatus_Admin"))
                                {
                                    if (dt.Rows[i]["ElectricalStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("ClutchAmount") &&  dt.Columns.Contains("ClutchStatus_Admin"))
                                {
                                    if (dt.Rows[i]["ClutchStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("AlternatorAmount") &&  dt.Columns.Contains("AlternatorStatus_Admin"))
                                {
                                    if (dt.Rows[i]["AlternatorStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("LeafAmount") &&  dt.Columns.Contains("LeafStatus_Admin"))
                                {
                                    if (dt.Rows[i]["LeafStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("SuspensionAmount") &&  dt.Columns.Contains("SuspensionStatus_Admin"))
                                {
                                    if (dt.Rows[i]["SuspensionStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("RadiatorAmount") &&  dt.Columns.Contains("RadiatorStatus_Admin"))
                                {
                                    if (dt.Rows[i]["RadiatorStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("AxleAmount") &&  dt.Columns.Contains("AxleStatus_Admin"))
                                {
                                    if (dt.Rows[i]["AxleStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("DifferentialAmount") &&  dt.Columns.Contains("DifferentialStatus_Admin"))
                                {
                                    if (dt.Rows[i]["DifferentialStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("PuncherAmount") &&  dt.Columns.Contains("PuncherStatus_Admin"))
                                {
                                    if (dt.Rows[i]["PuncherStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("FuelAmount") &&  dt.Columns.Contains("FuelStatus_Admin"))
                                {
                                    if (dt.Rows[i]["FuelStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("OilAmount") &&  dt.Columns.Contains("OilStatus_Admin"))
                                {
                                    if (dt.Rows[i]["OilStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("TurboAmount") &&  dt.Columns.Contains("TurboStatus_Admin"))
                                {
                                    if (dt.Rows[i]["TurboStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("EcmAmount") &&  dt.Columns.Contains("EcmStatus_Admin"))
                                {
                                    if (dt.Rows[i]["EcmStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("AccidentalAmount") &&  dt.Columns.Contains("AccidentalStatus_Admin"))
                                {
                                    if (dt.Rows[i]["AccidentalStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("GearBoxAmount") &&  dt.Columns.Contains("GearBoxStatus_Admin"))
                                {
                                    if (dt.Rows[i]["GearBoxStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("EngineWorkAmount") &&  dt.Columns.Contains("EngineWorkStatus_Admin"))
                                {
                                    if (dt.Rows[i]["EngineWorkStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if(a[j] == ("BreakWorkAmount") &&  dt.Columns.Contains("BreakWorkStatus_Admin"))
                                {
                                    if (dt.Rows[i]["BreakWorkStatus_Admin"].ToString() == "True" )                                   
                                    {
                                        b[k] = a[j];
                                    }               
                                }
                                    if (b[k] != null)
                                    {
                                        k++;
                                    }
                            }

                            StringBuilder sb = new StringBuilder();
                            for (int j = 0; j < b.Length; j++)
                            {
                                if (b[j] != null)
                                {
                                    sb.Append("[" + b[j] + "]");
                                }
                            }
                            string final = sb.ToString();
                            H.Type = final;
                            lstVehicle.Add(H);
                        }

                        lstVehicle = lstVehicle.Where(s => s.Type != "").ToList();
                    }
                }
             //return Json(lstVehicle, JsonRequestBehavior.AllowGet);
            }

            #endregion 

            #region SuperAdmin condition
            if (Name == "Admin" && UName == "Sanjay Pandey")
            {
                DataSet ds = st.ApprovedVehicleDetails(FromDate, ToDate, status, Name, UName);
                DataTable dt = new DataTable();

                //DataTable
                dt = ds.Tables[0];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {

                    Sales H = new Sales();
                    H.Status = dt.Rows[i]["Rec_Id"].ToString();
                    if (H.Status != "")
                    {
                        if (dt.Rows[i]["Type"].ToString() != "" && Convert.ToInt32(dt.Rows[i]["TotalAmount"].ToString()) != 0)
                        {
                            H.RecId = dt.Rows[i]["Rec_Id"].ToString();
                            H.VehicleNumber = dt.Rows[i]["VechileNumber"].ToString();
                            //H.SalesDate = dt.Rows[i]["SalesDate"].ToString();
                            // H.RouteNo = dt.Rows[i]["RouteNumber"].ToString();
                            H.RegionId = dt.Rows[i]["RegionId"].ToString();
                            H.RegionName = dt.Rows[i]["RegionName"].ToString();
                            H.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                            H.BranchName = dt.Rows[i]["BranchName"].ToString();
                            H.ActualDateTime = dt.Rows[i]["Actual_Date_Time"].ToString();
                            DateTime date = Convert.ToDateTime(H.ActualDateTime);
                            H.ActualDateTime = date.ToString("dd-MM-yyyy");
                            H.TotalAmount = dt.Rows[i]["TotalAmount"].ToString();

                            string[] a = new string[10];

                            string str = dt.Rows[i]["Type"].ToString();
                            int x = str.IndexOf("[");
                            int y = str.LastIndexOf("]");
                            int z = y - x;
                            str = str.Substring(x + 1, z - 1);



                            if (str.Contains("]["))
                            {
                                str = str.Replace("][", ",");
                                a = str.Split(',');
                            }
                            else
                            {
                                a[0] = str;
                            }

                            string[] b = new string[a.Length];

                            int k = 0;
                            for (int j = 0; j < a.Length; j++)
                            {
                                if (a[j] == null)
                                {
                                    continue;
                                }
                                if (a[j] == ("TyreAmount") && dt.Columns.Contains("TyreStatus_Admin"))
                                {
                                    if (dt.Rows[i]["TyreStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("BatteryAmount") && dt.Columns.Contains("BatteryStatus_Admin"))
                                {
                                    if (dt.Rows[i]["BatteryStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("DentingAmount") && dt.Columns.Contains("DentingStatus_Admin"))
                                {
                                    if (dt.Rows[i]["DentingStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("MinorAmount") && dt.Columns.Contains("MinorStatus_Admin"))
                                {
                                    if (dt.Rows[i]["MinorStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("RoutineAmount") && dt.Columns.Contains("RoutineStatus_Admin"))
                                {
                                    if (dt.Rows[i]["RoutineStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SeatAmount") && dt.Columns.Contains("SeatStatus_Admin"))
                                {
                                    if (dt.Rows[i]["SeatStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SelfAmount") && dt.Columns.Contains("SelfStatus_Admin"))
                                {
                                    if (dt.Rows[i]["SelfStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("ElectricalAmount") && dt.Columns.Contains("ElectricalStatus_Admin"))
                                {
                                    if (dt.Rows[i]["ElectricalStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("ClutchAmount") && dt.Columns.Contains("ClutchStatus_Admin"))
                                {
                                    if (dt.Rows[i]["ClutchStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AlternatorAmount") && dt.Columns.Contains("AlternatorStatus_Admin"))
                                {
                                    if (dt.Rows[i]["AlternatorStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("LeafAmount") && dt.Columns.Contains("LeafStatus_Admin"))
                                {
                                    if (dt.Rows[i]["LeafStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SuspensionAmount") && dt.Columns.Contains("SuspensionStatus_Admin"))
                                {
                                    if (dt.Rows[i]["SuspensionStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("RadiatorAmount") && dt.Columns.Contains("RadiatorStatus_Admin"))
                                {
                                    if (dt.Rows[i]["RadiatorStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AxleAmount") && dt.Columns.Contains("AxleStatus_Admin"))
                                {
                                    if (dt.Rows[i]["AxleStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("DifferentialAmount") && dt.Columns.Contains("DifferentialStatus_Admin"))
                                {
                                    if (dt.Rows[i]["DifferentialStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("PuncherAmount") && dt.Columns.Contains("PuncherStatus_Admin"))
                                {
                                    if (dt.Rows[i]["PuncherStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("FuelAmount") && dt.Columns.Contains("FuelStatus_Admin"))
                                {
                                    if (dt.Rows[i]["FuelStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("OilAmount") && dt.Columns.Contains("OilStatus_Admin"))
                                {
                                    if (dt.Rows[i]["OilStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("TurboAmount") && dt.Columns.Contains("TurboStatus_Admin"))
                                {
                                    if (dt.Rows[i]["TurboStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("EcmAmount") && dt.Columns.Contains("EcmStatus_Admin"))
                                {
                                    if (dt.Rows[i]["EcmStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AccidentalAmount") && dt.Columns.Contains("AccidentalStatus_Admin"))
                                {
                                    if (dt.Rows[i]["AccidentalStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("GearBoxAmount") && dt.Columns.Contains("GearBoxStatus_Admin"))
                                {
                                    if (dt.Rows[i]["GearBoxStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("EngineWorkAmount") && dt.Columns.Contains("EngineWorkStatus_Admin"))
                                {
                                    if (dt.Rows[i]["EngineWorkStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("BreakWorkAmount") && dt.Columns.Contains("BreakWorkStatus_Admin"))
                                {
                                    if (dt.Rows[i]["BreakWorkStatus_Admin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (b[k] != null)
                                {
                                    k++;
                                }
                            }

                            StringBuilder sb = new StringBuilder();
                            for (int j = 0; j < b.Length; j++)
                            {
                                if (b[j] != null)
                                {
                                    sb.Append("[" + b[j] + "]");
                                }
                            }
                            string final = sb.ToString();
                            H.Type = final;
                            lstVehicle.Add(H);
                        }

                        lstVehicle = lstVehicle.Where(s => s.Type != "").ToList();
                    }
                }
                //return Json(lstVehicle, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region User condition
            if (Name == "User")
            {
                DataSet ds = st.ApprovedVehicleDetails(FromDate, ToDate, status, Name, UName);
                DataTable dt = new DataTable();

                //DataTable
                dt = ds.Tables[0];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {

                    Sales H = new Sales();
                    H.Status = dt.Rows[i]["Rec_Id"].ToString();
                    if (H.Status != "")
                    {
                        if (dt.Rows[i]["Type"].ToString() != "" && Convert.ToInt32(dt.Rows[i]["TotalAmount"].ToString()) != 0)
                        {
                            H.RecId = dt.Rows[i]["Rec_Id"].ToString();
                            H.VehicleNumber = dt.Rows[i]["VechileNumber"].ToString();
                            //H.SalesDate = dt.Rows[i]["SalesDate"].ToString();
                            // H.RouteNo = dt.Rows[i]["RouteNumber"].ToString();
                            H.RegionId = dt.Rows[i]["RegionId"].ToString();
                            H.RegionName = dt.Rows[i]["RegionName"].ToString();
                            H.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                            H.BranchName = dt.Rows[i]["BranchName"].ToString();
                            H.ActualDateTime = dt.Rows[i]["Actual_Date_Time"].ToString();
                            DateTime date = Convert.ToDateTime(H.ActualDateTime);
                            H.ActualDateTime = date.ToString("dd-MM-yyyy");
                            H.TotalAmount = dt.Rows[i]["TotalAmount"].ToString();

                            string[] a = new string[10];

                            string str = dt.Rows[i]["Type"].ToString();
                            int x = str.IndexOf("[");
                            int y = str.LastIndexOf("]");
                            int z = y - x;
                            str = str.Substring(x + 1, z - 1);



                            if (str.Contains("]["))
                            {
                                str = str.Replace("][", ",");
                                a = str.Split(',');
                            }
                            else
                            {
                                a[0] = str;
                            }

                            string[] b = new string[a.Length];

                            int k = 0;
                            for (int j = 0; j < a.Length; j++)
                            {
                                if (a[j] == null)
                                {
                                    continue;
                                }
                                if (a[j] == ("TyreAmount"))
                                {
                                    if (dt.Rows[i]["TyreStatus_Admin"].ToString() == "True" && dt.Rows[i]["TyreStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("BatteryAmount"))
                                {
                                    if (dt.Rows[i]["BatteryStatus_Admin"].ToString() == "True" && dt.Rows[i]["BatteryStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("DentingAmount"))
                                {
                                    if (dt.Rows[i]["DentingStatus_Admin"].ToString() == "True" && dt.Rows[i]["DentingStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("MinorAmount"))
                                {
                                    if (dt.Rows[i]["MinorStatus_Admin"].ToString() == "True" && dt.Rows[i]["MinorStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("RoutineAmount"))
                                {
                                    if (dt.Rows[i]["RoutineStatus_Admin"].ToString() == "True" && dt.Rows[i]["RadiatorStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SeatAmount"))
                                {
                                    if (dt.Rows[i]["SeatStatus_Admin"].ToString() == "True" && dt.Rows[i]["SeatStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SelfAmount"))
                                {
                                    if (dt.Rows[i]["SelfStatus_Admin"].ToString() == "True" && dt.Rows[i]["SelfStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("ElectricalAmount"))
                                {
                                    if (dt.Rows[i]["ElectricalStatus_Admin"].ToString() == "True" && dt.Rows[i]["ElectricalStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("ClutchAmount"))
                                {
                                    if (dt.Rows[i]["ClutchStatus_Admin"].ToString() == "True" && dt.Rows[i]["ClutchStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AlternatorAmount"))
                                {
                                    if (dt.Rows[i]["AlternatorStatus_Admin"].ToString() == "True" && dt.Rows[i]["AlternatorStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("LeafAmount"))
                                {
                                    if (dt.Rows[i]["LeafStatus_Admin"].ToString() == "True" && dt.Rows[i]["LeafStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SuspensionAmount"))
                                {
                                    if (dt.Rows[i]["SuspensionStatus_Admin"].ToString() == "True" && dt.Rows[i]["SuspensionStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("RadiatorAmount"))
                                {
                                    if (dt.Rows[i]["RadiatorStatus_Admin"].ToString() == "True" && dt.Rows[i]["RadiatorStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AxleAmount"))
                                {
                                    if (dt.Rows[i]["AxleStatus_Admin"].ToString() == "True" && dt.Rows[i]["AxleStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("DifferentialAmount"))
                                {
                                    if (dt.Rows[i]["DifferentialStatus_Admin"].ToString() == "True" && dt.Rows[i]["DifferentialStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("PuncherAmount"))
                                {
                                    if (dt.Rows[i]["PuncherStatus_Admin"].ToString() == "True" && dt.Rows[i]["PuncherStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("FuelAmount"))
                                {
                                    if (dt.Rows[i]["FuelStatus_Admin"].ToString() == "True" && dt.Rows[i]["FuelStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("OilAmount"))
                                {
                                    if (dt.Rows[i]["OilStatus_Admin"].ToString() == "True" && dt.Rows[i]["OilStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("TurboAmount"))
                                {
                                    if (dt.Rows[i]["TurboStatus_Admin"].ToString() == "True" && dt.Rows[i]["TurboStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("EcmAmount"))
                                {
                                    if (dt.Rows[i]["EcmStatus_Admin"].ToString() == "True" && dt.Rows[i]["EcmStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AccidentalAmount"))
                                {
                                    if (dt.Rows[i]["AccidentalStatus_Admin"].ToString() == "True" && dt.Rows[i]["AccidentalStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("GearBoxAmount"))
                                {
                                    if (dt.Rows[i]["GearBoxStatus_Admin"].ToString() == "True" && dt.Rows[i]["GearBoxStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("EngineWorkAmount"))
                                {
                                    if (dt.Rows[i]["EngineWorkStatus_Admin"].ToString() == "True" && dt.Rows[i]["EngineWorkStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("BreakWorkAmount"))
                                {
                                    if (dt.Rows[i]["BreakWorkStatus_Admin"].ToString() == "True" && dt.Rows[i]["BreakWorkStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (b[k] != null)
                                {
                                    k++;
                                }
                            }

                            StringBuilder sb = new StringBuilder();
                            for (int j = 0; j < b.Length; j++)
                            {
                                if (b[j] != null)
                                {
                                    sb.Append("[" + b[j] + "]");
                                }
                            }
                            string final = sb.ToString();
                            H.Type = final;
                            lstVehicle.Add(H);
                        }

                        lstVehicle = lstVehicle.Where(s => s.Type != "").ToList();
                    }
                }
                //return Json(lstVehicle, JsonRequestBehavior.AllowGet);
            }
            #endregion

            return Json(lstVehicle, JsonRequestBehavior.AllowGet);
           

        }

        public JsonResult GetRejectedDetails(string FromDate, string ToDate, string status)
        {
            string Name = Session["UserType"].ToString();
            string UName = Session["UserName"].ToString();
            List<Sales> lstVehicle = new List<Sales>();

            #region SuperAdmin condition
            if (Name == "Admin" && UName == "Sanjay Pandey")
            {
                DataSet ds = st.RejectedVehicleDetails(FromDate, ToDate, status, Name, UName);
                DataTable dt = new DataTable();

                //DataTable
                dt = ds.Tables[0];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {

                    Sales H = new Sales();
                    H.Status = dt.Rows[i]["Rec_Id"].ToString();
                    if (H.Status != "")
                    {
                        if (dt.Rows[i]["Type"].ToString() != "" && Convert.ToInt32(dt.Rows[i]["TotalAmount"].ToString()) != 0)
                        {
                            H.RecId = dt.Rows[i]["Rec_Id"].ToString();
                            H.VehicleNumber = dt.Rows[i]["VechileNumber"].ToString();
                            //H.SalesDate = dt.Rows[i]["SalesDate"].ToString();
                            // H.RouteNo = dt.Rows[i]["RouteNumber"].ToString();
                            H.RegionId = dt.Rows[i]["RegionId"].ToString();
                            H.RegionName = dt.Rows[i]["RegionName"].ToString();
                            H.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                            H.BranchName = dt.Rows[i]["BranchName"].ToString();
                            H.ActualDateTime = dt.Rows[i]["Actual_Date_Time"].ToString();
                            DateTime date = Convert.ToDateTime(H.ActualDateTime);
                            H.ActualDateTime = date.ToString("dd-MM-yyyy");
                            H.TotalAmount = dt.Rows[i]["TotalAmount"].ToString();

                            string[] a = new string[10];

                            string str = dt.Rows[i]["Type"].ToString();
                            int x = str.IndexOf("[");
                            int y = str.LastIndexOf("]");
                            int z = y - x;
                            str = str.Substring(x + 1, z - 1);



                            if (str.Contains("]["))
                            {
                                str = str.Replace("][", ",");
                                a = str.Split(',');
                            }
                            else
                            {
                                a[0] = str;
                            }

                            string[] b = new string[a.Length];

                            int k = 0;
                            for (int j = 0; j < a.Length; j++)
                            {
                                if (a[j] == null)
                                {
                                    continue;
                                }
                                if (a[j] == ("TyreAmount") && dt.Columns.Contains("TyreStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["TyreStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("BatteryAmount") && dt.Columns.Contains("BatteryStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["BatteryStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("DentingAmount") && dt.Columns.Contains("DentingStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["DentingStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("MinorAmount") && dt.Columns.Contains("MinorStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["MinorStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("RoutineAmount") && dt.Columns.Contains("RoutineStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["RoutineStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SeatAmount") && dt.Columns.Contains("SeatStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["SeatStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SelfAmount") && dt.Columns.Contains("SelfStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["SelfStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("ElectricalAmount") && dt.Columns.Contains("ElectricalStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["ElectricalStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("ClutchAmount") && dt.Columns.Contains("ClutchStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["ClutchStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AlternatorAmount") && dt.Columns.Contains("AlternatorStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["AlternatorStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("LeafAmount") && dt.Columns.Contains("LeafStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["LeafStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SuspensionAmount") && dt.Columns.Contains("SuspensionStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["SuspensionStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("RadiatorAmount") && dt.Columns.Contains("RadiatorStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["RadiatorStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AxleAmount") && dt.Columns.Contains("AxleStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["AxleStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("DifferentialAmount") && dt.Columns.Contains("DifferentialStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["DifferentialStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("PuncherAmount") && dt.Columns.Contains("PuncherStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["PuncherStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("FuelAmount") && dt.Columns.Contains("FuelStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["FuelStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("OilAmount") && dt.Columns.Contains("OilStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["OilStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("TurboAmount") && dt.Columns.Contains("TurboStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["TurboStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("EcmAmount") && dt.Columns.Contains("EcmStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["EcmStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AccidentalAmount") && dt.Columns.Contains("AccidentalStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["AccidentalStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("GearBoxAmount") && dt.Columns.Contains("GearBoxStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["GearBoxStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("EngineWorkAmount") && dt.Columns.Contains("EngineWorkStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["EngineWorkStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("BreakWorkAmount") && dt.Columns.Contains("BreakWorkStatus_SuperAdmin"))
                                {
                                    if (dt.Rows[i]["BreakWorkStatus_SuperAdmin"].ToString() == "True")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (b[k] != null)
                                {
                                    k++;
                                }
                            }

                            StringBuilder sb = new StringBuilder();
                            for (int j = 0; j < b.Length; j++)
                            {
                                if (b[j] != null)
                                {
                                    sb.Append("[" + b[j] + "]");
                                }
                            }
                            string final = sb.ToString();
                            H.Type = final;
                            lstVehicle.Add(H);
                        }

                        lstVehicle = lstVehicle.Where(s => s.Type != "").ToList();
                    }
                }
                //return Json(lstVehicle, JsonRequestBehavior.AllowGet);
            }

            #endregion

            #region Admin condition
            if (Name == "Admin" && UName != "Sanjay Pandey")
            {
                DataSet ds = st.RejectedVehicleDetails(FromDate, ToDate, status, Name, UName);
                DataTable dt = new DataTable();

                //DataTable
                dt = ds.Tables[0];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {

                    Sales H = new Sales();
                    H.Status = dt.Rows[i]["Rec_Id"].ToString();
                    if (H.Status != "")
                    {
                        if (dt.Rows[i]["Type"].ToString() != "" && Convert.ToInt32(dt.Rows[i]["TotalAmount"].ToString()) != 0)
                        {
                            H.RecId = dt.Rows[i]["Rec_Id"].ToString();
                            H.VehicleNumber = dt.Rows[i]["VechileNumber"].ToString();
                            //H.SalesDate = dt.Rows[i]["SalesDate"].ToString();
                            // H.RouteNo = dt.Rows[i]["RouteNumber"].ToString();
                            H.RegionId = dt.Rows[i]["RegionId"].ToString();
                            H.RegionName = dt.Rows[i]["RegionName"].ToString();
                            H.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                            H.BranchName = dt.Rows[i]["BranchName"].ToString();
                            H.ActualDateTime = dt.Rows[i]["Actual_Date_Time"].ToString();
                            DateTime date = Convert.ToDateTime(H.ActualDateTime);
                            H.ActualDateTime = date.ToString("dd-MM-yyyy");
                            H.TotalAmount = dt.Rows[i]["TotalAmount"].ToString();

                            string[] a = new string[10];

                            string str = dt.Rows[i]["Type"].ToString();
                            int x = str.IndexOf("[");
                            int y = str.LastIndexOf("]");
                            int z = y - x;
                            str = str.Substring(x + 1, z - 1);



                            if (str.Contains("]["))
                            {
                                str = str.Replace("][", ",");
                                a = str.Split(',');
                            }
                            else
                            {
                                a[0] = str;
                            }

                            string[] b = new string[a.Length];

                            int k = 0;
                            for (int j = 0; j < a.Length; j++)
                            {
                                if (a[j] == null)
                                {
                                    continue;
                                }
                                if (a[j] == ("TyreAmount") && dt.Columns.Contains("TyreStatus_Admin"))
                                {
                                    if (dt.Rows[i]["TyreStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("BatteryAmount") && dt.Columns.Contains("BatteryStatus_Admin"))
                                {
                                    if (dt.Rows[i]["BatteryStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("DentingAmount") && dt.Columns.Contains("DentingStatus_Admin"))
                                {
                                    if (dt.Rows[i]["DentingStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("MinorAmount") && dt.Columns.Contains("MinorStatus_Admin"))
                                {
                                    if (dt.Rows[i]["MinorStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("RoutineAmount") && dt.Columns.Contains("RoutineStatus_Admin"))
                                {
                                    if (dt.Rows[i]["RoutineStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SeatAmount") && dt.Columns.Contains("SeatStatus_Admin"))
                                {
                                    if (dt.Rows[i]["SeatStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SelfAmount") && dt.Columns.Contains("SelfStatus_Admin"))
                                {
                                    if (dt.Rows[i]["SelfStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("ElectricalAmount") && dt.Columns.Contains("ElectricalStatus_Admin"))
                                {
                                    if (dt.Rows[i]["ElectricalStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("ClutchAmount") && dt.Columns.Contains("ClutchStatus_Admin"))
                                {
                                    if (dt.Rows[i]["ClutchStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AlternatorAmount") && dt.Columns.Contains("AlternatorStatus_Admin"))
                                {
                                    if (dt.Rows[i]["AlternatorStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("LeafAmount") && dt.Columns.Contains("LeafStatus_Admin"))
                                {
                                    if (dt.Rows[i]["LeafStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SuspensionAmount") && dt.Columns.Contains("SuspensionStatus_Admin"))
                                {
                                    if (dt.Rows[i]["SuspensionStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("RadiatorAmount") && dt.Columns.Contains("RadiatorStatus_Admin"))
                                {
                                    if (dt.Rows[i]["RadiatorStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AxleAmount") && dt.Columns.Contains("AxleStatus_Admin"))
                                {
                                    if (dt.Rows[i]["AxleStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("DifferentialAmount") && dt.Columns.Contains("DifferentialStatus_Admin"))
                                {
                                    if (dt.Rows[i]["DifferentialStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("PuncherAmount") && dt.Columns.Contains("PuncherStatus_Admin"))
                                {
                                    if (dt.Rows[i]["PuncherStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("FuelAmount") && dt.Columns.Contains("FuelStatus_Admin"))
                                {
                                    if (dt.Rows[i]["FuelStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("OilAmount") && dt.Columns.Contains("OilStatus_Admin"))
                                {
                                    if (dt.Rows[i]["OilStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("TurboAmount") && dt.Columns.Contains("TurboStatus_Admin"))
                                {
                                    if (dt.Rows[i]["TurboStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("EcmAmount") && dt.Columns.Contains("EcmStatus_Admin"))
                                {
                                    if (dt.Rows[i]["EcmStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AccidentalAmount") && dt.Columns.Contains("AccidentalStatus_Admin"))
                                {
                                    if (dt.Rows[i]["AccidentalStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("GearBoxAmount") && dt.Columns.Contains("GearBoxStatus_Admin"))
                                {
                                    if (dt.Rows[i]["GearBoxStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("EngineWorkAmount") && dt.Columns.Contains("EngineWorkStatus_Admin"))
                                {
                                    if (dt.Rows[i]["EngineWorkStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("BreakWorkAmount") && dt.Columns.Contains("BreakWorkStatus_Admin"))
                                {
                                    if (dt.Rows[i]["BreakWorkStatus_Admin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (b[k] != null)
                                {
                                    k++;
                                }
                            }

                            StringBuilder sb = new StringBuilder();
                            for (int j = 0; j < b.Length; j++)
                            {
                                if (b[j] != null)
                                {
                                    sb.Append("[" + b[j] + "]");
                                }
                            }
                            string final = sb.ToString();
                            H.Type = final;
                            lstVehicle.Add(H);
                        }

                        lstVehicle = lstVehicle.Where(s => s.Type != "").ToList();
                    }
                }
                //return Json(lstVehicle, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region User
            if (Name == "User")
            { 
                 DataSet ds = st.RejectedVehicleDetails(FromDate, ToDate, status, Name, UName);
                DataTable dt = new DataTable();

                //DataTable
                dt = ds.Tables[0];

                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {

                    Sales H = new Sales();
                    H.Status = dt.Rows[i]["Rec_Id"].ToString();
                    if (H.Status != "")
                    {
                        if (dt.Rows[i]["Type"].ToString() != "" && Convert.ToInt32(dt.Rows[i]["TotalAmount"].ToString()) != 0)
                        {
                            H.RecId = dt.Rows[i]["Rec_Id"].ToString();
                            H.VehicleNumber = dt.Rows[i]["VechileNumber"].ToString();
                            //H.SalesDate = dt.Rows[i]["SalesDate"].ToString();
                            // H.RouteNo = dt.Rows[i]["RouteNumber"].ToString();
                            H.RegionId = dt.Rows[i]["RegionId"].ToString();
                            H.RegionName = dt.Rows[i]["RegionName"].ToString();
                            H.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                            H.BranchName = dt.Rows[i]["BranchName"].ToString();                           
                            H.ActualDateTime = dt.Rows[i]["Actual_Date_Time"].ToString();
                            DateTime date = Convert.ToDateTime(H.ActualDateTime);
                            H.ActualDateTime = date.ToString("dd-MM-yyyy");

                            H.TotalAmount = dt.Rows[i]["TotalAmount"].ToString();

                            string[] a = new string[10];

                            string str = dt.Rows[i]["Type"].ToString();
                            int x = str.IndexOf("[");
                            int y = str.LastIndexOf("]");
                            int z = y - x;
                            str = str.Substring(x + 1, z - 1);



                            if (str.Contains("]["))
                            {
                                str = str.Replace("][", ",");
                                a = str.Split(',');
                            }
                            else
                            {
                                a[0] = str;
                            }

                            string[] b = new string[a.Length];

                            int k = 0;
                            for (int j = 0; j < a.Length; j++)
                            {
                                if (a[j] == null)
                                {
                                    continue;
                                }
                                if (a[j] == ("TyreAmount"))
                                {
                                    if (dt.Rows[i]["TyreStatus_Admin"].ToString() == "False" || dt.Rows[i]["TyreStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("BatteryAmount"))
                                {
                                    if (dt.Rows[i]["BatteryStatus_Admin"].ToString() == "False" || dt.Rows[i]["BatteryStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("DentingAmount"))
                                {
                                    if (dt.Rows[i]["DentingStatus_Admin"].ToString() == "False" || dt.Rows[i]["DentingStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("MinorAmount"))
                                {
                                    if (dt.Rows[i]["MinorStatus_Admin"].ToString() == "False" || dt.Rows[i]["MinorStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("RoutineAmount"))
                                {
                                    if (dt.Rows[i]["RoutineStatus_Admin"].ToString() == "False" || dt.Rows[i]["RoutineStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SeatAmount"))
                                {
                                    if (dt.Rows[i]["SeatStatus_Admin"].ToString() == "False" || dt.Rows[i]["SeatStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SelfAmount"))
                                {
                                    if (dt.Rows[i]["SelfStatus_Admin"].ToString() == "False" || dt.Rows[i]["SelfStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("ElectricalAmount"))
                                {
                                    if (dt.Rows[i]["ElectricalStatus_Admin"].ToString() == "False" || dt.Rows[i]["ElectricalStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("ClutchAmount"))
                                {
                                    if (dt.Rows[i]["ClutchStatus_Admin"].ToString() == "False" || dt.Rows[i]["ClutchStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AlternatorAmount"))
                                {
                                    if (dt.Rows[i]["AlternatorStatus_Admin"].ToString() == "False" || dt.Rows[i]["AlternatorStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("LeafAmount"))
                                {
                                    if (dt.Rows[i]["LeafStatus_Admin"].ToString() == "False" || dt.Rows[i]["LeafStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("SuspensionAmount"))
                                {
                                    if (dt.Rows[i]["SuspensionStatus_Admin"].ToString() == "False" || dt.Rows[i]["SuspensionStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("RadiatorAmount"))
                                {
                                    if (dt.Rows[i]["RadiatorStatus_Admin"].ToString() == "False" || dt.Rows[i]["RadiatorStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AxleAmount"))
                                {
                                    if (dt.Rows[i]["AxleStatus_Admin"].ToString() == "False" || dt.Rows[i]["AxleStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("DifferentialAmount"))
                                {
                                    if (dt.Rows[i]["DifferentialStatus_Admin"].ToString() == "False" || dt.Rows[i]["DifferentialStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("PuncherAmount"))
                                {
                                    if (dt.Rows[i]["PuncherStatus_Admin"].ToString() == "False" || dt.Rows[i]["PuncherStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("FuelAmount"))
                                {
                                    if (dt.Rows[i]["FuelStatus_Admin"].ToString() == "False" || dt.Rows[i]["FuelStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("OilAmount"))
                                {
                                    if (dt.Rows[i]["OilStatus_Admin"].ToString() == "False" || dt.Rows[i]["OilStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("TurboAmount"))
                                {
                                    if (dt.Rows[i]["TurboStatus_Admin"].ToString() == "False" || dt.Rows[i]["TurboStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("EcmAmount"))
                                {
                                    if (dt.Rows[i]["EcmStatus_Admin"].ToString() == "False" || dt.Rows[i]["EcmStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("AccidentalAmount"))
                                {
                                    if (dt.Rows[i]["AccidentalStatus_Admin"].ToString() == "False" || dt.Rows[i]["AccidentalStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("GearBoxAmount"))
                                {
                                    if (dt.Rows[i]["GearBoxStatus_Admin"].ToString() == "False" || dt.Rows[i]["GearBoxStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("EngineWorkAmount"))
                                {
                                    if (dt.Rows[i]["EngineWorkStatus_Admin"].ToString() == "False" || dt.Rows[i]["EngineWorkStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (a[j] == ("BreakWorkAmount"))
                                {
                                    if (dt.Rows[i]["BreakWorkStatus_Admin"].ToString() == "False" || dt.Rows[i]["BreakWorkStatus_SuperAdmin"].ToString() == "False")
                                    {
                                        b[k] = a[j];
                                    }
                                }
                                if (b[k] != null)
                                {
                                    k++;
                                }
                            }

                            StringBuilder sb = new StringBuilder();
                            for (int j = 0; j < b.Length; j++)
                            {
                                if (b[j] != null)
                                {
                                    sb.Append("[" + b[j] + "]");
                                }
                            }
                            string final = sb.ToString();
                            H.Type = final;
                            lstVehicle.Add(H);
                        }

                        lstVehicle = lstVehicle.Where(s => s.Type != "").ToList();
                    }
                }
            }

            #endregion

            return Json(lstVehicle, JsonRequestBehavior.AllowGet);


        }
    }
}
