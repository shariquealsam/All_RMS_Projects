using ClosedXML.Excel;
using RMS_Fleet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace RMS_Fleet.Controllers
{
    public class SalesController : Controller
    {
        SalesDetails Sd = new SalesDetails();
        clsImportant ci = new clsImportant();

        public ActionResult Index()
        {
            string RegionId = Session["RegionIds"].ToString();

            DataSet ds = new DataSet();

            if (RegionId == "0")
            {
                ds = Sd.GetBranch();
            }
            else
            {
                ds = Sd.GetBranch(RegionId);
            }
            DataSet ds1 = Sd.GetRegionName(RegionId);

            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Sales> lstBranch = new List<Sales>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Sales S = new Sales();

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
            DataSet ds = Sd.GetVehicleMaster(BranchId);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Sales> lstVechileMaster = new List<Sales>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Sales S = new Sales();

                S.VehicleNumber = dt.Rows[i]["VehicleNo"].ToString();

                lstVechileMaster.Add(S);
            }

            return Json(lstVechileMaster, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetVechileDetails(string VehicleNumber)
        {
            DataSet ds = Sd.GetVehicleDetails(VehicleNumber);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Sales> lstVechileDetails = new List<Sales>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Sales S = new Sales();

                S.RegionName = dt.Rows[i]["RegionName"].ToString();
                S.BranchName = dt.Rows[i]["BranchName"].ToString();
                S.VehicleNo = dt.Rows[i]["VehicleNo"].ToString();
                S.Make = dt.Rows[i]["Make"].ToString();
                S.Manufacturing_Year = dt.Rows[i]["Manufacturing_Year"].ToString();
                S.RouteNo = dt.Rows[i]["RouteNo"].ToString();

                lstVechileDetails.Add(S);
            }

            return Json(lstVechileDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSalesDetails(string NoOfTyres, string Tyre, string TyreCompanyName, string TyreVendorName, string TyreSize, string TyreInvoiceNo, string TyreInvoiceDate,
                                           string TyreKMReading, string TyreAmount, string TyreRemarks, string NoOfBattery, string Battery, string BatteryCompanyName, string BatteryVendorName,
                                           string BatteryInvoiceNo, string BatteryMSDMPR, string BatteryInvoiceDate, string BatteryKMReading, string BatteryAmount, string BatteryRemarks,
                                           string RoutineVendorName, string RoutineDealerType, string RoutineInvoiceNo, string RoutineInvoiceDate, string RoutineKMReading,
                                           string RoutineAmount, string RoutineRemarks, string DentingPaintingType, string DentingVendorName, string DentingInvoiceNo,
                                           string DentingInvoiceDate, string DentingKMReading, string DentingAmount, string DentingRemarks, string MinorVendorName, string MinorInvoiceNo,
                                           string MinorInvoiceDate, string MinorKMReading, string MinorAmount, string MinorRemarks, string SeatVendorname, string SeatInvoiceNo,
                                           string SeatInvoiceDate, string SeatKMReading, string SeatAmount, string SeatRemarks, string SelfVendorName, string SelfDealerType,
                                           string SelfInvoiceNo, string SelfInvoiceDate, string SelfKMReading, string SelfAmount, string SelfRemarks, string ElectricalVendorName,
                                           string ElectricalDealerType, string ElectricalInvoiceNo, string ElectricalInvoiceDate, string ElectricalKMReading, string ElectricalAmount,
                                           string ElectricalRemarks, string ClutchVendorName, string ClutchDealerType, string ClutchInvoiceNo, string ClutchInvoiceDate,
                                           string ClutchKMReading, string ClutchAmount, string ClutchRemarks, string AlternatorVendorName, string AlternatorDealerType, string AlternatorInvoiceNo,
                                           string AlternatorInvoiceDate, string AlternatorKMReading, string AlternatorAmount, string AlternatorRemarks, string LeafVendorName, string LeafDealerType,
                                           string LeafInvoiceNo, string LeafInvoiceDate, string LeafKMReading, string LeafAmount, string LeafRemarks, string SuspensionVendorName,
                                           string SuspensionDealerType, string SuspensionInvoiceNo, string SuspensionInvoiceDate, string SuspensionKMReading, string SuspensionAmount,
                                           string SuspensionRemarks, string GearBoxVendorName, string GearBoxDealerType, string GearBoxInvoiceNo, string GearBoxInvoiceDate, string GearBoxKMReading,
                                           string GearBoxAmount, string GearBoxRemarks, string BreakWorkVendorName, string BreakWorkDealerType, string BreakWorkInvoiceNo, string BreakWorkInvoiceDate,
                                           string BreakWorkKMReading, string BreakWorkAmount, string BreakWorkRemarks, string EngineWorkVendorName, string EngineWorkDealerType, string EngineWorkInvoiceNo,
                                           string EngineWorkInvoiceDate, string EngineWorkKMReading, string EngineWorkAmount, string EngineWorkRemarks, string FuelVendorName, string FuelDealerType,
                                           string FuelInvoiceNo, string FuelInvoiceDate, string FuelKMReading, string FuelAmount, string FuelRemarks, string PuncherVendorName, string PuncherNoofPuncher,
                                           string PuncherInvoiceNo, string PuncherInvoiceDate, string PuncherKMReading, string PuncherAmount, string PuncherRemarks, string OilVendorName, string OilLtr,
                                           string OilInvoiceNo, string OilInvoiceDate, string OilKMReading, string OilAmount, string OilRemarks, string RadiatorVendorName, string RadiatorDealerType,
                                           string RadiatorInvoiceNo, string RadiatorInvoiceDate, string RadiatorKMReading, string RadiatorAmount, string RadiatorRemarks, string AxleVendorName,
                                           string AxleDealerType, string AxleInvoiceNo, string AxleInvoiceDate, string AxleKMReading, string AxleAmount, string AxleRemarks, string DifferentialVendorName,
                                           string DifferentialDealerType, string DifferentialInvoiceNo, string DifferentialInvoiceDate, string DifferentialKMReading, string DifferentialAmount,
                                           string DifferentialRemarks, string TurboVendorName, string TurboDealerType, string TurboInvoiceNo, string TurboInvoiceDate, string TurboKMReading,
                                           string TurboAmount, string TurboNarration, string EcmVendorName, string EcmDealerType, string EcmInvoiceNo, string EcmInvoiceDate, string EcmKMReading,
                                           string EcmAmount, string EcmNarration, string AccidentalVendorName, string AccidentalDealerType, string AccidentalInvoiceNo, string AccidentalInvoiceDate,
                                           string AccidentalKMReading, string AccidentalInsCoveredAmount, string AccidentalDifferenceAmount, string AccidentalTotalAmount, string AccidentalNarration,
                                           string RegionId, string BranchId, string VechileNumber, string RouteNumber, string TypeSelected, string TyrePartNumber, string TyreGSTAmount, string TyreTotalAmount, 
                                           string BatteryPartNumber, string BatteryGSTAmount, string BatteryTotalAmount, string RoutinePartNumber, string RoutineGSTAmount, string RoutineTotalAmount, 
                                           string DentingPartNumber, string DentingGSTAmount, string DentingTotalAmount, string MinorPartNumber, string MinorGSTAmount, string MinorTotalAmount, string SeatPartNumber, 
                                           string SeatGSTAmount, string SeatTotalAmount, string SelfPartNumber, string SelfGSTAmount, string SelfTotalAmount, string ElectricalPartNumber, string ElectricalGSTAmount, 
                                           string ElectricalTotalAmount, string ClutchPartNumber, string ClutchGSTAmount, string ClutchTotalAmount, string AlternatorPartNumber, string AlternatorGSTAmount, 
                                           string AlternatorTotalAmount, string LeafPartNumber, string LeafGSTAmount, string LeafTotalAmount, string SuspensionPartNumber, string SuspensionGSTAmount, 
                                           string SuspensionTotalAmount, string GearBoxPartNumber, string GearBoxGSTAmount, string GearBoxTotalAmount, string BreakPartNumber, string BreakGSTAmount, 
                                           string BreakTotalAmount, string EnginePartNumber, string EngineGSTAmount, string EngineTotalAmount, string FuelPartNumber, string FuelGSTAmount, string FuelTotalAmount, 
                                           string PuncherPartNumber, string PuncherGSTAmount, string PuncherTotalAmount, string OilPartNumber, string OilGSTAmount, string OilTotalAmount, string RadiatorPartNumber, 
                                           string RadiatorGSTAmount, string RadiatorTotalAmount, string AxlePartNumber, string AxleGSTAmount, string AxleTotalAmount, string DifferentialPartNumber, string DifferentialGSTAmount, 
                                           string DifferentialTotalAmount, string TurboPartNumber, string TurboGSTAmount, string TurboTotalAmount, string EcmPartNumber, string EcmGSTAmount, string EcmTotalAmount, string AccidentalPartNumber,
                                           string AccidentalGSTAmount, string AccidentalGrossTotalAmount, string TyerDealerType, string BatteryDealerType, string DentingDealerType, string MinorDealerType,
                                           string SeatDealerType, string PuncherDealerType, string OilDealerType
)
        {
            string Name = Session["UserName"].ToString();

            var resultSuccess = new { Message = "Successfully Uploaded." };
            var resultError = new { Message = "Error Please Check." };

            int return1 = 0;

            string[] str = TypeSelected.Replace(" ", "").Split(',');

            bool IsError = false;

            if (Session["UserType"].ToString() == "Admin")
            {
                if (RegionId == "0")
                {
                    DataSet ds = Sd.GetRegionIdDetails(VechileNumber, BranchId);
                    RegionId = ds.Tables[0].Rows[0]["RegionId"].ToString();
                    
                }
            }

            if (Session["UserType"].ToString() == "User")
            {
                for (int i = 0; i <= str.Length - 1; i++)
                {
                    if (str.Contains("TYRE"))
                    {
                        DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        DateTime SelectedDate = Convert.ToDateTime(TyreInvoiceDate);

                        string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                        if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                        {
                            IsError = true;
                            resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                        }

                        //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                        //{
                        //    IsError = true;
                        //    resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                        //}
                        else
                        {
                            IsError = false;
                        }
                    }

                    if (str.Contains("BATTERY"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(BatteryInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry Battery Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("ROUTINESERVICE"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(RoutineInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry Routine Service Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("DENTING&PAINTING"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(DentingInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry DENTING & PAINTING Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("MINORREPAIRING"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(MinorInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry MINOR REPAIRING Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("SEATREPAIR"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(SeatInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry SEAT REPAIR Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("SEATREPAIR"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(SeatInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry SEAT REPAIR Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("SELFWORK"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(SelfInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry SELF WORK Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("ELECTRICALWORK"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(ElectricalInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry ELECTRICAL WORK Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("CLUTCHREPAIRING"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(ClutchInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry CLUTCH REPAIRING Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("ALTERNATOR"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(AlternatorInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry ALTERNATOR Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("LEAF/PATTISPRING"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(LeafInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry LEAF/PATTI SPRING Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("SUSPENSION"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(SuspensionInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry SUSPENSION Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("GEARBOX"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(GearBoxInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry GEARBOX Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("BREAKWORK"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(BreakWorkInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry BREAKWORK Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("ENGINEWORK"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(EngineWorkInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry ENGINEWORK Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("FUELPUMP"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(FuelInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry FUELPUMP Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("PUNCHER"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(PuncherInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry PUNCHER Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("OILTOPUP"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(OilInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry OILTOPUP Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("RADIATORANDWATERBODY"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(RadiatorInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry RADIATOR AND WATER BODY Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("AXLEWORK"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(AxleInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry AXLEWORK Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("DIFFERENTIALWORK"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(DifferentialInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry DIFFERENTIAL WORK Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("TURBO"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(TurboInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry TURBO Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("ECM"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(EcmInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry ECM Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                    if (str.Contains("ACCIDENTAL"))
                    {
                        if (IsError == false)
                        {
                            DateTime CurrentDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime SelectedDate = Convert.ToDateTime(AccidentalInvoiceDate);
                            string diff2 = (CurrentDate - SelectedDate).TotalDays.ToString();

                            //if (Convert.ToInt32(diff2) >= 0 && Convert.ToInt32(diff2) >= 8)
                            //{
                            //    IsError = true;
                            //    resultError = new { Message = "Date Selected For Entry ACCIDENTAL Details are beyond 7day's Please Check." };
                            //}
                            if (Convert.ToInt32(diff2) >= 8 || Convert.ToInt32(diff2) < 0)
                            {
                                IsError = true;
                                resultError = new { Message = "Date Selected For Entry Tyre Details are beyond 7day's Please Check." };
                            }
                            else
                            {
                                IsError = false;
                            }
                        }
                    }
                }
            }
            else
            {
                IsError = false;
            }

            
            if (IsError == false)
            {
                string CreatedBy_Email = Session["EmailId"].ToString();
                return1 = Sd.SalesDetailsInsert(NoOfTyres, Tyre, TyreCompanyName, TyreVendorName, TyreSize, TyreInvoiceNo, TyreInvoiceDate, TyreKMReading, TyreAmount, TyreRemarks,
                                                    NoOfBattery, Battery, BatteryCompanyName, BatteryVendorName, BatteryInvoiceNo, BatteryMSDMPR, BatteryInvoiceDate, BatteryKMReading,
                                                    BatteryAmount, BatteryRemarks, RoutineVendorName, RoutineDealerType, RoutineInvoiceNo, RoutineInvoiceDate, RoutineKMReading,
                                                    RoutineAmount, RoutineRemarks, DentingPaintingType, DentingVendorName, DentingInvoiceNo, DentingInvoiceDate, DentingKMReading,
                                                    DentingAmount, DentingRemarks, MinorVendorName, MinorInvoiceNo, MinorInvoiceDate, MinorKMReading, MinorAmount, MinorRemarks, SeatVendorname,
                                                    SeatInvoiceNo, SeatInvoiceDate, SeatKMReading, SeatAmount, SeatRemarks, SelfVendorName, SelfDealerType, SelfInvoiceNo, SelfInvoiceDate,
                                                    SelfKMReading, SelfAmount, SelfRemarks, ElectricalVendorName, ElectricalDealerType, ElectricalInvoiceNo, ElectricalInvoiceDate,
                                                    ElectricalKMReading, ElectricalAmount, ElectricalRemarks, ClutchVendorName, ClutchDealerType, ClutchInvoiceNo, ClutchInvoiceDate,
                                                    ClutchKMReading, ClutchAmount, ClutchRemarks, AlternatorVendorName, AlternatorDealerType, AlternatorInvoiceNo, AlternatorInvoiceDate,
                                                    AlternatorKMReading, AlternatorAmount, AlternatorRemarks, LeafVendorName, LeafDealerType, LeafInvoiceNo, LeafInvoiceDate, LeafKMReading,
                                                    LeafAmount, LeafRemarks, SuspensionVendorName, SuspensionDealerType, SuspensionInvoiceNo, SuspensionInvoiceDate, SuspensionKMReading,
                                                    SuspensionAmount, SuspensionRemarks, GearBoxVendorName, GearBoxDealerType, GearBoxInvoiceNo, GearBoxInvoiceDate, GearBoxKMReading,
                                                    GearBoxAmount, GearBoxRemarks, BreakWorkVendorName, BreakWorkDealerType, BreakWorkInvoiceNo, BreakWorkInvoiceDate, BreakWorkKMReading,
                                                    BreakWorkAmount, BreakWorkRemarks, EngineWorkVendorName, EngineWorkDealerType, EngineWorkInvoiceNo, EngineWorkInvoiceDate, EngineWorkKMReading,
                                                    EngineWorkAmount, EngineWorkRemarks, FuelVendorName, FuelDealerType, FuelInvoiceNo, FuelInvoiceDate, FuelKMReading, FuelAmount, FuelRemarks,
                                                    PuncherVendorName, PuncherNoofPuncher, PuncherInvoiceNo, PuncherInvoiceDate, PuncherKMReading, PuncherAmount, PuncherRemarks, OilVendorName,
                                                    OilLtr, OilInvoiceNo, OilInvoiceDate, OilKMReading, OilAmount, OilRemarks, RadiatorVendorName, RadiatorDealerType, RadiatorInvoiceNo,
                                                    RadiatorInvoiceDate, RadiatorKMReading, RadiatorAmount, RadiatorRemarks, AxleVendorName, AxleDealerType, AxleInvoiceNo, AxleInvoiceDate,
                                                    AxleKMReading, AxleAmount, AxleRemarks, DifferentialVendorName, DifferentialDealerType, DifferentialInvoiceNo, DifferentialInvoiceDate,
                                                    DifferentialKMReading, DifferentialAmount, DifferentialRemarks, TurboVendorName, TurboDealerType, TurboInvoiceNo, TurboInvoiceDate,
                                                    TurboKMReading, TurboAmount, TurboNarration, EcmVendorName, EcmDealerType, EcmInvoiceNo, EcmInvoiceDate, EcmKMReading, EcmAmount,
                                                    EcmNarration, AccidentalVendorName, AccidentalDealerType, AccidentalInvoiceNo, AccidentalInvoiceDate, AccidentalKMReading,
                                                    AccidentalInsCoveredAmount, AccidentalDifferenceAmount, AccidentalTotalAmount, AccidentalNarration, RegionId, BranchId,
                                                    VechileNumber, RouteNumber, Name,CreatedBy_Email, TyrePartNumber, TyreGSTAmount, TyreTotalAmount, BatteryPartNumber, BatteryGSTAmount, BatteryTotalAmount, 
                                                    RoutinePartNumber, RoutineGSTAmount, RoutineTotalAmount, DentingPartNumber, DentingGSTAmount, DentingTotalAmount, MinorPartNumber, MinorGSTAmount, 
                                                    MinorTotalAmount, SeatPartNumber, SeatGSTAmount, SeatTotalAmount, SelfPartNumber, SelfGSTAmount, SelfTotalAmount, ElectricalPartNumber, 
                                                    ElectricalGSTAmount, ElectricalTotalAmount, ClutchPartNumber, ClutchGSTAmount, ClutchTotalAmount, AlternatorPartNumber, AlternatorGSTAmount, 
                                                    AlternatorTotalAmount, LeafPartNumber, LeafGSTAmount, LeafTotalAmount, SuspensionPartNumber, SuspensionGSTAmount, SuspensionTotalAmount, GearBoxPartNumber, 
                                                    GearBoxGSTAmount, GearBoxTotalAmount, BreakPartNumber, BreakGSTAmount, BreakTotalAmount, EnginePartNumber, EngineGSTAmount, EngineTotalAmount, FuelPartNumber, 
                                                    FuelGSTAmount, FuelTotalAmount, PuncherPartNumber, PuncherGSTAmount, PuncherTotalAmount, OilPartNumber, OilGSTAmount, OilTotalAmount, RadiatorPartNumber, 
                                                    RadiatorGSTAmount, RadiatorTotalAmount, AxlePartNumber, AxleGSTAmount, AxleTotalAmount, DifferentialPartNumber, DifferentialGSTAmount, DifferentialTotalAmount, 
                                                    TurboPartNumber, TurboGSTAmount, TurboTotalAmount, EcmPartNumber, EcmGSTAmount, EcmTotalAmount, AccidentalPartNumber, AccidentalGSTAmount, AccidentalGrossTotalAmount,
                                                    TyerDealerType, BatteryDealerType, DentingDealerType, MinorDealerType, SeatDealerType, PuncherDealerType, OilDealerType);
            }
            if (return1 > 0)
            {
                DataSet ds = Sd.SalesRecIdDetails(RegionId, BranchId, VechileNumber, Name, DateTime.Now.ToString("yyyy-MM-dd"));
                Session["RecId"] = ds.Tables[0].Rows[0]["Rec_Id"].ToString();

                return Json(resultSuccess, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(resultError, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetSalesDetails(string VehicleNumber, string BranchID, string Date, string RegionID)
        {
            Date = DateTime.Now.ToString("yyyy-MM-dd");
            RegionID = Session["RegionIds"].ToString();
            List<Sales> lstSalesDetails = new List<Sales>();
            if (RegionID == "0")
            {
                DataSet ds1 = Sd.GetRegionIdDetails(VehicleNumber, BranchID);
                try
                {
                    RegionID = ds1.Tables[0].Rows[0]["RegionId"].ToString();
                }
                catch
                {
                    
                    return Json(lstSalesDetails, JsonRequestBehavior.AllowGet);
                }
            }

            DataSet ds = Sd.GetSalesEntryDetails(VehicleNumber, BranchID, Date, RegionID);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];


            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Sales S = new Sales();

                S.RecId = dt.Rows[i]["Rec_Id"].ToString();
                S.NoOfTyres = dt.Rows[i]["NoOfTyres"].ToString();
                S.Tyre = dt.Rows[i]["Tyre"].ToString();
                S.TyreCompanyName = dt.Rows[i]["TyreCompanyName"].ToString();
                S.TyreVendorName = dt.Rows[i]["TyreVendorName"].ToString();
                S.TyreSize = dt.Rows[i]["TyreSize"].ToString();
                S.TyreInvoiceNo = dt.Rows[i]["TyreInvoiceNo"].ToString();
                S.TyreInvoiceDate = Convert.ToDateTime(dt.Rows[i]["TyreInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.TyreKMReading = dt.Rows[i]["TyreKMReading"].ToString();
                S.TyreAmount = dt.Rows[i]["TyreAmount"].ToString();
                S.TyreRemarks = dt.Rows[i]["TyreRemarks"].ToString();
                S.NoOfBattery = dt.Rows[i]["NoOfBattery"].ToString();
                S.Battery = dt.Rows[i]["Battery"].ToString();
                S.BatteryCompanyName = dt.Rows[i]["BatteryCompanyName"].ToString();
                S.BatteryVendorName = dt.Rows[i]["BatteryVendorName"].ToString();
                S.BatteryInvoiceNo = dt.Rows[i]["BatteryInvoiceNo"].ToString();
                S.BatteryMSDMPR = dt.Rows[i]["BatteryMSDMPR"].ToString();
                S.BatteryInvoiceDate = Convert.ToDateTime(dt.Rows[i]["BatteryInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.BatteryKMReading = dt.Rows[i]["BatteryKMReading"].ToString();
                S.BatteryAmount = dt.Rows[i]["BatteryAmount"].ToString();
                S.BatteryRemarks = dt.Rows[i]["BatteryRemarks"].ToString();
                S.RoutineVendorName = dt.Rows[i]["RoutineVendorName"].ToString();
                S.RoutineDealerType = dt.Rows[i]["RoutineDealerType"].ToString();
                S.RoutineInvoiceNo = dt.Rows[i]["RoutineInvoiceNo"].ToString();
                S.RoutineInvoiceDate = Convert.ToDateTime(dt.Rows[i]["RoutineInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.RoutineKMReading = dt.Rows[i]["RoutineKMReading"].ToString();
                S.RoutineAmount = dt.Rows[i]["RoutineAmount"].ToString();
                S.RoutineRemarks = dt.Rows[i]["RoutineRemarks"].ToString();
                S.DentingPaintingType = dt.Rows[i]["DentingPaintingType"].ToString();
                S.DentingVendorName = dt.Rows[i]["DentingVendorName"].ToString();
                S.DentingInvoiceNo = dt.Rows[i]["DentingInvoiceNo"].ToString();
                S.DentingInvoiceDate = Convert.ToDateTime(dt.Rows[i]["DentingInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.DentingKMReading = dt.Rows[i]["DentingKMReading"].ToString();
                S.DentingAmount = dt.Rows[i]["DentingAmount"].ToString();
                S.DentingRemarks = dt.Rows[i]["DentingRemarks"].ToString();
                S.MinorVendorName = dt.Rows[i]["MinorVendorName"].ToString();
                S.MinorInvoiceNo = dt.Rows[i]["MinorInvoiceNo"].ToString();
                S.MinorInvoiceDate = Convert.ToDateTime(dt.Rows[i]["MinorInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.MinorKMReading = dt.Rows[i]["MinorKMReading"].ToString();
                S.MinorAmount = dt.Rows[i]["MinorAmount"].ToString();
                S.MinorRemarks = dt.Rows[i]["MinorRemarks"].ToString();
                S.SeatVendorname = dt.Rows[i]["SeatVendorname"].ToString();
                S.SeatInvoiceNo = dt.Rows[i]["SeatInvoiceNo"].ToString();
                S.SeatInvoiceDate = Convert.ToDateTime(dt.Rows[i]["SeatInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.SeatKMReading = dt.Rows[i]["SeatKMReading"].ToString();
                S.SeatAmount = dt.Rows[i]["SeatAmount"].ToString();
                S.SeatRemarks = dt.Rows[i]["SeatRemarks"].ToString();
                S.SelfVendorName = dt.Rows[i]["SelfVendorName"].ToString();
                S.SelfDealerType = dt.Rows[i]["SelfDealerType"].ToString();
                S.SelfInvoiceNo = dt.Rows[i]["SelfInvoiceNo"].ToString();
                S.SelfInvoiceDate = Convert.ToDateTime(dt.Rows[i]["SelfInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.SelfKMReading = dt.Rows[i]["SelfKMReading"].ToString();
                S.SelfAmount = dt.Rows[i]["SelfAmount"].ToString();
                S.SelfRemarks = dt.Rows[i]["SelfRemarks"].ToString();
                S.ElectricalVendorName = dt.Rows[i]["ElectricalVendorName"].ToString();
                S.ElectricalDealerType = dt.Rows[i]["ElectricalDealerType"].ToString();
                S.ElectricalInvoiceNo = dt.Rows[i]["ElectricalInvoiceNo"].ToString();
                S.ElectricalInvoiceDate = Convert.ToDateTime(dt.Rows[i]["ElectricalInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.ElectricalKMReading = dt.Rows[i]["ElectricalKMReading"].ToString();
                S.ElectricalAmount = dt.Rows[i]["ElectricalAmount"].ToString();
                S.ElectricalRemarks = dt.Rows[i]["ElectricalRemarks"].ToString();
                S.ClutchVendorName = dt.Rows[i]["ClutchVendorName"].ToString();
                S.ClutchDealerType = dt.Rows[i]["ClutchDealerType"].ToString();
                S.ClutchInvoiceNo = dt.Rows[i]["ClutchInvoiceNo"].ToString();
                S.ClutchInvoiceDate = Convert.ToDateTime(dt.Rows[i]["ClutchInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.ClutchKMReading = dt.Rows[i]["ClutchKMReading"].ToString();
                S.ClutchAmount = dt.Rows[i]["ClutchAmount"].ToString();
                S.ClutchRemarks = dt.Rows[i]["ClutchRemarks"].ToString();
                S.AlternatorVendorName = dt.Rows[i]["AlternatorVendorName"].ToString();
                S.AlternatorDealerType = dt.Rows[i]["AlternatorDealerType"].ToString();
                S.AlternatorInvoiceNo = dt.Rows[i]["AlternatorInvoiceNo"].ToString();
                S.AlternatorInvoiceDate = Convert.ToDateTime(dt.Rows[i]["AlternatorInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.AlternatorKMReading = dt.Rows[i]["AlternatorKMReading"].ToString();
                S.AlternatorAmount = dt.Rows[i]["AlternatorAmount"].ToString();
                S.AlternatorRemarks = dt.Rows[i]["AlternatorRemarks"].ToString();
                S.LeafVendorName = dt.Rows[i]["LeafVendorName"].ToString();
                S.LeafDealerType = dt.Rows[i]["LeafDealerType"].ToString();
                S.LeafInvoiceNo = dt.Rows[i]["LeafInvoiceNo"].ToString();
                S.LeafInvoiceDate = Convert.ToDateTime(dt.Rows[i]["LeafInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.LeafKMReading = dt.Rows[i]["LeafKMReading"].ToString();
                S.LeafAmount = dt.Rows[i]["LeafAmount"].ToString();
                S.LeafRemarks = dt.Rows[i]["LeafRemarks"].ToString();
                S.SuspensionVendorName = dt.Rows[i]["SuspensionVendorName"].ToString();
                S.SuspensionDealerType = dt.Rows[i]["SuspensionDealerType"].ToString();
                S.SuspensionInvoiceNo = dt.Rows[i]["SuspensionInvoiceNo"].ToString();
                S.SuspensionInvoiceDate = Convert.ToDateTime(dt.Rows[i]["SuspensionInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.SuspensionKMReading = dt.Rows[i]["SuspensionKMReading"].ToString();
                S.SuspensionAmount = dt.Rows[i]["SuspensionAmount"].ToString();
                S.SuspensionRemarks = dt.Rows[i]["SuspensionRemarks"].ToString();
                S.GearBoxVendorName = dt.Rows[i]["GearBoxVendorName"].ToString();
                S.GearBoxDealerType = dt.Rows[i]["GearBoxDealerType"].ToString();
                S.GearBoxInvoiceNo = dt.Rows[i]["GearBoxInvoiceNo"].ToString();
                S.GearBoxInvoiceDate = Convert.ToDateTime(dt.Rows[i]["GearBoxInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.GearBoxKMReading = dt.Rows[i]["GearBoxKMReading"].ToString();
                S.GearBoxAmount = dt.Rows[i]["GearBoxAmount"].ToString();
                S.GearBoxRemarks = dt.Rows[i]["GearBoxRemarks"].ToString();
                S.BreakWorkVendorName = dt.Rows[i]["BreakWorkVendorName"].ToString();
                S.BreakWorkDealerType = dt.Rows[i]["BreakWorkDealerType"].ToString();
                S.BreakWorkInvoiceNo = dt.Rows[i]["BreakWorkInvoiceNo"].ToString();
                S.BreakWorkInvoiceDate = Convert.ToDateTime(dt.Rows[i]["BreakWorkInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.BreakWorkKMReading = dt.Rows[i]["BreakWorkKMReading"].ToString();
                S.BreakWorkAmount = dt.Rows[i]["BreakWorkAmount"].ToString();
                S.BreakWorkRemarks = dt.Rows[i]["BreakWorkRemarks"].ToString();
                S.EngineWorkVendorName = dt.Rows[i]["EngineWorkVendorName"].ToString();
                S.EngineWorkDealerType = dt.Rows[i]["EngineWorkDealerType"].ToString();
                S.EngineWorkInvoiceNo = dt.Rows[i]["EngineWorkInvoiceNo"].ToString();
                S.EngineWorkInvoiceDate = Convert.ToDateTime(dt.Rows[i]["EngineWorkInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.EngineWorkKMReading = dt.Rows[i]["EngineWorkKMReading"].ToString();
                S.EngineWorkAmount = dt.Rows[i]["EngineWorkAmount"].ToString();
                S.EngineWorkRemarks = dt.Rows[i]["EngineWorkRemarks"].ToString();
                S.FuelVendorName = dt.Rows[i]["FuelVendorName"].ToString();
                S.FuelDealerType = dt.Rows[i]["FuelDealerType"].ToString();
                S.FuelInvoiceNo = dt.Rows[i]["FuelInvoiceNo"].ToString();
                S.FuelInvoiceDate = Convert.ToDateTime(dt.Rows[i]["FuelInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.FuelKMReading = dt.Rows[i]["FuelKMReading"].ToString();
                S.FuelAmount = dt.Rows[i]["FuelAmount"].ToString();
                S.FuelRemarks = dt.Rows[i]["FuelRemarks"].ToString();
                S.PuncherVendorName = dt.Rows[i]["PuncherVendorName"].ToString();
                S.PuncherNoofPuncher = dt.Rows[i]["PuncherNoofPuncher"].ToString();
                S.PuncherInvoiceNo = dt.Rows[i]["PuncherInvoiceNo"].ToString();
                S.PuncherInvoiceDate = Convert.ToDateTime(dt.Rows[i]["PuncherInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.PuncherKMReading = dt.Rows[i]["PuncherKMReading"].ToString();
                S.PuncherAmount = dt.Rows[i]["PuncherAmount"].ToString();
                S.PuncherRemarks = dt.Rows[i]["PuncherRemarks"].ToString();
                S.OilVendorName = dt.Rows[i]["OilVendorName"].ToString();
                S.OilLtr = dt.Rows[i]["OilLtr"].ToString();
                S.OilInvoiceNo = dt.Rows[i]["OilInvoiceNo"].ToString();
                S.OilInvoiceDate = Convert.ToDateTime(dt.Rows[i]["OilInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.OilKMReading = dt.Rows[i]["OilKMReading"].ToString();
                S.OilAmount = dt.Rows[i]["OilAmount"].ToString();
                S.OilRemarks = dt.Rows[i]["OilRemarks"].ToString();
                S.RadiatorVendorName = dt.Rows[i]["RadiatorVendorName"].ToString();
                S.RadiatorDealerType = dt.Rows[i]["RadiatorDealerType"].ToString();
                S.RadiatorInvoiceNo = dt.Rows[i]["RadiatorInvoiceNo"].ToString();
                S.RadiatorInvoiceDate = Convert.ToDateTime(dt.Rows[i]["RadiatorInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.RadiatorKMReading = dt.Rows[i]["RadiatorKMReading"].ToString();
                S.RadiatorAmount = dt.Rows[i]["RadiatorAmount"].ToString();
                S.RadiatorRemarks = dt.Rows[i]["RadiatorRemarks"].ToString();
                S.AxleVendorName = dt.Rows[i]["AxleVendorName"].ToString();
                S.AxleDealerType = dt.Rows[i]["AxleDealerType"].ToString();
                S.AxleInvoiceNo = dt.Rows[i]["AxleInvoiceNo"].ToString();
                S.AxleInvoiceDate = Convert.ToDateTime(dt.Rows[i]["AxleInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.AxleKMReading = dt.Rows[i]["AxleKMReading"].ToString();
                S.AxleAmount = dt.Rows[i]["AxleAmount"].ToString();
                S.AxleRemarks = dt.Rows[i]["AxleRemarks"].ToString();
                S.DifferentialVendorName = dt.Rows[i]["DifferentialVendorName"].ToString();
                S.DifferentialDealerType = dt.Rows[i]["DifferentialDealerType"].ToString();
                S.DifferentialInvoiceNo = dt.Rows[i]["DifferentialInvoiceNo"].ToString();
                S.DifferentialInvoiceDate = Convert.ToDateTime(dt.Rows[i]["DifferentialInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.DifferentialKMReading = dt.Rows[i]["DifferentialKMReading"].ToString();
                S.DifferentialAmount = dt.Rows[i]["DifferentialAmount"].ToString();
                S.DifferentialRemarks = dt.Rows[i]["DifferentialRemarks"].ToString();

                S.TurboVendorName = dt.Rows[i]["TurboVendorName"].ToString();
                S.TurboDealerType = dt.Rows[i]["TurboDealerType"].ToString();
                S.TurboInvoiceNo = dt.Rows[i]["TurboInvoiceNo"].ToString();
                S.TurboInvoiceDate = Convert.ToDateTime(dt.Rows[i]["TurboInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.TurboKMReading = dt.Rows[i]["TurboKMReading"].ToString();
                S.TurboAmount = dt.Rows[i]["TurboAmount"].ToString();
                S.TurboNarration = dt.Rows[i]["TurboNarration"].ToString();
                S.EcmVendorName = dt.Rows[i]["EcmVendorName"].ToString();
                S.EcmDealerType = dt.Rows[i]["EcmDealerType"].ToString();
                S.EcmInvoiceNo = dt.Rows[i]["EcmInvoiceNo"].ToString();
                S.EcmInvoiceDate = Convert.ToDateTime(dt.Rows[i]["EcmInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.EcmKMReading = dt.Rows[i]["EcmKMReading"].ToString();
                S.EcmAmount = dt.Rows[i]["EcmAmount"].ToString();
                S.EcmNarration = dt.Rows[i]["EcmNarration"].ToString();
                S.AccidentalVendorName = dt.Rows[i]["AccidentalVendorName"].ToString();
                S.AccidentalDealerType = dt.Rows[i]["AccidentalDealerType"].ToString();
                S.AccidentalInvoiceNo = dt.Rows[i]["AccidentalInvoiceNo"].ToString();
                S.AccidentalInvoiceDate = Convert.ToDateTime(dt.Rows[i]["AccidentalInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.AccidentalKMReading = dt.Rows[i]["AccidentalKMReading"].ToString();
                S.AccidentalInsCoveredAmount = dt.Rows[i]["AccidentalInsCoveredAmount"].ToString();
                S.AccidentalDifferenceAmount = dt.Rows[i]["AccidentalDifferenceAmount"].ToString();
                S.AccidentalTotalAmount = dt.Rows[i]["AccidentalTotalAmount"].ToString();
                S.AccidentalNarration = dt.Rows[i]["AccidentalNarration"].ToString();

                S.TyrePartNumber = dt.Rows[i]["TyrePartNumber"].ToString();
                S.TyreGSTAmount = dt.Rows[i]["TyreGSTAmount"].ToString();
                S.TyreTotalAmount = dt.Rows[i]["TyreTotalAmount"].ToString();
                S.BatteryPartNumber = dt.Rows[i]["BatteryPartNumber"].ToString();
                S.BatteryGSTAmount = dt.Rows[i]["BatteryGSTAmount"].ToString();
                S.BatteryTotalAmount = dt.Rows[i]["BatteryTotalAmount"].ToString();
                S.RoutinePartNumber = dt.Rows[i]["RoutinePartNumber"].ToString();
                S.RoutineGSTAmount = dt.Rows[i]["RoutineGSTAmount"].ToString();
                S.RoutineTotalAmount = dt.Rows[i]["RoutineTotalAmount"].ToString();
                S.DentingPartNumber = dt.Rows[i]["DentingPartNumber"].ToString();
                S.DentingGSTAmount = dt.Rows[i]["DentingGSTAmount"].ToString();
                S.DentingTotalAmount = dt.Rows[i]["DentingTotalAmount"].ToString();
                S.MinorPartNumber = dt.Rows[i]["MinorPartNumber"].ToString();
                S.MinorGSTAmount = dt.Rows[i]["MinorGSTAmount"].ToString();
                S.MinorTotalAmount = dt.Rows[i]["MinorTotalAmount"].ToString();
                S.SeatPartNumber = dt.Rows[i]["SeatPartNumber"].ToString();
                S.SeatGSTAmount = dt.Rows[i]["SeatGSTAmount"].ToString();
                S.SeatTotalAmount = dt.Rows[i]["SeatTotalAmount"].ToString();
                S.SelfPartNumber = dt.Rows[i]["SelfPartNumber"].ToString();
                S.SelfGSTAmount = dt.Rows[i]["SelfGSTAmount"].ToString();
                S.SelfTotalAmount = dt.Rows[i]["SelfTotalAmount"].ToString();
                S.ElectricalPartNumber = dt.Rows[i]["ElectricalPartNumber"].ToString();
                S.ElectricalGSTAmount = dt.Rows[i]["ElectricalGSTAmount"].ToString();
                S.ElectricalTotalAmount = dt.Rows[i]["ElectricalTotalAmount"].ToString();
                S.ClutchPartNumber = dt.Rows[i]["ClutchPartNumber"].ToString();
                S.ClutchGSTAmount = dt.Rows[i]["ClutchGSTAmount"].ToString();
                S.ClutchTotalAmount = dt.Rows[i]["ClutchTotalAmount"].ToString();
                S.AlternatorPartNumber = dt.Rows[i]["AlternatorPartNumber"].ToString();
                S.AlternatorGSTAmount = dt.Rows[i]["AlternatorGSTAmount"].ToString();
                S.AlternatorTotalAmount = dt.Rows[i]["AlternatorTotalAmount"].ToString();
                S.LeafPartNumber = dt.Rows[i]["LeafPartNumber"].ToString();
                S.LeafGSTAmount = dt.Rows[i]["LeafGSTAmount"].ToString();
                S.LeafTotalAmount = dt.Rows[i]["LeafTotalAmount"].ToString();
                S.SuspensionPartNumber = dt.Rows[i]["SuspensionPartNumber"].ToString();
                S.SuspensionGSTAmount = dt.Rows[i]["SuspensionGSTAmount"].ToString();
                S.SuspensionTotalAmount = dt.Rows[i]["SuspensionTotalAmount"].ToString();
                S.GearBoxPartNumber = dt.Rows[i]["GearBoxPartNumber"].ToString();
                S.GearBoxGSTAmount = dt.Rows[i]["GearBoxGSTAmount"].ToString();
                S.GearBoxTotalAmount = dt.Rows[i]["GearBoxTotalAmount"].ToString();
                S.BreakPartNumber = dt.Rows[i]["BreakPartNumber"].ToString();
                S.BreakGSTAmount = dt.Rows[i]["BreakGSTAmount"].ToString();
                S.BreakTotalAmount = dt.Rows[i]["BreakTotalAmount"].ToString();
                S.EnginePartNumber = dt.Rows[i]["EnginePartNumber"].ToString();
                S.EngineGSTAmount = dt.Rows[i]["EngineGSTAmount"].ToString();
                S.EngineTotalAmount = dt.Rows[i]["EngineTotalAmount"].ToString();
                S.FuelPartNumber = dt.Rows[i]["FuelPartNumber"].ToString();
                S.FuelGSTAmount = dt.Rows[i]["FuelGSTAmount"].ToString();
                S.FuelTotalAmount = dt.Rows[i]["FuelTotalAmount"].ToString();
                S.PuncherPartNumber = dt.Rows[i]["PuncherPartNumber"].ToString();
                S.PuncherGSTAmount = dt.Rows[i]["PuncherGSTAmount"].ToString();
                S.PuncherTotalAmount = dt.Rows[i]["PuncherTotalAmount"].ToString();
                S.OilPartNumber = dt.Rows[i]["OilPartNumber"].ToString();
                S.OilGSTAmount = dt.Rows[i]["OilGSTAmount"].ToString();
                S.OilTotalAmount = dt.Rows[i]["OilTotalAmount"].ToString();
                S.RadiatorPartNumber = dt.Rows[i]["RadiatorPartNumber"].ToString();
                S.RadiatorGSTAmount = dt.Rows[i]["RadiatorGSTAmount"].ToString();
                S.RadiatorTotalAmount = dt.Rows[i]["RadiatorTotalAmount"].ToString();
                S.AxlePartNumber = dt.Rows[i]["AxlePartNumber"].ToString();
                S.AxleGSTAmount = dt.Rows[i]["AxleGSTAmount"].ToString();
                S.AxleTotalAmount = dt.Rows[i]["AxleTotalAmount"].ToString();
                S.DifferentialPartNumber = dt.Rows[i]["DifferentialPartNumber"].ToString();
                S.DifferentialGSTAmount = dt.Rows[i]["DifferentialGSTAmount"].ToString();
                S.DifferentialTotalAmount = dt.Rows[i]["DifferentialTotalAmount"].ToString();
                S.TurboPartNumber = dt.Rows[i]["TurboPartNumber"].ToString();
                S.TurboGSTAmount = dt.Rows[i]["TurboGSTAmount"].ToString();
                S.TurboTotalAmount = dt.Rows[i]["TurboTotalAmount"].ToString();
                S.EcmPartNumber = dt.Rows[i]["EcmPartNumber"].ToString();
                S.EcmGSTAmount = dt.Rows[i]["EcmGSTAmount"].ToString();
                S.EcmTotalAmount = dt.Rows[i]["EcmTotalAmount"].ToString();
                S.AccidentalPartNumber = dt.Rows[i]["AccidentalPartNumber"].ToString();
                S.AccidentalGSTAmount = dt.Rows[i]["AccidentalGSTAmount"].ToString();
                S.AccidentalGrossTotalAmount = dt.Rows[i]["AccidentalGrossTotalAmount"].ToString();

                lstSalesDetails.Add(S);
            }

            return Json(lstSalesDetails, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetEditDetails()
        {
            GetRegionDetails();
            return View();
        }

        [HttpPost]
        public JsonResult GetRegionDetails()
        {
            DataSet ds = Sd.GetRegionName();
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Sales> lstRegion = new List<Sales>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Sales S = new Sales();

                S.RegionName = dt.Rows[i]["RegionName"].ToString();
                S.RegionId = dt.Rows[i]["RegionId"].ToString();

                lstRegion.Add(S);
            }
            ViewBag.RegionDetails = lstRegion;
            return Json(lstRegion, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBranchdetails(int RegionId)
        {

            DataSet ds = Sd.GetBranchDetails(RegionId);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Sales> lstBranch = new List<Sales>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Sales H = new Sales();

                H.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                H.BranchName = dt.Rows[i]["BranchName"].ToString();

                lstBranch.Add(H);
            }

            return Json(lstBranch, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult SearchVehicle(string FromDate, string ToDate, int RegionId, int BranchId, string VehicleNumber)
        {
            DataSet ds = Sd.SearchVehicleDetails(FromDate,ToDate,RegionId,BranchId,VehicleNumber);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Sales> lstVehicle = new List<Sales>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Sales H = new Sales();
                										
                H.RecId = dt.Rows[i]["Rec_Id"].ToString();
                H.VehicleNumber = dt.Rows[i]["VechileNumber"].ToString();
                H.SalesDate = dt.Rows[i]["SalesDate"].ToString();
                H.RouteNo = dt.Rows[i]["RouteNumber"].ToString();
                H.RegionId = dt.Rows[i]["RegionId"].ToString();
                H.RegionName = dt.Rows[i]["RegionName"].ToString();
                H.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                H.BranchName = dt.Rows[i]["BranchName"].ToString();
                H.ActualDateTime = dt.Rows[i]["Actual_Date_Time"].ToString();
                H.CreatedBy = dt.Rows[i]["Created_By"].ToString();
                H.TotalAmount = dt.Rows[i]["TotalAmount"].ToString();
                H.Type = dt.Rows[i]["Type"].ToString();

                lstVehicle.Add(H);
            }

            return Json(lstVehicle, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetEntryValue(string RecId)
        {
            DataSet ds = Sd.GetEntryDetails(RecId);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Sales> lstEdited = new List<Sales>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Sales S = new Sales();

                S.RecId = dt.Rows[i]["Rec_Id"].ToString();
                S.NoOfTyres = dt.Rows[i]["NoOfTyres"].ToString();
                S.Tyre = dt.Rows[i]["Tyre"].ToString();
                S.TyreCompanyName = dt.Rows[i]["TyreCompanyName"].ToString();
                S.TyreVendorName = dt.Rows[i]["TyreVendorName"].ToString();
                S.TyreSize = dt.Rows[i]["TyreSize"].ToString();
                S.TyreInvoiceNo = dt.Rows[i]["TyreInvoiceNo"].ToString();
                S.TyreInvoiceDate = Convert.ToDateTime(dt.Rows[i]["TyreInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.TyreKMReading = dt.Rows[i]["TyreKMReading"].ToString();
                S.TyreAmount = dt.Rows[i]["TyreAmount"].ToString();
                S.TyreRemarks = dt.Rows[i]["TyreRemarks"].ToString();
                S.NoOfBattery = dt.Rows[i]["NoOfBattery"].ToString();
                S.Battery = dt.Rows[i]["Battery"].ToString();
                S.BatteryCompanyName = dt.Rows[i]["BatteryCompanyName"].ToString();
                S.BatteryVendorName = dt.Rows[i]["BatteryVendorName"].ToString();
                S.BatteryInvoiceNo = dt.Rows[i]["BatteryInvoiceNo"].ToString();
                S.BatteryMSDMPR = dt.Rows[i]["BatteryMSDMPR"].ToString();
                S.BatteryInvoiceDate = Convert.ToDateTime(dt.Rows[i]["BatteryInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.BatteryKMReading = dt.Rows[i]["BatteryKMReading"].ToString();
                S.BatteryAmount = dt.Rows[i]["BatteryAmount"].ToString();
                S.BatteryRemarks = dt.Rows[i]["BatteryRemarks"].ToString();
                S.RoutineVendorName = dt.Rows[i]["RoutineVendorName"].ToString();
                S.RoutineDealerType = dt.Rows[i]["RoutineDealerType"].ToString();
                S.RoutineInvoiceNo = dt.Rows[i]["RoutineInvoiceNo"].ToString();
                S.RoutineInvoiceDate = Convert.ToDateTime(dt.Rows[i]["RoutineInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.RoutineKMReading = dt.Rows[i]["RoutineKMReading"].ToString();
                S.RoutineAmount = dt.Rows[i]["RoutineAmount"].ToString();
                S.RoutineRemarks = dt.Rows[i]["RoutineRemarks"].ToString();
                S.DentingPaintingType = dt.Rows[i]["DentingPaintingType"].ToString();
                S.DentingVendorName = dt.Rows[i]["DentingVendorName"].ToString();
                S.DentingInvoiceNo = dt.Rows[i]["DentingInvoiceNo"].ToString();
                S.DentingInvoiceDate = Convert.ToDateTime(dt.Rows[i]["DentingInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.DentingKMReading = dt.Rows[i]["DentingKMReading"].ToString();
                S.DentingAmount = dt.Rows[i]["DentingAmount"].ToString();
                S.DentingRemarks = dt.Rows[i]["DentingRemarks"].ToString();
                S.MinorVendorName = dt.Rows[i]["MinorVendorName"].ToString();
                S.MinorInvoiceNo = dt.Rows[i]["MinorInvoiceNo"].ToString();
                S.MinorInvoiceDate = Convert.ToDateTime(dt.Rows[i]["MinorInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.MinorKMReading = dt.Rows[i]["MinorKMReading"].ToString();
                S.MinorAmount = dt.Rows[i]["MinorAmount"].ToString();
                S.MinorRemarks = dt.Rows[i]["MinorRemarks"].ToString();
                S.SeatVendorname = dt.Rows[i]["SeatVendorname"].ToString();
                S.SeatInvoiceNo = dt.Rows[i]["SeatInvoiceNo"].ToString();
                S.SeatInvoiceDate = Convert.ToDateTime(dt.Rows[i]["SeatInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.SeatKMReading = dt.Rows[i]["SeatKMReading"].ToString();
                S.SeatAmount = dt.Rows[i]["SeatAmount"].ToString();
                S.SeatRemarks = dt.Rows[i]["SeatRemarks"].ToString();
                S.SelfVendorName = dt.Rows[i]["SelfVendorName"].ToString();
                S.SelfDealerType = dt.Rows[i]["SelfDealerType"].ToString();
                S.SelfInvoiceNo = dt.Rows[i]["SelfInvoiceNo"].ToString();
                S.SelfInvoiceDate = Convert.ToDateTime(dt.Rows[i]["SelfInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.SelfKMReading = dt.Rows[i]["SelfKMReading"].ToString();
                S.SelfAmount = dt.Rows[i]["SelfAmount"].ToString();
                S.SelfRemarks = dt.Rows[i]["SelfRemarks"].ToString();
                S.ElectricalVendorName = dt.Rows[i]["ElectricalVendorName"].ToString();
                S.ElectricalDealerType = dt.Rows[i]["ElectricalDealerType"].ToString();
                S.ElectricalInvoiceNo = dt.Rows[i]["ElectricalInvoiceNo"].ToString();
                S.ElectricalInvoiceDate = Convert.ToDateTime(dt.Rows[i]["ElectricalInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.ElectricalKMReading = dt.Rows[i]["ElectricalKMReading"].ToString();
                S.ElectricalAmount = dt.Rows[i]["ElectricalAmount"].ToString();
                S.ElectricalRemarks = dt.Rows[i]["ElectricalRemarks"].ToString();
                S.ClutchVendorName = dt.Rows[i]["ClutchVendorName"].ToString();
                S.ClutchDealerType = dt.Rows[i]["ClutchDealerType"].ToString();
                S.ClutchInvoiceNo = dt.Rows[i]["ClutchInvoiceNo"].ToString();
                S.ClutchInvoiceDate = Convert.ToDateTime(dt.Rows[i]["ClutchInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.ClutchKMReading = dt.Rows[i]["ClutchKMReading"].ToString();
                S.ClutchAmount = dt.Rows[i]["ClutchAmount"].ToString();
                S.ClutchRemarks = dt.Rows[i]["ClutchRemarks"].ToString();
                S.AlternatorVendorName = dt.Rows[i]["AlternatorVendorName"].ToString();
                S.AlternatorDealerType = dt.Rows[i]["AlternatorDealerType"].ToString();
                S.AlternatorInvoiceNo = dt.Rows[i]["AlternatorInvoiceNo"].ToString();
                S.AlternatorInvoiceDate = Convert.ToDateTime(dt.Rows[i]["AlternatorInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.AlternatorKMReading = dt.Rows[i]["AlternatorKMReading"].ToString();
                S.AlternatorAmount = dt.Rows[i]["AlternatorAmount"].ToString();
                S.AlternatorRemarks = dt.Rows[i]["AlternatorRemarks"].ToString();
                S.LeafVendorName = dt.Rows[i]["LeafVendorName"].ToString();
                S.LeafDealerType = dt.Rows[i]["LeafDealerType"].ToString();
                S.LeafInvoiceNo = dt.Rows[i]["LeafInvoiceNo"].ToString();
                S.LeafInvoiceDate = Convert.ToDateTime(dt.Rows[i]["LeafInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.LeafKMReading = dt.Rows[i]["LeafKMReading"].ToString();
                S.LeafAmount = dt.Rows[i]["LeafAmount"].ToString();
                S.LeafRemarks = dt.Rows[i]["LeafRemarks"].ToString();
                S.SuspensionVendorName = dt.Rows[i]["SuspensionVendorName"].ToString();
                S.SuspensionDealerType = dt.Rows[i]["SuspensionDealerType"].ToString();
                S.SuspensionInvoiceNo = dt.Rows[i]["SuspensionInvoiceNo"].ToString();
                S.SuspensionInvoiceDate = Convert.ToDateTime(dt.Rows[i]["SuspensionInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.SuspensionKMReading = dt.Rows[i]["SuspensionKMReading"].ToString();
                S.SuspensionAmount = dt.Rows[i]["SuspensionAmount"].ToString();
                S.SuspensionRemarks = dt.Rows[i]["SuspensionRemarks"].ToString();
                S.GearBoxVendorName = dt.Rows[i]["GearBoxVendorName"].ToString();
                S.GearBoxDealerType = dt.Rows[i]["GearBoxDealerType"].ToString();
                S.GearBoxInvoiceNo = dt.Rows[i]["GearBoxInvoiceNo"].ToString();
                S.GearBoxInvoiceDate = Convert.ToDateTime(dt.Rows[i]["GearBoxInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.GearBoxKMReading = dt.Rows[i]["GearBoxKMReading"].ToString();
                S.GearBoxAmount = dt.Rows[i]["GearBoxAmount"].ToString();
                S.GearBoxRemarks = dt.Rows[i]["GearBoxRemarks"].ToString();
                S.BreakWorkVendorName = dt.Rows[i]["BreakWorkVendorName"].ToString();
                S.BreakWorkDealerType = dt.Rows[i]["BreakWorkDealerType"].ToString();
                S.BreakWorkInvoiceNo = dt.Rows[i]["BreakWorkInvoiceNo"].ToString();
                S.BreakWorkInvoiceDate = Convert.ToDateTime(dt.Rows[i]["BreakWorkInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.BreakWorkKMReading = dt.Rows[i]["BreakWorkKMReading"].ToString();
                S.BreakWorkAmount = dt.Rows[i]["BreakWorkAmount"].ToString();
                S.BreakWorkRemarks = dt.Rows[i]["BreakWorkRemarks"].ToString();
                S.EngineWorkVendorName = dt.Rows[i]["EngineWorkVendorName"].ToString();
                S.EngineWorkDealerType = dt.Rows[i]["EngineWorkDealerType"].ToString();
                S.EngineWorkInvoiceNo = dt.Rows[i]["EngineWorkInvoiceNo"].ToString();
                S.EngineWorkInvoiceDate = Convert.ToDateTime(dt.Rows[i]["EngineWorkInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.EngineWorkKMReading = dt.Rows[i]["EngineWorkKMReading"].ToString();
                S.EngineWorkAmount = dt.Rows[i]["EngineWorkAmount"].ToString();
                S.EngineWorkRemarks = dt.Rows[i]["EngineWorkRemarks"].ToString();
                S.FuelVendorName = dt.Rows[i]["FuelVendorName"].ToString();
                S.FuelDealerType = dt.Rows[i]["FuelDealerType"].ToString();
                S.FuelInvoiceNo = dt.Rows[i]["FuelInvoiceNo"].ToString();
                S.FuelInvoiceDate = Convert.ToDateTime(dt.Rows[i]["FuelInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.FuelKMReading = dt.Rows[i]["FuelKMReading"].ToString();
                S.FuelAmount = dt.Rows[i]["FuelAmount"].ToString();
                S.FuelRemarks = dt.Rows[i]["FuelRemarks"].ToString();
                S.PuncherVendorName = dt.Rows[i]["PuncherVendorName"].ToString();
                S.PuncherNoofPuncher = dt.Rows[i]["PuncherNoofPuncher"].ToString();
                S.PuncherInvoiceNo = dt.Rows[i]["PuncherInvoiceNo"].ToString();
                S.PuncherInvoiceDate = Convert.ToDateTime(dt.Rows[i]["PuncherInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.PuncherKMReading = dt.Rows[i]["PuncherKMReading"].ToString();
                S.PuncherAmount = dt.Rows[i]["PuncherAmount"].ToString();
                S.PuncherRemarks = dt.Rows[i]["PuncherRemarks"].ToString();
                S.OilVendorName = dt.Rows[i]["OilVendorName"].ToString();
                S.OilLtr = dt.Rows[i]["OilLtr"].ToString();
                S.OilInvoiceNo = dt.Rows[i]["OilInvoiceNo"].ToString();
                S.OilInvoiceDate = Convert.ToDateTime(dt.Rows[i]["OilInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.OilKMReading = dt.Rows[i]["OilKMReading"].ToString();
                S.OilAmount = dt.Rows[i]["OilAmount"].ToString();
                S.OilRemarks = dt.Rows[i]["OilRemarks"].ToString();
                S.RadiatorVendorName = dt.Rows[i]["RadiatorVendorName"].ToString();
                S.RadiatorDealerType = dt.Rows[i]["RadiatorDealerType"].ToString();
                S.RadiatorInvoiceNo = dt.Rows[i]["RadiatorInvoiceNo"].ToString();
                S.RadiatorInvoiceDate = Convert.ToDateTime(dt.Rows[i]["RadiatorInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.RadiatorKMReading = dt.Rows[i]["RadiatorKMReading"].ToString();
                S.RadiatorAmount = dt.Rows[i]["RadiatorAmount"].ToString();
                S.RadiatorRemarks = dt.Rows[i]["RadiatorRemarks"].ToString();
                S.AxleVendorName = dt.Rows[i]["AxleVendorName"].ToString();
                S.AxleDealerType = dt.Rows[i]["AxleDealerType"].ToString();
                S.AxleInvoiceNo = dt.Rows[i]["AxleInvoiceNo"].ToString();
                S.AxleInvoiceDate = Convert.ToDateTime(dt.Rows[i]["AxleInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.AxleKMReading = dt.Rows[i]["AxleKMReading"].ToString();
                S.AxleAmount = dt.Rows[i]["AxleAmount"].ToString();
                S.AxleRemarks = dt.Rows[i]["AxleRemarks"].ToString();
                S.DifferentialVendorName = dt.Rows[i]["DifferentialVendorName"].ToString();
                S.DifferentialDealerType = dt.Rows[i]["DifferentialDealerType"].ToString();
                S.DifferentialInvoiceNo = dt.Rows[i]["DifferentialInvoiceNo"].ToString();
                S.DifferentialInvoiceDate = Convert.ToDateTime(dt.Rows[i]["DifferentialInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.DifferentialKMReading = dt.Rows[i]["DifferentialKMReading"].ToString();
                S.DifferentialAmount = dt.Rows[i]["DifferentialAmount"].ToString();
                S.DifferentialRemarks = dt.Rows[i]["DifferentialRemarks"].ToString();

                S.TurboVendorName = dt.Rows[i]["TurboVendorName"].ToString();
                S.TurboDealerType = dt.Rows[i]["TurboDealerType"].ToString();
                S.TurboInvoiceNo = dt.Rows[i]["TurboInvoiceNo"].ToString();
                S.TurboInvoiceDate = Convert.ToDateTime(dt.Rows[i]["TurboInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.TurboKMReading = dt.Rows[i]["TurboKMReading"].ToString();
                S.TurboAmount = dt.Rows[i]["TurboAmount"].ToString();
                S.TurboNarration = dt.Rows[i]["TurboNarration"].ToString();
                S.EcmVendorName = dt.Rows[i]["EcmVendorName"].ToString();
                S.EcmDealerType = dt.Rows[i]["EcmDealerType"].ToString();
                S.EcmInvoiceNo = dt.Rows[i]["EcmInvoiceNo"].ToString();
                S.EcmInvoiceDate = Convert.ToDateTime(dt.Rows[i]["EcmInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.EcmKMReading = dt.Rows[i]["EcmKMReading"].ToString();
                S.EcmAmount = dt.Rows[i]["EcmAmount"].ToString();
                S.EcmNarration = dt.Rows[i]["EcmNarration"].ToString();
                S.AccidentalVendorName = dt.Rows[i]["AccidentalVendorName"].ToString();
                S.AccidentalDealerType = dt.Rows[i]["AccidentalDealerType"].ToString();
                S.AccidentalInvoiceNo = dt.Rows[i]["AccidentalInvoiceNo"].ToString();
                S.AccidentalInvoiceDate = Convert.ToDateTime(dt.Rows[i]["AccidentalInvoiceDate"].ToString()).ToString("yyyy-MM-dd");
                S.AccidentalKMReading = dt.Rows[i]["AccidentalKMReading"].ToString();
                S.AccidentalInsCoveredAmount = dt.Rows[i]["AccidentalInsCoveredAmount"].ToString();
                S.AccidentalDifferenceAmount = dt.Rows[i]["AccidentalDifferenceAmount"].ToString();
                S.AccidentalTotalAmount = dt.Rows[i]["AccidentalTotalAmount"].ToString();
                S.AccidentalNarration = dt.Rows[i]["AccidentalNarration"].ToString();

                S.TyrePartNumber = dt.Rows[i]["TyrePartNumber"].ToString();
                S.TyreGSTAmount = dt.Rows[i]["TyreGSTAmount"].ToString();
                S.TyreTotalAmount = dt.Rows[i]["TyreTotalAmount"].ToString();
                S.BatteryPartNumber = dt.Rows[i]["BatteryPartNumber"].ToString();
                S.BatteryGSTAmount = dt.Rows[i]["BatteryGSTAmount"].ToString();
                S.BatteryTotalAmount = dt.Rows[i]["BatteryTotalAmount"].ToString();
                S.RoutinePartNumber = dt.Rows[i]["RoutinePartNumber"].ToString();
                S.RoutineGSTAmount = dt.Rows[i]["RoutineGSTAmount"].ToString();
                S.RoutineTotalAmount = dt.Rows[i]["RoutineTotalAmount"].ToString();
                S.DentingPartNumber = dt.Rows[i]["DentingPartNumber"].ToString();
                S.DentingGSTAmount = dt.Rows[i]["DentingGSTAmount"].ToString();
                S.DentingTotalAmount = dt.Rows[i]["DentingTotalAmount"].ToString();
                S.MinorPartNumber = dt.Rows[i]["MinorPartNumber"].ToString();
                S.MinorGSTAmount = dt.Rows[i]["MinorGSTAmount"].ToString();
                S.MinorTotalAmount = dt.Rows[i]["MinorTotalAmount"].ToString();
                S.SeatPartNumber = dt.Rows[i]["SeatPartNumber"].ToString();
                S.SeatGSTAmount = dt.Rows[i]["SeatGSTAmount"].ToString();
                S.SeatTotalAmount = dt.Rows[i]["SeatTotalAmount"].ToString();
                S.SelfPartNumber = dt.Rows[i]["SelfPartNumber"].ToString();
                S.SelfGSTAmount = dt.Rows[i]["SelfGSTAmount"].ToString();
                S.SelfTotalAmount = dt.Rows[i]["SelfTotalAmount"].ToString();
                S.ElectricalPartNumber = dt.Rows[i]["ElectricalPartNumber"].ToString();
                S.ElectricalGSTAmount = dt.Rows[i]["ElectricalGSTAmount"].ToString();
                S.ElectricalTotalAmount = dt.Rows[i]["ElectricalTotalAmount"].ToString();
                S.ClutchPartNumber = dt.Rows[i]["ClutchPartNumber"].ToString();
                S.ClutchGSTAmount = dt.Rows[i]["ClutchGSTAmount"].ToString();
                S.ClutchTotalAmount = dt.Rows[i]["ClutchTotalAmount"].ToString();
                S.AlternatorPartNumber = dt.Rows[i]["AlternatorPartNumber"].ToString();
                S.AlternatorGSTAmount = dt.Rows[i]["AlternatorGSTAmount"].ToString();
                S.AlternatorTotalAmount = dt.Rows[i]["AlternatorTotalAmount"].ToString();
                S.LeafPartNumber = dt.Rows[i]["LeafPartNumber"].ToString();
                S.LeafGSTAmount = dt.Rows[i]["LeafGSTAmount"].ToString();
                S.LeafTotalAmount = dt.Rows[i]["LeafTotalAmount"].ToString();
                S.SuspensionPartNumber = dt.Rows[i]["SuspensionPartNumber"].ToString();
                S.SuspensionGSTAmount = dt.Rows[i]["SuspensionGSTAmount"].ToString();
                S.SuspensionTotalAmount = dt.Rows[i]["SuspensionTotalAmount"].ToString();
                S.GearBoxPartNumber = dt.Rows[i]["GearBoxPartNumber"].ToString();
                S.GearBoxGSTAmount = dt.Rows[i]["GearBoxGSTAmount"].ToString();
                S.GearBoxTotalAmount = dt.Rows[i]["GearBoxTotalAmount"].ToString();
                S.BreakPartNumber = dt.Rows[i]["BreakPartNumber"].ToString();
                S.BreakGSTAmount = dt.Rows[i]["BreakGSTAmount"].ToString();
                S.BreakTotalAmount = dt.Rows[i]["BreakTotalAmount"].ToString();
                S.EnginePartNumber = dt.Rows[i]["EnginePartNumber"].ToString();
                S.EngineGSTAmount = dt.Rows[i]["EngineGSTAmount"].ToString();
                S.EngineTotalAmount = dt.Rows[i]["EngineTotalAmount"].ToString();
                S.FuelPartNumber = dt.Rows[i]["FuelPartNumber"].ToString();
                S.FuelGSTAmount = dt.Rows[i]["FuelGSTAmount"].ToString();
                S.FuelTotalAmount = dt.Rows[i]["FuelTotalAmount"].ToString();
                S.PuncherPartNumber = dt.Rows[i]["PuncherPartNumber"].ToString();
                S.PuncherGSTAmount = dt.Rows[i]["PuncherGSTAmount"].ToString();
                S.PuncherTotalAmount = dt.Rows[i]["PuncherTotalAmount"].ToString();
                S.OilPartNumber = dt.Rows[i]["OilPartNumber"].ToString();
                S.OilGSTAmount = dt.Rows[i]["OilGSTAmount"].ToString();
                S.OilTotalAmount = dt.Rows[i]["OilTotalAmount"].ToString();
                S.RadiatorPartNumber = dt.Rows[i]["RadiatorPartNumber"].ToString();
                S.RadiatorGSTAmount = dt.Rows[i]["RadiatorGSTAmount"].ToString();
                S.RadiatorTotalAmount = dt.Rows[i]["RadiatorTotalAmount"].ToString();
                S.AxlePartNumber = dt.Rows[i]["AxlePartNumber"].ToString();
                S.AxleGSTAmount = dt.Rows[i]["AxleGSTAmount"].ToString();
                S.AxleTotalAmount = dt.Rows[i]["AxleTotalAmount"].ToString();
                S.DifferentialPartNumber = dt.Rows[i]["DifferentialPartNumber"].ToString();
                S.DifferentialGSTAmount = dt.Rows[i]["DifferentialGSTAmount"].ToString();
                S.DifferentialTotalAmount = dt.Rows[i]["DifferentialTotalAmount"].ToString();
                S.TurboPartNumber = dt.Rows[i]["TurboPartNumber"].ToString();
                S.TurboGSTAmount = dt.Rows[i]["TurboGSTAmount"].ToString();
                S.TurboTotalAmount = dt.Rows[i]["TurboTotalAmount"].ToString();
                S.EcmPartNumber = dt.Rows[i]["EcmPartNumber"].ToString();
                S.EcmGSTAmount = dt.Rows[i]["EcmGSTAmount"].ToString();
                S.EcmTotalAmount = dt.Rows[i]["EcmTotalAmount"].ToString();
                S.AccidentalPartNumber = dt.Rows[i]["AccidentalPartNumber"].ToString();
                S.AccidentalGSTAmount = dt.Rows[i]["AccidentalGSTAmount"].ToString();
                S.AccidentalGrossTotalAmount = dt.Rows[i]["AccidentalGrossTotalAmount"].ToString();

                S.TyerDealerType = dt.Rows[i]["TyerDealerType"].ToString();
                S.BatteryDealerType = dt.Rows[i]["BatteryDealerType"].ToString();
                S.DentingDealerType = dt.Rows[i]["DentingDealerType"].ToString();
                S.MinorDealerType = dt.Rows[i]["MinorDealerType"].ToString();
                S.SeatDealerType = dt.Rows[i]["SeatDealerType"].ToString();
                S.PuncherDealerType = dt.Rows[i]["PuncherDealerType"].ToString();
                S.OilDealerType = dt.Rows[i]["OilDealerType"].ToString();


                lstEdited.Add(S);
            }

            return Json(lstEdited, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult UpdateSalesDetails(string NoOfTyres, string Tyre, string TyreCompanyName, string TyreVendorName, string TyreSize, string TyreInvoiceNo, string TyreInvoiceDate,
                                           string TyreKMReading, string TyreAmount, string TyreRemarks, string NoOfBattery, string Battery, string BatteryCompanyName, string BatteryVendorName,
                                           string BatteryInvoiceNo, string BatteryMSDMPR, string BatteryInvoiceDate, string BatteryKMReading, string BatteryAmount, string BatteryRemarks,
                                           string RoutineVendorName, string RoutineDealerType, string RoutineInvoiceNo, string RoutineInvoiceDate, string RoutineKMReading,
                                           string RoutineAmount, string RoutineRemarks, string DentingPaintingType, string DentingVendorName, string DentingInvoiceNo,
                                           string DentingInvoiceDate, string DentingKMReading, string DentingAmount, string DentingRemarks, string MinorVendorName, string MinorInvoiceNo,
                                           string MinorInvoiceDate, string MinorKMReading, string MinorAmount, string MinorRemarks, string SeatVendorname, string SeatInvoiceNo,
                                           string SeatInvoiceDate, string SeatKMReading, string SeatAmount, string SeatRemarks, string SelfVendorName, string SelfDealerType,
                                           string SelfInvoiceNo, string SelfInvoiceDate, string SelfKMReading, string SelfAmount, string SelfRemarks, string ElectricalVendorName,
                                           string ElectricalDealerType, string ElectricalInvoiceNo, string ElectricalInvoiceDate, string ElectricalKMReading, string ElectricalAmount,
                                           string ElectricalRemarks, string ClutchVendorName, string ClutchDealerType, string ClutchInvoiceNo, string ClutchInvoiceDate,
                                           string ClutchKMReading, string ClutchAmount, string ClutchRemarks, string AlternatorVendorName, string AlternatorDealerType, string AlternatorInvoiceNo,
                                           string AlternatorInvoiceDate, string AlternatorKMReading, string AlternatorAmount, string AlternatorRemarks, string LeafVendorName, string LeafDealerType,
                                           string LeafInvoiceNo, string LeafInvoiceDate, string LeafKMReading, string LeafAmount, string LeafRemarks, string SuspensionVendorName,
                                           string SuspensionDealerType, string SuspensionInvoiceNo, string SuspensionInvoiceDate, string SuspensionKMReading, string SuspensionAmount,
                                           string SuspensionRemarks, string GearBoxVendorName, string GearBoxDealerType, string GearBoxInvoiceNo, string GearBoxInvoiceDate, string GearBoxKMReading,
                                           string GearBoxAmount, string GearBoxRemarks, string BreakWorkVendorName, string BreakWorkDealerType, string BreakWorkInvoiceNo, string BreakWorkInvoiceDate,
                                           string BreakWorkKMReading, string BreakWorkAmount, string BreakWorkRemarks, string EngineWorkVendorName, string EngineWorkDealerType, string EngineWorkInvoiceNo,
                                           string EngineWorkInvoiceDate, string EngineWorkKMReading, string EngineWorkAmount, string EngineWorkRemarks, string FuelVendorName, string FuelDealerType,
                                           string FuelInvoiceNo, string FuelInvoiceDate, string FuelKMReading, string FuelAmount, string FuelRemarks, string PuncherVendorName, string PuncherNoofPuncher,
                                           string PuncherInvoiceNo, string PuncherInvoiceDate, string PuncherKMReading, string PuncherAmount, string PuncherRemarks, string OilVendorName, string OilLtr,
                                           string OilInvoiceNo, string OilInvoiceDate, string OilKMReading, string OilAmount, string OilRemarks, string RadiatorVendorName, string RadiatorDealerType,
                                           string RadiatorInvoiceNo, string RadiatorInvoiceDate, string RadiatorKMReading, string RadiatorAmount, string RadiatorRemarks, string AxleVendorName,
                                           string AxleDealerType, string AxleInvoiceNo, string AxleInvoiceDate, string AxleKMReading, string AxleAmount, string AxleRemarks, string DifferentialVendorName,
                                           string DifferentialDealerType, string DifferentialInvoiceNo, string DifferentialInvoiceDate, string DifferentialKMReading, string DifferentialAmount,
                                           string DifferentialRemarks, string TurboVendorName, string TurboDealerType, string TurboInvoiceNo, string TurboInvoiceDate, string TurboKMReading,
                                           string TurboAmount, string TurboNarration, string EcmVendorName, string EcmDealerType, string EcmInvoiceNo, string EcmInvoiceDate, string EcmKMReading,
                                           string EcmAmount, string EcmNarration, string AccidentalVendorName, string AccidentalDealerType, string AccidentalInvoiceNo, string AccidentalInvoiceDate,
                                           string AccidentalKMReading, string AccidentalInsCoveredAmount, string AccidentalDifferenceAmount, string AccidentalTotalAmount, string AccidentalNarration,
                                           string TyrePartNumber, string TyreGSTAmount, string TyreTotalAmount, 
                                           string BatteryPartNumber, string BatteryGSTAmount, string BatteryTotalAmount, string RoutinePartNumber, string RoutineGSTAmount, string RoutineTotalAmount, 
                                           string DentingPartNumber, string DentingGSTAmount, string DentingTotalAmount, string MinorPartNumber, string MinorGSTAmount, string MinorTotalAmount, string SeatPartNumber, 
                                           string SeatGSTAmount, string SeatTotalAmount, string SelfPartNumber, string SelfGSTAmount, string SelfTotalAmount, string ElectricalPartNumber, string ElectricalGSTAmount, 
                                           string ElectricalTotalAmount, string ClutchPartNumber, string ClutchGSTAmount, string ClutchTotalAmount, string AlternatorPartNumber, string AlternatorGSTAmount, 
                                           string AlternatorTotalAmount, string LeafPartNumber, string LeafGSTAmount, string LeafTotalAmount, string SuspensionPartNumber, string SuspensionGSTAmount, 
                                           string SuspensionTotalAmount, string GearBoxPartNumber, string GearBoxGSTAmount, string GearBoxTotalAmount, string BreakPartNumber, string BreakGSTAmount, 
                                           string BreakTotalAmount, string EnginePartNumber, string EngineGSTAmount, string EngineTotalAmount, string FuelPartNumber, string FuelGSTAmount, string FuelTotalAmount, 
                                           string PuncherPartNumber, string PuncherGSTAmount, string PuncherTotalAmount, string OilPartNumber, string OilGSTAmount, string OilTotalAmount, string RadiatorPartNumber, 
                                           string RadiatorGSTAmount, string RadiatorTotalAmount, string AxlePartNumber, string AxleGSTAmount, string AxleTotalAmount, string DifferentialPartNumber, string DifferentialGSTAmount, 
                                           string DifferentialTotalAmount, string TurboPartNumber, string TurboGSTAmount, string TurboTotalAmount, string EcmPartNumber, string EcmGSTAmount, string EcmTotalAmount, string AccidentalPartNumber,
                                           string AccidentalGSTAmount, string AccidentalGrossTotalAmount, string RecId, string TyerDealerType, string BatteryDealerType, string DentingDealerType, string MinorDealerType,
                                           string SeatDealerType, string PuncherDealerType, string OilDealerType)
        {
            string Name = Session["UserName"].ToString();

            var resultSuccess = new { Message = "Updated Successfully Uploaded." };
            var resultError = new { Message = "Error Please Check." };

            int return1 = 0;

            return1 = Sd.SalesDetailsUpdate(NoOfTyres, Tyre, TyreCompanyName, TyreVendorName, TyreSize, TyreInvoiceNo, TyreInvoiceDate, TyreKMReading, TyreAmount, TyreRemarks,
                                            NoOfBattery, Battery, BatteryCompanyName, BatteryVendorName, BatteryInvoiceNo, BatteryMSDMPR, BatteryInvoiceDate, BatteryKMReading,
                                            BatteryAmount, BatteryRemarks, RoutineVendorName, RoutineDealerType, RoutineInvoiceNo, RoutineInvoiceDate, RoutineKMReading,
                                            RoutineAmount, RoutineRemarks, DentingPaintingType, DentingVendorName, DentingInvoiceNo, DentingInvoiceDate, DentingKMReading,
                                            DentingAmount, DentingRemarks, MinorVendorName, MinorInvoiceNo, MinorInvoiceDate, MinorKMReading, MinorAmount, MinorRemarks, SeatVendorname,
                                            SeatInvoiceNo, SeatInvoiceDate, SeatKMReading, SeatAmount, SeatRemarks, SelfVendorName, SelfDealerType, SelfInvoiceNo, SelfInvoiceDate,
                                            SelfKMReading, SelfAmount, SelfRemarks, ElectricalVendorName, ElectricalDealerType, ElectricalInvoiceNo, ElectricalInvoiceDate,
                                            ElectricalKMReading, ElectricalAmount, ElectricalRemarks, ClutchVendorName, ClutchDealerType, ClutchInvoiceNo, ClutchInvoiceDate,
                                            ClutchKMReading, ClutchAmount, ClutchRemarks, AlternatorVendorName, AlternatorDealerType, AlternatorInvoiceNo, AlternatorInvoiceDate,
                                            AlternatorKMReading, AlternatorAmount, AlternatorRemarks, LeafVendorName, LeafDealerType, LeafInvoiceNo, LeafInvoiceDate, LeafKMReading,
                                            LeafAmount, LeafRemarks, SuspensionVendorName, SuspensionDealerType, SuspensionInvoiceNo, SuspensionInvoiceDate, SuspensionKMReading,
                                            SuspensionAmount, SuspensionRemarks, GearBoxVendorName, GearBoxDealerType, GearBoxInvoiceNo, GearBoxInvoiceDate, GearBoxKMReading,
                                            GearBoxAmount, GearBoxRemarks, BreakWorkVendorName, BreakWorkDealerType, BreakWorkInvoiceNo, BreakWorkInvoiceDate, BreakWorkKMReading,
                                            BreakWorkAmount, BreakWorkRemarks, EngineWorkVendorName, EngineWorkDealerType, EngineWorkInvoiceNo, EngineWorkInvoiceDate, EngineWorkKMReading,
                                            EngineWorkAmount, EngineWorkRemarks, FuelVendorName, FuelDealerType, FuelInvoiceNo, FuelInvoiceDate, FuelKMReading, FuelAmount, FuelRemarks,
                                            PuncherVendorName, PuncherNoofPuncher, PuncherInvoiceNo, PuncherInvoiceDate, PuncherKMReading, PuncherAmount, PuncherRemarks, OilVendorName,
                                            OilLtr, OilInvoiceNo, OilInvoiceDate, OilKMReading, OilAmount, OilRemarks, RadiatorVendorName, RadiatorDealerType, RadiatorInvoiceNo,
                                            RadiatorInvoiceDate, RadiatorKMReading, RadiatorAmount, RadiatorRemarks, AxleVendorName, AxleDealerType, AxleInvoiceNo, AxleInvoiceDate,
                                            AxleKMReading, AxleAmount, AxleRemarks, DifferentialVendorName, DifferentialDealerType, DifferentialInvoiceNo, DifferentialInvoiceDate,
                                            DifferentialKMReading, DifferentialAmount, DifferentialRemarks, TurboVendorName, TurboDealerType, TurboInvoiceNo, TurboInvoiceDate,
                                            TurboKMReading, TurboAmount, TurboNarration, EcmVendorName, EcmDealerType, EcmInvoiceNo, EcmInvoiceDate, EcmKMReading, EcmAmount,
                                            EcmNarration, AccidentalVendorName, AccidentalDealerType, AccidentalInvoiceNo, AccidentalInvoiceDate, AccidentalKMReading,
                                            AccidentalInsCoveredAmount, AccidentalDifferenceAmount, AccidentalTotalAmount, AccidentalNarration, RecId, Name, TyrePartNumber,
                                            TyreGSTAmount, TyreTotalAmount, BatteryPartNumber, BatteryGSTAmount, BatteryTotalAmount,
                                            RoutinePartNumber, RoutineGSTAmount, RoutineTotalAmount, DentingPartNumber, DentingGSTAmount, DentingTotalAmount, MinorPartNumber, MinorGSTAmount,
                                            MinorTotalAmount, SeatPartNumber, SeatGSTAmount, SeatTotalAmount, SelfPartNumber, SelfGSTAmount, SelfTotalAmount, ElectricalPartNumber,
                                            ElectricalGSTAmount, ElectricalTotalAmount, ClutchPartNumber, ClutchGSTAmount, ClutchTotalAmount, AlternatorPartNumber, AlternatorGSTAmount,
                                            AlternatorTotalAmount, LeafPartNumber, LeafGSTAmount, LeafTotalAmount, SuspensionPartNumber, SuspensionGSTAmount, SuspensionTotalAmount, GearBoxPartNumber,
                                            GearBoxGSTAmount, GearBoxTotalAmount, BreakPartNumber, BreakGSTAmount, BreakTotalAmount, EnginePartNumber, EngineGSTAmount, EngineTotalAmount, FuelPartNumber,
                                            FuelGSTAmount, FuelTotalAmount, PuncherPartNumber, PuncherGSTAmount, PuncherTotalAmount, OilPartNumber, OilGSTAmount, OilTotalAmount, RadiatorPartNumber,
                                            RadiatorGSTAmount, RadiatorTotalAmount, AxlePartNumber, AxleGSTAmount, AxleTotalAmount, DifferentialPartNumber, DifferentialGSTAmount, DifferentialTotalAmount,
                                            TurboPartNumber, TurboGSTAmount, TurboTotalAmount, EcmPartNumber, EcmGSTAmount, EcmTotalAmount, AccidentalPartNumber, AccidentalGSTAmount, AccidentalGrossTotalAmount,
                                            TyerDealerType, BatteryDealerType, DentingDealerType, MinorDealerType,SeatDealerType, PuncherDealerType, OilDealerType);
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
        public ActionResult UploadFiles()
        {
                ci.ErrorLog("C:\\RMS_Fleet_App\\DocumentUploadError\\", "UploadFiles" + "Uploaded Start" + DateTime.Now);
                string RecId = Session["RecId"].ToString();
                ci.ErrorLog("C:\\RMS_Fleet_App\\DocumentUploadError\\", "RecId" + "RecId found" + DateTime.Now);
                //Create Directory
                if (Directory.Exists("C:\\RMS_Fleet\\Uploaded_Files\\" + RecId + "\\") == false)
                {
                    ci.ErrorLog("C:\\RMS_Fleet_App\\DocumentUploadError\\", "Directory" + "Exists Check" + DateTime.Now);
                    Directory.CreateDirectory(("C:\\RMS_Fleet\\Uploaded_Files\\" + RecId + "\\"));
                    ci.ErrorLog("C:\\RMS_Fleet_App\\DocumentUploadError\\", "Directory" + "Exists" + DateTime.Now);
                }

                string path = ("C:\\RMS_Fleet\\Uploaded_Files\\" + RecId + "\\").ToString();
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    ci.ErrorLog("C:\\RMS_Fleet_App\\DocumentUploadError\\", "Loop Started" + "Loop Started" + DateTime.Now);
                    HttpPostedFileBase file = files[i];
                    file.SaveAs(path + RecId.ToString() + "_" + files.AllKeys[i] + ".pdf");

                    ci.ErrorLog("C:\\RMS_Fleet_App\\DocumentUploadError\\", "file stored" + " first Loop Started" + DateTime.Now);
                }
                Session["RecId"] = null;
                return Json(files.Count + " Files Uploaded!");
        }

        [HttpPost]
        public JsonResult ExportExcel()
        {
            string RecId = Session["RecId"].ToString().Trim();
            string Type = Session["Type"].ToString().Trim();
            string VehicleNo = Session["VehicleNo"].ToString().Trim();

            //// DataTable dt = DataService.GetData();
            var fileName = "" + VehicleNo +" _ "+ Type + ".pdf";

            var filePath = "C:\\RMS_Fleet\\Uploaded_Files\\" + RecId + "\\" + RecId + "_" + Type + ".pdf";
            var errorMessage ="";
            WebRequest webRequest = WebRequest.Create(filePath);
            webRequest.Timeout = 1200; // miliseconds
            webRequest.Method = "HEAD";

            try
            {
                webRequest.GetResponse();
                Session["fullPath"] = filePath;
            }
            catch
            {
                fileName = "";
                errorMessage = "No Details Found For The Seletect Type!";
            }

            //return the Excel file name
            return Json(new { fileName = fileName, errorMessage = errorMessage });
        }

        [HttpGet]
        public ActionResult Download(string file)
        {
            string fullPath ="";
            try
            {
                if (Session["fullPath"].ToString() != "")
                {
                    fullPath = Session["fullPath"].ToString();
                    Session["fullPath"] = null;
                    return File(fullPath, "application/pdf", file);
                }
            }
            catch
            {
                Session["fullPath"] = null;
            }

            return Json("Sucessfully Value Set", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SetSessionValue(string RecId, string Type, string VehicleNo)
        {
            Session["RecId"] = RecId;
            Session["Type"]  = Type;
            Session["VehicleNo"] = VehicleNo;

            return Json("Sucessfully Value Set", JsonRequestBehavior.AllowGet);

        }
    }
}
