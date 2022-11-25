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
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

namespace RMS_Fleet.Controllers
{
    public class ApprovalRejectionController : Controller
    {
        ApprovalRejection Sd = new ApprovalRejection();
        clsImportant ci = new clsImportant();
        string superEmail = System.Configuration.ConfigurationManager.AppSettings["superadminEmail"].ToString();
        //
        // GET: /ApprovalRejection/

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult AdminApprovalRejection()
        //{
        //    SearchPending();
        //    GetRegionDetails();
        //    return View();
        //    //return RedirectToAction();
        //}
            
        [HttpGet]
        public JsonResult GetRegionDetails()
        {
            List<Sales> lstRegion = new List<Sales>();
            DataSet ds = Sd.GetRegionName();
            if (ds.Tables[0].Rows.Count > 0)
            {
                lstRegion = ds.Tables[0].AsEnumerable().Select(r => new Sales
                {
                   RegionId =  r.Field<int>("RegionId").ToString(),
                   RegionName = r.Field<string>("RegionName")
                }).ToList();
            }
            string usertype = Session["UserTypeApr"].ToString();
            string uesrname = Session["UserName"].ToString();
            return Json(new { lstRegion, usertype }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBranchdetails(int RegionId)
        {
            List<Sales> lstBranch = new List<Sales>();
            DataSet ds = Sd.GetBranchDetails(RegionId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lstBranch = ds.Tables[0].AsEnumerable().Select(r => new Sales
                {
                    BranchId = r.Field<int>("BranchId"),
                    BranchName = r.Field<string>("BranchName")
                }).ToList();
            }
            return Json(new { lstBranch }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetVechileList(int BranchId)
        {
            List<Sales> lstVechileMaster = new List<Sales>();
            DataSet ds = Sd.GetVehicleMaster(BranchId);
            if (ds.Tables[0].Rows.Count > 0)
            {
                lstVechileMaster = ds.Tables[0].AsEnumerable().Select(r => new Sales
                {
                    VehicleNumber = r.Field<string>("VehicleNo")
                }).ToList();
            }

            return Json(new { lstVechileMaster }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchVehicle(string FromDate, string ToDate, int RegionId, int BranchId, string VehicleNumber,string status)
        {
            string usertype = Session["UserTypeApr"].ToString();
            string uesrname = Session["UserName"].ToString();
            List<Sales> lstVehiclePending = new List<Sales>();
            List<Sales> lstVehicleRecommend = new List<Sales>();
            List<Sales> lstVehicleReject = new List<Sales>();
            List<Sales> lstVehicleApproved = new List<Sales>();
            List<Sales> lstVehicleRejectSA = new List<Sales>();
            List<Sales> lstVehicleRejectUser = new List<Sales>();
            if (usertype == "Admin")
            {
                lstVehiclePending = Sd.SearchVehicleDetailsNew(FromDate, ToDate, RegionId, BranchId, VehicleNumber, usertype, uesrname, "Pending");
                lstVehicleRecommend = Sd.SearchVehicleDetailsNew(FromDate, ToDate, RegionId, BranchId, VehicleNumber, usertype, uesrname, "Recommend");
                lstVehicleReject = Sd.SearchVehicleDetailsNew(FromDate, ToDate, RegionId, BranchId, VehicleNumber, usertype, uesrname, "Rejected");
                lstVehicleRejectSA = Sd.SearchVehicleDetailsNew(FromDate, ToDate, RegionId, BranchId, VehicleNumber, "SuperAdmin", uesrname, "RejectedSA");
                lstVehicleReject = lstVehicleReject.Concat(lstVehicleRejectSA).ToList();
                lstVehicleRejectSA = new List<Sales>();
            }
            else if(usertype=="SuperAdmin")
            {
                lstVehiclePending = Sd.SearchVehicleDetailsNew(FromDate, ToDate, RegionId, BranchId, VehicleNumber, usertype, uesrname, "Pending");
                lstVehicleRecommend = Sd.SearchVehicleDetailsNew(FromDate, ToDate, RegionId, BranchId, VehicleNumber, usertype, uesrname, "PendingSA");
                lstVehicleApproved = Sd.SearchVehicleDetailsNew(FromDate, ToDate, RegionId, BranchId, VehicleNumber, usertype, uesrname, "Approved");
                lstVehicleRejectSA = Sd.SearchVehicleDetailsNew(FromDate, ToDate, RegionId, BranchId, VehicleNumber, usertype, uesrname, "RejectedSA");
            }
            else
            {
                lstVehiclePending = Sd.SearchVehicleDetailsNew(FromDate, ToDate, RegionId, BranchId, VehicleNumber, usertype, uesrname, "Pending");
                lstVehicleApproved = Sd.SearchVehicleDetailsNew(FromDate, ToDate, RegionId, BranchId, VehicleNumber, usertype, uesrname, "Approved");
                lstVehicleReject = Sd.SearchVehicleDetailsNew(FromDate, ToDate, RegionId, BranchId, VehicleNumber, usertype, uesrname, "Rejected");
                lstVehicleRejectSA = Sd.SearchVehicleDetailsNew(FromDate, ToDate, RegionId, BranchId, VehicleNumber, usertype, uesrname, "RejectedSA");
                lstVehicleRejectUser = lstVehicleRejectSA.Concat(lstVehicleReject).ToList();
                lstVehicleReject = new List<Sales>();
                lstVehicleRejectSA = new List<Sales>();
            }

            //return Json(new { lstVehicle }, JsonRequestBehavior.AllowGet);
            return new JsonResult()
            {
                Data = new { lstVehiclePending, lstVehicleRecommend, lstVehicleReject, lstVehicleApproved, lstVehicleRejectSA, lstVehicleRejectUser },
                MaxJsonLength = Int32.MaxValue,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }       
            
        [HttpPost]
        public JsonResult getVehicleDetailsByRecId(int recId)
        {
            string UserName = Session["UserName"].ToString();
            string userType = Session["UserTypeApr"].ToString();

            Sales salesdet = Sd.getVehicleDetailByRecId(recId);
            Sales apprdet = Sd.getVehicleApprovalByRecId(recId);
            return Json(new { salesdet, apprdet, userType }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateApproveRejectEntry(Sales sales)
        {
            ArrayList invoiceNo = new ArrayList();
            ArrayList VendorName = new ArrayList();
            string msg = string.Empty; ArrayList services = new ArrayList();
            ArrayList totalAmtArry = new ArrayList();
            string UserName = Session["UserName"].ToString();
            string userType = Session["UserTypeApr"].ToString();
            string extraSubject = string.Empty;
            if (userType == "SuperAdmin")
            {
                extraSubject = "(VP-Fleet)";
            }
            else
            {
                extraSubject = "";
            }
            sales.CreatedBy = UserName;
            if (string.IsNullOrEmpty(sales.optionalEmail))
            {
                sales.optionalEmail = superEmail;
            }
            else
            {
                sales.optionalEmail = superEmail + ";" + sales.optionalEmail;
            }
            Sales masterdet = Sd.getVehicleDetailByRecId(Convert.ToInt32(sales.RecId));
            if (sales != null)
            {
                if (sales.TyreStatus_RH == true || sales.BatteryStatus_RH==true || sales.RoutineStatus_RH==true)
                {
                    sales.RoutineStatus_Admin = (sales.RoutineStatus_RH == true && userType=="Admin") ? sales.Status : sales.RoutineStatus_Admin;
                    sales.RoutineStatus_SuperAdmin = (sales.RoutineStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.RoutineStatus_SuperAdmin;

                    sales.BatteryStatus_Admin = (sales.BatteryStatus_RH == true && userType == "Admin") ? sales.Status : sales.BatteryStatus_Admin;
                    sales.BatteryStatus_SuperAdmin = (sales.BatteryStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.BatteryStatus_SuperAdmin;

                    sales.TyreStatus_Admin = (sales.TyreStatus_RH == true && userType == "Admin") ? sales.Status : sales.TyreStatus_Admin;
                    sales.TyreStatus_SuperAdmin = (sales.TyreStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.TyreStatus_SuperAdmin;

                    msg = Sd.ApprovalRejectionDetailUpdate(sales, "MinorService", userType);
                    if (msg.Equals("s"))
                    {
                        
                        if (sales.TyreStatus_RH == true)
                        {
                            services.Add("Tyre Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.TyreTotalAmount));
                            invoiceNo.Add(masterdet.TyreInvoiceNo);
                            VendorName.Add(masterdet.TyreVendorName);
                            //SendMailByParams("Tyre Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.TyreTotalAmount, sales.optionalEmail);
                        }
                        if (sales.BatteryStatus_RH == true)
                        {
                            services.Add("Battery Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.BatteryTotalAmount));
                            invoiceNo.Add(masterdet.BatteryInvoiceNo);
                            VendorName.Add(masterdet.BatteryVendorName);
                            //SendMailByParams("Battery Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.BatteryTotalAmount, sales.optionalEmail);
                        }
                        if (sales.RoutineStatus_RH == true)
                        {
                            services.Add("Routine Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.RoutineTotalAmount));
                            invoiceNo.Add(masterdet.RoutineInvoiceNo);
                            VendorName.Add(masterdet.RoutineVendorName);
                            //SendMailByParams("Routine Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.RoutineTotalAmount, sales.optionalEmail);
                        }
                    }
                }

                if (sales.DentingStatus_RH == true  || sales.MinorStatus_RH == true || sales.SeatStatus_RH == true)
                {
                    sales.DentingStatus_Admin = (sales.DentingStatus_RH == true && userType == "Admin") ? sales.Status : sales.DentingStatus_Admin;
                    sales.DentingStatus_SuperAdmin = (sales.DentingStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.DentingStatus_SuperAdmin;

                    sales.MinorStatus_Admin = (sales.MinorStatus_RH == true && userType == "Admin") ? sales.Status : sales.MinorStatus_Admin;
                    sales.MinorStatus_SuperAdmin = (sales.MinorStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.MinorStatus_SuperAdmin;

                    sales.SeatStatus_Admin = (sales.SeatStatus_RH == true && userType == "Admin") ? sales.Status : sales.SeatStatus_Admin;
                    sales.SeatStatus_SuperAdmin = (sales.SeatStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.SeatStatus_SuperAdmin;

                    msg = Sd.ApprovalRejectionDetailUpdate(sales, "MinorRepairing", userType);
                    if (msg.Equals("s"))
                    {
                        if (sales.DentingStatus_RH == true)
                        {
                            services.Add("Denting Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.DentingTotalAmount));
                            invoiceNo.Add(masterdet.DentingInvoiceNo);
                            VendorName.Add(masterdet.DentingVendorName);
                            //SendMailByParams("Denting Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.DentingTotalAmount, sales.optionalEmail);
                        }
                        if (sales.MinorStatus_RH == true)
                        {
                            services.Add("Minor Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.MinorTotalAmount));
                            invoiceNo.Add(masterdet.MinorInvoiceNo);
                            VendorName.Add(masterdet.MinorVendorName);
                            //SendMailByParams("Minor Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.MinorTotalAmount, sales.optionalEmail);
                        }
                        if (sales.SeatStatus_RH == true)
                        {
                            services.Add("Seat Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.SeatTotalAmount));
                            invoiceNo.Add(masterdet.SeatInvoiceNo);
                            VendorName.Add(masterdet.SeatVendorname);
                            //SendMailByParams("Seat Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.SeatTotalAmount, sales.optionalEmail);
                        }
                    }
                }

                if (sales.SelfStatus_RH == true || sales.ElectricalStatus_RH == true || sales.ClutchStatus_RH == true)
                {
                    sales.SelfStatus_Admin = (sales.SelfStatus_RH == true && userType == "Admin") ? sales.Status : sales.SelfStatus_Admin;
                    sales.SelfStatus_SuperAdmin = (sales.SelfStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.SelfStatus_SuperAdmin;

                    sales.ElectricalStatus_Admin = (sales.ElectricalStatus_RH == true && userType == "Admin") ? sales.Status : sales.ElectricalStatus_Admin;
                    sales.ElectricalStatus_SuperAdmin = (sales.ElectricalStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.ElectricalStatus_SuperAdmin;

                    sales.ClutchStatus_Admin = (sales.ClutchStatus_RH == true && userType == "Admin") ? sales.Status : sales.ClutchStatus_Admin;
                    sales.ClutchStatus_SuperAdmin = (sales.ClutchStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.ClutchStatus_SuperAdmin;

                    msg = Sd.ApprovalRejectionDetailUpdate(sales, "MajorService", userType);
                    if (msg.Equals("s"))
                    {
                        if (sales.SelfStatus_RH == true)
                        {
                            services.Add("Self Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.SelfTotalAmount));
                            invoiceNo.Add(masterdet.SelfInvoiceNo);
                            VendorName.Add(masterdet.SelfVendorName);
                            //SendMailByParams("Self Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.SelfTotalAmount, sales.optionalEmail);
                        }
                        if (sales.ElectricalStatus_RH == true)
                        {
                            services.Add("Electrical Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.ElectricalTotalAmount));
                            invoiceNo.Add(masterdet.ElectricalInvoiceNo);
                            VendorName.Add(masterdet.ElectricalVendorName);
                            //SendMailByParams("Electrical Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.ElectricalTotalAmount, sales.optionalEmail);
                        }
                        if (sales.ClutchStatus_RH == true)
                        {
                            services.Add("Clutch Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.ClutchTotalAmount));
                            invoiceNo.Add(masterdet.ClutchInvoiceNo);
                            VendorName.Add(masterdet.ClutchVendorName);
                            //SendMailByParams("Clutch Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.ClutchTotalAmount, sales.optionalEmail);
                        }
                    }
                }

                if (sales.AlternatorStatus_RH == true  || sales.LeafStatus_RH == true || sales.SuspensionStatus_RH == true)
                {
                    sales.AlternatorStatus_Admin = (sales.AlternatorStatus_RH == true && userType == "Admin") ? sales.Status : sales.AlternatorStatus_Admin;
                    sales.AlternatorStatus_SuperAdmin = (sales.AlternatorStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.AlternatorStatus_SuperAdmin;

                    sales.LeafStatus_Admin = (sales.LeafStatus_RH == true && userType == "Admin") ? sales.Status : sales.LeafStatus_Admin;
                    sales.LeafStatus_SuperAdmin = (sales.LeafStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.LeafStatus_SuperAdmin;

                    sales.SuspensionStatus_Admin = (sales.SuspensionStatus_RH == true && userType == "Admin") ? sales.Status : sales.SuspensionStatus_Admin;
                    sales.SuspensionStatus_SuperAdmin = (sales.SuspensionStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.SuspensionStatus_SuperAdmin;

                    msg = Sd.ApprovalRejectionDetailUpdate(sales, "MajorRepairing", userType);
                    if (msg.Equals("s"))
                    {
                        if (sales.AlternatorStatus_RH == true)
                        {
                            services.Add("Alternator Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.AlternatorTotalAmount));
                            invoiceNo.Add(masterdet.AlternatorInvoiceNo);
                            VendorName.Add(masterdet.AlternatorVendorName);
                            //SendMailByParams("Alternator Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.AlternatorTotalAmount, sales.optionalEmail);
                        }
                        if (sales.LeafStatus_RH == true)
                        {
                            services.Add("Leaf Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.LeafTotalAmount));
                            invoiceNo.Add(masterdet.LeafInvoiceNo);
                            VendorName.Add(masterdet.LeafVendorName);
                            //SendMailByParams("Leaf Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.LeafTotalAmount, sales.optionalEmail);
                        }
                        if (sales.SuspensionStatus_RH == true)
                        {
                            services.Add("Suspesion Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.SuspensionTotalAmount));
                            invoiceNo.Add(masterdet.SuspensionInvoiceNo);
                            VendorName.Add(masterdet.SuspensionVendorName);
                            //SendMailByParams("Suspesion Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.SuspensionTotalAmount, sales.optionalEmail);
                        }
                    }
                }

                if (sales.GearBoxStatus_RH == true || sales.BreakWorkStatus_RH == true || sales.EngineWorkStatus_RH == true)
                {
                    sales.GearBoxStatus_Admin = (sales.GearBoxStatus_RH == true && userType == "Admin") ? sales.Status : sales.GearBoxStatus_Admin;
                    sales.GearBoxStatus_SuperAdmin = (sales.GearBoxStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.GearBoxStatus_SuperAdmin;

                    sales.BreakWorkStatus_Admin = (sales.BreakWorkStatus_RH == true && userType == "Admin") ? sales.Status : sales.BreakWorkStatus_Admin;
                    sales.BreakWorkStatus_SuperAdmin = (sales.BreakWorkStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.BreakWorkStatus_SuperAdmin;

                    sales.EngineWorkStatus_Admin = (sales.EngineWorkStatus_RH == true && userType == "Admin") ? sales.Status : sales.EngineWorkStatus_Admin;
                    sales.EngineWorkStatus_SuperAdmin = (sales.EngineWorkStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.EngineWorkStatus_SuperAdmin;

                    msg = Sd.ApprovalRejectionDetailUpdate(sales, "WorkBookService", userType);
                    if (msg.Equals("s"))
                    {
                        if (sales.GearBoxStatus_RH == true)
                        {
                            services.Add("Gear Box Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.GearBoxTotalAmount));
                            invoiceNo.Add(masterdet.GearBoxInvoiceNo);
                            VendorName.Add(masterdet.GearBoxVendorName);
                            //SendMailByParams("Gear Box Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.GearBoxTotalAmount, sales.optionalEmail);
                        }
                        if (sales.BreakWorkStatus_RH == true)
                        {
                            services.Add("Break Work Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.BreakTotalAmount));
                            invoiceNo.Add(masterdet.BreakWorkInvoiceNo);
                            VendorName.Add(masterdet.BreakWorkVendorName);
                            //SendMailByParams("Break Work Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.BreakWorkAmount, sales.optionalEmail);
                        }
                        if (sales.EngineWorkStatus_RH == true)
                        {
                            services.Add("Engine Work Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.EngineTotalAmount));
                            invoiceNo.Add(masterdet.EngineWorkInvoiceNo);
                            VendorName.Add(masterdet.EngineWorkVendorName);
                            //SendMailByParams("Engine Work Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.EngineWorkAmount, sales.optionalEmail);
                        }
                    }
                }

                if (sales.FuelStatus_RH == true  || sales.PuncherStatus_RH == true || sales.OilStatus_RH == true)
                {
                    sales.FuelStatus_Admin = (sales.FuelStatus_RH == true && userType == "Admin") ? sales.Status : sales.FuelStatus_Admin;
                    sales.FuelStatus_SuperAdmin = (sales.FuelStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.FuelStatus_SuperAdmin;

                    sales.PuncherStatus_Admin = (sales.PuncherStatus_RH == true && userType == "Admin") ? sales.Status : sales.PuncherStatus_Admin;
                    sales.PuncherStatus_SuperAdmin = (sales.PuncherStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.PuncherStatus_SuperAdmin;

                    sales.OilStatus_Admin = (sales.OilStatus_RH == true && userType == "Admin") ? sales.Status : sales.OilStatus_Admin;
                    sales.OilStatus_SuperAdmin = (sales.OilStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.OilStatus_SuperAdmin;

                    msg = Sd.ApprovalRejectionDetailUpdate(sales, "TopupService", userType);
                    if (msg.Equals("s"))
                    {
                        if (sales.FuelStatus_RH == true)
                        {
                            services.Add("Fuel Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.FuelTotalAmount));
                            invoiceNo.Add(masterdet.FuelInvoiceNo);
                            VendorName.Add(masterdet.FuelVendorName);
                            //SendMailByParams("Fuel Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.FuelTotalAmount, sales.optionalEmail);
                        }
                        if (sales.PuncherStatus_RH == true)
                        {
                            services.Add("Puncher Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.PuncherTotalAmount));
                            invoiceNo.Add(masterdet.PuncherInvoiceNo);
                            VendorName.Add(masterdet.PuncherVendorName);
                            //SendMailByParams("Puncher Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.PuncherTotalAmount, sales.optionalEmail);
                        }
                        if (sales.OilStatus_RH == true)
                        {
                            services.Add("Oil Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.OilTotalAmount));
                            invoiceNo.Add(masterdet.OilInvoiceNo);
                            VendorName.Add(masterdet.OilVendorName);
                            //SendMailByParams("Oil Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.OilTotalAmount, sales.optionalEmail);
                        }
                    }
                }

                if (sales.RadiatorStatus_RH == true || sales.AxleStatus_RH == true || sales.DifferentialStatus_RH == true)
                {
                    sales.RadiatorStatus_Admin = (sales.RadiatorStatus_RH == true && userType == "Admin") ? sales.Status : sales.RadiatorStatus_Admin;
                    sales.RadiatorStatus_SuperAdmin = (sales.RadiatorStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.RadiatorStatus_SuperAdmin;

                    sales.AxleStatus_Admin = (sales.AxleStatus_RH == true && userType == "Admin") ? sales.Status : sales.AxleStatus_Admin;
                    sales.AxleStatus_SuperAdmin = (sales.AxleStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.AxleStatus_SuperAdmin;

                    sales.DifferentialStatus_Admin = (sales.DifferentialStatus_RH == true && userType == "Admin") ? sales.Status : sales.DifferentialStatus_Admin;
                    sales.DifferentialStatus_SuperAdmin = (sales.DifferentialStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.DifferentialStatus_SuperAdmin;

                    msg = Sd.ApprovalRejectionDetailUpdate(sales, "OtherService", userType);
                    if (msg.Equals("s"))
                    {
                        if (sales.RadiatorStatus_RH == true)
                        {
                            services.Add("Radiator Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.RadiatorTotalAmount));
                            invoiceNo.Add(masterdet.RadiatorInvoiceNo);
                            VendorName.Add(masterdet.RadiatorVendorName);
                            //SendMailByParams("Radiator Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.RadiatorTotalAmount, sales.optionalEmail);
                        }
                        if (sales.AxleStatus_RH == true)
                        {
                            services.Add("Axle Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.AxleTotalAmount));
                            invoiceNo.Add(masterdet.AxleInvoiceNo);
                            VendorName.Add(masterdet.AxleVendorName);
                            //SendMailByParams("Axle Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.AxleTotalAmount, sales.optionalEmail);
                        }
                        if (sales.DifferentialStatus_RH == true)
                        {
                            services.Add("Differential Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.DifferentialTotalAmount));
                            invoiceNo.Add(masterdet.DifferentialInvoiceNo);
                            VendorName.Add(masterdet.DifferentialVendorName);
                            //SendMailByParams("Differential Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.DifferentialTotalAmount, sales.optionalEmail);
                        }
                    }
                }

                if (sales.TurboStatus_RH == true || sales.EcmStatus_RH == true || sales.AccidentalStatus_RH == true)
                {
                    sales.TurboStatus_Admin = (sales.TurboStatus_RH == true && userType == "Admin") ? sales.Status : sales.TurboStatus_Admin;
                    sales.TurboStatus_SuperAdmin = (sales.TurboStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.TurboStatus_SuperAdmin;

                    sales.EcmStatus_Admin = (sales.EcmStatus_RH == true && userType == "Admin") ? sales.Status : sales.EcmStatus_Admin;
                    sales.EcmStatus_SuperAdmin = (sales.EcmStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.EcmStatus_SuperAdmin;

                    sales.AccidentalStatus_Admin = (sales.AccidentalStatus_RH == true && userType == "Admin") ? sales.Status : sales.AccidentalStatus_Admin;
                    sales.AccidentalStatus_SuperAdmin = (sales.AccidentalStatus_RH == true && userType == "SuperAdmin") ? sales.Status : sales.AccidentalStatus_SuperAdmin;

                    msg = Sd.ApprovalRejectionDetailUpdate(sales, "TurboService", userType);
                    if (msg.Equals("s"))
                    {
                        if (sales.TurboStatus_RH == true)
                        {
                            services.Add("Turbo Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.TurboTotalAmount));
                            invoiceNo.Add(masterdet.TurboInvoiceDate);
                            VendorName.Add(masterdet.TurboVendorName);
                            //SendMailByParams("Turbo Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.TurboTotalAmount, sales.optionalEmail);
                        }
                        if (sales.EcmStatus_RH == true)
                        {
                            services.Add("Ecm Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.EcmTotalAmount));
                            invoiceNo.Add(masterdet.EcmInvoiceNo);
                            VendorName.Add(masterdet.EcmVendorName);
                            //SendMailByParams("Ecm Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.EcmTotalAmount, sales.optionalEmail);
                        }
                        if (sales.AccidentalStatus_RH == true)
                        {
                            services.Add("Accidental Service");
                            totalAmtArry.Add(Convert.ToInt32(masterdet.AccidentalTotalAmount));
                            invoiceNo.Add(masterdet.AccidentalInvoiceNo);
                            VendorName.Add(masterdet.AccidentalVendorName);
                            //SendMailByParams("Accidental Service", sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                            //    masterdet.BranchName,
                            //    (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                            //    masterdet.AccidentalTotalAmount, sales.optionalEmail);
                        }
                    }
                    
                }
                //send mail here 
                if (services.Count > 0 && totalAmtArry.Count > 0)
                {
                    string servicename = string.Join(", ", services.Cast<string>().ToArray());
                    string invoicenumber = string.Join(", ", invoiceNo.Cast<string>().ToArray());
                    string vendername = string.Join(", ", VendorName.Cast<string>().ToArray());
                    int totalamount = totalAmtArry.Cast<int>().ToArray().Sum();
                    SendMailByParams(servicename, sales.Status, sales.VehicleNumber, UserName, masterdet.CreatedBy, sales.CreatedBy_Email, masterdet.RegionName,
                        masterdet.BranchName,
                        (userType == "SuperAdmin" ? sales.reason_sadmin : sales.reason_admin),
                        totalamount.ToString(), sales.optionalEmail,invoicenumber,vendername);

                    //insert into Fleet_Sales_ApprovalRejection_Email
                    sales.optionalEmail = sales.optionalEmail.Replace("sanjay.pandey@sisprosegur.com;", "");
                    msg = Sd.FleetSalesApprovalRejectionEmail_Insert(Convert.ToInt32(sales.RecId), sales.optionalEmail);
                }
            }
            return Json(new { msg }, JsonRequestBehavior.AllowGet);
        }
        public void SendMailByParams(string type,string status,string vehicleno,string username,string createdby,
            string createbyEmail,string reagion,string branch,string remarks,string amount,string ExtraEmail,string invoicenumber,string vendername)
        {
            string userType = Session["UserTypeApr"].ToString();
            string extraSubject = string.Empty;
            if (userType == "SuperAdmin")
            {
                extraSubject = "(VP-Fleet)";
            }
            else
            {
                extraSubject = "";
            }
            string subject = "R & M Bill Approved/ Rejected,"  +extraSubject+ "Vehicle Number: " + vehicleno;
            string template = "<h3><p>Dear " + createdby + "  </p></h3>" +
           "<p>" + type + " bill against " + vehicleno + " is checked & <b> " + status + " by " + username + "</b></p>" +
           "<p>Details are stated below: </p>" +
           "<p>Vehicle Number: " + vehicleno + "</p>" +
           "<p>Region: " + reagion + "</p>" +
           "<p>Branch: " + branch + "</p>" +
           "<p>Type: " + type + "</p>" +
           "<p>Amount: " + amount + "</p>" +
           "<p>Invoice No: " + invoicenumber + "</p>" +
           "<p>Vendor Name: " + vendername + "</p>" +
           "<p>status: <b>" + status + "</b></p>" +
           "<p>Remarks: <b>" + remarks + "</b></p>" +
           "<p> </p>";

            ci.SendMailMessage("fleetbills@sisprosegur.com", createbyEmail, "", ExtraEmail,subject , template.ToString());
        }
        
        [HttpPost]
        public JsonResult UpdateApproveRejectEntryInBulk(List<Sales> sales)
        {
            string strRecs = string.Empty;string status = "Approved";string aprRemarks = "Approved";
            ArrayList invoiceNo = new ArrayList();
            ArrayList VendorName = new ArrayList();
            ArrayList services = new ArrayList();
            ArrayList totalAmtArry = new ArrayList();
            string msg = string.Empty;int count = 0;
            string UserName = Session["UserName"].ToString();
            string userType = Session["UserTypeApr"].ToString();
            if (sales != null && sales.Count>0)
            {
                string[] recarray = sales.Select(x => x.RecId).ToList().ToArray();
                strRecs = string.Join(",", recarray);
                List<Sales> apprdetlist = Sd.getVehicleApprovalListByRecIds(strRecs);
                if (apprdetlist.Count > 0)
                {
                    foreach(var apr in apprdetlist)
                    {
                        if (string.IsNullOrEmpty(apr.optionalEmail))
                        {
                            apr.optionalEmail = superEmail;
                        }
                        else
                        {
                            apr.optionalEmail = superEmail + ";" + apr.optionalEmail;
                        }
                        Sales masterdet = Sd.getVehicleDetailByRecId(Convert.ToInt32(apr.RecId));
                        if ((apr.TyreStatus_Admin == "Recommend" && apr.TyreStatus_SuperAdmin =="") || 
                            (apr.BatteryStatus_Admin == "Recommend" && apr.BatteryStatus_SuperAdmin =="") || 
                            (apr.RoutineStatus_Admin == "Recommend" && apr.RoutineStatus_SuperAdmin ==""))
                        {
                            apr.RoutineRemarks_SuperAdmin = (apr.RoutineStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.RoutineRemarks_SuperAdmin;
                            apr.RoutineStatus_SuperAdmin = (apr.RoutineStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.RoutineStatus_SuperAdmin;

                            apr.BatteryRemarks_SuperAdmin = (apr.BatteryStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.BatteryRemarks_SuperAdmin;
                            apr.BatteryStatus_SuperAdmin = (apr.BatteryStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.BatteryStatus_SuperAdmin;

                            apr.TyreRemarks_SuperAdmin = (apr.TyreStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.TyreRemarks_SuperAdmin;
                            apr.TyreStatus_SuperAdmin = (apr.TyreStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.TyreStatus_SuperAdmin;

                            msg = Sd.ApprovalRejectionDetailUpdate(apr, "MinorService", userType);
                            if (msg.Equals("s"))
                            {
                                if (apr.TyreStatus_Admin == "Recommend")
                                {
                                    services.Add("Tyre Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.TyreTotalAmount));
                                    invoiceNo.Add(masterdet.TyreInvoiceNo);
                                    VendorName.Add(masterdet.TyreVendorName);
                                    //SendMailByParams("Tyre Service", status, apr.VehicleNumber, UserName, apr.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.TyreRemarks_SuperAdmin : apr.TyreRemarks_Admin),
                                    //    masterdet.TyreTotalAmount, apr.optionalEmail);
                                }
                                if (apr.BatteryStatus_Admin == "Recommend")
                                {
                                    services.Add("Battery Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.BatteryTotalAmount));
                                    invoiceNo.Add(masterdet.BatteryInvoiceNo);
                                    VendorName.Add(masterdet.BatteryVendorName);
                                    //SendMailByParams("Battery Service", status, apr.VehicleNumber, UserName, apr.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.BatteryRemarks_SuperAdmin : apr.BatteryRemarks_Admin),
                                    //    masterdet.BatteryTotalAmount, apr.optionalEmail);
                                }
                                if (apr.RoutineStatus_Admin == "Recommend")
                                {
                                    services.Add("Routine Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.RoutineTotalAmount));
                                    invoiceNo.Add(masterdet.RoutineInvoiceNo);
                                    VendorName.Add(masterdet.RoutineVendorName);
                                    //SendMailByParams("Routine Service", status, apr.VehicleNumber, UserName, apr.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.RoutineRemarks_SuperAdmin : apr.RoutineRemarks_Admin),
                                    //    masterdet.RoutineTotalAmount, apr.optionalEmail);
                                }
                            }
                        }

                        if ((apr.DentingStatus_Admin == "Recommend" && apr.DentingStatus_SuperAdmin =="") || 
                            (apr.MinorStatus_Admin == "Recommend" && apr.MinorStatus_SuperAdmin =="") || 
                            apr.SeatStatus_Admin == "Recommend" && apr.SeatStatus_SuperAdmin =="")
                        {
                            apr.DentingRemarks_SuperAdmin = (apr.DentingStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.DentingRemarks_SuperAdmin;
                            apr.DentingStatus_SuperAdmin = (apr.DentingStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.DentingStatus_SuperAdmin;

                            apr.MinorRemarks_SuperAdmin = (apr.MinorStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.MinorRemarks_SuperAdmin;
                            apr.MinorStatus_SuperAdmin = (apr.MinorStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.MinorStatus_SuperAdmin;

                            apr.SeatRemarks_SuperAdmin = (apr.SeatStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.SeatRemarks_SuperAdmin;
                            apr.SeatStatus_SuperAdmin = (apr.SeatStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.SeatStatus_SuperAdmin;

                            msg = Sd.ApprovalRejectionDetailUpdate(apr, "MinorRepairing", userType);
                            if (msg.Equals("s"))
                            {
                                if (apr.DentingStatus_Admin == "Recommend")
                                {
                                    services.Add("Denting Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.DentingTotalAmount));
                                    invoiceNo.Add(masterdet.DentingInvoiceNo);
                                    VendorName.Add(masterdet.DentingVendorName);
                                    //SendMailByParams("Denting Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.DentingRemarks_SuperAdmin : apr.DentingRemarks_Admin),
                                    //    masterdet.DentingTotalAmount, apr.optionalEmail);
                                }
                                if (apr.MinorStatus_Admin == "Recommend")
                                {
                                    services.Add("Minor Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.MinorTotalAmount));
                                    invoiceNo.Add(masterdet.MinorInvoiceNo);
                                    VendorName.Add(masterdet.MinorVendorName);
                                    //SendMailByParams("Minor Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.MinorRemarks_SuperAdmin : apr.MinorRemarks_Admin),
                                    //    masterdet.MinorTotalAmount, apr.optionalEmail);
                                }
                                if (apr.SeatStatus_Admin == "Recommend")
                                {
                                    services.Add("Seat Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.SeatTotalAmount));
                                    invoiceNo.Add(masterdet.SeatInvoiceNo);
                                    VendorName.Add(masterdet.SeatVendorname);
                                    //SendMailByParams("Seat Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.SeatRemarks_SuperAdmin : apr.SeatRemarks_Admin),
                                    //    masterdet.SeatTotalAmount, apr.optionalEmail);
                                }
                            }
                        }

                        if ((apr.SelfStatus_Admin == "Recommend" && apr.SelfStatus_SuperAdmin =="") || 
                            (apr.ElectricalStatus_Admin == "Recommend" && apr.ElectricalStatus_SuperAdmin == "Approved") || 
                            (apr.ClutchStatus_Admin == "Recommend" && apr.ClutchStatus_SuperAdmin == "Approved"))
                        {
                            apr.SelfRemarks_SuperAdmin = (apr.SelfStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.SelfRemarks_SuperAdmin;
                            apr.SelfStatus_SuperAdmin = (apr.SelfStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.SelfStatus_SuperAdmin;

                            apr.ElectricalRemarks_SuperAdmin = (apr.ElectricalStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.ElectricalRemarks_SuperAdmin;
                            apr.ElectricalStatus_SuperAdmin = (apr.ElectricalStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.ElectricalStatus_SuperAdmin;

                            apr.ClutchRemarks_SuperAdmin = (apr.ClutchStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.ClutchRemarks_SuperAdmin;
                            apr.ClutchStatus_SuperAdmin = (apr.ClutchStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.ClutchStatus_SuperAdmin;

                            msg = Sd.ApprovalRejectionDetailUpdate(apr, "MajorService", userType);
                            if (msg.Equals("s"))
                            {
                                if (apr.SelfStatus_Admin == "Recommend")
                                {
                                    services.Add("Self Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.SelfTotalAmount));
                                    invoiceNo.Add(masterdet.SelfInvoiceNo);
                                    VendorName.Add(masterdet.SelfVendorName);
                                    //SendMailByParams("Self Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.SelfRemarks_SuperAdmin : apr.SelfRemarks_Admin),
                                    //    masterdet.SelfTotalAmount, apr.optionalEmail);
                                }
                                if (apr.ElectricalStatus_Admin == "Recommend")
                                {
                                    services.Add("Electrical Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.ElectricalTotalAmount));
                                    invoiceNo.Add(masterdet.ElectricalInvoiceNo);
                                    VendorName.Add(masterdet.ElectricalVendorName);
                                    //SendMailByParams("Electrical Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.ElectricalRemarks_SuperAdmin : apr.ElectricalRemarks_Admin),
                                    //    masterdet.ElectricalTotalAmount, apr.optionalEmail);
                                }
                                if (apr.ClutchStatus_Admin == "Recommend")
                                {
                                    services.Add("Clutch Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.ClutchTotalAmount));
                                    invoiceNo.Add(masterdet.ClutchInvoiceNo);
                                    VendorName.Add(masterdet.ClutchVendorName);
                                    //SendMailByParams("Clutch Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.ClutchRemarks_SuperAdmin : apr.ClutchRemarks_Admin),
                                    //    masterdet.ClutchTotalAmount, apr.optionalEmail);
                                }
                            }
                        }

                        if ((apr.AlternatorStatus_Admin == "Recommend" && apr.AlternatorStatus_SuperAdmin =="") || 
                            (apr.LeafStatus_Admin == "Recommend" && apr.LeafStatus_SuperAdmin =="") || 
                            (apr.SuspensionStatus_Admin == "Recommend" && apr.SuspensionStatus_SuperAdmin ==""))
                        {
                            apr.AlternatorRemarks_SuperAdmin = (apr.AlternatorStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.AlternatorRemarks_SuperAdmin;
                            apr.AlternatorStatus_SuperAdmin = (apr.AlternatorStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.AlternatorStatus_SuperAdmin;

                            apr.LeafRemarks_SuperAdmin = (apr.LeafStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.LeafRemarks_SuperAdmin;
                            apr.LeafStatus_SuperAdmin = (apr.LeafStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.LeafStatus_SuperAdmin;

                            apr.SuspensionRemarks_SuperAdmin = (apr.SuspensionStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.SuspensionRemarks_SuperAdmin;
                            apr.SuspensionStatus_SuperAdmin = (apr.SuspensionStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.SuspensionStatus_SuperAdmin;

                            msg = Sd.ApprovalRejectionDetailUpdate(apr, "MajorRepairing", userType);
                            if (msg.Equals("s"))
                            {
                                if (apr.AlternatorStatus_Admin == "Recommend")
                                {
                                    services.Add("Alternator Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.AlternatorTotalAmount));
                                    invoiceNo.Add(masterdet.AlternatorInvoiceNo);
                                    VendorName.Add(masterdet.AlternatorVendorName);
                                    //SendMailByParams("Alternator Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.AlternatorRemarks_SuperAdmin : apr.AlternatorRemarks_Admin),
                                    //    masterdet.AlternatorTotalAmount, apr.optionalEmail);
                                }
                                if (apr.LeafStatus_Admin == "Recommend")
                                {
                                    services.Add("Leaf Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.LeafTotalAmount));
                                    invoiceNo.Add(masterdet.LeafInvoiceNo);
                                    VendorName.Add(masterdet.LeafVendorName);
                                    //SendMailByParams("Leaf Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.LeafRemarks_SuperAdmin : apr.LeafRemarks_Admin),
                                    //    masterdet.LeafTotalAmount, apr.optionalEmail);
                                }
                                if (apr.SuspensionStatus_Admin == "Recommend")
                                {
                                    services.Add("Suspesion Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.SuspensionTotalAmount));
                                    invoiceNo.Add(masterdet.SuspensionInvoiceNo);
                                    VendorName.Add(masterdet.SuspensionVendorName);
                                    //SendMailByParams("Suspesion Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.SuspensionRemarks_SuperAdmin : apr.SuspensionRemarks_Admin),
                                    //    masterdet.SuspensionTotalAmount, apr.optionalEmail);
                                }
                            }
                        }

                        if ((apr.GearBoxStatus_Admin == "Recommend" && apr.GearBoxStatus_SuperAdmin =="") || 
                            (apr.BreakWorkStatus_Admin == "Recommend" && apr.BreakWorkStatus_SuperAdmin =="") || 
                            (apr.EngineWorkStatus_Admin == "Recommend" && apr.EngineWorkStatus_SuperAdmin ==""))
                        {
                            apr.GearBoxRemarks_SuperAdmin = (apr.GearBoxStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.GearBoxRemarks_SuperAdmin;
                            apr.GearBoxStatus_SuperAdmin = (apr.GearBoxStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.GearBoxStatus_SuperAdmin;

                            apr.BreakWorkRemarks_SuperAdmin = (apr.BreakWorkStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.BreakWorkRemarks_SuperAdmin;
                            apr.BreakWorkStatus_SuperAdmin = (apr.BreakWorkStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.BreakWorkStatus_SuperAdmin;

                            apr.EngineWorkRemarks_SuperAdmin = (apr.EngineWorkStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.EngineWorkRemarks_SuperAdmin;
                            apr.EngineWorkStatus_SuperAdmin = (apr.EngineWorkStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.EngineWorkStatus_SuperAdmin;

                            msg = Sd.ApprovalRejectionDetailUpdate(apr, "WorkBookService", userType);
                            if (msg.Equals("s"))
                            {
                                if (apr.GearBoxStatus_Admin == "Recommend")
                                {
                                    services.Add("Gear Box Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.GearBoxTotalAmount));
                                    invoiceNo.Add(masterdet.GearBoxInvoiceNo);
                                    VendorName.Add(masterdet.GearBoxVendorName);
                                    //SendMailByParams("Gear Box Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.GearBoxRemarks_SuperAdmin : apr.GearBoxRemarks_Admin),
                                    //    masterdet.GearBoxTotalAmount, apr.optionalEmail);
                                }
                                if (apr.BreakWorkStatus_Admin == "Recommend")
                                {
                                    services.Add("Break Work Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.BreakTotalAmount));
                                    invoiceNo.Add(masterdet.BreakWorkInvoiceNo);
                                    VendorName.Add(masterdet.BreakWorkVendorName);
                                    //SendMailByParams("Break Work Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.BreakWorkRemarks_SuperAdmin : apr.BreakWorkRemarks_Admin),
                                    //    masterdet.BreakWorkAmount, apr.optionalEmail);
                                }
                                if (apr.EngineWorkStatus_Admin == "Recommend")
                                {
                                    services.Add("Engine Work Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.EngineWorkAmount));
                                    invoiceNo.Add(masterdet.EngineWorkInvoiceNo);
                                    VendorName.Add(masterdet.EngineWorkVendorName);
                                    //SendMailByParams("Engine Work Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.EngineWorkRemarks_SuperAdmin : apr.EngineWorkRemarks_Admin),
                                    //    masterdet.EngineWorkAmount, apr.optionalEmail);
                                }
                            }
                        }

                        if ((apr.FuelStatus_Admin == "Recommend" && apr.FuelStatus_SuperAdmin =="") || 
                            (apr.PuncherStatus_Admin == "Recommend" && apr.PuncherStatus_SuperAdmin =="") || 
                            (apr.OilStatus_Admin == "Recommend" && apr.OilStatus_SuperAdmin ==""))
                        {
                            apr.FuelRemarks_SuperAdmin = (apr.FuelStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.FuelRemarks_SuperAdmin;
                            apr.FuelStatus_SuperAdmin = (apr.FuelStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.FuelStatus_SuperAdmin;

                            apr.PuncherRemarks_SuperAdmin = (apr.PuncherStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.PuncherRemarks_SuperAdmin;
                            apr.PuncherStatus_SuperAdmin = (apr.PuncherStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.PuncherStatus_SuperAdmin;

                            apr.OilRemarks_SuperAdmin = (apr.OilStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.OilRemarks_SuperAdmin;
                            apr.OilStatus_SuperAdmin = (apr.OilStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.OilStatus_SuperAdmin;

                            msg = Sd.ApprovalRejectionDetailUpdate(apr, "TopupService", userType);
                            if (msg.Equals("s"))
                            {
                                if (apr.FuelStatus_Admin == "Recommend")
                                {
                                    services.Add("Fuel Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.FuelTotalAmount));
                                    invoiceNo.Add(masterdet.FuelInvoiceNo);
                                    VendorName.Add(masterdet.FuelVendorName);
                                    //SendMailByParams("Fuel Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.FuelRemarks_SuperAdmin : apr.FuelRemarks_Admin),
                                    //    masterdet.FuelTotalAmount, apr.optionalEmail);
                                }
                                if (apr.PuncherStatus_Admin == "Recommend")
                                {
                                    services.Add("Puncher Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.PuncherTotalAmount));
                                    invoiceNo.Add(masterdet.PuncherInvoiceNo);
                                    VendorName.Add(masterdet.PuncherVendorName);
                                    //SendMailByParams("Puncher Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.PuncherRemarks_SuperAdmin : apr.PuncherRemarks_Admin),
                                    //    masterdet.PuncherTotalAmount, apr.optionalEmail);
                                }
                                if (apr.OilStatus_Admin == "Recommend")
                                {
                                    services.Add("Oil Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.OilTotalAmount));
                                    invoiceNo.Add(masterdet.OilInvoiceNo);
                                    VendorName.Add(masterdet.OilVendorName);
                                    //SendMailByParams("Oil Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.OilRemarks_SuperAdmin : apr.OilRemarks_Admin),
                                    //    masterdet.OilTotalAmount, apr.optionalEmail);
                                }
                            }
                        }

                        if ((apr.RadiatorStatus_Admin == "Recommend" && apr.RadiatorStatus_SuperAdmin =="") || 
                            (apr.AxleStatus_Admin == "Recommend" && apr.AxleStatus_SuperAdmin =="") || 
                            (apr.DifferentialStatus_Admin == "Recommend" && apr.DifferentialStatus_SuperAdmin ==""))
                        {
                            apr.RadiatorRemarks_SuperAdmin = (apr.RadiatorStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.RadiatorRemarks_SuperAdmin;
                            apr.RadiatorStatus_SuperAdmin = (apr.RadiatorStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.RadiatorStatus_SuperAdmin;

                            apr.AxleRemarks_SuperAdmin = (apr.AxleStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.AxleStatus_SuperAdmin;
                            apr.AxleStatus_SuperAdmin = (apr.AxleStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.AxleStatus_SuperAdmin;

                            apr.DifferentialRemarks_SuperAdmin = (apr.DifferentialStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.DifferentialStatus_SuperAdmin;
                            apr.DifferentialStatus_SuperAdmin = (apr.DifferentialStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.DifferentialStatus_SuperAdmin;

                            msg = Sd.ApprovalRejectionDetailUpdate(apr, "OtherService", userType);
                            if (msg.Equals("s"))
                            {
                                if (apr.RadiatorStatus_Admin == "Recommend")
                                {
                                    services.Add("Radiator Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.RadiatorTotalAmount));
                                    invoiceNo.Add(masterdet.RadiatorInvoiceNo);
                                    VendorName.Add(masterdet.RadiatorVendorName);
                                    //SendMailByParams("Radiator Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.RadiatorRemarks_SuperAdmin : apr.RadiatorRemarks_Admin),
                                    //    masterdet.RadiatorTotalAmount, apr.optionalEmail);
                                }
                                if (apr.AxleStatus_Admin == "Recommend")
                                {
                                    services.Add("Axle Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.AxleTotalAmount));
                                    invoiceNo.Add(masterdet.AxleInvoiceNo);
                                    VendorName.Add(masterdet.AxleVendorName);

                                    //SendMailByParams("Axle Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.AxleRemarks_SuperAdmin : apr.AxleRemarks_Admin),
                                    //    apr.AxleTotalAmount, apr.optionalEmail);
                                }
                                if (apr.DifferentialStatus_Admin == "Recommend")
                                {
                                    services.Add("Differential Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.DifferentialTotalAmount));
                                    invoiceNo.Add(masterdet.DifferentialInvoiceNo);
                                    VendorName.Add(masterdet.DifferentialVendorName);
                                    //SendMailByParams("Differential Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.DifferentialRemarks_SuperAdmin : apr.DifferentialRemarks_Admin),
                                    //    masterdet.DifferentialTotalAmount, apr.optionalEmail);
                                }
                            }
                        }

                        if ((apr.TurboStatus_Admin == "Recommend" && apr.TurboStatus_SuperAdmin =="") || 
                            (apr.EcmStatus_Admin == "Recommend" && apr.EcmStatus_SuperAdmin =="") || 
                            (apr.AccidentalStatus_Admin == "Recommend" && apr.AccidentalStatus_SuperAdmin ==""))
                        {
                            apr.TurboRemarks_SuperAdmin = (apr.TurboStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.TurboRemarks_SuperAdmin;
                            apr.TurboStatus_SuperAdmin = (apr.TurboStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.TurboStatus_SuperAdmin;

                            apr.EcmRemarks_SuperAdmin = (apr.EcmStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.EcmRemarks_SuperAdmin;
                            apr.EcmStatus_SuperAdmin = (apr.EcmStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.EcmStatus_SuperAdmin;

                            apr.AccidentalRemarks_SuperAdmin = (apr.AccidentalStatus_Admin == "Recommend" && userType == "SuperAdmin") ? aprRemarks : apr.AccidentalRemarks_SuperAdmin;
                            apr.AccidentalStatus_SuperAdmin = (apr.AccidentalStatus_Admin == "Recommend" && userType == "SuperAdmin") ? status : apr.AccidentalStatus_SuperAdmin;

                            msg = Sd.ApprovalRejectionDetailUpdate(apr, "TurboService", userType);
                            if (msg.Equals("s"))
                            {
                                if (apr.TurboStatus_Admin == "Recommend")
                                {
                                    services.Add("Turbo Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.TurboTotalAmount));
                                    invoiceNo.Add(masterdet.TurboInvoiceNo);
                                    VendorName.Add(masterdet.TurboVendorName);
                                    //SendMailByParams("Turbo Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.TurboRemarks_SuperAdmin : apr.TurboRemarks_Admin),
                                    //    masterdet.TurboTotalAmount, apr.optionalEmail);
                                }
                                if (apr.EcmStatus_Admin == "Recommend")
                                {
                                    services.Add("Ecm Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.EcmTotalAmount));
                                    invoiceNo.Add(masterdet.EcmInvoiceNo);
                                    VendorName.Add(masterdet.EcmVendorName);
                                    //SendMailByParams("Ecm Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.EcmRemarks_SuperAdmin : apr.EcmRemarks_Admin),
                                    //    masterdet.EcmTotalAmount, apr.optionalEmail);
                                }
                                if (apr.AccidentalStatus_Admin == "Recommend")
                                {
                                    services.Add("Accidental Service");
                                    totalAmtArry.Add(Convert.ToInt32(masterdet.AccidentalTotalAmount));
                                    invoiceNo.Add(masterdet.AccidentalInvoiceNo);
                                    VendorName.Add(masterdet.AccidentalVendorName);
                                    //SendMailByParams("Accidental Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                    //    masterdet.BranchName,
                                    //    (userType == "SuperAdmin" ? apr.AccidentalRemarks_SuperAdmin : apr.AccidentalRemarks_Admin),
                                    //    masterdet.AccidentalTotalAmount, apr.optionalEmail);
                                }
                            }

                        }
                        if (msg.Equals("s"))
                        {
                            count += 1;
                        }

                        //send mail here 
                        if (services.Count > 0 && totalAmtArry.Count > 0)
                        {
                            string servicename = string.Join(", ", services.Cast<string>().ToArray());
                            string invoicenumber = string.Join(", ", invoiceNo.Cast<string>().ToArray());

                            string vendername = string.Join(", ", VendorName.Cast<string>().ToArray());
                            int totalamount = totalAmtArry.Cast<int>().ToArray().Sum();
                            SendMailByParams(servicename, status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                                masterdet.BranchName,
                                (userType == "SuperAdmin" ? apr.TurboRemarks_SuperAdmin : apr.TurboRemarks_Admin),
                                totalamount.ToString(), apr.optionalEmail,invoicenumber, vendername);
                            totalAmtArry.Clear();
                            services.Clear();
                            invoiceNo.Clear();
                            VendorName.Clear();
                        }
                    }
                    if (count > 0)
                        msg = "s";
                    else
                        msg = "f";
                }
            }
           
            
            return Json(new { msg }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateRejectedToPending(Sales sales)
        {
            string msg = string.Empty;string status = "";int count = 0;
            string UserName = Session["UserName"].ToString();
            string userType = Session["UserTypeApr"].ToString();
            sales.CreatedBy = UserName;
            List<Sales> apprdetlist = Sd.getVehicleApprovalListByRecIds(sales.RecId.ToString());
            if (apprdetlist.Count > 0)
            {
                foreach (var apr in apprdetlist)
                {
                   // Sales masterdet = Sd.getVehicleDetailByRecId(Convert.ToInt32(apr.RecId));
                    if ((apr.TyreStatus_Admin == "Rejected" || apr.TyreStatus_SuperAdmin == "Rejected") ||
                        (apr.BatteryStatus_Admin == "Rejected" || apr.BatteryStatus_SuperAdmin == "Rejected") ||
                        (apr.RoutineStatus_Admin == "Rejected" || apr.RoutineStatus_SuperAdmin == "Rejected"))
                    {
                        apr.RoutineStatus_Admin = (apr.RoutineStatus_Admin == "Rejected" || apr.RoutineStatus_SuperAdmin == "Rejected") ? status : apr.RoutineStatus_Admin;
                        apr.RoutineStatus_SuperAdmin = (apr.RoutineStatus_Admin == "Rejected" || apr.RoutineStatus_SuperAdmin == "Rejected") ? status : apr.RoutineStatus_SuperAdmin;

                        apr.BatteryStatus_Admin = (apr.BatteryStatus_Admin == "Rejected" || apr.BatteryStatus_SuperAdmin == "Rejected") ? status : apr.BatteryStatus_Admin;
                        apr.BatteryStatus_SuperAdmin = (apr.BatteryStatus_Admin == "Rejected" || apr.BatteryStatus_SuperAdmin == "Rejected") ? status : apr.BatteryStatus_SuperAdmin;

                        apr.TyreStatus_Admin = (apr.TyreStatus_Admin == "Rejected" || apr.TyreStatus_SuperAdmin == "Rejected") ? status : apr.TyreStatus_Admin;
                        apr.TyreStatus_SuperAdmin = (apr.TyreStatus_Admin == "Rejected" || apr.TyreStatus_SuperAdmin == "Rejected") ? status : apr.TyreStatus_SuperAdmin;

                        msg = Sd.ApprovalRejectionDetailUpdate(apr, "MinorService", userType);
                        if (msg.Equals("s"))
                        {
                            //if (apr.TyreStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Tyre Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.TyreTotalAmount));
                            //    //SendMailByParams("Tyre Service", status, apr.VehicleNumber, UserName, apr.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.TyreRemarks_SuperAdmin : apr.TyreRemarks_Admin),
                            //    //    masterdet.TyreTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.BatteryStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Battery Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.BatteryTotalAmount));
                            //    //SendMailByParams("Battery Service", status, apr.VehicleNumber, UserName, apr.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.BatteryRemarks_SuperAdmin : apr.BatteryRemarks_Admin),
                            //    //    masterdet.BatteryTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.RoutineStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Routine Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.RoutineTotalAmount));
                            //    //SendMailByParams("Routine Service", status, apr.VehicleNumber, UserName, apr.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.RoutineRemarks_SuperAdmin : apr.RoutineRemarks_Admin),
                            //    //    masterdet.RoutineTotalAmount, apr.optionalEmail);
                            //}
                        }
                    }

                    if ((apr.DentingStatus_Admin == "Rejected" || apr.DentingStatus_SuperAdmin == "Rejected") ||
                        (apr.MinorStatus_Admin == "Rejected" || apr.MinorStatus_SuperAdmin == "Rejected") ||
                        apr.SeatStatus_Admin == "Rejected" || apr.SeatStatus_SuperAdmin == "Rejected")
                    {
                        apr.DentingStatus_Admin = (apr.DentingStatus_Admin == "Rejected" || apr.DentingStatus_SuperAdmin == "Rejected") ? status : apr.DentingStatus_Admin;
                        apr.DentingStatus_SuperAdmin = (apr.DentingStatus_Admin == "Rejected" || apr.DentingStatus_SuperAdmin == "Rejected") ? status : apr.DentingStatus_SuperAdmin;

                        apr.MinorStatus_Admin = (apr.MinorStatus_Admin == "Rejected" || apr.MinorStatus_SuperAdmin == "Rejected") ? status : apr.MinorStatus_Admin;
                        apr.MinorStatus_SuperAdmin = (apr.MinorStatus_Admin == "Rejected" || apr.MinorStatus_SuperAdmin == "Rejected") ? status : apr.MinorStatus_SuperAdmin;

                        apr.SeatStatus_Admin = (apr.SeatStatus_Admin == "Rejected" || apr.SeatStatus_SuperAdmin == "Rejected") ? status : apr.SeatStatus_Admin;
                        apr.SeatStatus_SuperAdmin = (apr.SeatStatus_Admin == "Rejected" || apr.SeatStatus_SuperAdmin == "Rejected") ? status : apr.SeatStatus_SuperAdmin;

                        msg = Sd.ApprovalRejectionDetailUpdate(apr, "MinorRepairing", userType);
                        if (msg.Equals("s"))
                        {
                            //if (apr.DentingStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Denting Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.DentingTotalAmount));
                            //    //SendMailByParams("Denting Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.DentingRemarks_SuperAdmin : apr.DentingRemarks_Admin),
                            //    //    masterdet.DentingTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.MinorStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Minor Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.MinorTotalAmount));
                            //    //SendMailByParams("Minor Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.MinorRemarks_SuperAdmin : apr.MinorRemarks_Admin),
                            //    //    masterdet.MinorTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.SeatStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Seat Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.SeatTotalAmount));
                            //    //SendMailByParams("Seat Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.SeatRemarks_SuperAdmin : apr.SeatRemarks_Admin),
                            //    //    masterdet.SeatTotalAmount, apr.optionalEmail);
                            //}
                        }
                    }

                    if ((apr.SelfStatus_Admin == "Rejected" || apr.SelfStatus_SuperAdmin == "Rejected") ||
                        (apr.ElectricalStatus_Admin == "Rejected" || apr.ElectricalStatus_SuperAdmin == "Rejected") ||
                        (apr.ClutchStatus_Admin == "Rejected" || apr.ClutchStatus_SuperAdmin == "Rejected"))
                    {
                        apr.SelfStatus_Admin = (apr.SelfStatus_Admin == "Rejected" || apr.SelfStatus_SuperAdmin == "Rejected") ? status : apr.SelfStatus_Admin;
                        apr.SelfStatus_SuperAdmin = (apr.SelfStatus_Admin == "Rejected" || apr.SelfStatus_SuperAdmin == "Rejected") ? status : apr.SelfStatus_SuperAdmin;

                        apr.ElectricalStatus_Admin = (apr.ElectricalStatus_Admin == "Rejected" || apr.ElectricalStatus_SuperAdmin == "Rejected") ? status : apr.ElectricalStatus_Admin;
                        apr.ElectricalStatus_SuperAdmin = (apr.ElectricalStatus_Admin == "Rejected" || apr.ElectricalStatus_SuperAdmin == "Rejected") ? status : apr.ElectricalStatus_SuperAdmin;

                        apr.ClutchStatus_Admin = (apr.ClutchStatus_Admin == "Rejected" || apr.ClutchStatus_SuperAdmin == "Rejected") ? status : apr.ClutchStatus_Admin;
                        apr.ClutchStatus_SuperAdmin = (apr.ClutchStatus_Admin == "Rejected" || apr.ClutchStatus_SuperAdmin == "Rejected") ? status : apr.ClutchStatus_SuperAdmin;

                        msg = Sd.ApprovalRejectionDetailUpdate(apr, "MajorService", userType);
                        if (msg.Equals("s"))
                        {
                            //if (apr.SelfStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Self Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.SelfTotalAmount));
                            //    //SendMailByParams("Self Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.SelfRemarks_SuperAdmin : apr.SelfRemarks_Admin),
                            //    //    masterdet.SelfTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.ElectricalStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Electrical Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.ElectricalTotalAmount));
                            //    //SendMailByParams("Electrical Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.ElectricalRemarks_SuperAdmin : apr.ElectricalRemarks_Admin),
                            //    //    masterdet.ElectricalTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.ClutchStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Clutch Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.ClutchTotalAmount));
                            //    //SendMailByParams("Clutch Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.ClutchRemarks_SuperAdmin : apr.ClutchRemarks_Admin),
                            //    //    masterdet.ClutchTotalAmount, apr.optionalEmail);
                            //}
                        }
                    }

                    if ((apr.AlternatorStatus_Admin == "Rejected" || apr.AlternatorStatus_SuperAdmin == "Rejected") ||
                        (apr.LeafStatus_Admin == "Rejected" || apr.LeafStatus_SuperAdmin == "Rejected") ||
                        (apr.SuspensionStatus_Admin == "Rejected" || apr.SuspensionStatus_SuperAdmin == "Rejected"))
                    {
                        apr.AlternatorStatus_Admin = (apr.AlternatorStatus_Admin == "Rejected" || apr.AlternatorStatus_SuperAdmin == "Rejected") ? status : apr.AlternatorStatus_Admin;
                        apr.AlternatorStatus_SuperAdmin = (apr.AlternatorStatus_Admin == "Rejected" || apr.AlternatorStatus_SuperAdmin == "Rejected") ? status : apr.AlternatorStatus_SuperAdmin;

                        apr.LeafStatus_Admin = (apr.LeafStatus_Admin == "Rejected" || apr.LeafStatus_SuperAdmin == "Rejected") ? status : apr.LeafStatus_Admin;
                        apr.LeafStatus_SuperAdmin = (apr.LeafStatus_Admin == "Rejected" || apr.LeafStatus_SuperAdmin == "Rejected") ? status : apr.LeafStatus_SuperAdmin;

                        apr.SuspensionStatus_Admin = (apr.SuspensionStatus_Admin == "Rejected" || apr.SuspensionStatus_SuperAdmin == "Rejected") ? status : apr.SuspensionStatus_Admin;
                        apr.SuspensionStatus_SuperAdmin = (apr.SuspensionStatus_Admin == "Rejected" || apr.SuspensionStatus_SuperAdmin == "Rejected") ? status : apr.SuspensionStatus_SuperAdmin;

                        msg = Sd.ApprovalRejectionDetailUpdate(apr, "MajorRepairing", userType);
                        if (msg.Equals("s"))
                        {
                            //if (apr.AlternatorStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Alternator Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.AlternatorTotalAmount));
                            //    //SendMailByParams("Alternator Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.AlternatorRemarks_SuperAdmin : apr.AlternatorRemarks_Admin),
                            //    //    masterdet.AlternatorTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.LeafStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Leaf Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.LeafTotalAmount));
                            //    //SendMailByParams("Leaf Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.LeafRemarks_SuperAdmin : apr.LeafRemarks_Admin),
                            //    //    masterdet.LeafTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.SuspensionStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Suspesion Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.SuspensionTotalAmount));
                            //    //SendMailByParams("Suspesion Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.SuspensionRemarks_SuperAdmin : apr.SuspensionRemarks_Admin),
                            //    //    masterdet.SuspensionTotalAmount, apr.optionalEmail);
                            //}
                        }
                    }

                    if ((apr.GearBoxStatus_Admin == "Rejected" || apr.GearBoxStatus_SuperAdmin == "Rejected") ||
                        (apr.BreakWorkStatus_Admin == "Rejected" || apr.BreakWorkStatus_SuperAdmin == "Rejected") ||
                        (apr.EngineWorkStatus_Admin == "Rejected" || apr.EngineWorkStatus_SuperAdmin == "Rejected"))
                    {
                        apr.GearBoxStatus_Admin = (apr.GearBoxStatus_Admin == "Rejected" || apr.GearBoxStatus_SuperAdmin == "Rejected") ? status : apr.GearBoxStatus_Admin;
                        apr.GearBoxStatus_SuperAdmin = (apr.GearBoxStatus_Admin == "Rejected" || apr.GearBoxStatus_SuperAdmin == "Rejected") ? status : apr.GearBoxStatus_SuperAdmin;

                        apr.BreakWorkStatus_Admin = (apr.BreakWorkStatus_Admin == "Rejected" || apr.BreakWorkStatus_SuperAdmin == "Rejected") ? status : apr.BreakWorkStatus_Admin;
                        apr.BreakWorkStatus_SuperAdmin = (apr.BreakWorkStatus_Admin == "Rejected" || apr.BreakWorkStatus_SuperAdmin == "Rejected") ? status : apr.BreakWorkStatus_SuperAdmin;

                        apr.EngineWorkStatus_Admin = (apr.EngineWorkStatus_Admin == "Rejected" || apr.EngineWorkStatus_SuperAdmin == "Rejected") ? status : apr.EngineWorkStatus_Admin;
                        apr.EngineWorkStatus_SuperAdmin = (apr.EngineWorkStatus_Admin == "Rejected" || apr.EngineWorkStatus_SuperAdmin == "Rejected") ? status : apr.EngineWorkStatus_SuperAdmin;

                        msg = Sd.ApprovalRejectionDetailUpdate(apr, "WorkBookService", userType);
                        if (msg.Equals("s"))
                        {
                            //if (apr.GearBoxStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Gear Box Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.GearBoxTotalAmount));
                            //    //SendMailByParams("Gear Box Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.GearBoxRemarks_SuperAdmin : apr.GearBoxRemarks_Admin),
                            //    //    masterdet.GearBoxTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.BreakWorkStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Break Work Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.BreakWorkAmount));
                            //    //SendMailByParams("Break Work Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.BreakWorkRemarks_SuperAdmin : apr.BreakWorkRemarks_Admin),
                            //    //    masterdet.BreakWorkAmount, apr.optionalEmail);
                            //}
                            //if (apr.EngineWorkStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Engine Work Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.EngineWorkAmount));
                            //    //SendMailByParams("Engine Work Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.EngineWorkRemarks_SuperAdmin : apr.EngineWorkRemarks_Admin),
                            //    //    masterdet.EngineWorkAmount, apr.optionalEmail);
                            //}
                        }
                    }

                    if ((apr.FuelStatus_Admin == "Rejected" || apr.FuelStatus_SuperAdmin == "Rejected") ||
                        (apr.PuncherStatus_Admin == "Rejected" || apr.PuncherStatus_SuperAdmin == "Rejected") ||
                        (apr.OilStatus_Admin == "Rejected" || apr.OilStatus_SuperAdmin == "Rejected"))
                    {
                        apr.FuelStatus_Admin = (apr.FuelStatus_Admin == "Rejected" || apr.FuelStatus_SuperAdmin == "Rejected") ? status : apr.FuelStatus_Admin;
                        apr.FuelStatus_SuperAdmin = (apr.FuelStatus_Admin == "Rejected" || apr.FuelStatus_SuperAdmin == "Rejected") ? status : apr.FuelStatus_SuperAdmin;

                        apr.PuncherStatus_Admin = (apr.PuncherStatus_Admin == "Rejected" || apr.PuncherStatus_SuperAdmin == "Rejected") ? status : apr.PuncherStatus_Admin;
                        apr.PuncherStatus_SuperAdmin = (apr.PuncherStatus_Admin == "Rejected" || apr.PuncherStatus_SuperAdmin == "Rejected") ? status : apr.PuncherStatus_SuperAdmin;

                        apr.OilStatus_Admin = (apr.OilStatus_Admin == "Rejected" || apr.OilStatus_SuperAdmin == "Rejected") ? status : apr.OilStatus_Admin;
                        apr.OilStatus_SuperAdmin = (apr.OilStatus_Admin == "Rejected" || apr.OilStatus_SuperAdmin == "Rejected") ? status : apr.OilStatus_SuperAdmin;

                        msg = Sd.ApprovalRejectionDetailUpdate(apr, "TopupService", userType);
                        if (msg.Equals("s"))
                        {
                            //if (apr.FuelStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Fuel Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.FuelTotalAmount));
                            //    //SendMailByParams("Fuel Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.FuelRemarks_SuperAdmin : apr.FuelRemarks_Admin),
                            //    //    masterdet.FuelTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.PuncherStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Puncher Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.PuncherTotalAmount));
                            //    //SendMailByParams("Puncher Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.PuncherRemarks_SuperAdmin : apr.PuncherRemarks_Admin),
                            //    //    masterdet.PuncherTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.OilStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Oil Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.OilTotalAmount));
                            //    //SendMailByParams("Oil Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.OilRemarks_SuperAdmin : apr.OilRemarks_Admin),
                            //    //    masterdet.OilTotalAmount, apr.optionalEmail);
                            //}
                        }
                    }

                    if ((apr.RadiatorStatus_Admin == "Rejected" || apr.RadiatorStatus_SuperAdmin == "Rejected") ||
                        (apr.AxleStatus_Admin == "Rejected" || apr.AxleStatus_SuperAdmin == "Rejected") ||
                        (apr.DifferentialStatus_Admin == "Rejected" || apr.DifferentialStatus_SuperAdmin == "Rejected"))
                    {
                        apr.RadiatorStatus_Admin = (apr.RadiatorStatus_Admin == "Rejected" || apr.RadiatorStatus_SuperAdmin == "Rejected") ? status : apr.RadiatorStatus_Admin;
                        apr.RadiatorStatus_SuperAdmin = (apr.RadiatorStatus_Admin == "Rejected" || apr.RadiatorStatus_SuperAdmin == "Rejected") ? status : apr.RadiatorStatus_SuperAdmin;

                        apr.AxleStatus_Admin = (apr.AxleStatus_Admin == "Rejected" || apr.AxleStatus_SuperAdmin == "Rejected") ? status : apr.AxleStatus_Admin;
                        apr.AxleStatus_SuperAdmin = (apr.AxleStatus_Admin == "Rejected" || apr.AxleStatus_SuperAdmin == "Rejected") ? status : apr.AxleStatus_SuperAdmin;

                        apr.DifferentialStatus_Admin = (apr.DifferentialStatus_Admin == "Rejected" || apr.DifferentialStatus_SuperAdmin == "Rejected") ? status : apr.DifferentialStatus_Admin;
                        apr.DifferentialStatus_SuperAdmin = (apr.DifferentialStatus_Admin == "Rejected" || apr.DifferentialStatus_SuperAdmin == "Rejected") ? status : apr.DifferentialStatus_SuperAdmin;

                        msg = Sd.ApprovalRejectionDetailUpdate(apr, "OtherService", userType);
                        if (msg.Equals("s"))
                        {
                            //if (apr.RadiatorStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Radiator Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.RadiatorTotalAmount));
                            //    //SendMailByParams("Radiator Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.RadiatorRemarks_SuperAdmin : apr.RadiatorRemarks_Admin),
                            //    //    masterdet.RadiatorTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.AxleStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Axle Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.AxleTotalAmount));
                            //    //SendMailByParams("Axle Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.AxleRemarks_SuperAdmin : apr.AxleRemarks_Admin),
                            //    //    apr.AxleTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.DifferentialStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Differential Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.DifferentialTotalAmount));
                            //    //SendMailByParams("Differential Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.DifferentialRemarks_SuperAdmin : apr.DifferentialRemarks_Admin),
                            //    //    masterdet.DifferentialTotalAmount, apr.optionalEmail);
                            //}
                        }
                    }

                    if ((apr.TurboStatus_Admin == "Rejected" || apr.TurboStatus_SuperAdmin == "Rejected") ||
                        (apr.EcmStatus_Admin == "Rejected" || apr.EcmStatus_SuperAdmin == "Rejected") ||
                        (apr.AccidentalStatus_Admin == "Rejected" || apr.AccidentalStatus_SuperAdmin == "Rejected"))
                    {
                        apr.TurboStatus_Admin = (apr.TurboStatus_Admin == "Rejected" || apr.TurboStatus_SuperAdmin == "Rejected") ? status : apr.TurboStatus_Admin;
                        apr.TurboStatus_SuperAdmin = (apr.TurboStatus_Admin == "Rejected" || apr.TurboStatus_SuperAdmin == "Rejected") ? status : apr.TurboStatus_SuperAdmin;

                        apr.EcmStatus_Admin = (apr.EcmStatus_Admin == "Rejected" || apr.EcmStatus_SuperAdmin == "Rejected") ? status : apr.EcmStatus_Admin;
                        apr.EcmStatus_SuperAdmin = (apr.EcmStatus_Admin == "Rejected" || apr.EcmStatus_SuperAdmin == "Rejected") ? status : apr.EcmStatus_SuperAdmin;

                        apr.AccidentalStatus_Admin = (apr.AccidentalStatus_Admin == "Rejected" || apr.AccidentalStatus_SuperAdmin == "Rejected") ? status : apr.AccidentalStatus_Admin;
                        apr.AccidentalStatus_SuperAdmin = (apr.AccidentalStatus_Admin == "Rejected" || apr.AccidentalStatus_SuperAdmin == "Rejected") ? status : apr.AccidentalStatus_SuperAdmin;

                        msg = Sd.ApprovalRejectionDetailUpdate(apr, "TurboService", userType);
                        if (msg.Equals("s"))
                        {
                            //if (apr.TurboStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Turbo Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.TurboTotalAmount));
                            //    //SendMailByParams("Turbo Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.TurboRemarks_SuperAdmin : apr.TurboRemarks_Admin),
                            //    //    masterdet.TurboTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.EcmStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Ecm Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.EcmTotalAmount));
                            //    //SendMailByParams("Ecm Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.EcmRemarks_SuperAdmin : apr.EcmRemarks_Admin),
                            //    //    masterdet.EcmTotalAmount, apr.optionalEmail);
                            //}
                            //if (apr.AccidentalStatus_Admin == "Rejected")
                            //{
                            //    services.Add("Accidental Service");
                            //    totalAmtArry.Add(Convert.ToInt32(masterdet.AccidentalTotalAmount));
                            //    //SendMailByParams("Accidental Service", status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                            //    //    masterdet.BranchName,
                            //    //    (userType == "SuperAdmin" ? apr.AccidentalRemarks_SuperAdmin : apr.AccidentalRemarks_Admin),
                            //    //    masterdet.AccidentalTotalAmount, apr.optionalEmail);
                            //}
                        }

                    }
                    //if (msg.Equals("s"))
                    //{
                    //    count += 1;
                    //}

                    //send mail here 
                    //if (services.Count > 0 && totalAmtArry.Count > 0)
                    //{
                    //    string servicename = string.Join(", ", services.Cast<string>().ToArray());
                    //    int totalamount = totalAmtArry.Cast<int>().ToArray().Sum();
                    //    SendMailByParams(servicename, status, apr.VehicleNumber, UserName, masterdet.CreatedBy, apr.CreatedBy_Email, masterdet.RegionName,
                    //        masterdet.BranchName,
                    //        (userType == "SuperAdmin" ? apr.TurboRemarks_SuperAdmin : apr.TurboRemarks_Admin),
                    //        totalamount.ToString(), apr.optionalEmail);
                    //}
                    if (msg.Equals("s"))
                        count += 1;
                }
                if (count > 0)
                    msg = "s";
                else
                    msg = "f";
            }
            return Json(new { msg }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SetSessionValue(string RecId,string Type,string VehicleNo)
        {
            string base64ImageString = string.Empty;
            //string Rec_Id = RecId;
            //string SType = Type;
            //string Vehicle_No = VehicleNo;

            //// DataTable dt = DataService.GetData();
            var fileName = "" + VehicleNo + " _ " + Type + ".pdf";

            var filePath = "C:\\RMS_Fleet\\Uploaded_Files\\" + RecId + "\\" + RecId + "_" + Type + ".pdf";
            var errorMessage = "";
            WebRequest webRequest = WebRequest.Create(filePath);
            webRequest.Timeout = 1200; // miliseconds
            webRequest.Method = "HEAD";

            try
            {
                webRequest.GetResponse();
                byte[] imgArray = System.IO.File.ReadAllBytes(filePath);
                base64ImageString = Convert.ToBase64String(imgArray);
                //Session["fullPath"] = filePath;
            }
            catch
            {
                base64ImageString = "";
                errorMessage = "No Details Found For The Seletect Type!";
            }

            //return the Excel file name
            return Json(new { base64ImageString, errorMessage }, JsonRequestBehavior.AllowGet);
        }
    }
}
