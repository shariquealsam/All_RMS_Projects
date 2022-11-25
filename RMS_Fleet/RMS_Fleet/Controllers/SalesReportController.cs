using ClosedXML.Excel;
using RMS_Fleet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace RMS_Fleet.Controllers
{
    public class SalesReportController : Controller
    {
        SalesReportData Sr = new SalesReportData();

        public ActionResult Index()
        {
            GetRegion();
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
              
            }
        }

        [HttpPost]
        public JsonResult GetBranch(int RegionId)
        {

            DataSet ds = Sr.GetBranchDetails(RegionId);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<SalesReport> lstBranch = new List<SalesReport>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                SalesReport H = new SalesReport();

                H.BranchId = Convert.ToInt32(dt.Rows[i]["BranchId"].ToString());
                H.BranchName = dt.Rows[i]["BranchName"].ToString();

                lstBranch.Add(H);
            }

            return Json(lstBranch, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetRegion()
        {
            string User = Session["UserType"].ToString();
            string RId = Session["RegionIds"].ToString();

            DataSet ds = new DataSet();

            if (User == "User" || User == "RH")
            {
                ds = Sr.GetRegionWithRegionDetails(RId);
            }
            else if (User == "Admin")
            {
                ds = Sr.GetRegionDetails();
            }
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<Vechile> lstRegion = new List<Vechile>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                Vechile H = new Vechile();

                H.RegionId = Convert.ToInt32(dt.Rows[i]["RegionId"].ToString());
                H.RegionName = dt.Rows[i]["RegionName"].ToString();

                lstRegion.Add(H);
            }
            ViewBag.RegionDetails = lstRegion;
            return Json(lstRegion, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetVehicle(int RegionId,int BranchId)
        {
            DataSet ds = Sr.GetVehicleDetails(RegionId, BranchId);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<SalesReport> lstVehicle = new List<SalesReport>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                SalesReport H = new SalesReport();

                H.VehicleNo = dt.Rows[i]["VehicleNo"].ToString();

                lstVehicle.Add(H);
            }

            return Json(lstVehicle, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult CheckTBranchVechileAndUploadVechile(int RegionId, int BranchId)
        {
            
                DataSet ds = Sr.CountDetailsOfVechileByBranch(BranchId,RegionId);
                DataTable dt = ds.Tables[0];
                List<SalesReport> lstVechileCount = new List<SalesReport>();
                SalesReport s = new SalesReport();
                s.TotalVechileByBranch = dt.Rows[0]["TOTAL"].ToString();
                s.UploadVechleByBranch = dt.Rows[0]["MODIFI"].ToString();
                lstVechileCount.Add(s);

                return Json(lstVechileCount, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Download(FormCollection Fd)
        {
            Get_from_config();

            string FromDate = Request.Form["txtFromDate"];
            string ToDate = Request.Form["txtToDate"];
            string RegionID = Request.Form["ddlRegion"];
            string BranchID = Request.Form["ddlBranch"];
            string VechileNumber = Request.Form["txtVehicleNumber"].Trim();
            string Vechile = Request.Form["ddlVehicleNumber"];

            string UserType = Session["UserType"].ToString();
            string RegionId = Session["RegionIds"].ToString();

            if (Vechile == "-- Select Vehicle --")
            {
                Vechile = "";
            }

            if (BranchID == "-- Select Branch --")
            {
                BranchID = "";
            }
            
            string VechileNo = "";
            int i = 0;

            foreach (var s in VechileNumber.Split(','))
            {
                if (i == 0)
                {
                    VechileNo = "'" + s + "'";
                    i = 1;
                }
                else
                {
                    VechileNo = VechileNo + ",'" + s + "'";
                }
            }

            string query1 = "";

            DataSet dsFinal = new DataSet();
            DataSet ds = new DataSet();
            DataSet ds1 = new DataSet();
            DataSet ds2 = new DataSet();
            DataSet ds3 = new DataSet();

            try
            {
                if (UserType == "Admin")
                {
                    if ((FromDate != null || FromDate != "") && (ToDate != null || ToDate != "") && (RegionID == "0" || RegionID == "") && (BranchID == "0" || BranchID == "") && (VechileNumber == ""))
                    {
                        query1 = "SELECT RegionName,BranchName,VechileNumber,Make,Manufacturing_Year, " +
                                "NoOfTyres,TyreAmount,Battery,Routine,[Denting&Painting],MinorRepairing,  " +
                                "SeatRepair,SelfWork,ElectricalWork,ClutchRepairing,Alternator,[Leaf/PattiSpring],  " +
                                "Suspension,GearBox,BreakWork,EngineWork,  " +
                                "FuelPump,Puncher,OilTopUp,RadiatorandWaterBody,AxleWork,DifferentialWork,  " +
                                "Turbo,[Ecm/Sencer],Accidental,TotalValue, TotalGSTValue,(TotalValue + TotalGSTValue) As TotalFinalAmount, " +
                                "CONCAT('TyreInvoiceNo :-',TyreInvoiceNo,'|BatteryInvoiceNo :-',BatteryInvoiceNo,'|RoutineInvoiceNo :-',RoutineInvoiceNo,  " +
                                "'|DentingInvoiceNo :-',DentingInvoiceNo,'|MinorInvoiceNo :-',MinorInvoiceNo  " +
                                ",'|SeatInvoiceNo :-',SeatInvoiceNo,'|SelfInvoiceNo :-',SelfInvoiceNo,'|ElectricalInvoiceNo :-',  " +
                                "ElectricalInvoiceNo,'|ClutchInvoiceNo :-',ClutchInvoiceNo,'|AlternatorInvoiceNo :-',AlternatorInvoiceNo,  " +
                                "'|LeafInvoiceNo :-',LeafInvoiceNo,'|SuspensionInvoiceNo :-',SuspensionInvoiceNo,'|GearBoxInvoiceNo :-',  " +
                                "GearBoxInvoiceNo,'|BreakWorkInvoiceNo :-',BreakWorkInvoiceNo,'|EngineWorkInvoiceNo:-',EngineWorkInvoiceNo,'|FuelInvoiceNo :-',  " +
                                "FuelInvoiceNo,'|PuncherInvoiceNo :-',PuncherInvoiceNo,'|OilInvoiceNo :-',OilInvoiceNo,'|RadiatorInvoiceNo :-',RadiatorInvoiceNo,  " +
                                "'|AxleInvoiceNo :-',AxleInvoiceNo,'|DifferentialInvoiceNo:-',DifferentialInvoiceNo  " +
                                ",'|TurboInvoiceNo:-',TurboInvoiceNo,'|EcmInvoiceNo:-',EcmInvoiceNo,'|AccidentalInvoiceNo:-',AccidentalInvoiceNo) As InvoiceNo  " +
                                "FROM " +
                                "( " +
                                "SELECT RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                                "NoOfTyres ,TyreAmount ,BatteryAmount As Battery,RoutineAmount As Routine,DentingAmount As [Denting&Painting],MinorAmount As MinorRepairing,  " +
                                " SeatAmount As SeatRepair,SelfAmount As SelfWork,ElectricalAmount As ElectricalWork,ClutchAmount As ClutchRepairing,AlternatorAmount As Alternator,LeafAmount As [Leaf/PattiSpring],  " +
                                " SuspensionAmount As Suspension,GearBoxAmount As GearBox,BreakWorkAmount As BreakWork,EngineWorkAmount As EngineWork,  " +
                                " FuelAmount As FuelPump,PuncherAmount As Puncher,OilAmount As OilTopUp,RadiatorAmount As RadiatorandWaterBody,AxleAmount As AxleWork,DifferentialAmount As DifferentialWork,  " +
                                " TurboAmount As Turbo,EcmAmount As [Ecm/Sencer], AccidentalTotalAmount As Accidental,  " +
                                "SUM(TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                                "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                                "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                                "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                                "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
                                "SUM( " +
                                "ISNULL([TyreGSTAmount],0)+ISNULL([BatteryGSTAmount],0)+ISNULL([RoutineGSTAmount],0)+ISNULL([DentingGSTAmount],0)+ISNULL([MinorGSTAmount],0)+ISNULL([SeatGSTAmount],0)+ISNULL([SelfGSTAmount],0)+ " +
                                "ISNULL([ElectricalGSTAmount],0)+ISNULL([ClutchGSTAmount],0)+ISNULL([AlternatorGSTAmount],0)+ISNULL([LeafGSTAmount],0)+ISNULL([SuspensionGSTAmount],0)+ISNULL([GearBoxGSTAmount],0)+ " +
                                "ISNULL([BreakGSTAmount],0)+ISNULL([EngineGSTAmount],0)+ISNULL([FuelGSTAmount],0)+ISNULL([PuncherGSTAmount],0)+ISNULL([OilGSTAmount],0)+ " +
                                "ISNULL([RadiatorGSTAmount],0)+ISNULL([AxleGSTAmount],0)+ ISNULL([DifferentialGSTAmount],0)+ISNULL([TurboGSTAmount],0)+ISNULL([EcmGSTAmount],0)+ISNULL([AccidentalGSTAmount],0) " +
                                ") As TotalGSTValue, " +
                                "SUM( " +
                                "[TyreTotalAmount]+[BatteryTotalAmount]+[RoutineTotalAmount]+[DentingTotalAmount]+[MinorTotalAmount]+[SeatTotalAmount]+[SelfTotalAmount]+ " +
                                "[ElectricalTotalAmount]+[ClutchTotalAmount]+[AlternatorTotalAmount]+[LeafTotalAmount]+[SuspensionTotalAmount]+[GearBoxTotalAmount]+ " +
                                "[BreakTotalAmount]+[EngineTotalAmount]+[FuelTotalAmount]+[PuncherTotalAmount]+[OilTotalAmount]+[RadiatorTotalAmount]+[AxleTotalAmount]+ " +
                                "[DifferentialTotalAmount]+[TurboTotalAmount]+[EcmTotalAmount]+[AccidentalGrossTotalAmount] " +
                                ") As TotalFinalAmount , " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.TyreInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as TyreInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.BatteryInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as BatteryInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.RoutineInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as RoutineInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.DentingInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as DentingInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.MinorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as MinorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SeatInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SeatInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SelfInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SelfInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.ElectricalInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as ElectricalInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.ClutchInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as ClutchInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AlternatorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AlternatorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.LeafInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as LeafInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SuspensionInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SuspensionInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.GearBoxInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as GearBoxInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.BreakWorkInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as BreakWorkInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.EngineWorkInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as EngineWorkInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.FuelInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as FuelInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.PuncherInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as PuncherInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.OilInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as OilInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.RadiatorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as RadiatorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AxleInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AxleInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.DifferentialInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as DifferentialInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.TurboInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as TurboInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.EcmInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as EcmInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AccidentalInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AccidentalInvoiceNo " +
                                " " +
                                "FROM  " +
                                "(  " +
                                "SELECT FRD.RegionName,FBD.BranchName,FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,  " +
                                "SUM(FSMSS.NoOfTyres) As NoOfTyres,SUM(FSMSS.TyreAmount) As TyreAmount,  " +
                                "SUM(FSGSO.[TyreGSTAmount]) As TyreGSTAmount, SUM(FSGSO.[TyreTotalAmount]) As [TyreTotalAmount],  " +
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,  " +
                                "SUM(FSGSO.[BatteryGSTAmount]) As BatteryGSTAmount, SUM(FSGSO.[BatteryTotalAmount]) As BatteryTotalAmount,  " +
                                "SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSGSO.[RoutineGSTAmount]) As RoutineGSTAmount, SUM(FSGSO.[RoutineTotalAmount]) As RoutineTotalAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,  " +
                                "SUM(FSGSO.[DentingGSTAmount]) As DentingGSTAmount, SUM(FSGSO.[DentingTotalAmount]) As DentingTotalAmount,  " +
                                "SUM(FSMRR.MinorAmount) As MinorAmount,    " +
                                "SUM(FSGSO.[MinorGSTAmount]) As MinorGSTAmount, SUM(FSGSO.[MinorTotalAmount]) As MinorTotalAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount,  " +
                                "SUM(FSGSO.[SeatGSTAmount]) As SeatGSTAmount, SUM(FSGSO.[SeatTotalAmount]) As SeatTotalAmount,  " +
                                "SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSGSO.[SelfGSTAmount]) As SelfGSTAmount, SUM(FSGSO.[SelfTotalAmount]) As SelfTotalAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount,  " +
                                "SUM(FSGSO.[ElectricalGSTAmount]) As ElectricalGSTAmount, SUM(FSGSO.[ElectricalTotalAmount]) As ElectricalTotalAmount,  " +
                                "SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSGST.[ClutchGSTAmount]) As ClutchGSTAmount, SUM(FSGST.[ClutchTotalAmount]) As ClutchTotalAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount,   " +
                                "SUM(FSGST.[AlternatorGSTAmount]) As AlternatorGSTAmount, SUM(FSGST.[AlternatorTotalAmount]) As AlternatorTotalAmount,  " +
                                "SUM(FSMR.LeafAmount) As LeafAmount,    " +
                                "SUM(FSGST.[LeafGSTAmount]) As LeafGSTAmount, SUM(FSGST.[LeafTotalAmount]) As LeafTotalAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount,   " +
                                "SUM(FSGST.[SuspensionGSTAmount]) As SuspensionGSTAmount, SUM(FSGST.[SuspensionTotalAmount]) As SuspensionTotalAmount,  " +
                                "SUM(FSWS.GearBoxAmount) As GearBoxAmount,    " +
                                "SUM(FSGST.[GearBoxGSTAmount]) As GearBoxGSTAmount, SUM(FSGST.[GearBoxTotalAmount]) As GearBoxTotalAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount,   " +
                                "SUM(FSGST.[BreakGSTAmount]) As BreakGSTAmount, SUM(FSGST.[BreakTotalAmount]) As BreakTotalAmount,  " +
                                "SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,    " +
                                "SUM(FSGST.[EngineGSTAmount]) As EngineGSTAmount, SUM(FSGST.[EngineTotalAmount]) As EngineTotalAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,  " +
                                "SUM(FSGST.[FuelGSTAmount]) As FuelGSTAmount, SUM(FSGST.[FuelTotalAmount]) As FuelTotalAmount,  " +
                                "SUM(FSTS.PuncherAmount) As PuncherAmount,    " +
                                "SUM(FSGSTT.[PuncherGSTAmount]) As PuncherGSTAmount, SUM(FSGSTT.[PuncherTotalAmount]) As PuncherTotalAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,  " +
                                "SUM(FSGSTT.[OilGSTAmount]) As OilGSTAmount, SUM(FSGSTT.[OilTotalAmount]) As OilTotalAmount,  " +
                                "SUM(FSOS.RadiatorAmount) As RadiatorAmount,    " +
                                "SUM(FSGSTT.[RadiatorGSTAmount]) As RadiatorGSTAmount, SUM(FSGSTT.[RadiatorTotalAmount]) As RadiatorTotalAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount,   " +
                                "SUM(FSGSTT.[AxleGSTAmount]) As AxleGSTAmount, SUM(FSGSTT.[AxleTotalAmount]) As AxleTotalAmount,  " +
                                "SUM(FSOS.DifferentialAmount) As DifferentialAmount,    " +
                                "SUM(FSGSTT.[DifferentialGSTAmount]) As DifferentialGSTAmount, SUM(FSGSTT.[DifferentialTotalAmount]) As DifferentialTotalAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount,   " +
                                "SUM(FSGSTT.[TurboGSTAmount]) As TurboGSTAmount, SUM(FSGSTT.[TurboTotalAmount]) As TurboTotalAmount,  " +
                                "SUM(FSTSS.EcmAmount) AS EcmAmount,    " +
                                "SUM(FSGSTT.[EcmGSTAmount]) As EcmGSTAmount, SUM(FSGSTT.[EcmTotalAmount]) As EcmTotalAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount ,  " +
                                "SUM(FSGSTT.[AccidentalGSTAmount]) As AccidentalGSTAmount, SUM(FSGSTT.[AccidentalGrossTotalAmount]) As AccidentalGrossTotalAmount    " + 
                                " " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMR.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMRR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMRR.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMSS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON (FSMD.Rec_Id = FSOS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTSS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON (FSMD.Rec_Id = FSWS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON (FSMD.BranchId = FBD.BranchId)  " +
                                "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FSMD.RegionId = FRD.RegionId)  " +
                                "INNER JOIN (select * from (select *,ROW_NUMBER() over (partition by vehicleno order by [date] desc) rm from [Fleet_Vehicle_Details_Date] with(nolock)where [Date] >='" + FromDate + "' and [Date] <='" + ToDate + "') tbl where tbl.rm=1 ) FVD  ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSO.Rec_Id)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON (FSMD.Rec_Id = FSGST.Rec_Id)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSTT.Rec_Id)  " +
                                "WHERE SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "GROUP BY FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,FRD.RegionName,FBD.BranchName " +
                                ")TblData  " +
                                "GROUP BY RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                                "NoOfTyres,TyreAmount,BatteryAmount,RoutineAmount,DentingAmount,MinorAmount,  " +
                                "SeatAmount,SelfAmount,ElectricalAmount,ClutchAmount,AlternatorAmount,LeafAmount,  " +
                                "SuspensionAmount,GearBoxAmount,BreakWorkAmount,EngineWorkAmount,  " +
                                "FuelAmount,PuncherAmount,OilAmount,RadiatorAmount,AxleAmount,DifferentialAmount,  " +
                                "TurboAmount,EcmAmount,AccidentalTotalAmount " +
                                ")tbl ";
                    }
                }
                if (UserType == "User")
                {
                    if ((FromDate != null || FromDate != "") && (ToDate != null || ToDate != "") && (RegionID == "0" || RegionID == "") && (BranchID == "0" || BranchID == "") && (VechileNumber == ""))
                    {
                        query1 = "SELECT RegionName,BranchName,VechileNumber,Make,Manufacturing_Year, " +
                                "NoOfTyres,TyreAmount,Battery,Routine,[Denting&Painting],MinorRepairing,  " +
                                "SeatRepair,SelfWork,ElectricalWork,ClutchRepairing,Alternator,[Leaf/PattiSpring],  " +
                                "Suspension,GearBox,BreakWork,EngineWork,  " +
                                "FuelPump,Puncher,OilTopUp,RadiatorandWaterBody,AxleWork,DifferentialWork,  " +
                                "Turbo,[Ecm/Sencer],Accidental,TotalValue, TotalGSTValue,(TotalValue + TotalGSTValue) As TotalFinalAmount,  " +
                                "CONCAT('TyreInvoiceNo :-',TyreInvoiceNo,'|BatteryInvoiceNo :-',BatteryInvoiceNo,'|RoutineInvoiceNo :-',RoutineInvoiceNo,  " +
                                "'|DentingInvoiceNo :-',DentingInvoiceNo,'|MinorInvoiceNo :-',MinorInvoiceNo  " +
                                ",'|SeatInvoiceNo :-',SeatInvoiceNo,'|SelfInvoiceNo :-',SelfInvoiceNo,'|ElectricalInvoiceNo :-',  " +
                                "ElectricalInvoiceNo,'|ClutchInvoiceNo :-',ClutchInvoiceNo,'|AlternatorInvoiceNo :-',AlternatorInvoiceNo,  " +
                                "'|LeafInvoiceNo :-',LeafInvoiceNo,'|SuspensionInvoiceNo :-',SuspensionInvoiceNo,'|GearBoxInvoiceNo :-',  " +
                                "GearBoxInvoiceNo,'|BreakWorkInvoiceNo :-',BreakWorkInvoiceNo,'|EngineWorkInvoiceNo:-',EngineWorkInvoiceNo,'|FuelInvoiceNo :-',  " +
                                "FuelInvoiceNo,'|PuncherInvoiceNo :-',PuncherInvoiceNo,'|OilInvoiceNo :-',OilInvoiceNo,'|RadiatorInvoiceNo :-',RadiatorInvoiceNo,  " +
                                "'|AxleInvoiceNo :-',AxleInvoiceNo,'|DifferentialInvoiceNo:-',DifferentialInvoiceNo  " +
                                ",'|TurboInvoiceNo:-',TurboInvoiceNo,'|EcmInvoiceNo:-',EcmInvoiceNo,'|AccidentalInvoiceNo:-',AccidentalInvoiceNo) As InvoiceNo  " +
                                "FROM " +
                                "( " +
                                "SELECT RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                                "NoOfTyres ,TyreAmount ,BatteryAmount As Battery,RoutineAmount As Routine,DentingAmount As [Denting&Painting],MinorAmount As MinorRepairing,  " +
                                " SeatAmount As SeatRepair,SelfAmount As SelfWork,ElectricalAmount As ElectricalWork,ClutchAmount As ClutchRepairing,AlternatorAmount As Alternator,LeafAmount As [Leaf/PattiSpring],  " +
                                " SuspensionAmount As Suspension,GearBoxAmount As GearBox,BreakWorkAmount As BreakWork,EngineWorkAmount As EngineWork,  " +
                                " FuelAmount As FuelPump,PuncherAmount As Puncher,OilAmount As OilTopUp,RadiatorAmount As RadiatorandWaterBody,AxleAmount As AxleWork,DifferentialAmount As DifferentialWork,  " +
                                " TurboAmount As Turbo,EcmAmount As [Ecm/Sencer], AccidentalTotalAmount As Accidental,  " +
                                "SUM(TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                                "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                                "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                                "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                                "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
                                "SUM( " +
                                "ISNULL([TyreGSTAmount],0)+ISNULL([BatteryGSTAmount],0)+ISNULL([RoutineGSTAmount],0)+ISNULL([DentingGSTAmount],0)+ISNULL([MinorGSTAmount],0)+ISNULL([SeatGSTAmount],0)+ISNULL([SelfGSTAmount],0)+ " +
                                "ISNULL([ElectricalGSTAmount],0)+ISNULL([ClutchGSTAmount],0)+ISNULL([AlternatorGSTAmount],0)+ISNULL([LeafGSTAmount],0)+ISNULL([SuspensionGSTAmount],0)+ISNULL([GearBoxGSTAmount],0)+ " +
                                "ISNULL([BreakGSTAmount],0)+ISNULL([EngineGSTAmount],0)+ISNULL([FuelGSTAmount],0)+ISNULL([PuncherGSTAmount],0)+ISNULL([OilGSTAmount],0)+ " +
                                "ISNULL([RadiatorGSTAmount],0)+ISNULL([AxleGSTAmount],0)+ ISNULL([DifferentialGSTAmount],0)+ISNULL([TurboGSTAmount],0)+ISNULL([EcmGSTAmount],0)+ISNULL([AccidentalGSTAmount],0) " +
                                ") As TotalGSTValue, " +
                                "SUM( " +
                                "[TyreTotalAmount]+[BatteryTotalAmount]+[RoutineTotalAmount]+[DentingTotalAmount]+[MinorTotalAmount]+[SeatTotalAmount]+[SelfTotalAmount]+ " +
                                "[ElectricalTotalAmount]+[ClutchTotalAmount]+[AlternatorTotalAmount]+[LeafTotalAmount]+[SuspensionTotalAmount]+[GearBoxTotalAmount]+ " +
                                "[BreakTotalAmount]+[EngineTotalAmount]+[FuelTotalAmount]+[PuncherTotalAmount]+[OilTotalAmount]+[RadiatorTotalAmount]+[AxleTotalAmount]+ " +
                                "[DifferentialTotalAmount]+[TurboTotalAmount]+[EcmTotalAmount]+[AccidentalGrossTotalAmount] " +
                                ") As TotalFinalAmount , " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.TyreInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as TyreInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.BatteryInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as BatteryInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.RoutineInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as RoutineInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.DentingInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as DentingInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.MinorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as MinorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SeatInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SeatInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SelfInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SelfInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.ElectricalInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as ElectricalInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.ClutchInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as ClutchInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AlternatorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AlternatorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.LeafInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as LeafInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SuspensionInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SuspensionInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.GearBoxInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as GearBoxInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.BreakWorkInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as BreakWorkInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.EngineWorkInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as EngineWorkInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.FuelInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as FuelInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.PuncherInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as PuncherInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.OilInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as OilInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.RadiatorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as RadiatorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AxleInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AxleInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.DifferentialInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as DifferentialInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.TurboInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as TurboInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.EcmInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as EcmInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AccidentalInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AccidentalInvoiceNo " +
                                " " +
                                "FROM  " +
                                "(  " +
                                "SELECT FRD.RegionName,FBD.BranchName,FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,  " +
                                "SUM(FSMSS.NoOfTyres) As NoOfTyres,SUM(FSMSS.TyreAmount) As TyreAmount,  " +
                                "SUM(FSGSO.[TyreGSTAmount]) As TyreGSTAmount, SUM(FSGSO.[TyreTotalAmount]) As [TyreTotalAmount],  " +
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,  " +
                                "SUM(FSGSO.[BatteryGSTAmount]) As BatteryGSTAmount, SUM(FSGSO.[BatteryTotalAmount]) As BatteryTotalAmount,  " +
                                "SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSGSO.[RoutineGSTAmount]) As RoutineGSTAmount, SUM(FSGSO.[RoutineTotalAmount]) As RoutineTotalAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,  " +
                                "SUM(FSGSO.[DentingGSTAmount]) As DentingGSTAmount, SUM(FSGSO.[DentingTotalAmount]) As DentingTotalAmount,  " +
                                "SUM(FSMRR.MinorAmount) As MinorAmount,    " +
                                "SUM(FSGSO.[MinorGSTAmount]) As MinorGSTAmount, SUM(FSGSO.[MinorTotalAmount]) As MinorTotalAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount,  " +
                                "SUM(FSGSO.[SeatGSTAmount]) As SeatGSTAmount, SUM(FSGSO.[SeatTotalAmount]) As SeatTotalAmount,  " +
                                "SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSGSO.[SelfGSTAmount]) As SelfGSTAmount, SUM(FSGSO.[SelfTotalAmount]) As SelfTotalAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount,  " +
                                "SUM(FSGSO.[ElectricalGSTAmount]) As ElectricalGSTAmount, SUM(FSGSO.[ElectricalTotalAmount]) As ElectricalTotalAmount,  " +
                                "SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSGST.[ClutchGSTAmount]) As ClutchGSTAmount, SUM(FSGST.[ClutchTotalAmount]) As ClutchTotalAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount,   " +
                                "SUM(FSGST.[AlternatorGSTAmount]) As AlternatorGSTAmount, SUM(FSGST.[AlternatorTotalAmount]) As AlternatorTotalAmount,  " +
                                "SUM(FSMR.LeafAmount) As LeafAmount,    " +
                                "SUM(FSGST.[LeafGSTAmount]) As LeafGSTAmount, SUM(FSGST.[LeafTotalAmount]) As LeafTotalAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount,   " +
                                "SUM(FSGST.[SuspensionGSTAmount]) As SuspensionGSTAmount, SUM(FSGST.[SuspensionTotalAmount]) As SuspensionTotalAmount,  " +
                                "SUM(FSWS.GearBoxAmount) As GearBoxAmount,    " +
                                "SUM(FSGST.[GearBoxGSTAmount]) As GearBoxGSTAmount, SUM(FSGST.[GearBoxTotalAmount]) As GearBoxTotalAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount,   " +
                                "SUM(FSGST.[BreakGSTAmount]) As BreakGSTAmount, SUM(FSGST.[BreakTotalAmount]) As BreakTotalAmount,  " +
                                "SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,    " +
                                "SUM(FSGST.[EngineGSTAmount]) As EngineGSTAmount, SUM(FSGST.[EngineTotalAmount]) As EngineTotalAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,  " +
                                "SUM(FSGST.[FuelGSTAmount]) As FuelGSTAmount, SUM(FSGST.[FuelTotalAmount]) As FuelTotalAmount,  " +
                                "SUM(FSTS.PuncherAmount) As PuncherAmount,    " +
                                "SUM(FSGSTT.[PuncherGSTAmount]) As PuncherGSTAmount, SUM(FSGSTT.[PuncherTotalAmount]) As PuncherTotalAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,  " +
                                "SUM(FSGSTT.[OilGSTAmount]) As OilGSTAmount, SUM(FSGSTT.[OilTotalAmount]) As OilTotalAmount,  " +
                                "SUM(FSOS.RadiatorAmount) As RadiatorAmount,    " +
                                "SUM(FSGSTT.[RadiatorGSTAmount]) As RadiatorGSTAmount, SUM(FSGSTT.[RadiatorTotalAmount]) As RadiatorTotalAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount,   " +
                                "SUM(FSGSTT.[AxleGSTAmount]) As AxleGSTAmount, SUM(FSGSTT.[AxleTotalAmount]) As AxleTotalAmount,  " +
                                "SUM(FSOS.DifferentialAmount) As DifferentialAmount,    " +
                                "SUM(FSGSTT.[DifferentialGSTAmount]) As DifferentialGSTAmount, SUM(FSGSTT.[DifferentialTotalAmount]) As DifferentialTotalAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount,   " +
                                "SUM(FSGSTT.[TurboGSTAmount]) As TurboGSTAmount, SUM(FSGSTT.[TurboTotalAmount]) As TurboTotalAmount,  " +
                                "SUM(FSTSS.EcmAmount) AS EcmAmount,    " +
                                "SUM(FSGSTT.[EcmGSTAmount]) As EcmGSTAmount, SUM(FSGSTT.[EcmTotalAmount]) As EcmTotalAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount ,  " +
                                "SUM(FSGSTT.[AccidentalGSTAmount]) As AccidentalGSTAmount, SUM(FSGSTT.[AccidentalGrossTotalAmount]) As AccidentalGrossTotalAmount    " + 
                                " " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMR.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMRR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMRR.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMSS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON (FSMD.Rec_Id = FSOS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTSS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON (FSMD.Rec_Id = FSWS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON (FSMD.BranchId = FBD.BranchId)  " +
                                "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FSMD.RegionId = FRD.RegionId)  " +
                                "INNER JOIN (select * from (select *,ROW_NUMBER() over (partition by vehicleno order by [date] desc) rm from [Fleet_Vehicle_Details_Date] with(nolock)where [Date] >='2021-07-01' and [Date] <='2021-08-25') tbl where tbl.rm=1 ) FVD  ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSO.Rec_Id)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON (FSMD.Rec_Id = FSGST.Rec_Id)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSTT.Rec_Id)  " +
                                "WHERE SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  AND FSMD.RegionId =" + RegionId + " " +
                                "GROUP BY FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,FRD.RegionName,FBD.BranchName " +
                                ")TblData  " +
                                "GROUP BY RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                                "NoOfTyres,TyreAmount,BatteryAmount,RoutineAmount,DentingAmount,MinorAmount,  " +
                                "SeatAmount,SelfAmount,ElectricalAmount,ClutchAmount,AlternatorAmount,LeafAmount,  " +
                                "SuspensionAmount,GearBoxAmount,BreakWorkAmount,EngineWorkAmount,  " +
                                "FuelAmount,PuncherAmount,OilAmount,RadiatorAmount,AxleAmount,DifferentialAmount,  " +
                                "TurboAmount,EcmAmount,AccidentalTotalAmount " +
                                ")tbl ";       
                    }
                }
               
                if (FromDate != null && ToDate != null && RegionID != "" && BranchID == "" && VechileNumber == "" && Vechile == "")
                {
                    query1 = "SELECT RegionName,BranchName,VechileNumber,Make,Manufacturing_Year, " +
                               "NoOfTyres,TyreAmount,Battery,Routine,[Denting&Painting],MinorRepairing,  " +
                               "SeatRepair,SelfWork,ElectricalWork,ClutchRepairing,Alternator,[Leaf/PattiSpring],  " +
                               "Suspension,GearBox,BreakWork,EngineWork,  " +
                               "FuelPump,Puncher,OilTopUp,RadiatorandWaterBody,AxleWork,DifferentialWork,  " +
                               "Turbo,[Ecm/Sencer],Accidental,TotalValue, TotalGSTValue,(TotalValue + TotalGSTValue) As TotalFinalAmount,  " +
                               "CONCAT('TyreInvoiceNo :-',TyreInvoiceNo,'|BatteryInvoiceNo :-',BatteryInvoiceNo,'|RoutineInvoiceNo :-',RoutineInvoiceNo,  " +
                               "'|DentingInvoiceNo :-',DentingInvoiceNo,'|MinorInvoiceNo :-',MinorInvoiceNo  " +
                               ",'|SeatInvoiceNo :-',SeatInvoiceNo,'|SelfInvoiceNo :-',SelfInvoiceNo,'|ElectricalInvoiceNo :-',  " +
                               "ElectricalInvoiceNo,'|ClutchInvoiceNo :-',ClutchInvoiceNo,'|AlternatorInvoiceNo :-',AlternatorInvoiceNo,  " +
                               "'|LeafInvoiceNo :-',LeafInvoiceNo,'|SuspensionInvoiceNo :-',SuspensionInvoiceNo,'|GearBoxInvoiceNo :-',  " +
                               "GearBoxInvoiceNo,'|BreakWorkInvoiceNo :-',BreakWorkInvoiceNo,'|EngineWorkInvoiceNo:-',EngineWorkInvoiceNo,'|FuelInvoiceNo :-',  " +
                               "FuelInvoiceNo,'|PuncherInvoiceNo :-',PuncherInvoiceNo,'|OilInvoiceNo :-',OilInvoiceNo,'|RadiatorInvoiceNo :-',RadiatorInvoiceNo,  " +
                               "'|AxleInvoiceNo :-',AxleInvoiceNo,'|DifferentialInvoiceNo:-',DifferentialInvoiceNo  " +
                               ",'|TurboInvoiceNo:-',TurboInvoiceNo,'|EcmInvoiceNo:-',EcmInvoiceNo,'|AccidentalInvoiceNo:-',AccidentalInvoiceNo) As InvoiceNo  " +
                               "FROM " +
                               "( " +
                               "SELECT RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                               "NoOfTyres ,TyreAmount ,BatteryAmount As Battery,RoutineAmount As Routine,DentingAmount As [Denting&Painting],MinorAmount As MinorRepairing,  " +
                               " SeatAmount As SeatRepair,SelfAmount As SelfWork,ElectricalAmount As ElectricalWork,ClutchAmount As ClutchRepairing,AlternatorAmount As Alternator,LeafAmount As [Leaf/PattiSpring],  " +
                               " SuspensionAmount As Suspension,GearBoxAmount As GearBox,BreakWorkAmount As BreakWork,EngineWorkAmount As EngineWork,  " +
                               " FuelAmount As FuelPump,PuncherAmount As Puncher,OilAmount As OilTopUp,RadiatorAmount As RadiatorandWaterBody,AxleAmount As AxleWork,DifferentialAmount As DifferentialWork,  " +
                               " TurboAmount As Turbo,EcmAmount As [Ecm/Sencer], AccidentalTotalAmount As Accidental,  " +
                               "SUM(TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                               "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                               "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                               "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                               "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
                               "SUM( " +
                                "ISNULL([TyreGSTAmount],0)+ISNULL([BatteryGSTAmount],0)+ISNULL([RoutineGSTAmount],0)+ISNULL([DentingGSTAmount],0)+ISNULL([MinorGSTAmount],0)+ISNULL([SeatGSTAmount],0)+ISNULL([SelfGSTAmount],0)+ " +
                                "ISNULL([ElectricalGSTAmount],0)+ISNULL([ClutchGSTAmount],0)+ISNULL([AlternatorGSTAmount],0)+ISNULL([LeafGSTAmount],0)+ISNULL([SuspensionGSTAmount],0)+ISNULL([GearBoxGSTAmount],0)+ " +
                                "ISNULL([BreakGSTAmount],0)+ISNULL([EngineGSTAmount],0)+ISNULL([FuelGSTAmount],0)+ISNULL([PuncherGSTAmount],0)+ISNULL([OilGSTAmount],0)+ " +
                                "ISNULL([RadiatorGSTAmount],0)+ISNULL([AxleGSTAmount],0)+ ISNULL([DifferentialGSTAmount],0)+ISNULL([TurboGSTAmount],0)+ISNULL([EcmGSTAmount],0)+ISNULL([AccidentalGSTAmount],0) " +
                                ") As TotalGSTValue, " +
                                "SUM( " +
                                "[TyreTotalAmount]+[BatteryTotalAmount]+[RoutineTotalAmount]+[DentingTotalAmount]+[MinorTotalAmount]+[SeatTotalAmount]+[SelfTotalAmount]+ " +
                                "[ElectricalTotalAmount]+[ClutchTotalAmount]+[AlternatorTotalAmount]+[LeafTotalAmount]+[SuspensionTotalAmount]+[GearBoxTotalAmount]+ " +
                                "[BreakTotalAmount]+[EngineTotalAmount]+[FuelTotalAmount]+[PuncherTotalAmount]+[OilTotalAmount]+[RadiatorTotalAmount]+[AxleTotalAmount]+ " +
                                "[DifferentialTotalAmount]+[TurboTotalAmount]+[EcmTotalAmount]+[AccidentalGrossTotalAmount] " +
                                ") As TotalFinalAmount , " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.TyreInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as TyreInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.BatteryInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as BatteryInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.RoutineInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as RoutineInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.DentingInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as DentingInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.MinorInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as MinorInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.SeatInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as SeatInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.SelfInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as SelfInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.ElectricalInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as ElectricalInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.ClutchInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as ClutchInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.AlternatorInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as AlternatorInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.LeafInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as LeafInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.SuspensionInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as SuspensionInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.GearBoxInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as GearBoxInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.BreakWorkInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as BreakWorkInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.EngineWorkInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as EngineWorkInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.FuelInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as FuelInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.PuncherInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as PuncherInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.OilInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as OilInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.RadiatorInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as RadiatorInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.AxleInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as AxleInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.DifferentialInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as DifferentialInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.TurboInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as TurboInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.EcmInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as EcmInvoiceNo, " +
                               " " +
                               "(SELECT STUFF( " +
                               "(SELECT  '~' + s.AccidentalInvoiceNo " +
                               "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                               "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and t1.VechileNumber=tblData.VechileNumber " +
                               "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                               "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                               "and  VechileNumber=tblData.VechileNumber " +
                               "GROUP BY VechileNumber " +
                               ") as AccidentalInvoiceNo " +
                               " " +
                               "FROM  " +
                               "(  " +
                               "SELECT FRD.RegionName,FBD.BranchName,FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,  " +
                              "SUM(FSMSS.NoOfTyres) As NoOfTyres,SUM(FSMSS.TyreAmount) As TyreAmount,  " +
                                "SUM(FSGSO.[TyreGSTAmount]) As TyreGSTAmount, SUM(FSGSO.[TyreTotalAmount]) As [TyreTotalAmount],  " +
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,  " +
                                "SUM(FSGSO.[BatteryGSTAmount]) As BatteryGSTAmount, SUM(FSGSO.[BatteryTotalAmount]) As BatteryTotalAmount,  " +
                                "SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSGSO.[RoutineGSTAmount]) As RoutineGSTAmount, SUM(FSGSO.[RoutineTotalAmount]) As RoutineTotalAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,  " +
                                "SUM(FSGSO.[DentingGSTAmount]) As DentingGSTAmount, SUM(FSGSO.[DentingTotalAmount]) As DentingTotalAmount,  " +
                                "SUM(FSMRR.MinorAmount) As MinorAmount,    " +
                                "SUM(FSGSO.[MinorGSTAmount]) As MinorGSTAmount, SUM(FSGSO.[MinorTotalAmount]) As MinorTotalAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount,  " +
                                "SUM(FSGSO.[SeatGSTAmount]) As SeatGSTAmount, SUM(FSGSO.[SeatTotalAmount]) As SeatTotalAmount,  " +
                                "SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSGSO.[SelfGSTAmount]) As SelfGSTAmount, SUM(FSGSO.[SelfTotalAmount]) As SelfTotalAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount,  " +
                                "SUM(FSGSO.[ElectricalGSTAmount]) As ElectricalGSTAmount, SUM(FSGSO.[ElectricalTotalAmount]) As ElectricalTotalAmount,  " +
                                "SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSGST.[ClutchGSTAmount]) As ClutchGSTAmount, SUM(FSGST.[ClutchTotalAmount]) As ClutchTotalAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount,   " +
                                "SUM(FSGST.[AlternatorGSTAmount]) As AlternatorGSTAmount, SUM(FSGST.[AlternatorTotalAmount]) As AlternatorTotalAmount,  " +
                                "SUM(FSMR.LeafAmount) As LeafAmount,    " +
                                "SUM(FSGST.[LeafGSTAmount]) As LeafGSTAmount, SUM(FSGST.[LeafTotalAmount]) As LeafTotalAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount,   " +
                                "SUM(FSGST.[SuspensionGSTAmount]) As SuspensionGSTAmount, SUM(FSGST.[SuspensionTotalAmount]) As SuspensionTotalAmount,  " +
                                "SUM(FSWS.GearBoxAmount) As GearBoxAmount,    " +
                                "SUM(FSGST.[GearBoxGSTAmount]) As GearBoxGSTAmount, SUM(FSGST.[GearBoxTotalAmount]) As GearBoxTotalAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount,   " +
                                "SUM(FSGST.[BreakGSTAmount]) As BreakGSTAmount, SUM(FSGST.[BreakTotalAmount]) As BreakTotalAmount,  " +
                                "SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,    " +
                                "SUM(FSGST.[EngineGSTAmount]) As EngineGSTAmount, SUM(FSGST.[EngineTotalAmount]) As EngineTotalAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,  " +
                                "SUM(FSGST.[FuelGSTAmount]) As FuelGSTAmount, SUM(FSGST.[FuelTotalAmount]) As FuelTotalAmount,  " +
                                "SUM(FSTS.PuncherAmount) As PuncherAmount,    " +
                                "SUM(FSGSTT.[PuncherGSTAmount]) As PuncherGSTAmount, SUM(FSGSTT.[PuncherTotalAmount]) As PuncherTotalAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,  " +
                                "SUM(FSGSTT.[OilGSTAmount]) As OilGSTAmount, SUM(FSGSTT.[OilTotalAmount]) As OilTotalAmount,  " +
                                "SUM(FSOS.RadiatorAmount) As RadiatorAmount,    " +
                                "SUM(FSGSTT.[RadiatorGSTAmount]) As RadiatorGSTAmount, SUM(FSGSTT.[RadiatorTotalAmount]) As RadiatorTotalAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount,   " +
                                "SUM(FSGSTT.[AxleGSTAmount]) As AxleGSTAmount, SUM(FSGSTT.[AxleTotalAmount]) As AxleTotalAmount,  " +
                                "SUM(FSOS.DifferentialAmount) As DifferentialAmount,    " +
                                "SUM(FSGSTT.[DifferentialGSTAmount]) As DifferentialGSTAmount, SUM(FSGSTT.[DifferentialTotalAmount]) As DifferentialTotalAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount,   " +
                                "SUM(FSGSTT.[TurboGSTAmount]) As TurboGSTAmount, SUM(FSGSTT.[TurboTotalAmount]) As TurboTotalAmount,  " +
                                "SUM(FSTSS.EcmAmount) AS EcmAmount,    " +
                                "SUM(FSGSTT.[EcmGSTAmount]) As EcmGSTAmount, SUM(FSGSTT.[EcmTotalAmount]) As EcmTotalAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount ,  " +
                                "SUM(FSGSTT.[AccidentalGSTAmount]) As AccidentalGSTAmount, SUM(FSGSTT.[AccidentalGrossTotalAmount]) As AccidentalGrossTotalAmount    " + 
                               " " +
                               "FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK)  " +
                               "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMR.Rec_Id)  " +
                               "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMS.Rec_Id)  " +
                               "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMRR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMRR.Rec_Id)  " +
                               "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMSS.Rec_Id)  " +
                               "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON (FSMD.Rec_Id = FSOS.Rec_Id)  " +
                               "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTS.Rec_Id)  " +
                               "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTSS.Rec_Id)  " +
                               "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON (FSMD.Rec_Id = FSWS.Rec_Id)  " +
                               "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON (FSMD.BranchId = FBD.BranchId)  " +
                               "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FSMD.RegionId = FRD.RegionId)  " +
                               "INNER JOIN (select * from (select *,ROW_NUMBER() over (partition by vehicleno order by [date] desc) rm from [Fleet_Vehicle_Details_Date] with(nolock)where [Date] >='2021-07-01' and [Date] <='2021-08-25') tbl where tbl.rm=1 ) FVD  ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
                               "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSO.Rec_Id)  " +
                               "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON (FSMD.Rec_Id = FSGST.Rec_Id)  " +
                               "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSTT.Rec_Id)  " +
                               "WHERE SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "' AND FSMD.RegionId =" + RegionId + " " +
                               "GROUP BY FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,FRD.RegionName,FBD.BranchName " +
                               ")TblData  " +
                               "GROUP BY RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                               "NoOfTyres,TyreAmount,BatteryAmount,RoutineAmount,DentingAmount,MinorAmount,  " +
                               "SeatAmount,SelfAmount,ElectricalAmount,ClutchAmount,AlternatorAmount,LeafAmount,  " +
                               "SuspensionAmount,GearBoxAmount,BreakWorkAmount,EngineWorkAmount,  " +
                               "FuelAmount,PuncherAmount,OilAmount,RadiatorAmount,AxleAmount,DifferentialAmount,  " +
                               "TurboAmount,EcmAmount,AccidentalTotalAmount " +
                               ")tbl ";
                }
                if (FromDate != null && ToDate != null && RegionID != "" && BranchID != "" && VechileNumber == "" && Vechile == "")
                {
                     query1 = "SELECT RegionName,BranchName,VechileNumber,Make,Manufacturing_Year, " +
                                "NoOfTyres,TyreAmount,Battery,Routine,[Denting&Painting],MinorRepairing,  " +
                                "SeatRepair,SelfWork,ElectricalWork,ClutchRepairing,Alternator,[Leaf/PattiSpring],  " +
                                "Suspension,GearBox,BreakWork,EngineWork,  " +
                                "FuelPump,Puncher,OilTopUp,RadiatorandWaterBody,AxleWork,DifferentialWork,  " +
                                "Turbo,[Ecm/Sencer],Accidental,TotalValue, TotalGSTValue,(TotalValue + TotalGSTValue) As TotalFinalAmount,  " +
                                "CONCAT('TyreInvoiceNo :-',TyreInvoiceNo,'|BatteryInvoiceNo :-',BatteryInvoiceNo,'|RoutineInvoiceNo :-',RoutineInvoiceNo,  " +
                                "'|DentingInvoiceNo :-',DentingInvoiceNo,'|MinorInvoiceNo :-',MinorInvoiceNo  " +
                                ",'|SeatInvoiceNo :-',SeatInvoiceNo,'|SelfInvoiceNo :-',SelfInvoiceNo,'|ElectricalInvoiceNo :-',  " +
                                "ElectricalInvoiceNo,'|ClutchInvoiceNo :-',ClutchInvoiceNo,'|AlternatorInvoiceNo :-',AlternatorInvoiceNo,  " +
                                "'|LeafInvoiceNo :-',LeafInvoiceNo,'|SuspensionInvoiceNo :-',SuspensionInvoiceNo,'|GearBoxInvoiceNo :-',  " +
                                "GearBoxInvoiceNo,'|BreakWorkInvoiceNo :-',BreakWorkInvoiceNo,'|EngineWorkInvoiceNo:-',EngineWorkInvoiceNo,'|FuelInvoiceNo :-',  " +
                                "FuelInvoiceNo,'|PuncherInvoiceNo :-',PuncherInvoiceNo,'|OilInvoiceNo :-',OilInvoiceNo,'|RadiatorInvoiceNo :-',RadiatorInvoiceNo,  " +
                                "'|AxleInvoiceNo :-',AxleInvoiceNo,'|DifferentialInvoiceNo:-',DifferentialInvoiceNo  " +
                                ",'|TurboInvoiceNo:-',TurboInvoiceNo,'|EcmInvoiceNo:-',EcmInvoiceNo,'|AccidentalInvoiceNo:-',AccidentalInvoiceNo) As InvoiceNo  " +
                                "FROM " +
                                "( " +
                                "SELECT RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                                "NoOfTyres ,TyreAmount ,BatteryAmount As Battery,RoutineAmount As Routine,DentingAmount As [Denting&Painting],MinorAmount As MinorRepairing,  " +
                                " SeatAmount As SeatRepair,SelfAmount As SelfWork,ElectricalAmount As ElectricalWork,ClutchAmount As ClutchRepairing,AlternatorAmount As Alternator,LeafAmount As [Leaf/PattiSpring],  " +
                                " SuspensionAmount As Suspension,GearBoxAmount As GearBox,BreakWorkAmount As BreakWork,EngineWorkAmount As EngineWork,  " +
                                " FuelAmount As FuelPump,PuncherAmount As Puncher,OilAmount As OilTopUp,RadiatorAmount As RadiatorandWaterBody,AxleAmount As AxleWork,DifferentialAmount As DifferentialWork,  " +
                                " TurboAmount As Turbo,EcmAmount As [Ecm/Sencer], AccidentalTotalAmount As Accidental,  " +
                                "SUM(TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                                "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                                "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                                "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                                "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
                                "SUM( " +
                                "ISNULL([TyreGSTAmount],0)+ISNULL([BatteryGSTAmount],0)+ISNULL([RoutineGSTAmount],0)+ISNULL([DentingGSTAmount],0)+ISNULL([MinorGSTAmount],0)+ISNULL([SeatGSTAmount],0)+ISNULL([SelfGSTAmount],0)+ " +
                                "ISNULL([ElectricalGSTAmount],0)+ISNULL([ClutchGSTAmount],0)+ISNULL([AlternatorGSTAmount],0)+ISNULL([LeafGSTAmount],0)+ISNULL([SuspensionGSTAmount],0)+ISNULL([GearBoxGSTAmount],0)+ " +
                                "ISNULL([BreakGSTAmount],0)+ISNULL([EngineGSTAmount],0)+ISNULL([FuelGSTAmount],0)+ISNULL([PuncherGSTAmount],0)+ISNULL([OilGSTAmount],0)+ " +
                                "ISNULL([RadiatorGSTAmount],0)+ISNULL([AxleGSTAmount],0)+ ISNULL([DifferentialGSTAmount],0)+ISNULL([TurboGSTAmount],0)+ISNULL([EcmGSTAmount],0)+ISNULL([AccidentalGSTAmount],0) " +
                                ") As TotalGSTValue, " +
                                "SUM( " +
                                "[TyreTotalAmount]+[BatteryTotalAmount]+[RoutineTotalAmount]+[DentingTotalAmount]+[MinorTotalAmount]+[SeatTotalAmount]+[SelfTotalAmount]+ " +
                                "[ElectricalTotalAmount]+[ClutchTotalAmount]+[AlternatorTotalAmount]+[LeafTotalAmount]+[SuspensionTotalAmount]+[GearBoxTotalAmount]+ " +
                                "[BreakTotalAmount]+[EngineTotalAmount]+[FuelTotalAmount]+[PuncherTotalAmount]+[OilTotalAmount]+[RadiatorTotalAmount]+[AxleTotalAmount]+ " +
                                "[DifferentialTotalAmount]+[TurboTotalAmount]+[EcmTotalAmount]+[AccidentalGrossTotalAmount] " +
                                ") As TotalFinalAmount , " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.TyreInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as TyreInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.BatteryInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as BatteryInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.RoutineInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as RoutineInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.DentingInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as DentingInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.MinorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as MinorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SeatInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SeatInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SelfInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SelfInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.ElectricalInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as ElectricalInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.ClutchInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as ClutchInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AlternatorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AlternatorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.LeafInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as LeafInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SuspensionInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SuspensionInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.GearBoxInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as GearBoxInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.BreakWorkInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as BreakWorkInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.EngineWorkInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as EngineWorkInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.FuelInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as FuelInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.PuncherInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as PuncherInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.OilInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as OilInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.RadiatorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as RadiatorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AxleInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AxleInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.DifferentialInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as DifferentialInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.TurboInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as TurboInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.EcmInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as EcmInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AccidentalInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AccidentalInvoiceNo " +
                                " " +
                                "FROM  " +
                                "(  " +
                                "SELECT FRD.RegionName,FBD.BranchName,FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,  " +
                                "SUM(FSMSS.NoOfTyres) As NoOfTyres,SUM(FSMSS.TyreAmount) As TyreAmount,  " +
                                "SUM(FSGSO.[TyreGSTAmount]) As TyreGSTAmount, SUM(FSGSO.[TyreTotalAmount]) As [TyreTotalAmount],  " +
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,  " +
                                "SUM(FSGSO.[BatteryGSTAmount]) As BatteryGSTAmount, SUM(FSGSO.[BatteryTotalAmount]) As BatteryTotalAmount,  " +
                                "SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSGSO.[RoutineGSTAmount]) As RoutineGSTAmount, SUM(FSGSO.[RoutineTotalAmount]) As RoutineTotalAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,  " +
                                "SUM(FSGSO.[DentingGSTAmount]) As DentingGSTAmount, SUM(FSGSO.[DentingTotalAmount]) As DentingTotalAmount,  " +
                                "SUM(FSMRR.MinorAmount) As MinorAmount,    " +
                                "SUM(FSGSO.[MinorGSTAmount]) As MinorGSTAmount, SUM(FSGSO.[MinorTotalAmount]) As MinorTotalAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount,  " +
                                "SUM(FSGSO.[SeatGSTAmount]) As SeatGSTAmount, SUM(FSGSO.[SeatTotalAmount]) As SeatTotalAmount,  " +
                                "SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSGSO.[SelfGSTAmount]) As SelfGSTAmount, SUM(FSGSO.[SelfTotalAmount]) As SelfTotalAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount,  " +
                                "SUM(FSGSO.[ElectricalGSTAmount]) As ElectricalGSTAmount, SUM(FSGSO.[ElectricalTotalAmount]) As ElectricalTotalAmount,  " +
                                "SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSGST.[ClutchGSTAmount]) As ClutchGSTAmount, SUM(FSGST.[ClutchTotalAmount]) As ClutchTotalAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount,   " +
                                "SUM(FSGST.[AlternatorGSTAmount]) As AlternatorGSTAmount, SUM(FSGST.[AlternatorTotalAmount]) As AlternatorTotalAmount,  " +
                                "SUM(FSMR.LeafAmount) As LeafAmount,    " +
                                "SUM(FSGST.[LeafGSTAmount]) As LeafGSTAmount, SUM(FSGST.[LeafTotalAmount]) As LeafTotalAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount,   " +
                                "SUM(FSGST.[SuspensionGSTAmount]) As SuspensionGSTAmount, SUM(FSGST.[SuspensionTotalAmount]) As SuspensionTotalAmount,  " +
                                "SUM(FSWS.GearBoxAmount) As GearBoxAmount,    " +
                                "SUM(FSGST.[GearBoxGSTAmount]) As GearBoxGSTAmount, SUM(FSGST.[GearBoxTotalAmount]) As GearBoxTotalAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount,   " +
                                "SUM(FSGST.[BreakGSTAmount]) As BreakGSTAmount, SUM(FSGST.[BreakTotalAmount]) As BreakTotalAmount,  " +
                                "SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,    " +
                                "SUM(FSGST.[EngineGSTAmount]) As EngineGSTAmount, SUM(FSGST.[EngineTotalAmount]) As EngineTotalAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,  " +
                                "SUM(FSGST.[FuelGSTAmount]) As FuelGSTAmount, SUM(FSGST.[FuelTotalAmount]) As FuelTotalAmount,  " +
                                "SUM(FSTS.PuncherAmount) As PuncherAmount,    " +
                                "SUM(FSGSTT.[PuncherGSTAmount]) As PuncherGSTAmount, SUM(FSGSTT.[PuncherTotalAmount]) As PuncherTotalAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,  " +
                                "SUM(FSGSTT.[OilGSTAmount]) As OilGSTAmount, SUM(FSGSTT.[OilTotalAmount]) As OilTotalAmount,  " +
                                "SUM(FSOS.RadiatorAmount) As RadiatorAmount,    " +
                                "SUM(FSGSTT.[RadiatorGSTAmount]) As RadiatorGSTAmount, SUM(FSGSTT.[RadiatorTotalAmount]) As RadiatorTotalAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount,   " +
                                "SUM(FSGSTT.[AxleGSTAmount]) As AxleGSTAmount, SUM(FSGSTT.[AxleTotalAmount]) As AxleTotalAmount,  " +
                                "SUM(FSOS.DifferentialAmount) As DifferentialAmount,    " +
                                "SUM(FSGSTT.[DifferentialGSTAmount]) As DifferentialGSTAmount, SUM(FSGSTT.[DifferentialTotalAmount]) As DifferentialTotalAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount,   " +
                                "SUM(FSGSTT.[TurboGSTAmount]) As TurboGSTAmount, SUM(FSGSTT.[TurboTotalAmount]) As TurboTotalAmount,  " +
                                "SUM(FSTSS.EcmAmount) AS EcmAmount,    " +
                                "SUM(FSGSTT.[EcmGSTAmount]) As EcmGSTAmount, SUM(FSGSTT.[EcmTotalAmount]) As EcmTotalAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount ,  " +
                                "SUM(FSGSTT.[AccidentalGSTAmount]) As AccidentalGSTAmount, SUM(FSGSTT.[AccidentalGrossTotalAmount]) As AccidentalGrossTotalAmount    " + 
                                " " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMR.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMRR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMRR.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMSS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON (FSMD.Rec_Id = FSOS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTSS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON (FSMD.Rec_Id = FSWS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON (FSMD.BranchId = FBD.BranchId)  " +
                                "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FSMD.RegionId = FRD.RegionId)  " +
                                "INNER JOIN (select * from (select *,ROW_NUMBER() over (partition by vehicleno order by [date] desc) rm from [Fleet_Vehicle_Details_Date] with(nolock)where [Date] >='2021-07-01' and [Date] <='2021-08-25') tbl where tbl.rm=1 ) FVD  ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSO.Rec_Id)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON (FSMD.Rec_Id = FSGST.Rec_Id)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSTT.Rec_Id)  " +
                                "WHERE SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  AND FSMD.RegionId =" + RegionId + " AND FSMD.BranchId =" + BranchID + " " +
                                "GROUP BY FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,FRD.RegionName,FBD.BranchName " +
                                ")TblData  " +
                                "GROUP BY RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                                "NoOfTyres,TyreAmount,BatteryAmount,RoutineAmount,DentingAmount,MinorAmount,  " +
                                "SeatAmount,SelfAmount,ElectricalAmount,ClutchAmount,AlternatorAmount,LeafAmount,  " +
                                "SuspensionAmount,GearBoxAmount,BreakWorkAmount,EngineWorkAmount,  " +
                                "FuelAmount,PuncherAmount,OilAmount,RadiatorAmount,AxleAmount,DifferentialAmount,  " +
                                "TurboAmount,EcmAmount,AccidentalTotalAmount " +
                                ")tbl ";
                                  
                }
                if (FromDate != null && ToDate != null && RegionID != "" && BranchID != "" && VechileNumber != "")
                {
                    query1 = "SELECT RegionName,BranchName,VechileNumber,Make,Manufacturing_Year, " +
                                "NoOfTyres,TyreAmount,Battery,Routine,[Denting&Painting],MinorRepairing,  " +
                                "SeatRepair,SelfWork,ElectricalWork,ClutchRepairing,Alternator,[Leaf/PattiSpring],  " +
                                "Suspension,GearBox,BreakWork,EngineWork,  " +
                                "FuelPump,Puncher,OilTopUp,RadiatorandWaterBody,AxleWork,DifferentialWork,  " +
                                "Turbo,[Ecm/Sencer],Accidental,TotalValue, TotalGSTValue,(TotalValue + TotalGSTValue) As TotalFinalAmount, " +
                                "CONCAT('TyreInvoiceNo :-',TyreInvoiceNo,'|BatteryInvoiceNo :-',BatteryInvoiceNo,'|RoutineInvoiceNo :-',RoutineInvoiceNo,  " +
                                "'|DentingInvoiceNo :-',DentingInvoiceNo,'|MinorInvoiceNo :-',MinorInvoiceNo  " +
                                ",'|SeatInvoiceNo :-',SeatInvoiceNo,'|SelfInvoiceNo :-',SelfInvoiceNo,'|ElectricalInvoiceNo :-',  " +
                                "ElectricalInvoiceNo,'|ClutchInvoiceNo :-',ClutchInvoiceNo,'|AlternatorInvoiceNo :-',AlternatorInvoiceNo,  " +
                                "'|LeafInvoiceNo :-',LeafInvoiceNo,'|SuspensionInvoiceNo :-',SuspensionInvoiceNo,'|GearBoxInvoiceNo :-',  " +
                                "GearBoxInvoiceNo,'|BreakWorkInvoiceNo :-',BreakWorkInvoiceNo,'|EngineWorkInvoiceNo:-',EngineWorkInvoiceNo,'|FuelInvoiceNo :-',  " +
                                "FuelInvoiceNo,'|PuncherInvoiceNo :-',PuncherInvoiceNo,'|OilInvoiceNo :-',OilInvoiceNo,'|RadiatorInvoiceNo :-',RadiatorInvoiceNo,  " +
                                "'|AxleInvoiceNo :-',AxleInvoiceNo,'|DifferentialInvoiceNo:-',DifferentialInvoiceNo  " +
                                ",'|TurboInvoiceNo:-',TurboInvoiceNo,'|EcmInvoiceNo:-',EcmInvoiceNo,'|AccidentalInvoiceNo:-',AccidentalInvoiceNo) As InvoiceNo  " +
                                "FROM " +
                                "( " +
                                "SELECT RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                                "NoOfTyres ,TyreAmount ,BatteryAmount As Battery,RoutineAmount As Routine,DentingAmount As [Denting&Painting],MinorAmount As MinorRepairing,  " +
                                " SeatAmount As SeatRepair,SelfAmount As SelfWork,ElectricalAmount As ElectricalWork,ClutchAmount As ClutchRepairing,AlternatorAmount As Alternator,LeafAmount As [Leaf/PattiSpring],  " +
                                " SuspensionAmount As Suspension,GearBoxAmount As GearBox,BreakWorkAmount As BreakWork,EngineWorkAmount As EngineWork,  " +
                                " FuelAmount As FuelPump,PuncherAmount As Puncher,OilAmount As OilTopUp,RadiatorAmount As RadiatorandWaterBody,AxleAmount As AxleWork,DifferentialAmount As DifferentialWork,  " +
                                " TurboAmount As Turbo,EcmAmount As [Ecm/Sencer], AccidentalTotalAmount As Accidental,  " +
                                "SUM(TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                                "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                                "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                                "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                                "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
                               "SUM( " +
                                "ISNULL([TyreGSTAmount],0)+ISNULL([BatteryGSTAmount],0)+ISNULL([RoutineGSTAmount],0)+ISNULL([DentingGSTAmount],0)+ISNULL([MinorGSTAmount],0)+ISNULL([SeatGSTAmount],0)+ISNULL([SelfGSTAmount],0)+ " +
                                "ISNULL([ElectricalGSTAmount],0)+ISNULL([ClutchGSTAmount],0)+ISNULL([AlternatorGSTAmount],0)+ISNULL([LeafGSTAmount],0)+ISNULL([SuspensionGSTAmount],0)+ISNULL([GearBoxGSTAmount],0)+ " +
                                "ISNULL([BreakGSTAmount],0)+ISNULL([EngineGSTAmount],0)+ISNULL([FuelGSTAmount],0)+ISNULL([PuncherGSTAmount],0)+ISNULL([OilGSTAmount],0)+ " +
                                "ISNULL([RadiatorGSTAmount],0)+ISNULL([AxleGSTAmount],0)+ ISNULL([DifferentialGSTAmount],0)+ISNULL([TurboGSTAmount],0)+ISNULL([EcmGSTAmount],0)+ISNULL([AccidentalGSTAmount],0) " +
                                ") As TotalGSTValue, " +
                                "SUM( " +
                                "[TyreTotalAmount]+[BatteryTotalAmount]+[RoutineTotalAmount]+[DentingTotalAmount]+[MinorTotalAmount]+[SeatTotalAmount]+[SelfTotalAmount]+ " +
                                "[ElectricalTotalAmount]+[ClutchTotalAmount]+[AlternatorTotalAmount]+[LeafTotalAmount]+[SuspensionTotalAmount]+[GearBoxTotalAmount]+ " +
                                "[BreakTotalAmount]+[EngineTotalAmount]+[FuelTotalAmount]+[PuncherTotalAmount]+[OilTotalAmount]+[RadiatorTotalAmount]+[AxleTotalAmount]+ " +
                                "[DifferentialTotalAmount]+[TurboTotalAmount]+[EcmTotalAmount]+[AccidentalGrossTotalAmount] " +
                                ") As TotalFinalAmount , " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.TyreInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as TyreInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.BatteryInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as BatteryInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.RoutineInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as RoutineInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.DentingInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as DentingInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.MinorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as MinorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SeatInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SeatInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SelfInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SelfInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.ElectricalInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as ElectricalInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.ClutchInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as ClutchInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AlternatorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AlternatorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.LeafInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as LeafInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SuspensionInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SuspensionInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.GearBoxInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as GearBoxInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.BreakWorkInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as BreakWorkInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.EngineWorkInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as EngineWorkInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.FuelInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as FuelInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.PuncherInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as PuncherInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.OilInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as OilInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.RadiatorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as RadiatorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AxleInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AxleInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.DifferentialInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as DifferentialInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.TurboInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as TurboInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.EcmInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as EcmInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AccidentalInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AccidentalInvoiceNo " +
                                " " +
                                "FROM  " +
                                "(  " +
                                "SELECT FRD.RegionName,FBD.BranchName,FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,  " +
                                "SUM(FSMSS.NoOfTyres) As NoOfTyres,SUM(FSMSS.TyreAmount) As TyreAmount,  " +
                                "SUM(FSGSO.[TyreGSTAmount]) As TyreGSTAmount, SUM(FSGSO.[TyreTotalAmount]) As [TyreTotalAmount],  " +
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,  " +
                                "SUM(FSGSO.[BatteryGSTAmount]) As BatteryGSTAmount, SUM(FSGSO.[BatteryTotalAmount]) As BatteryTotalAmount,  " +
                                "SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSGSO.[RoutineGSTAmount]) As RoutineGSTAmount, SUM(FSGSO.[RoutineTotalAmount]) As RoutineTotalAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,  " +
                                "SUM(FSGSO.[DentingGSTAmount]) As DentingGSTAmount, SUM(FSGSO.[DentingTotalAmount]) As DentingTotalAmount,  " +
                                "SUM(FSMRR.MinorAmount) As MinorAmount,    " +
                                "SUM(FSGSO.[MinorGSTAmount]) As MinorGSTAmount, SUM(FSGSO.[MinorTotalAmount]) As MinorTotalAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount,  " +
                                "SUM(FSGSO.[SeatGSTAmount]) As SeatGSTAmount, SUM(FSGSO.[SeatTotalAmount]) As SeatTotalAmount,  " +
                                "SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSGSO.[SelfGSTAmount]) As SelfGSTAmount, SUM(FSGSO.[SelfTotalAmount]) As SelfTotalAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount,  " +
                                "SUM(FSGSO.[ElectricalGSTAmount]) As ElectricalGSTAmount, SUM(FSGSO.[ElectricalTotalAmount]) As ElectricalTotalAmount,  " +
                                "SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSGST.[ClutchGSTAmount]) As ClutchGSTAmount, SUM(FSGST.[ClutchTotalAmount]) As ClutchTotalAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount,   " +
                                "SUM(FSGST.[AlternatorGSTAmount]) As AlternatorGSTAmount, SUM(FSGST.[AlternatorTotalAmount]) As AlternatorTotalAmount,  " +
                                "SUM(FSMR.LeafAmount) As LeafAmount,    " +
                                "SUM(FSGST.[LeafGSTAmount]) As LeafGSTAmount, SUM(FSGST.[LeafTotalAmount]) As LeafTotalAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount,   " +
                                "SUM(FSGST.[SuspensionGSTAmount]) As SuspensionGSTAmount, SUM(FSGST.[SuspensionTotalAmount]) As SuspensionTotalAmount,  " +
                                "SUM(FSWS.GearBoxAmount) As GearBoxAmount,    " +
                                "SUM(FSGST.[GearBoxGSTAmount]) As GearBoxGSTAmount, SUM(FSGST.[GearBoxTotalAmount]) As GearBoxTotalAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount,   " +
                                "SUM(FSGST.[BreakGSTAmount]) As BreakGSTAmount, SUM(FSGST.[BreakTotalAmount]) As BreakTotalAmount,  " +
                                "SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,    " +
                                "SUM(FSGST.[EngineGSTAmount]) As EngineGSTAmount, SUM(FSGST.[EngineTotalAmount]) As EngineTotalAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,  " +
                                "SUM(FSGST.[FuelGSTAmount]) As FuelGSTAmount, SUM(FSGST.[FuelTotalAmount]) As FuelTotalAmount,  " +
                                "SUM(FSTS.PuncherAmount) As PuncherAmount,    " +
                                "SUM(FSGSTT.[PuncherGSTAmount]) As PuncherGSTAmount, SUM(FSGSTT.[PuncherTotalAmount]) As PuncherTotalAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,  " +
                                "SUM(FSGSTT.[OilGSTAmount]) As OilGSTAmount, SUM(FSGSTT.[OilTotalAmount]) As OilTotalAmount,  " +
                                "SUM(FSOS.RadiatorAmount) As RadiatorAmount,    " +
                                "SUM(FSGSTT.[RadiatorGSTAmount]) As RadiatorGSTAmount, SUM(FSGSTT.[RadiatorTotalAmount]) As RadiatorTotalAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount,   " +
                                "SUM(FSGSTT.[AxleGSTAmount]) As AxleGSTAmount, SUM(FSGSTT.[AxleTotalAmount]) As AxleTotalAmount,  " +
                                "SUM(FSOS.DifferentialAmount) As DifferentialAmount,    " +
                                "SUM(FSGSTT.[DifferentialGSTAmount]) As DifferentialGSTAmount, SUM(FSGSTT.[DifferentialTotalAmount]) As DifferentialTotalAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount,   " +
                                "SUM(FSGSTT.[TurboGSTAmount]) As TurboGSTAmount, SUM(FSGSTT.[TurboTotalAmount]) As TurboTotalAmount,  " +
                                "SUM(FSTSS.EcmAmount) AS EcmAmount,    " +
                                "SUM(FSGSTT.[EcmGSTAmount]) As EcmGSTAmount, SUM(FSGSTT.[EcmTotalAmount]) As EcmTotalAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount ,  " +
                                "SUM(FSGSTT.[AccidentalGSTAmount]) As AccidentalGSTAmount, SUM(FSGSTT.[AccidentalGrossTotalAmount]) As AccidentalGrossTotalAmount    " + 
                                " " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMR.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMRR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMRR.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMSS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON (FSMD.Rec_Id = FSOS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTSS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON (FSMD.Rec_Id = FSWS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON (FSMD.BranchId = FBD.BranchId)  " +
                                "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FSMD.RegionId = FRD.RegionId)  " +
                                "INNER JOIN (select * from (select *,ROW_NUMBER() over (partition by vehicleno order by [date] desc) rm from [Fleet_Vehicle_Details_Date] with(nolock)where [Date] >='2021-07-01' and [Date] <='2021-08-25') tbl where tbl.rm=1 ) FVD  ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSO.Rec_Id)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON (FSMD.Rec_Id = FSGST.Rec_Id)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSTT.Rec_Id)  " +
                                "WHERE SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "' AND FSMD.RegionId =" + RegionId + " AND FSMD.BranchId =" + BranchID + " AND FSMD.VechileNumber IN (" + VechileNo + ") " +
                                "GROUP BY FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,FRD.RegionName,FBD.BranchName " +
                                ")TblData  " +
                                "GROUP BY RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                                "NoOfTyres,TyreAmount,BatteryAmount,RoutineAmount,DentingAmount,MinorAmount,  " +
                                "SeatAmount,SelfAmount,ElectricalAmount,ClutchAmount,AlternatorAmount,LeafAmount,  " +
                                "SuspensionAmount,GearBoxAmount,BreakWorkAmount,EngineWorkAmount,  " +
                                "FuelAmount,PuncherAmount,OilAmount,RadiatorAmount,AxleAmount,DifferentialAmount,  " +
                                "TurboAmount,EcmAmount,AccidentalTotalAmount " +
                                ")tbl ";

                }
                if (FromDate != null && ToDate != null && RegionID == "" && BranchID == "" && VechileNumber != "")
                {
                    query1 = "SELECT RegionName,BranchName,VechileNumber,Make,Manufacturing_Year, " +
                                "NoOfTyres,TyreAmount,Battery,Routine,[Denting&Painting],MinorRepairing,  " +
                                "SeatRepair,SelfWork,ElectricalWork,ClutchRepairing,Alternator,[Leaf/PattiSpring],  " +
                                "Suspension,GearBox,BreakWork,EngineWork,  " +
                                "FuelPump,Puncher,OilTopUp,RadiatorandWaterBody,AxleWork,DifferentialWork,  " +
                                "Turbo,[Ecm/Sencer],Accidental,TotalValue, TotalGSTValue,(TotalValue + TotalGSTValue) As TotalFinalAmount,  " +
                                "CONCAT('TyreInvoiceNo :-',TyreInvoiceNo,'|BatteryInvoiceNo :-',BatteryInvoiceNo,'|RoutineInvoiceNo :-',RoutineInvoiceNo,  " +
                                "'|DentingInvoiceNo :-',DentingInvoiceNo,'|MinorInvoiceNo :-',MinorInvoiceNo  " +
                                ",'|SeatInvoiceNo :-',SeatInvoiceNo,'|SelfInvoiceNo :-',SelfInvoiceNo,'|ElectricalInvoiceNo :-',  " +
                                "ElectricalInvoiceNo,'|ClutchInvoiceNo :-',ClutchInvoiceNo,'|AlternatorInvoiceNo :-',AlternatorInvoiceNo,  " +
                                "'|LeafInvoiceNo :-',LeafInvoiceNo,'|SuspensionInvoiceNo :-',SuspensionInvoiceNo,'|GearBoxInvoiceNo :-',  " +
                                "GearBoxInvoiceNo,'|BreakWorkInvoiceNo :-',BreakWorkInvoiceNo,'|EngineWorkInvoiceNo:-',EngineWorkInvoiceNo,'|FuelInvoiceNo :-',  " +
                                "FuelInvoiceNo,'|PuncherInvoiceNo :-',PuncherInvoiceNo,'|OilInvoiceNo :-',OilInvoiceNo,'|RadiatorInvoiceNo :-',RadiatorInvoiceNo,  " +
                                "'|AxleInvoiceNo :-',AxleInvoiceNo,'|DifferentialInvoiceNo:-',DifferentialInvoiceNo  " +
                                ",'|TurboInvoiceNo:-',TurboInvoiceNo,'|EcmInvoiceNo:-',EcmInvoiceNo,'|AccidentalInvoiceNo:-',AccidentalInvoiceNo) As InvoiceNo  " +
                                "FROM " +
                                "( " +
                                "SELECT RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                                "NoOfTyres ,TyreAmount ,BatteryAmount As Battery,RoutineAmount As Routine,DentingAmount As [Denting&Painting],MinorAmount As MinorRepairing,  " +
                                " SeatAmount As SeatRepair,SelfAmount As SelfWork,ElectricalAmount As ElectricalWork,ClutchAmount As ClutchRepairing,AlternatorAmount As Alternator,LeafAmount As [Leaf/PattiSpring],  " +
                                " SuspensionAmount As Suspension,GearBoxAmount As GearBox,BreakWorkAmount As BreakWork,EngineWorkAmount As EngineWork,  " +
                                " FuelAmount As FuelPump,PuncherAmount As Puncher,OilAmount As OilTopUp,RadiatorAmount As RadiatorandWaterBody,AxleAmount As AxleWork,DifferentialAmount As DifferentialWork,  " +
                                " TurboAmount As Turbo,EcmAmount As [Ecm/Sencer], AccidentalTotalAmount As Accidental,  " +
                                "SUM(TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                                "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                                "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                                "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                                "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
                                "SUM( " +
                                "ISNULL([TyreGSTAmount],0)+ISNULL([BatteryGSTAmount],0)+ISNULL([RoutineGSTAmount],0)+ISNULL([DentingGSTAmount],0)+ISNULL([MinorGSTAmount],0)+ISNULL([SeatGSTAmount],0)+ISNULL([SelfGSTAmount],0)+ " +
                                "ISNULL([ElectricalGSTAmount],0)+ISNULL([ClutchGSTAmount],0)+ISNULL([AlternatorGSTAmount],0)+ISNULL([LeafGSTAmount],0)+ISNULL([SuspensionGSTAmount],0)+ISNULL([GearBoxGSTAmount],0)+ " +
                                "ISNULL([BreakGSTAmount],0)+ISNULL([EngineGSTAmount],0)+ISNULL([FuelGSTAmount],0)+ISNULL([PuncherGSTAmount],0)+ISNULL([OilGSTAmount],0)+ " +
                                "ISNULL([RadiatorGSTAmount],0)+ISNULL([AxleGSTAmount],0)+ ISNULL([DifferentialGSTAmount],0)+ISNULL([TurboGSTAmount],0)+ISNULL([EcmGSTAmount],0)+ISNULL([AccidentalGSTAmount],0) " +
                                ") As TotalGSTValue, " +
                                "SUM( " +
                                "[TyreTotalAmount]+[BatteryTotalAmount]+[RoutineTotalAmount]+[DentingTotalAmount]+[MinorTotalAmount]+[SeatTotalAmount]+[SelfTotalAmount]+ " +
                                "[ElectricalTotalAmount]+[ClutchTotalAmount]+[AlternatorTotalAmount]+[LeafTotalAmount]+[SuspensionTotalAmount]+[GearBoxTotalAmount]+ " +
                                "[BreakTotalAmount]+[EngineTotalAmount]+[FuelTotalAmount]+[PuncherTotalAmount]+[OilTotalAmount]+[RadiatorTotalAmount]+[AxleTotalAmount]+ " +
                                "[DifferentialTotalAmount]+[TurboTotalAmount]+[EcmTotalAmount]+[AccidentalGrossTotalAmount] " +
                                ") As TotalFinalAmount , " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.TyreInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as TyreInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.BatteryInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as BatteryInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.RoutineInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as RoutineInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.DentingInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as DentingInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.MinorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as MinorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SeatInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SeatInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SelfInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SelfInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.ElectricalInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as ElectricalInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.ClutchInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as ClutchInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AlternatorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AlternatorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.LeafInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as LeafInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SuspensionInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SuspensionInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.GearBoxInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as GearBoxInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.BreakWorkInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as BreakWorkInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.EngineWorkInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as EngineWorkInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.FuelInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as FuelInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.PuncherInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as PuncherInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.OilInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as OilInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.RadiatorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as RadiatorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AxleInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AxleInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.DifferentialInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as DifferentialInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.TurboInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as TurboInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.EcmInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as EcmInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AccidentalInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AccidentalInvoiceNo " +
                                " " +
                                "FROM  " +
                                "(  " +
                                "SELECT FRD.RegionName,FBD.BranchName,FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,  " +
                                "SUM(FSMSS.NoOfTyres) As NoOfTyres,SUM(FSMSS.TyreAmount) As TyreAmount,  " +
                                "SUM(FSGSO.[TyreGSTAmount]) As TyreGSTAmount, SUM(FSGSO.[TyreTotalAmount]) As [TyreTotalAmount],  " +
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,  " +
                                "SUM(FSGSO.[BatteryGSTAmount]) As BatteryGSTAmount, SUM(FSGSO.[BatteryTotalAmount]) As BatteryTotalAmount,  " +
                                "SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSGSO.[RoutineGSTAmount]) As RoutineGSTAmount, SUM(FSGSO.[RoutineTotalAmount]) As RoutineTotalAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,  " +
                                "SUM(FSGSO.[DentingGSTAmount]) As DentingGSTAmount, SUM(FSGSO.[DentingTotalAmount]) As DentingTotalAmount,  " +
                                "SUM(FSMRR.MinorAmount) As MinorAmount,    " +
                                "SUM(FSGSO.[MinorGSTAmount]) As MinorGSTAmount, SUM(FSGSO.[MinorTotalAmount]) As MinorTotalAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount,  " +
                                "SUM(FSGSO.[SeatGSTAmount]) As SeatGSTAmount, SUM(FSGSO.[SeatTotalAmount]) As SeatTotalAmount,  " +
                                "SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSGSO.[SelfGSTAmount]) As SelfGSTAmount, SUM(FSGSO.[SelfTotalAmount]) As SelfTotalAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount,  " +
                                "SUM(FSGSO.[ElectricalGSTAmount]) As ElectricalGSTAmount, SUM(FSGSO.[ElectricalTotalAmount]) As ElectricalTotalAmount,  " +
                                "SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSGST.[ClutchGSTAmount]) As ClutchGSTAmount, SUM(FSGST.[ClutchTotalAmount]) As ClutchTotalAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount,   " +
                                "SUM(FSGST.[AlternatorGSTAmount]) As AlternatorGSTAmount, SUM(FSGST.[AlternatorTotalAmount]) As AlternatorTotalAmount,  " +
                                "SUM(FSMR.LeafAmount) As LeafAmount,    " +
                                "SUM(FSGST.[LeafGSTAmount]) As LeafGSTAmount, SUM(FSGST.[LeafTotalAmount]) As LeafTotalAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount,   " +
                                "SUM(FSGST.[SuspensionGSTAmount]) As SuspensionGSTAmount, SUM(FSGST.[SuspensionTotalAmount]) As SuspensionTotalAmount,  " +
                                "SUM(FSWS.GearBoxAmount) As GearBoxAmount,    " +
                                "SUM(FSGST.[GearBoxGSTAmount]) As GearBoxGSTAmount, SUM(FSGST.[GearBoxTotalAmount]) As GearBoxTotalAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount,   " +
                                "SUM(FSGST.[BreakGSTAmount]) As BreakGSTAmount, SUM(FSGST.[BreakTotalAmount]) As BreakTotalAmount,  " +
                                "SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,    " +
                                "SUM(FSGST.[EngineGSTAmount]) As EngineGSTAmount, SUM(FSGST.[EngineTotalAmount]) As EngineTotalAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,  " +
                                "SUM(FSGST.[FuelGSTAmount]) As FuelGSTAmount, SUM(FSGST.[FuelTotalAmount]) As FuelTotalAmount,  " +
                                "SUM(FSTS.PuncherAmount) As PuncherAmount,    " +
                                "SUM(FSGSTT.[PuncherGSTAmount]) As PuncherGSTAmount, SUM(FSGSTT.[PuncherTotalAmount]) As PuncherTotalAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,  " +
                                "SUM(FSGSTT.[OilGSTAmount]) As OilGSTAmount, SUM(FSGSTT.[OilTotalAmount]) As OilTotalAmount,  " +
                                "SUM(FSOS.RadiatorAmount) As RadiatorAmount,    " +
                                "SUM(FSGSTT.[RadiatorGSTAmount]) As RadiatorGSTAmount, SUM(FSGSTT.[RadiatorTotalAmount]) As RadiatorTotalAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount,   " +
                                "SUM(FSGSTT.[AxleGSTAmount]) As AxleGSTAmount, SUM(FSGSTT.[AxleTotalAmount]) As AxleTotalAmount,  " +
                                "SUM(FSOS.DifferentialAmount) As DifferentialAmount,    " +
                                "SUM(FSGSTT.[DifferentialGSTAmount]) As DifferentialGSTAmount, SUM(FSGSTT.[DifferentialTotalAmount]) As DifferentialTotalAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount,   " +
                                "SUM(FSGSTT.[TurboGSTAmount]) As TurboGSTAmount, SUM(FSGSTT.[TurboTotalAmount]) As TurboTotalAmount,  " +
                                "SUM(FSTSS.EcmAmount) AS EcmAmount,    " +
                                "SUM(FSGSTT.[EcmGSTAmount]) As EcmGSTAmount, SUM(FSGSTT.[EcmTotalAmount]) As EcmTotalAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount ,  " +
                                "SUM(FSGSTT.[AccidentalGSTAmount]) As AccidentalGSTAmount, SUM(FSGSTT.[AccidentalGrossTotalAmount]) As AccidentalGrossTotalAmount    " + 
                                " " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMR.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMRR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMRR.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMSS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON (FSMD.Rec_Id = FSOS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTSS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON (FSMD.Rec_Id = FSWS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON (FSMD.BranchId = FBD.BranchId)  " +
                                "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FSMD.RegionId = FRD.RegionId)  " +
                                "INNER JOIN (select * from (select *,ROW_NUMBER() over (partition by vehicleno order by [date] desc) rm from [Fleet_Vehicle_Details_Date] with(nolock)where [Date] >='2021-07-01' and [Date] <='2021-08-25') tbl where tbl.rm=1 ) FVD  ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSO.Rec_Id)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON (FSMD.Rec_Id = FSGST.Rec_Id)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSTT.Rec_Id)  " +
                                "WHERE SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "' AND FSMD.VechileNumber IN (" + VechileNo + ") " +
                                "GROUP BY FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,FRD.RegionName,FBD.BranchName " +
                                ")TblData  " +
                                "GROUP BY RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                                "NoOfTyres,TyreAmount,BatteryAmount,RoutineAmount,DentingAmount,MinorAmount,  " +
                                "SeatAmount,SelfAmount,ElectricalAmount,ClutchAmount,AlternatorAmount,LeafAmount,  " +
                                "SuspensionAmount,GearBoxAmount,BreakWorkAmount,EngineWorkAmount,  " +
                                "FuelAmount,PuncherAmount,OilAmount,RadiatorAmount,AxleAmount,DifferentialAmount,  " +
                                "TurboAmount,EcmAmount,AccidentalTotalAmount " +
                                ")tbl ";
                                
                }
                if (FromDate != null && ToDate != null && RegionID != "" && BranchID != "" && Vechile != "" && VechileNumber == "")
                {
                    query1 = "SELECT RegionName,BranchName,VechileNumber,Make,Manufacturing_Year, " +
                                "NoOfTyres,TyreAmount,Battery,Routine,[Denting&Painting],MinorRepairing,  " +
                                "SeatRepair,SelfWork,ElectricalWork,ClutchRepairing,Alternator,[Leaf/PattiSpring],  " +
                                "Suspension,GearBox,BreakWork,EngineWork,  " +
                                "FuelPump,Puncher,OilTopUp,RadiatorandWaterBody,AxleWork,DifferentialWork,  " +
                                "Turbo,[Ecm/Sencer],Accidental,TotalValue, TotalGSTValue,(TotalValue + TotalGSTValue) As TotalFinalAmount,  " +
                                "CONCAT('TyreInvoiceNo :-',TyreInvoiceNo,'|BatteryInvoiceNo :-',BatteryInvoiceNo,'|RoutineInvoiceNo :-',RoutineInvoiceNo,  " +
                                "'|DentingInvoiceNo :-',DentingInvoiceNo,'|MinorInvoiceNo :-',MinorInvoiceNo  " +
                                ",'|SeatInvoiceNo :-',SeatInvoiceNo,'|SelfInvoiceNo :-',SelfInvoiceNo,'|ElectricalInvoiceNo :-',  " +
                                "ElectricalInvoiceNo,'|ClutchInvoiceNo :-',ClutchInvoiceNo,'|AlternatorInvoiceNo :-',AlternatorInvoiceNo,  " +
                                "'|LeafInvoiceNo :-',LeafInvoiceNo,'|SuspensionInvoiceNo :-',SuspensionInvoiceNo,'|GearBoxInvoiceNo :-',  " +
                                "GearBoxInvoiceNo,'|BreakWorkInvoiceNo :-',BreakWorkInvoiceNo,'|EngineWorkInvoiceNo:-',EngineWorkInvoiceNo,'|FuelInvoiceNo :-',  " +
                                "FuelInvoiceNo,'|PuncherInvoiceNo :-',PuncherInvoiceNo,'|OilInvoiceNo :-',OilInvoiceNo,'|RadiatorInvoiceNo :-',RadiatorInvoiceNo,  " +
                                "'|AxleInvoiceNo :-',AxleInvoiceNo,'|DifferentialInvoiceNo:-',DifferentialInvoiceNo  " +
                                ",'|TurboInvoiceNo:-',TurboInvoiceNo,'|EcmInvoiceNo:-',EcmInvoiceNo,'|AccidentalInvoiceNo:-',AccidentalInvoiceNo) As InvoiceNo  " +
                                "FROM " +
                                "( " +
                                "SELECT RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                                "NoOfTyres ,TyreAmount ,BatteryAmount As Battery,RoutineAmount As Routine,DentingAmount As [Denting&Painting],MinorAmount As MinorRepairing,  " +
                                " SeatAmount As SeatRepair,SelfAmount As SelfWork,ElectricalAmount As ElectricalWork,ClutchAmount As ClutchRepairing,AlternatorAmount As Alternator,LeafAmount As [Leaf/PattiSpring],  " +
                                " SuspensionAmount As Suspension,GearBoxAmount As GearBox,BreakWorkAmount As BreakWork,EngineWorkAmount As EngineWork,  " +
                                " FuelAmount As FuelPump,PuncherAmount As Puncher,OilAmount As OilTopUp,RadiatorAmount As RadiatorandWaterBody,AxleAmount As AxleWork,DifferentialAmount As DifferentialWork,  " +
                                " TurboAmount As Turbo,EcmAmount As [Ecm/Sencer], AccidentalTotalAmount As Accidental,  " +
                                "SUM(TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                                "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                                "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                                "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                                "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
                                "SUM( " +
                                "ISNULL([TyreGSTAmount],0)+ISNULL([BatteryGSTAmount],0)+ISNULL([RoutineGSTAmount],0)+ISNULL([DentingGSTAmount],0)+ISNULL([MinorGSTAmount],0)+ISNULL([SeatGSTAmount],0)+ISNULL([SelfGSTAmount],0)+ " +
                                "ISNULL([ElectricalGSTAmount],0)+ISNULL([ClutchGSTAmount],0)+ISNULL([AlternatorGSTAmount],0)+ISNULL([LeafGSTAmount],0)+ISNULL([SuspensionGSTAmount],0)+ISNULL([GearBoxGSTAmount],0)+ " +
                                "ISNULL([BreakGSTAmount],0)+ISNULL([EngineGSTAmount],0)+ISNULL([FuelGSTAmount],0)+ISNULL([PuncherGSTAmount],0)+ISNULL([OilGSTAmount],0)+ " +
                                "ISNULL([RadiatorGSTAmount],0)+ISNULL([AxleGSTAmount],0)+ ISNULL([DifferentialGSTAmount],0)+ISNULL([TurboGSTAmount],0)+ISNULL([EcmGSTAmount],0)+ISNULL([AccidentalGSTAmount],0) " +
                                ") As TotalGSTValue, " +
                                "SUM( " +
                                "[TyreTotalAmount]+[BatteryTotalAmount]+[RoutineTotalAmount]+[DentingTotalAmount]+[MinorTotalAmount]+[SeatTotalAmount]+[SelfTotalAmount]+ " +
                                "[ElectricalTotalAmount]+[ClutchTotalAmount]+[AlternatorTotalAmount]+[LeafTotalAmount]+[SuspensionTotalAmount]+[GearBoxTotalAmount]+ " +
                                "[BreakTotalAmount]+[EngineTotalAmount]+[FuelTotalAmount]+[PuncherTotalAmount]+[OilTotalAmount]+[RadiatorTotalAmount]+[AxleTotalAmount]+ " +
                                "[DifferentialTotalAmount]+[TurboTotalAmount]+[EcmTotalAmount]+[AccidentalGrossTotalAmount] " +
                                ") As TotalFinalAmount , " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.TyreInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as TyreInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.BatteryInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as BatteryInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.RoutineInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as RoutineInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.DentingInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as DentingInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.MinorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as MinorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SeatInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SeatInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SelfInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SelfInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.ElectricalInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as ElectricalInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.ClutchInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as ClutchInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AlternatorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AlternatorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.LeafInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as LeafInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SuspensionInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SuspensionInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.GearBoxInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as GearBoxInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.BreakWorkInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as BreakWorkInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.EngineWorkInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as EngineWorkInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.FuelInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as FuelInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.PuncherInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as PuncherInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.OilInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as OilInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.RadiatorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as RadiatorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AxleInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AxleInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.DifferentialInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as DifferentialInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.TurboInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as TurboInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.EcmInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as EcmInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AccidentalInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AccidentalInvoiceNo " +
                                " " +
                                "FROM  " +
                                "(  " +
                                "SELECT FRD.RegionName,FBD.BranchName,FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,  " +
                                "SUM(FSMSS.NoOfTyres) As NoOfTyres,SUM(FSMSS.TyreAmount) As TyreAmount,  " +
                                "SUM(FSGSO.[TyreGSTAmount]) As TyreGSTAmount, SUM(FSGSO.[TyreTotalAmount]) As [TyreTotalAmount],  " +
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,  " +
                                "SUM(FSGSO.[BatteryGSTAmount]) As BatteryGSTAmount, SUM(FSGSO.[BatteryTotalAmount]) As BatteryTotalAmount,  " +
                                "SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSGSO.[RoutineGSTAmount]) As RoutineGSTAmount, SUM(FSGSO.[RoutineTotalAmount]) As RoutineTotalAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,  " +
                                "SUM(FSGSO.[DentingGSTAmount]) As DentingGSTAmount, SUM(FSGSO.[DentingTotalAmount]) As DentingTotalAmount,  " +
                                "SUM(FSMRR.MinorAmount) As MinorAmount,    " +
                                "SUM(FSGSO.[MinorGSTAmount]) As MinorGSTAmount, SUM(FSGSO.[MinorTotalAmount]) As MinorTotalAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount,  " +
                                "SUM(FSGSO.[SeatGSTAmount]) As SeatGSTAmount, SUM(FSGSO.[SeatTotalAmount]) As SeatTotalAmount,  " +
                                "SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSGSO.[SelfGSTAmount]) As SelfGSTAmount, SUM(FSGSO.[SelfTotalAmount]) As SelfTotalAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount,  " +
                                "SUM(FSGSO.[ElectricalGSTAmount]) As ElectricalGSTAmount, SUM(FSGSO.[ElectricalTotalAmount]) As ElectricalTotalAmount,  " +
                                "SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSGST.[ClutchGSTAmount]) As ClutchGSTAmount, SUM(FSGST.[ClutchTotalAmount]) As ClutchTotalAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount,   " +
                                "SUM(FSGST.[AlternatorGSTAmount]) As AlternatorGSTAmount, SUM(FSGST.[AlternatorTotalAmount]) As AlternatorTotalAmount,  " +
                                "SUM(FSMR.LeafAmount) As LeafAmount,    " +
                                "SUM(FSGST.[LeafGSTAmount]) As LeafGSTAmount, SUM(FSGST.[LeafTotalAmount]) As LeafTotalAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount,   " +
                                "SUM(FSGST.[SuspensionGSTAmount]) As SuspensionGSTAmount, SUM(FSGST.[SuspensionTotalAmount]) As SuspensionTotalAmount,  " +
                                "SUM(FSWS.GearBoxAmount) As GearBoxAmount,    " +
                                "SUM(FSGST.[GearBoxGSTAmount]) As GearBoxGSTAmount, SUM(FSGST.[GearBoxTotalAmount]) As GearBoxTotalAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount,   " +
                                "SUM(FSGST.[BreakGSTAmount]) As BreakGSTAmount, SUM(FSGST.[BreakTotalAmount]) As BreakTotalAmount,  " +
                                "SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,    " +
                                "SUM(FSGST.[EngineGSTAmount]) As EngineGSTAmount, SUM(FSGST.[EngineTotalAmount]) As EngineTotalAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,  " +
                                "SUM(FSGST.[FuelGSTAmount]) As FuelGSTAmount, SUM(FSGST.[FuelTotalAmount]) As FuelTotalAmount,  " +
                                "SUM(FSTS.PuncherAmount) As PuncherAmount,    " +
                                "SUM(FSGSTT.[PuncherGSTAmount]) As PuncherGSTAmount, SUM(FSGSTT.[PuncherTotalAmount]) As PuncherTotalAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,  " +
                                "SUM(FSGSTT.[OilGSTAmount]) As OilGSTAmount, SUM(FSGSTT.[OilTotalAmount]) As OilTotalAmount,  " +
                                "SUM(FSOS.RadiatorAmount) As RadiatorAmount,    " +
                                "SUM(FSGSTT.[RadiatorGSTAmount]) As RadiatorGSTAmount, SUM(FSGSTT.[RadiatorTotalAmount]) As RadiatorTotalAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount,   " +
                                "SUM(FSGSTT.[AxleGSTAmount]) As AxleGSTAmount, SUM(FSGSTT.[AxleTotalAmount]) As AxleTotalAmount,  " +
                                "SUM(FSOS.DifferentialAmount) As DifferentialAmount,    " +
                                "SUM(FSGSTT.[DifferentialGSTAmount]) As DifferentialGSTAmount, SUM(FSGSTT.[DifferentialTotalAmount]) As DifferentialTotalAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount,   " +
                                "SUM(FSGSTT.[TurboGSTAmount]) As TurboGSTAmount, SUM(FSGSTT.[TurboTotalAmount]) As TurboTotalAmount,  " +
                                "SUM(FSTSS.EcmAmount) AS EcmAmount,    " +
                                "SUM(FSGSTT.[EcmGSTAmount]) As EcmGSTAmount, SUM(FSGSTT.[EcmTotalAmount]) As EcmTotalAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount ,  " +
                                "SUM(FSGSTT.[AccidentalGSTAmount]) As AccidentalGSTAmount, SUM(FSGSTT.[AccidentalGrossTotalAmount]) As AccidentalGrossTotalAmount    " + 
                                " " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMR.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMRR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMRR.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMSS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON (FSMD.Rec_Id = FSOS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTSS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON (FSMD.Rec_Id = FSWS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON (FSMD.BranchId = FBD.BranchId)  " +
                                "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FSMD.RegionId = FRD.RegionId)  " +
                                "INNER JOIN (select * from (select *,ROW_NUMBER() over (partition by vehicleno order by [date] desc) rm from [Fleet_Vehicle_Details_Date] with(nolock)where [Date] >='2021-07-01' and [Date] <='2021-08-25') tbl where tbl.rm=1 ) FVD  ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSO.Rec_Id)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON (FSMD.Rec_Id = FSGST.Rec_Id)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSTT.Rec_Id)  " +
                                "WHERE SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "' AND FSMD.VechileNumber ='" + Vechile + "' " +
                                "GROUP BY FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,FRD.RegionName,FBD.BranchName " +
                                ")TblData  " +
                                "GROUP BY RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                                "NoOfTyres,TyreAmount,BatteryAmount,RoutineAmount,DentingAmount,MinorAmount,  " +
                                "SeatAmount,SelfAmount,ElectricalAmount,ClutchAmount,AlternatorAmount,LeafAmount,  " +
                                "SuspensionAmount,GearBoxAmount,BreakWorkAmount,EngineWorkAmount,  " +
                                "FuelAmount,PuncherAmount,OilAmount,RadiatorAmount,AxleAmount,DifferentialAmount,  " +
                                "TurboAmount,EcmAmount,AccidentalTotalAmount " +
                                ")tbl ";

                }
                if (FromDate != null && ToDate != null && RegionID != "" && BranchID == "" && VechileNumber == "" && Vechile == "")
                {
                    query1 = "SELECT RegionName,BranchName,VechileNumber,Make,Manufacturing_Year, " +
                                "NoOfTyres,TyreAmount,Battery,Routine,[Denting&Painting],MinorRepairing,  " +
                                "SeatRepair,SelfWork,ElectricalWork,ClutchRepairing,Alternator,[Leaf/PattiSpring],  " +
                                "Suspension,GearBox,BreakWork,EngineWork,  " +
                                "FuelPump,Puncher,OilTopUp,RadiatorandWaterBody,AxleWork,DifferentialWork,  " +
                                "Turbo,[Ecm/Sencer],Accidental,TotalValue, TotalGSTValue,(TotalValue + TotalGSTValue) As TotalFinalAmount,  " +
                                "CONCAT('TyreInvoiceNo :-',TyreInvoiceNo,'|BatteryInvoiceNo :-',BatteryInvoiceNo,'|RoutineInvoiceNo :-',RoutineInvoiceNo,  " +
                                "'|DentingInvoiceNo :-',DentingInvoiceNo,'|MinorInvoiceNo :-',MinorInvoiceNo  " +
                                ",'|SeatInvoiceNo :-',SeatInvoiceNo,'|SelfInvoiceNo :-',SelfInvoiceNo,'|ElectricalInvoiceNo :-',  " +
                                "ElectricalInvoiceNo,'|ClutchInvoiceNo :-',ClutchInvoiceNo,'|AlternatorInvoiceNo :-',AlternatorInvoiceNo,  " +
                                "'|LeafInvoiceNo :-',LeafInvoiceNo,'|SuspensionInvoiceNo :-',SuspensionInvoiceNo,'|GearBoxInvoiceNo :-',  " +
                                "GearBoxInvoiceNo,'|BreakWorkInvoiceNo :-',BreakWorkInvoiceNo,'|EngineWorkInvoiceNo:-',EngineWorkInvoiceNo,'|FuelInvoiceNo :-',  " +
                                "FuelInvoiceNo,'|PuncherInvoiceNo :-',PuncherInvoiceNo,'|OilInvoiceNo :-',OilInvoiceNo,'|RadiatorInvoiceNo :-',RadiatorInvoiceNo,  " +
                                "'|AxleInvoiceNo :-',AxleInvoiceNo,'|DifferentialInvoiceNo:-',DifferentialInvoiceNo  " +
                                ",'|TurboInvoiceNo:-',TurboInvoiceNo,'|EcmInvoiceNo:-',EcmInvoiceNo,'|AccidentalInvoiceNo:-',AccidentalInvoiceNo) As InvoiceNo  " +
                                "FROM " +
                                "( " +
                                "SELECT RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                                "NoOfTyres ,TyreAmount ,BatteryAmount As Battery,RoutineAmount As Routine,DentingAmount As [Denting&Painting],MinorAmount As MinorRepairing,  " +
                                " SeatAmount As SeatRepair,SelfAmount As SelfWork,ElectricalAmount As ElectricalWork,ClutchAmount As ClutchRepairing,AlternatorAmount As Alternator,LeafAmount As [Leaf/PattiSpring],  " +
                                " SuspensionAmount As Suspension,GearBoxAmount As GearBox,BreakWorkAmount As BreakWork,EngineWorkAmount As EngineWork,  " +
                                " FuelAmount As FuelPump,PuncherAmount As Puncher,OilAmount As OilTopUp,RadiatorAmount As RadiatorandWaterBody,AxleAmount As AxleWork,DifferentialAmount As DifferentialWork,  " +
                                " TurboAmount As Turbo,EcmAmount As [Ecm/Sencer], AccidentalTotalAmount As Accidental,  " +
                                "SUM(TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                                "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                                "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                                "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                                "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
                               "SUM( " +
                                "ISNULL([TyreGSTAmount],0)+ISNULL([BatteryGSTAmount],0)+ISNULL([RoutineGSTAmount],0)+ISNULL([DentingGSTAmount],0)+ISNULL([MinorGSTAmount],0)+ISNULL([SeatGSTAmount],0)+ISNULL([SelfGSTAmount],0)+ " +
                                "ISNULL([ElectricalGSTAmount],0)+ISNULL([ClutchGSTAmount],0)+ISNULL([AlternatorGSTAmount],0)+ISNULL([LeafGSTAmount],0)+ISNULL([SuspensionGSTAmount],0)+ISNULL([GearBoxGSTAmount],0)+ " +
                                "ISNULL([BreakGSTAmount],0)+ISNULL([EngineGSTAmount],0)+ISNULL([FuelGSTAmount],0)+ISNULL([PuncherGSTAmount],0)+ISNULL([OilGSTAmount],0)+ " +
                                "ISNULL([RadiatorGSTAmount],0)+ISNULL([AxleGSTAmount],0)+ ISNULL([DifferentialGSTAmount],0)+ISNULL([TurboGSTAmount],0)+ISNULL([EcmGSTAmount],0)+ISNULL([AccidentalGSTAmount],0) " +
                                ") As TotalGSTValue, " +
                                "SUM( " +
                                "[TyreTotalAmount]+[BatteryTotalAmount]+[RoutineTotalAmount]+[DentingTotalAmount]+[MinorTotalAmount]+[SeatTotalAmount]+[SelfTotalAmount]+ " +
                                "[ElectricalTotalAmount]+[ClutchTotalAmount]+[AlternatorTotalAmount]+[LeafTotalAmount]+[SuspensionTotalAmount]+[GearBoxTotalAmount]+ " +
                                "[BreakTotalAmount]+[EngineTotalAmount]+[FuelTotalAmount]+[PuncherTotalAmount]+[OilTotalAmount]+[RadiatorTotalAmount]+[AxleTotalAmount]+ " +
                                "[DifferentialTotalAmount]+[TurboTotalAmount]+[EcmTotalAmount]+[AccidentalGrossTotalAmount] " +
                                ") As TotalFinalAmount , " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.TyreInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as TyreInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.BatteryInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as BatteryInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.RoutineInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as RoutineInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.DentingInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as DentingInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.MinorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as MinorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SeatInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Minor_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SeatInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SelfInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SelfInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.ElectricalInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as ElectricalInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.ClutchInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as ClutchInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AlternatorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AlternatorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.LeafInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as LeafInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.SuspensionInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Major_Repairing] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as SuspensionInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.GearBoxInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as GearBoxInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.BreakWorkInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as BreakWorkInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.EngineWorkInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_WorkBox_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as EngineWorkInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.FuelInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as FuelInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.PuncherInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as PuncherInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.OilInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Topup_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as OilInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.RadiatorInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as RadiatorInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AxleInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AxleInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.DifferentialInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Other_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as DifferentialInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.TurboInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as TurboInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.EcmInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as EcmInvoiceNo, " +
                                " " +
                                "(SELECT STUFF( " +
                                "(SELECT  '~' + s.AccidentalInvoiceNo " +
                                "FROM [dbo].[Fleet_Sales_Turbo_Service] s WITH(NOLOCK) " +
                                "inner join [dbo].[Fleet_Sales_Master_Details] AS t1 WITH(NOLOCK) on s.Rec_Id = t1.Rec_Id " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and t1.VechileNumber=tblData.VechileNumber " +
                                "FOR XML PATH('')),1,1,'') AS [Location_Remark] " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] AS t WITH(NOLOCK) " +
                                "where SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  " +
                                "and  VechileNumber=tblData.VechileNumber " +
                                "GROUP BY VechileNumber " +
                                ") as AccidentalInvoiceNo " +
                                " " +
                                "FROM  " +
                                "(  " +
                                "SELECT FRD.RegionName,FBD.BranchName,FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,  " +
                                "SUM(FSMSS.NoOfTyres) As NoOfTyres,SUM(FSMSS.TyreAmount) As TyreAmount,  " +
                                "SUM(FSGSO.[TyreGSTAmount]) As TyreGSTAmount, SUM(FSGSO.[TyreTotalAmount]) As [TyreTotalAmount],  " +
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,  " +
                                "SUM(FSGSO.[BatteryGSTAmount]) As BatteryGSTAmount, SUM(FSGSO.[BatteryTotalAmount]) As BatteryTotalAmount,  " +
                                "SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSGSO.[RoutineGSTAmount]) As RoutineGSTAmount, SUM(FSGSO.[RoutineTotalAmount]) As RoutineTotalAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,  " +
                                "SUM(FSGSO.[DentingGSTAmount]) As DentingGSTAmount, SUM(FSGSO.[DentingTotalAmount]) As DentingTotalAmount,  " +
                                "SUM(FSMRR.MinorAmount) As MinorAmount,    " +
                                "SUM(FSGSO.[MinorGSTAmount]) As MinorGSTAmount, SUM(FSGSO.[MinorTotalAmount]) As MinorTotalAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount,  " +
                                "SUM(FSGSO.[SeatGSTAmount]) As SeatGSTAmount, SUM(FSGSO.[SeatTotalAmount]) As SeatTotalAmount,  " +
                                "SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSGSO.[SelfGSTAmount]) As SelfGSTAmount, SUM(FSGSO.[SelfTotalAmount]) As SelfTotalAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount,  " +
                                "SUM(FSGSO.[ElectricalGSTAmount]) As ElectricalGSTAmount, SUM(FSGSO.[ElectricalTotalAmount]) As ElectricalTotalAmount,  " +
                                "SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSGST.[ClutchGSTAmount]) As ClutchGSTAmount, SUM(FSGST.[ClutchTotalAmount]) As ClutchTotalAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount,   " +
                                "SUM(FSGST.[AlternatorGSTAmount]) As AlternatorGSTAmount, SUM(FSGST.[AlternatorTotalAmount]) As AlternatorTotalAmount,  " +
                                "SUM(FSMR.LeafAmount) As LeafAmount,    " +
                                "SUM(FSGST.[LeafGSTAmount]) As LeafGSTAmount, SUM(FSGST.[LeafTotalAmount]) As LeafTotalAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount,   " +
                                "SUM(FSGST.[SuspensionGSTAmount]) As SuspensionGSTAmount, SUM(FSGST.[SuspensionTotalAmount]) As SuspensionTotalAmount,  " +
                                "SUM(FSWS.GearBoxAmount) As GearBoxAmount,    " +
                                "SUM(FSGST.[GearBoxGSTAmount]) As GearBoxGSTAmount, SUM(FSGST.[GearBoxTotalAmount]) As GearBoxTotalAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount,   " +
                                "SUM(FSGST.[BreakGSTAmount]) As BreakGSTAmount, SUM(FSGST.[BreakTotalAmount]) As BreakTotalAmount,  " +
                                "SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,    " +
                                "SUM(FSGST.[EngineGSTAmount]) As EngineGSTAmount, SUM(FSGST.[EngineTotalAmount]) As EngineTotalAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,  " +
                                "SUM(FSGST.[FuelGSTAmount]) As FuelGSTAmount, SUM(FSGST.[FuelTotalAmount]) As FuelTotalAmount,  " +
                                "SUM(FSTS.PuncherAmount) As PuncherAmount,    " +
                                "SUM(FSGSTT.[PuncherGSTAmount]) As PuncherGSTAmount, SUM(FSGSTT.[PuncherTotalAmount]) As PuncherTotalAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,  " +
                                "SUM(FSGSTT.[OilGSTAmount]) As OilGSTAmount, SUM(FSGSTT.[OilTotalAmount]) As OilTotalAmount,  " +
                                "SUM(FSOS.RadiatorAmount) As RadiatorAmount,    " +
                                "SUM(FSGSTT.[RadiatorGSTAmount]) As RadiatorGSTAmount, SUM(FSGSTT.[RadiatorTotalAmount]) As RadiatorTotalAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount,   " +
                                "SUM(FSGSTT.[AxleGSTAmount]) As AxleGSTAmount, SUM(FSGSTT.[AxleTotalAmount]) As AxleTotalAmount,  " +
                                "SUM(FSOS.DifferentialAmount) As DifferentialAmount,    " +
                                "SUM(FSGSTT.[DifferentialGSTAmount]) As DifferentialGSTAmount, SUM(FSGSTT.[DifferentialTotalAmount]) As DifferentialTotalAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount,   " +
                                "SUM(FSGSTT.[TurboGSTAmount]) As TurboGSTAmount, SUM(FSGSTT.[TurboTotalAmount]) As TurboTotalAmount,  " +
                                "SUM(FSTSS.EcmAmount) AS EcmAmount,    " +
                                "SUM(FSGSTT.[EcmGSTAmount]) As EcmGSTAmount, SUM(FSGSTT.[EcmTotalAmount]) As EcmTotalAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount ,  " +
                                "SUM(FSGSTT.[AccidentalGSTAmount]) As AccidentalGSTAmount, SUM(FSGSTT.[AccidentalGrossTotalAmount]) As AccidentalGrossTotalAmount    " + 
                                " " +
                                "FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMR.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMRR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMRR.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMSS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON (FSMD.Rec_Id = FSOS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTSS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON (FSMD.Rec_Id = FSWS.Rec_Id)  " +
                                "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON (FSMD.BranchId = FBD.BranchId)  " +
                                "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FSMD.RegionId = FRD.RegionId)  " +
                                "INNER JOIN (select * from (select *,ROW_NUMBER() over (partition by vehicleno order by [date] desc) rm from [Fleet_Vehicle_Details_Date] with(nolock)where [Date] >='2021-07-01' and [Date] <='2021-08-25') tbl where tbl.rm=1 ) FVD  ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_One] FSGSO WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSO.Rec_Id)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Two] FSGST WITH(NOLOCK) ON (FSMD.Rec_Id = FSGST.Rec_Id)  " +
                                "LEFT JOIN  [dbo].[Fleet_Sales_GST_Segment_Three] FSGSTT WITH(NOLOCK) ON (FSMD.Rec_Id = FSGSTT.Rec_Id)  " +
                                "WHERE SalesDate >='" + FromDate + "' and SalesDate <= '" + ToDate + "'  AND FSMD.RegionId =" + RegionID + " " +
                                "GROUP BY FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,FRD.RegionName,FBD.BranchName " +
                                ")TblData  " +
                                "GROUP BY RegionName,BranchName,tblData.VechileNumber,Make,Manufacturing_Year,  " +
                                "NoOfTyres,TyreAmount,BatteryAmount,RoutineAmount,DentingAmount,MinorAmount,  " +
                                "SeatAmount,SelfAmount,ElectricalAmount,ClutchAmount,AlternatorAmount,LeafAmount,  " +
                                "SuspensionAmount,GearBoxAmount,BreakWorkAmount,EngineWorkAmount,  " +
                                "FuelAmount,PuncherAmount,OilAmount,RadiatorAmount,AxleAmount,DifferentialAmount,  " +
                                "TurboAmount,EcmAmount,AccidentalTotalAmount " +
                                ")tbl ";
                                 
                }
                SqlConnection con = new SqlConnection(strSqlConnectionString);
                con.Open();

                //query1 ATM MIS Report
                SqlCommand cmd = new SqlCommand(query1, con);
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                DataTable dt = ds.Tables[0].Copy();
                dt.TableName = "Report";

                dsFinal.Tables.Add(dt);

                //Set Name of DataTables.
                ds.Tables[0].TableName = "ReportData";

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
                    Response.AddHeader("content-disposition", "attachment;filename=Repair And Maintance_Report.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }


            }
            catch(Exception ex)
            {
                Session["Message"] = String.Format("No Data Available / Some Information Missing !!");
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetMaintainanceReport(string FromDate, string ToDate, int RegionId, int BranchId, string VechileNumber)
        {
            DataSet ds = Sr.GetMaintainanceDetails(FromDate, ToDate, RegionId, BranchId, VechileNumber);
            DataTable dt = new DataTable();

            //DataTable
            dt = ds.Tables[0];

            List<SalesReport> lstVehicle = new List<SalesReport>();

            for (int i = 0; i <= dt.Rows.Count - 1; i++)
            {
                SalesReport H = new SalesReport();

                H.RegionName = dt.Rows[i]["RegionName"].ToString();
                H.BranchName = dt.Rows[i]["BranchName"].ToString();
                H.VechileNumber = dt.Rows[i]["VechileNumber"].ToString();
                H.Make = dt.Rows[i]["Make"].ToString();
                H.ManufacturingYear = dt.Rows[i]["Manufacturing_Year"].ToString();
                H.NoOfTyres = dt.Rows[i]["NoOfTyres"].ToString();
                H.TyreAmount = dt.Rows[i]["TyreAmount"].ToString();
                H.Battery = dt.Rows[i]["Battery"].ToString();
                H.Routine = dt.Rows[i]["Routine"].ToString();
                H.DentingPainting = dt.Rows[i]["Denting&Painting"].ToString();
                H.MinorRepairing = dt.Rows[i]["MinorRepairing"].ToString();
                H.SeatRepair = dt.Rows[i]["SeatRepair"].ToString();
                H.SelfWork = dt.Rows[i]["SelfWork"].ToString();
                H.ElectricalWork = dt.Rows[i]["ElectricalWork"].ToString();
                H.ClutchRepairing = dt.Rows[i]["ClutchRepairing"].ToString();
                H.Alternator = dt.Rows[i]["Alternator"].ToString();
                H.LeafPattiSpring = dt.Rows[i]["Leaf/PattiSpring"].ToString();
                H.Suspension = dt.Rows[i]["Suspension"].ToString();
                H.GearBox = dt.Rows[i]["GearBox"].ToString();
                H.BreakWork = dt.Rows[i]["BreakWork"].ToString();
                H.EngineWork = dt.Rows[i]["EngineWork"].ToString();
                H.FuelPump = dt.Rows[i]["FuelPump"].ToString();
                H.Puncher = dt.Rows[i]["Puncher"].ToString();
                H.OilTopUp = dt.Rows[i]["OilTopUp"].ToString();
                H.RadiatorandWaterBody = dt.Rows[i]["RadiatorandWaterBody"].ToString();
                H.AxleWork = dt.Rows[i]["AxleWork"].ToString();
                H.DifferentialWork = dt.Rows[i]["DifferentialWork"].ToString();
                H.Turbo = dt.Rows[i]["Turbo"].ToString();
                H.EcmSencer = dt.Rows[i]["Ecm/Sencer"].ToString();
                H.Accidental = dt.Rows[i]["Accidental"].ToString();
                H.TotalValue = dt.Rows[i]["TotalValue"].ToString();
                H.InvoiceNo = dt.Rows[i]["InvoiceNo"].ToString();


                lstVehicle.Add(H);
            }

            return Json(lstVehicle, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GetKPLReports(string Year, string Month, int RegionId, int BranchId, string VechileNumber)
        {
            DataSet ds = Sr.GetKPLReports(Year, Month, RegionId, BranchId, VechileNumber);
            DataTable dt = new DataTable();
            List<KPLReports> lstKPLReports = new List<KPLReports>();
            try
            {
                dt = ds.Tables[0];
                KPLReports k = new KPLReports();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i <= dt.Rows.Count - 1; i++)
                    {
                        k = new KPLReports();
                        k.Month = dt.Rows[i]["Month"].ToString();
                        k.RegionIDD = dt.Rows[i]["RegionIDD"].ToString();
                        k.RegionName = dt.Rows[i]["RegionName"].ToString();
                        k.BranchIDD = dt.Rows[i]["BranchIDD"].ToString();
                        k.BranchName = dt.Rows[i]["BranchName"].ToString();
                        k.VechileNumber = dt.Rows[i]["VehicleNo"].ToString();
                        k.OpeningKM = dt.Rows[i]["OpeningKM"].ToString();
                        k.ClosingKM = dt.Rows[i]["ClosingKM"].ToString();
                        k.Make = dt.Rows[i]["Make"].ToString();
                        k.Manufacturing_Year = dt.Rows[i]["Manufacturing_Year"].ToString();
                        k.FuelType = dt.Rows[i]["FuelType"].ToString();
                        k.PetroCardNumber = dt.Rows[i]["PetroCardNumber"].ToString();
                        k.OpeningFuel = dt.Rows[i]["OpeningFuel"].ToString();
                        k.FuelPuchasedInLtr = dt.Rows[i]["FuelPuchasedInLtr"].ToString();
                        k.ClosingFuelLtr = dt.Rows[i]["ClosingFuelLtr"].ToString();
                        k.FuelCombustion = dt.Rows[i]["FuelCombustion"].ToString();
                        k.BillingKM = dt.Rows[i]["BillingKM"].ToString();
                        k.NonBillingKM = dt.Rows[i]["NonBillingKM"].ToString();
                        k.TOTALKM = dt.Rows[i]["TOTALKM"].ToString();
                        k.KMPL = dt.Rows[i]["KMPL"].ToString();
                        k.BpNonBp = dt.Rows[i]["BpNonBp"].ToString();
                        k.StdKmpl = dt.Rows[i]["StdKmpl"].ToString();
                        k.TypeOfServices = dt.Rows[i]["TypeOfServices"].ToString();
                        k.RouteNumber = dt.Rows[i]["RouteNumber"].ToString();
                        k.UnitName = dt.Rows[i]["UnitName"].ToString();
                        k.FuelCostPerKM = dt.Rows[i]["FuelCostPerKM"].ToString();
                        k.FuelRateInRsPerLtr = dt.Rows[i]["FuelRateInRsPerLtr"].ToString();
                        k.FuelPurchasedInCash = dt.Rows[i]["FuelPurchasedInCash"].ToString();
                        k.FuelPuchasedThroughPetroCard = dt.Rows[i]["FuelPuchasedThroughPetroCard"].ToString();
                        k.Remarks = dt.Rows[i]["Remarks"].ToString();
                        k.ExtraFuelTakenByCard = dt.Rows[i]["ExtraFuelTakenByCard"].ToString();
                        k.ExtraAmountSwipedByCard = dt.Rows[i]["ExtraAmountSwipedByCard"].ToString();
                        k.VendorName = dt.Rows[i]["VendorName"].ToString();
                        k.DriverName = dt.Rows[i]["DriverName"].ToString();
                        k.DriverPatId = dt.Rows[i]["DriverPatId"].ToString();
                        k.ActualFuelTakenAsPerSTD_KPL = dt.Rows[i]["ActualFuelTakenAsPerSTD_KPL"].ToString();

                        lstKPLReports.Add(k);
                    }
                }
                else
                {
                    k.Month = null;
                    k.RegionIDD = null;
                    k.RegionName = null;
                    k.BranchIDD = null;
                    k.BranchName = null;
                    k.VechileNumber = null;
                    k.OpeningKM = null;
                    k.ClosingKM = null;
                    k.Make = null;
                    k.Manufacturing_Year = null;
                    k.FuelType = null;
                    k.PetroCardNumber = null;
                    k.OpeningFuel = null;
                    k.FuelPuchasedInLtr = null;
                    k.ClosingFuelLtr = null;
                    k.FuelCombustion = null;
                    k.BillingKM = null;
                    k.NonBillingKM = null;
                    k.TOTALKM = null;
                    k.KMPL = null;
                    k.BpNonBp = null;
                    k.StdKmpl = null;
                    k.TypeOfServices = null;
                    k.RouteNumber = null;
                    k.UnitName = null;
                    k.FuelCostPerKM = null;
                    k.FuelRateInRsPerLtr = null;
                    k.FuelPurchasedInCash = null;
                    k.FuelPuchasedThroughPetroCard = null;
                    k.Remarks = null;
                    k.ExtraFuelTakenByCard = null;
                    k.ExtraAmountSwipedByCard = null;
                    k.VendorName = null;
                    k.DriverName = null;
                    k.DriverPatId = null;
                    k.ActualFuelTakenAsPerSTD_KPL = null;
                    lstKPLReports.Add(k);
                }
            }
            catch
            {
            }

            return Json(lstKPLReports, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DownloadKPLReport(string cmbYear, string cmbMonth, int ddlRegionKPL, int ddlBranchKPL, string ddlVehicleNumberKPL)
        {

            Get_from_config();
            DataSet ds = Sr.GetKPLReports(cmbYear, cmbMonth, ddlRegionKPL, ddlBranchKPL, ddlVehicleNumberKPL);
            DataTable dt = new DataTable();

            dt = ds.Tables[0];
            dt.TableName = "KPLReport";

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
                Response.AddHeader("content-disposition", "attachment;filename=KPLReport.xlsx");
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
        public ActionResult DailyMisDownload(FormCollection Fd)
        {
            string Date = Request.Form["txtDailyFromDate"];
            string RegionId = Request.Form["ddlDailyRegion"];
            string BranchId = Request.Form["ddlDailyBranch"];

            if (Session["UserType"].ToString() == "User")
            {
                if (RegionId == "")
                {
                    RegionId = Session["RegionIds"].ToString();
                }
            }
            if (BranchId == "" || BranchId == null)
            {
                BranchId = "";
            }

            DataSet ds = new DataSet();

            if (Session["UserType"].ToString() == "Admin")
            {
                 ds = Sr.GetDilyMisReports(Date);
            }

            if (Session["UserType"].ToString() == "User" || Session["UserType"].ToString()=="RH")
            {
                ds = Sr.GetDilyMisReports(Date, RegionId, BranchId);
            }

            DataSet ds1 = new DataSet();

            if (Session["UserType"].ToString() == "Admin")
            {
                 ds1 = Sr.GetDailyMisDashBoard(Date);
            }

            if (Session["UserType"].ToString() == "User" || Session["UserType"].ToString()=="RH")
            {
                ds1 = Sr.GetDailyMisDashBoard(Date, RegionId, BranchId);
            }


            DataTable dt = new DataTable();
            DataTable dt1 = new DataTable();
            DataSet dsFinal = new DataSet();

            dt = ds.Tables[0];
            dt1 = ds1.Tables[0];

            DataTable dt12 = new DataTable("DailyMisReport");
            DataTable dt13 = new DataTable("Summary");

            dt12 = dt.Copy();
            dt12.TableName = "DailyMisReport";
            dt13 = dt1.Copy();
            dt13.TableName = "Summary";

            dsFinal.Tables.Add(dt12);
            dsFinal.Tables.Add(dt13);

            //
            //

            // Generate a new unique identifier against which the file can be stored
            //string handle = Guid.NewGuid().ToString();

            using (XLWorkbook wb = new XLWorkbook())
            {
                foreach (DataTable dt11 in dsFinal.Tables)
                {
                    //Add DataTable as Worksheet.
                    wb.Worksheets.Add(dt11);
                }

                //Export the Excel file.
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=DailyMisReport.xlsx");
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
        public ActionResult RepairMaintainanceDownload(FormCollection Fd)
        {
            try
            {
                string FromDate = Request.Form["txtRepairFromDate"];
                string ToDate = Request.Form["txtRepairToDate"];
                string RegionID = Session["RegionIds"].ToString();

                DataSet ds = new DataSet();

                if (Session["UserType"].ToString() == "Admin")
                {
                    ds = Sr.GetDateWiseReports(FromDate, ToDate);
                }

                if (Session["UserType"].ToString() == "User")
                {
                    ds = Sr.GetDateWiseReports(FromDate, ToDate, RegionID);
                }


                DataTable dt = new DataTable();

                dt = ds.Tables[0];
                dt.TableName = "Report";

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
                    Response.AddHeader("content-disposition", "attachment;filename=DailyMisReport.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception e)
            { 
                
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult MaintainanceTaxsDownload(FormCollection Fd)
        {
            try
            {
                string DocumentType = Request.Form["hfDocumentType"];

                DataSet ds = new DataSet();

                string strDocumentType = "";

                string[] Documentlength = DocumentType.Split(',');

                for (int i = 0; i <= Documentlength.Length - 1; i++)
                {
                    if (i == 0)
                    {
                        strDocumentType = "'" + Documentlength[i].ToString().TrimStart().TrimEnd() + "'";
                    }
                    else
                    {
                        strDocumentType = strDocumentType + ",'" + Documentlength[i].ToString().TrimStart().TrimEnd() + "'";
                    }
                }

                if (Session["UserType"].ToString() == "Admin")
                {
                    ds = Sr.GetDocumnetTypeReport(strDocumentType);
                }
                if (Session["UserType"].ToString() == "User")
                {
                    ds = Sr.GetDocumnetTypeReport(strDocumentType, Session["RegionIds"].ToString());
                }

                DataTable dt = new DataTable();

                dt = ds.Tables[0];
                dt.TableName = "Report";

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
                    Response.AddHeader("content-disposition", "attachment;filename=Taxs_Report.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DailyMisMonthlyDownload(FormCollection Fd)
        {
            try
            {
                string FromDate = Request.Form["txtFromDailyMisDate"];
                string ToDate = Request.Form["txtToDailyMisDate"];

                DataSet ds = new DataSet();


                ds = Sr.GetDailyMisMonthlyReport(FromDate, ToDate);

                DataTable dt = new DataTable();

                dt = ds.Tables[0];
                dt.TableName = "Report";

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
                    Response.AddHeader("content-disposition", "attachment;filename=Daily_Mis_Monthly_Report.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RoutineTrackerDownload()
        {
            try
            {

                DataSet ds = new DataSet();
                ds = Sr.GetRoutineTrackerReport();

                DataTable dt = new DataTable();

                dt = ds.Tables[0];
                dt.TableName = "Report";

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
                    Response.AddHeader("content-disposition", "attachment;filename=Routine_Tracker_Report.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult MonthlyMisReport(FormCollection Fd)
        {
            try
            {
                string FromDate = Request.Form["txtFromMonthlyReportMisDate"];
                string ToDate = Request.Form["txtToMonthlyReportMisDate"];

                DataSet ds = new DataSet();

                if (Session["UserType"].ToString() == "Admin")
                {
                    ds = Sr.GetMonthlyMisReport(FromDate, ToDate);

                }
                if (Session["UserType"].ToString() == "User")
                {
                    ds = Sr.GetMonthlyMisReport(FromDate, ToDate, Session["RegionIds"].ToString());
                }

                if (Session["UserType"].ToString() == "RH")
                {
                    ds = Sr.GetMonthlyMisReport(FromDate, ToDate, Session["RegionIds"].ToString());
                }

                DataTable dt = new DataTable();

                dt = ds.Tables[0];
                dt.TableName = "Report";

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
                    Response.AddHeader("content-disposition", "attachment;filename=Monthly_Mis_Report.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult HiredVehicleReport(FormCollection Fd)
        {
            try
            {
                string FromDate = Request.Form["txtFromHiredDate"];
                string ToDate = Request.Form["txtToHiredMisDate"];

                DataSet ds = new DataSet();

                if (Session["UserType"].ToString() == "Admin")
                {
                    ds = Sr.GetMonthlyHiredReport(FromDate, ToDate);

                }
                if (Session["UserType"].ToString() == "User")
                {
                    ds = Sr.GetMonthlyHiredReport(FromDate, ToDate, Session["RegionIds"].ToString());
                }

                if (Session["UserType"].ToString() == "RH")
                {
                    ds = Sr.GetMonthlyHiredReport(FromDate, ToDate, Session["RegionIds"].ToString());
                }

                DataTable dt = new DataTable();

                dt = ds.Tables[0];
                dt.TableName = "Report";

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
                    Response.AddHeader("content-disposition", "attachment;filename=Hired_Vehicle_Report.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("Index");
        }
    }
}
