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

            if (User == "User")
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
                                "Turbo,[Ecm/Sencer],Accidental,TotalValue, " +
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
                                "SUM(NoOfTyres+TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                                "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                                "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                                "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                                "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
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
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,SUM(FSMRR.MinorAmount) As MinorAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount, SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount, SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount, SUM(FSMR.LeafAmount) As LeafAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount, SUM(FSWS.GearBoxAmount) As GearBoxAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount, SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,SUM(FSTS.PuncherAmount) As PuncherAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,SUM(FSOS.RadiatorAmount) As RadiatorAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount, SUM(FSOS.DifferentialAmount) As DifferentialAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount, SUM(FSTSS.EcmAmount) AS EcmAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount " +
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
                                "INNER JOIN [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK) ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
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
                                "Turbo,[Ecm/Sencer],Accidental,TotalValue, " +
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
                                "SUM(NoOfTyres+TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                                "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                                "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                                "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                                "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
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
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,SUM(FSMRR.MinorAmount) As MinorAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount, SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount, SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount, SUM(FSMR.LeafAmount) As LeafAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount, SUM(FSWS.GearBoxAmount) As GearBoxAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount, SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,SUM(FSTS.PuncherAmount) As PuncherAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,SUM(FSOS.RadiatorAmount) As RadiatorAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount, SUM(FSOS.DifferentialAmount) As DifferentialAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount, SUM(FSTSS.EcmAmount) AS EcmAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount " +
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
                                "INNER JOIN [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK) ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
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
                               "Turbo,[Ecm/Sencer],Accidental,TotalValue, " +
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
                               "SUM(NoOfTyres+TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                               "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                               "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                               "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                               "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
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
                               "SUM(FSMSS.BatteryAmount) As BatteryAmount,SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                               "SUM(FSMRR.DentingAmount) As DentingAmount,SUM(FSMRR.MinorAmount) As MinorAmount,  " +
                               "SUM(FSMRR.SeatAmount) As SeatAmount, SUM(FSMS.SelfAmount) As SelfAmount,  " +
                               "SUM(FSMS.ElectricalAmount) As ElectricalAmount, SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                               "SUM(FSMR.AlternatorAmount) As AlternatorAmount, SUM(FSMR.LeafAmount) As LeafAmount,  " +
                               "SUM(FSMR.SuspensionAmount) As SuspensionAmount, SUM(FSWS.GearBoxAmount) As GearBoxAmount,  " +
                               "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount, SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,  " +
                               "SUM(FSTS.FuelAmount) As FuelAmount,SUM(FSTS.PuncherAmount) As PuncherAmount,  " +
                               "SUM(FSTS.OilAmount) As OilAmount,SUM(FSOS.RadiatorAmount) As RadiatorAmount,  " +
                               "SUM(FSOS.AxleAmount) As AxleAmount, SUM(FSOS.DifferentialAmount) As DifferentialAmount,  " +
                               "SUM(FSTSS.TurboAmount) As TurboAmount, SUM(FSTSS.EcmAmount) AS EcmAmount,  " +
                               "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount " +
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
                               "INNER JOIN [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK) ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
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
                                "Turbo,[Ecm/Sencer],Accidental,TotalValue, " +
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
                                "SUM(NoOfTyres+TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                                "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                                "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                                "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                                "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
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
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,SUM(FSMRR.MinorAmount) As MinorAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount, SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount, SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount, SUM(FSMR.LeafAmount) As LeafAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount, SUM(FSWS.GearBoxAmount) As GearBoxAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount, SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,SUM(FSTS.PuncherAmount) As PuncherAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,SUM(FSOS.RadiatorAmount) As RadiatorAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount, SUM(FSOS.DifferentialAmount) As DifferentialAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount, SUM(FSTSS.EcmAmount) AS EcmAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount " +
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
                                "INNER JOIN [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK) ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
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
                                "Turbo,[Ecm/Sencer],Accidental,TotalValue, " +
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
                                "SUM(NoOfTyres+TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                                "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                                "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                                "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                                "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
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
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,SUM(FSMRR.MinorAmount) As MinorAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount, SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount, SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount, SUM(FSMR.LeafAmount) As LeafAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount, SUM(FSWS.GearBoxAmount) As GearBoxAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount, SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,SUM(FSTS.PuncherAmount) As PuncherAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,SUM(FSOS.RadiatorAmount) As RadiatorAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount, SUM(FSOS.DifferentialAmount) As DifferentialAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount, SUM(FSTSS.EcmAmount) AS EcmAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount " +
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
                                "INNER JOIN [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK) ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
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
                                "Turbo,[Ecm/Sencer],Accidental,TotalValue, " +
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
                                "SUM(NoOfTyres+TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                                "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                                "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                                "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                                "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
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
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,SUM(FSMRR.MinorAmount) As MinorAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount, SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount, SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount, SUM(FSMR.LeafAmount) As LeafAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount, SUM(FSWS.GearBoxAmount) As GearBoxAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount, SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,SUM(FSTS.PuncherAmount) As PuncherAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,SUM(FSOS.RadiatorAmount) As RadiatorAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount, SUM(FSOS.DifferentialAmount) As DifferentialAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount, SUM(FSTSS.EcmAmount) AS EcmAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount " +
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
                                "INNER JOIN [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK) ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
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
                                "Turbo,[Ecm/Sencer],Accidental,TotalValue, " +
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
                                "SUM(NoOfTyres+TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                                "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                                "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                                "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                                "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
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
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,SUM(FSMRR.MinorAmount) As MinorAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount, SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount, SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount, SUM(FSMR.LeafAmount) As LeafAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount, SUM(FSWS.GearBoxAmount) As GearBoxAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount, SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,SUM(FSTS.PuncherAmount) As PuncherAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,SUM(FSOS.RadiatorAmount) As RadiatorAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount, SUM(FSOS.DifferentialAmount) As DifferentialAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount, SUM(FSTSS.EcmAmount) AS EcmAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount " +
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
                                "INNER JOIN [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK) ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
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
                                "Turbo,[Ecm/Sencer],Accidental,TotalValue, " +
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
                                "SUM(NoOfTyres+TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+  " +
                                "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+  " +
                                "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+  " +
                                "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+  " +
                                "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
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
                                "SUM(FSMSS.BatteryAmount) As BatteryAmount,SUM(FSMSS.RoutineAmount) As RoutineAmount,  " +
                                "SUM(FSMRR.DentingAmount) As DentingAmount,SUM(FSMRR.MinorAmount) As MinorAmount,  " +
                                "SUM(FSMRR.SeatAmount) As SeatAmount, SUM(FSMS.SelfAmount) As SelfAmount,  " +
                                "SUM(FSMS.ElectricalAmount) As ElectricalAmount, SUM(FSMS.ClutchAmount) As ClutchAmount,  " +
                                "SUM(FSMR.AlternatorAmount) As AlternatorAmount, SUM(FSMR.LeafAmount) As LeafAmount,  " +
                                "SUM(FSMR.SuspensionAmount) As SuspensionAmount, SUM(FSWS.GearBoxAmount) As GearBoxAmount,  " +
                                "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount, SUM(FSWS.EngineWorkAmount) As EngineWorkAmount,  " +
                                "SUM(FSTS.FuelAmount) As FuelAmount,SUM(FSTS.PuncherAmount) As PuncherAmount,  " +
                                "SUM(FSTS.OilAmount) As OilAmount,SUM(FSOS.RadiatorAmount) As RadiatorAmount,  " +
                                "SUM(FSOS.AxleAmount) As AxleAmount, SUM(FSOS.DifferentialAmount) As DifferentialAmount,  " +
                                "SUM(FSTSS.TurboAmount) As TurboAmount, SUM(FSTSS.EcmAmount) AS EcmAmount,  " +
                                "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount " +
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
                                "INNER JOIN [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK) ON (FSMD.VechileNumber = FVD.VehicleNo)  " +
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
        //public JsonResult GetKPLReports(string FromDate, string ToDate, int RegionId)
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

        
    }
}
