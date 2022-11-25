using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMS_Fleet.Models
{
    public class SalesReport
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public string VehicleNo { get; set; }

        public string VechileNumber { get; set; }
        public string Make { get; set; }
        public string ManufacturingYear { get; set; }
        public string NoOfTyres { get; set; }
        public string TyreAmount { get; set; }
        public string Battery { get; set; }
        public string Routine { get; set; }
        public string DentingPainting { get; set; }
        public string MinorRepairing { get; set; }
        public string SeatRepair { get; set; }
        public string SelfWork { get; set; }
        public string ElectricalWork { get; set; }
        public string ClutchRepairing { get; set; }
        public string Alternator { get; set; }
        public string LeafPattiSpring { get; set; }
        public string Suspension { get; set; }
        public string GearBox { get; set; }
        public string BreakWork { get; set; }
        public string EngineWork { get; set; }
        public string FuelPump { get; set; }
        public string Puncher { get; set; }
        public string OilTopUp { get; set; }
        public string RadiatorandWaterBody { get; set; }
        public string AxleWork { get; set; }
        public string DifferentialWork { get; set; }
        public string Turbo { get; set; }
        public string EcmSencer { get; set; }
        public string Accidental { get; set; }
        public string TotalValue { get; set; }
        public string InvoiceNo { get; set; }


    }

    public class SalesReportData
    {
        clsImportant ci = new clsImportant();
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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "Get_from_config" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - Get_from_config", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
        }

        public DataSet GetBranchDetails(int RegionID)
        {
            Get_from_config();
            //Dataset 
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = " SELECT BranchId,BranchName FROM [dbo].[Fleet_Branch_Details] WITH (NOLOCK)  WHERE RegionId='" + RegionID + "' Order By BranchName ";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 1200;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetBranchDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetBranchDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetRegionDetails()
        {
            Get_from_config();
            //Dataset 
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = " SELECT RegionId,RegionName  FROM [dbo].[Fleet_Region_Details] WITH (NOLOCK) Order By RegionName ";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 1200;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetRegionDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetRegionDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetRegionWithRegionDetails(string RegionID)
        {
            Get_from_config();
            //Dataset 
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = " SELECT RegionId,RegionName  FROM [dbo].[Fleet_Region_Details] WITH (NOLOCK) Where RegionId = " + RegionID + " Order By RegionName ";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 1200;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetRegionDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetRegionDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetVehicleDetails(int RegionId, int BranchId)
        {
            Get_from_config();
            //Dataset 
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = " SELECT VehicleNo FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) WHERE RegionId = " + RegionId + " ANd BranchId = " + BranchId + " ";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 1200;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetBranchDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetBranchDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetMaintainanceDetails(string FromDate, string ToDate, int RegionId, int BranchId, string VechileNumber)
        {
            Get_from_config();
            //Dataset 
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "SELECT * FROM" +
                                       "( " +
                                       "SELECT RegionName,BranchName,VechileNumber,Make,Manufacturing_Year, " +
                                       "NoOfTyres ,TyreAmount ,BatteryAmount As Battery,RoutineAmount As Routine,DentingAmount As [Denting&Painting],MinorAmount As MinorRepairing, " +
                                       " SeatAmount As SeatRepair,SelfAmount As SelfWork,ElectricalAmount As ElectricalWork,ClutchAmount As ClutchRepairing,AlternatorAmount As Alternator,LeafAmount As [Leaf/PattiSpring], " +
                                       " SuspensionAmount As Suspension,GearBoxAmount As GearBox,BreakWorkAmount As BreakWork,EngineWorkAmount As EngineWork, " +
                                       " FuelAmount As FuelPump,PuncherAmount As Puncher,OilAmount As OilTopUp,RadiatorAmount As RadiatorandWaterBody,AxleAmount As AxleWork,DifferentialAmount As DifferentialWork, " +
                                       " TurboAmount As Turbo,EcmAmount As [Ecm/Sencer], AccidentalTotalAmount As Accidental, " +
                                       "SUM(NoOfTyres+TyreAmount+BatteryAmount+RoutineAmount+DentingAmount+MinorAmount+ " +
                                       "SeatAmount+SelfAmount+ElectricalAmount+ClutchAmount+AlternatorAmount+LeafAmount+ " +
                                       "SuspensionAmount+GearBoxAmount+BreakWorkAmount+EngineWorkAmount+ " +
                                       "FuelAmount+PuncherAmount+OilAmount+RadiatorAmount+AxleAmount+DifferentialAmount+ " +
                                       "TurboAmount+EcmAmount+AccidentalTotalAmount) As TotalValue, " +
                                       "CONCAT('|TyreInvoiceNo :-',TyreInvoiceNo,'|BatteryInvoiceNo :-',BatteryInvoiceNo,'|RoutineInvoiceNo :-',RoutineInvoiceNo, " +
                                       "'|DentingInvoiceNo :-',DentingInvoiceNo,'|MinorInvoiceNo :-',MinorInvoiceNo " +
                                       ",'|SeatInvoiceNo :-',SeatInvoiceNo,'|SelfInvoiceNo :-',SelfInvoiceNo,'|ElectricalInvoiceNo :-', " +
                                       "ElectricalInvoiceNo,'|ClutchInvoiceNo :-',ClutchInvoiceNo,'|AlternatorInvoiceNo :-',AlternatorInvoiceNo, " +
                                       "'|LeafInvoiceNo :-',LeafInvoiceNo,'|SuspensionInvoiceNo :-',SuspensionInvoiceNo,'|GearBoxInvoiceNo :-', " +
                                       "GearBoxInvoiceNo,'|BreakWorkInvoiceNo :-',BreakWorkInvoiceNo,'|EngineWorkInvoiceNo:-',EngineWorkInvoiceNo,'|FuelInvoiceNo :-', " +
                                       "FuelInvoiceNo,'|PuncherInvoiceNo :-',PuncherInvoiceNo,'|OilInvoiceNo :-',OilInvoiceNo,'|RadiatorInvoiceNo :-',RadiatorInvoiceNo, " +
                                       "'|AxleInvoiceNo :-',AxleInvoiceNo,'|DifferentialInvoiceNo:-',DifferentialInvoiceNo " +
                                       ",'|TurboInvoiceNo:-',TurboInvoiceNo,'|EcmInvoiceNo:-',EcmInvoiceNo,'|AccidentalInvoiceNo:-',AccidentalInvoiceNo) As InvoiceNo " +
                                       "FROM " +
                                       "( " +
                                       "SELECT FRD.RegionName,FBD.BranchName,FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year, " +
                                       "SUM(FSMSS.NoOfTyres) As NoOfTyres,SUM(FSMSS.TyreAmount) As TyreAmount, " +
                                       "SUM(FSMSS.BatteryAmount) As BatteryAmount,SUM(FSMSS.RoutineAmount) As RoutineAmount, " +
                                       "SUM(FSMRR.DentingAmount) As DentingAmount,SUM(FSMRR.MinorAmount) As MinorAmount, " +
                                       "SUM(FSMRR.SeatAmount) As SeatAmount, SUM(FSMS.SelfAmount) As SelfAmount, " +
                                       "SUM(FSMS.ElectricalAmount) As ElectricalAmount, SUM(FSMS.ClutchAmount) As ClutchAmount, " +
                                       "SUM(FSMR.AlternatorAmount) As AlternatorAmount, SUM(FSMR.LeafAmount) As LeafAmount, " +
                                       "SUM(FSMR.SuspensionAmount) As SuspensionAmount, SUM(FSWS.GearBoxAmount) As GearBoxAmount, " +
                                       "SUM(FSWS.BreakWorkAmount) As BreakWorkAmount, SUM(FSWS.EngineWorkAmount) As EngineWorkAmount, " +
                                       "SUM(FSTS.FuelAmount) As FuelAmount,SUM(FSTS.PuncherAmount) As PuncherAmount, " +
                                       "SUM(FSTS.OilAmount) As OilAmount,SUM(FSOS.RadiatorAmount) As RadiatorAmount, " +
                                       "SUM(FSOS.AxleAmount) As AxleAmount, SUM(FSOS.DifferentialAmount) As DifferentialAmount, " +
                                       "SUM(FSTSS.TurboAmount) As TurboAmount, SUM(FSTSS.EcmAmount) AS EcmAmount, " +
                                       "SUM(FSTSS.AccidentalTotalAmount) As AccidentalTotalAmount, " +
                                       "FSMSS.TyreInvoiceNo,FSMSS.BatteryInvoiceNo,FSMSS.RoutineInvoiceNo, " +
                                       "FSMRR.DentingInvoiceNo,FSMRR.MinorInvoiceNo,FSMRR.SeatInvoiceNo, " +
                                       "FSMS.SelfInvoiceNo,FSMS.ElectricalInvoiceNo,FSMS.ClutchInvoiceNo, " +
                                       "FSMR.AlternatorInvoiceNo,FSMR.LeafInvoiceNo,FSMR.SuspensionInvoiceNo, " +
                                       "FSWS.GearBoxInvoiceNo,FSWS.BreakWorkInvoiceNo,FSWS.EngineWorkInvoiceNo, " +
                                       "FSTS.FuelInvoiceNo,FSTS.PuncherInvoiceNo,FSTS.OilInvoiceNo, " +
                                       "FSOS.RadiatorInvoiceNo,FSOS.AxleInvoiceNo,FSOS.DifferentialInvoiceNo, " +
                                       "FSTSS.TurboInvoiceNo,FSTSS.EcmInvoiceNo,FSTSS.AccidentalInvoiceNo " +
                                       "FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK) " +
                                       "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMR.Rec_Id) " +
                                       "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMS.Rec_Id) " +
                                       "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMRR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMRR.Rec_Id) " +
                                       "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMSS.Rec_Id) " +
                                       "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON (FSMD.Rec_Id = FSOS.Rec_Id) " +
                                       "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTS.Rec_Id) " +
                                       "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTSS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTSS.Rec_Id) " +
                                       "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON (FSMD.Rec_Id = FSWS.Rec_Id) " +
                                       "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON (FSMD.BranchId = FBD.BranchId) " +
                                       "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON (FSMD.RegionId = FRD.RegionId) " +
                                       "INNER JOIN [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK) ON (FSMD.VechileNumber = FVD.VehicleNo) " +
                                       "WHERE SalesDate Between '" + FromDate + "' and  '" + ToDate + "' AND FSMD.RegionId = " + RegionId + " AND FSMD.BranchId = " + BranchId + " AND FSMD.VechileNumber ='" + VechileNumber + "' " +
                                       "GROUP BY FSMD.VechileNumber,FVD.Make,FVD.Manufacturing_Year,FRD.RegionName,FBD.BranchName, " +
                                       "FSMSS.TyreInvoiceNo,FSMSS.BatteryInvoiceNo,FSMSS.RoutineInvoiceNo, " +
                                       "FSMRR.DentingInvoiceNo,FSMRR.MinorInvoiceNo,FSMRR.SeatInvoiceNo, " +
                                       "FSMS.SelfInvoiceNo,FSMS.ElectricalInvoiceNo,FSMS.ClutchInvoiceNo, " +
                                       "FSMR.AlternatorInvoiceNo,FSMR.LeafInvoiceNo,FSMR.SuspensionInvoiceNo, " +
                                       "FSWS.GearBoxInvoiceNo,FSWS.BreakWorkInvoiceNo,FSWS.EngineWorkInvoiceNo, " +
                                       "FSTS.FuelInvoiceNo,FSTS.PuncherInvoiceNo,FSTS.OilInvoiceNo, " +
                                       "FSOS.RadiatorInvoiceNo,FSOS.AxleInvoiceNo,FSOS.DifferentialInvoiceNo, " +
                                       "FSTSS.TurboInvoiceNo,FSTSS.EcmInvoiceNo,FSTSS.AccidentalInvoiceNo " +
                                       ")TblData " +
                                       "GROUP BY RegionName,BranchName,VechileNumber,Make,Manufacturing_Year, " +
                                       "NoOfTyres,TyreAmount,BatteryAmount,RoutineAmount,DentingAmount,MinorAmount, " +
                                       "SeatAmount,SelfAmount,ElectricalAmount,ClutchAmount,AlternatorAmount,LeafAmount, " +
                                       "SuspensionAmount,GearBoxAmount,BreakWorkAmount,EngineWorkAmount, " +
                                       "FuelAmount,PuncherAmount,OilAmount,RadiatorAmount,AxleAmount,DifferentialAmount, " +
                                       "TurboAmount,EcmAmount,AccidentalTotalAmount,TyreInvoiceNo,BatteryInvoiceNo,RoutineInvoiceNo, " +
                                       "DentingInvoiceNo,MinorInvoiceNo,SeatInvoiceNo, " +
                                       "SelfInvoiceNo,ElectricalInvoiceNo,ClutchInvoiceNo, " +
                                       "AlternatorInvoiceNo,LeafInvoiceNo,SuspensionInvoiceNo, " +
                                       "GearBoxInvoiceNo,BreakWorkInvoiceNo,EngineWorkInvoiceNo, " +
                                       "FuelInvoiceNo,PuncherInvoiceNo,OilInvoiceNo, " +
                                       "RadiatorInvoiceNo,AxleInvoiceNo,DifferentialInvoiceNo, " +
                                       "TurboInvoiceNo,EcmInvoiceNo,AccidentalInvoiceNo " +
                                       ")tbl ";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Connection Time Out
                cmd.CommandTimeout = 1200;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetBranchDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetBranchDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }
    }
}