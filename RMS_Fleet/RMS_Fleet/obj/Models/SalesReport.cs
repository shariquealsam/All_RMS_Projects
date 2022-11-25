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


        public string TotalVechileByBranch { get; set; }
        public string UploadVechleByBranch { get; set; }


    }

    public class KPLReports
    {
        public string Month { get; set; }
        public string RegionIDD { get; set; }
        public string RegionName { get; set; }
        public string BranchIDD { get; set; }
        public string BranchName { get; set; }
        public string VechileNumber { get; set; }
        public string OpeningKM { get; set; }
        public string ClosingKM { get; set; }
        public string Make { get; set; }
        public string Manufacturing_Year { get; set; }
        public string FuelType { get; set; }
        public string PetroCardNumber { get; set; }
        public string OpeningFuel { get; set; }
        public string FuelPuchasedInLtr { get; set; }
        public string ClosingFuelLtr { get; set; }
        public string FuelCombustion { get; set; }
        public string BillingKM { get; set; }
        public string NonBillingKM { get; set; }
        public string TOTALKM { get; set; }
        public string KMPL { get; set; }
        public string BpNonBp { get; set; }
        public string StdKmpl { get; set; }
        public string TypeOfServices { get; set; }
        public string RouteNumber { get; set; }
        public string UnitName { get; set; }
        public string FuelCostPerKM { get; set; }
        public string FuelRateInRsPerLtr { get; set; }
        public string FuelPurchasedInCash { get; set; }
        public string FuelPuchasedThroughPetroCard { get; set; }
        public string Remarks { get; set; }
        public string ExtraFuelTakenByCard { get; set; }
        public string ExtraAmountSwipedByCard { get; set; }
        public string VendorName { get; set; }
        public string DriverName { get; set; }
        public string DriverPatId { get; set; }
        public string ActualFuelTakenAsPerSTD_KPL { get; set; }

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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Get_from_config", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetBranchDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRegionDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRegionDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetBranchDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetBranchDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet CountDetailsOfVechileByBranch(int BranchId, int RegionId)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(strSqlConnectionString);
                string sql = @"SELECT (SELECT COUNT(*) FROM Fleet_Vehicle_Details WHERE RegionId=@RegionId AND BranchId=@BranchId) AS TOTAL,MODI AS MODIFI FROM (
                            SELECT CASE WHEN COUNT(MODIFIED)>0 THEN COUNT(MODIFIED) ELSE 0 END AS MODI FROM (
                            SELECT COUNT(*) AS MODIFIED,VechileNumber FROM Fleet_KPL_Branch_Details WHERE VechileNumber IN (SELECT VehicleNo FROM Fleet_Vehicle_Details WHERE RegionId=@RegionId AND BranchId=@BranchId)
                            AND FORMAT(Month,'MMMM')=FORMAT(DATEADD(MONTH,-1,GETDATE()),'MMMM')
                            GROUP BY VechileNumber
                            )TAB
                            )TAB2";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@BranchId", BranchId);
                cmd.Parameters.AddWithValue("@RegionId", RegionId);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                //ci.ErrorLog("C:\\RMS_Fleet_App\\", "CountDetailsOfVechileByBranch" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - CountDetailsOfVechileByBranch", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetKPLReports(string Year, string Month, int RegionId, int BranchId, string VechileNumber)
        {
            string Date = "";
            int nMonth = Int32.Parse(Month);
            int  newYear=Int32.Parse(Year);
            
            var daysInMonth = DateTime.DaysInMonth(newYear, nMonth);
            if (Month.Length == 2)
            {
                Date = Year + "-" + Month + "-" + daysInMonth;
            }
            else
            {
                Date = Year + "-0" + Month + "-" + daysInMonth;
            }

            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;
                SqlConnection con = new SqlConnection(strSqlConnectionString);
                sql = "Fleet_KPL_Get_KPL_Report_By_VechlieNumber_test";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Region", RegionId);
                cmd.Parameters.AddWithValue("@Branch", BranchId);
                cmd.Parameters.AddWithValue("@VechileNumber", VechileNumber);
                cmd.Parameters.AddWithValue("@Year", Year);
                cmd.Parameters.AddWithValue("@Month", Month);
                cmd.Parameters.AddWithValue("@Date", Date);

                cmd.CommandTimeout = 1200;

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetKPLReports" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetKPLReports", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetDilyMisReports(string Date)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "Fleet_Daily_MIS_Report";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Date", Date);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetDilyMisReports" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetDilyMisReports", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetDailyMisDashBoard(string Date)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "Fleet_DailyMis_Dashboard_Report";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Date", Date);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetDailyMisDashBoard" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetDailyMisDashBoard", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetDilyMisReports(string Date, string RegionId, string BranchId)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "Fleet_Daily_MIS_Report_User";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Date", Date);
                cmd.Parameters.AddWithValue("@RegionId", RegionId);
                cmd.Parameters.AddWithValue("@BranchId", BranchId);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetDilyMisReports" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetDilyMisReports", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetDailyMisDashBoard(string Date, string RegionId, string BranchId)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "Fleet_DailyMis_Dashboard_Report_User";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Date", Date);
                cmd.Parameters.AddWithValue("@RegionId", RegionId);
                cmd.Parameters.AddWithValue("@BranchId", BranchId);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetDailyMisDashBoard" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetDailyMisDashBoard", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetDateWiseReports(string FromDate, string ToDate)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "Fleet_RepairMaintainance_DateWise_Report";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetDateWiseReports" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetDateWiseReports", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetDateWiseReports(string FromDate, string ToDate, string RegionID)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "Fleet_RepairMaintainance_DateWise_Report_User";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);
                cmd.Parameters.AddWithValue("@RegionID", RegionID);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetDilyMisReports" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetDilyMisReports", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetDocumnetTypeReport(string DocumentType)
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
                //Changed On 22-05-2020

                //sSql = " SELECT RegionName, BranchName, VehicleNumber, Make, Model, ChessisNumber, ExpiryDate, " +
                //        " [Document], ReceiptedAmount, PenaltyAmount, NonReceipteAmount, TotalAmount, IssueDate, PolicyNumber, " +
                //        " IDVValue, NCBAmount, PremiumAmount, Claim, ClaimNumber, TotalRepairAmt, ClaimAmount  " +
                //        " FROM [dbo].[Fleet_Maintenance_Tax_Details] WITH(NOLOCK) " +
                //        " WHERE Document In (" + DocumentType + ") " +
                //        " ORDER BY VehicleNumber,RegionName,BranchName ";

                sSql = " SELECT RegionName, BranchName, FVD.VehicleNo AS VehicleNumber, FMTD.Make, Model, ChessisNumber, ExpiryDate, ReceivingDate, " +
                        " [Document], ReceiptedAmount, PenaltyAmount, NonReceipteAmount, TotalAmount, CASE WHEN FMTD.ModifiedOn IS NULL THEN FMTD.CreatedOn else FMTD.ModifiedOn END  IssueDate, PolicyNumber, " +
                        " IDVValue, NCBAmount, PremiumAmount, Claim, ClaimNumber, TotalRepairAmt, ClaimAmount  " +
                        " FROM [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK) " +
                        " inner JOIN [dbo].[Fleet_Maintenance_Tax_Details] FMTD WITH(NOLOCK)  " +
                        " ON FMTD.VehicleNumber=FVD.VehicleNo and FMTD.Document in (" + DocumentType + ")  " +
                        " ORDER BY FMTD.VehicleNumber,FMTD.RegionName,FMTD.BranchName ";
                            

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetDocumnetTypeReport" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetDocumnetTypeReport", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetDocumnetTypeReport(string DocumentType,string RegionId)
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
                //Changed On 22-05-2020

                //sSql = " SELECT RegionName, BranchName, VehicleNumber, Make, Model, ChessisNumber, ExpiryDate, " +
                //        " [Document], ReceiptedAmount, PenaltyAmount, NonReceipteAmount, TotalAmount, IssueDate, PolicyNumber, " +
                //        " IDVValue, NCBAmount, PremiumAmount, Claim, ClaimNumber, TotalRepairAmt, ClaimAmount  " +
                //        " FROM [dbo].[Fleet_Maintenance_Tax_Details] WITH(NOLOCK) " +
                //        " WHERE Document In (" + DocumentType + ") " +
                //        " ORDER BY VehicleNumber,RegionName,BranchName ";

                sSql = " SELECT RegionName, BranchName, FVD.VehicleNo AS VehicleNumber, FMTD.Make, Model, ChessisNumber, ExpiryDate, ReceivingDate, " +
                        " [Document], ReceiptedAmount, PenaltyAmount, NonReceipteAmount, TotalAmount, CASE WHEN FMTD.ModifiedOn IS NULL THEN FMTD.CreatedOn else FMTD.ModifiedOn END  IssueDate, PolicyNumber, " +
                        " IDVValue, NCBAmount, PremiumAmount, Claim, ClaimNumber, TotalRepairAmt, ClaimAmount  " +
                        " FROM [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK) " +
                        " inner JOIN [dbo].[Fleet_Maintenance_Tax_Details] FMTD WITH(NOLOCK)  " +
                        " ON FMTD.VehicleNumber=FVD.VehicleNo and FMTD.Document in (" + DocumentType + ") and FMTD.RegionId= " + RegionId + " " +
                        " ORDER BY FMTD.VehicleNumber,FMTD.RegionName,FMTD.BranchName ";


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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetDocumnetTypeReport" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetDocumnetTypeReport", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetDailyMisMonthlyReport(string FromDate, string ToDate)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "Fleet_Daily_MIS_Monthly_Report";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 1200;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FromDate", FromDate);
                cmd.Parameters.AddWithValue("@ToDate", ToDate);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetDailyMisMonthlyReport" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetDailyMisMonthlyReport", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetRoutineTrackerReport()
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "Fleet_Routine_Tracker_Report";

                SqlCommand cmd = new SqlCommand(sql, con);

                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetRoutineTrackerReport" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetRoutineTrackerReport", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetMonthlyMisReport(string FromDate, string ToDate)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "MonthlyDailyMisRerpot";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 1200;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StartDate", FromDate);
                cmd.Parameters.AddWithValue("@EndDate", ToDate);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetDailyMisMonthlyReport" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetDailyMisMonthlyReport", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetMonthlyMisReport(string FromDate, string ToDate, string RegionId)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "MonthlyDailyMis_User_Report";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 1200;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StartDate", FromDate);
                cmd.Parameters.AddWithValue("@EndDate", ToDate);
                cmd.Parameters.AddWithValue("@RegionId", RegionId);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetDailyMisMonthlyReport" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetDailyMisMonthlyReport", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetMonthlyHiredReport(string FromDate, string ToDate)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "HiredVehicleRerpot_Admin";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 1200;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StartDate", FromDate);
                cmd.Parameters.AddWithValue("@EndDate", ToDate);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetMonthlyHiredReport" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetMonthlyHiredReport", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetMonthlyHiredReport(string FromDate, string ToDate, string RegionId)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "HiredVehicleRerpot_User";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandTimeout = 1200;

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StartDate", FromDate);
                cmd.Parameters.AddWithValue("@EndDate", ToDate);
                cmd.Parameters.AddWithValue("@RegionId", RegionId);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetMonthlyHiredReport" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetMonthlyHiredReport", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }
    }
}