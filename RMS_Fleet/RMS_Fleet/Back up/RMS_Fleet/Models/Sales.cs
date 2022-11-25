using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMS_Fleet.Models
{
    public class Sales
    {
        public string BranchName { get; set; }
        public string VehicleNumber { get; set; }
        public int BranchId { get; set; }
        public string RegionName { get; set; }
        public string VehicleNo { get; set; }
        public string Make { get; set; }
        public string Manufacturing_Year { get; set; }
        public string RouteNo { get; set; }

        public string NoOfTyres { get; set; }
        public string Tyre { get; set; }
        public string TyreCompanyName { get; set; }
        public string TyreVendorName { get; set; }
        public string TyreSize { get; set; }
        public string TyreInvoiceNo { get; set; }
        public string TyreInvoiceDate { get; set; }
        public string TyreKMReading { get; set; }
        public string TyreAmount { get; set; }
        public string TyreRemarks { get; set; }
        public string NoOfBattery { get; set; }
        public string Battery { get; set; }
        public string BatteryCompanyName { get; set; }
        public string BatteryVendorName { get; set; }
        public string BatteryInvoiceNo { get; set; }
        public string BatteryMSDMPR { get; set; }
        public string BatteryInvoiceDate { get; set; }
        public string BatteryKMReading { get; set; }
        public string BatteryAmount { get; set; }
        public string BatteryRemarks { get; set; }
        public string RoutineVendorName { get; set; }
        public string RoutineDealerType { get; set; }
        public string RoutineInvoiceNo { get; set; }
        public string RoutineInvoiceDate { get; set; }
        public string RoutineKMReading { get; set; }
        public string RoutineAmount { get; set; }
        public string RoutineRemarks { get; set; }
        public string DentingPaintingType { get; set; }
        public string DentingVendorName { get; set; }
        public string DentingInvoiceNo { get; set; }
        public string DentingInvoiceDate { get; set; }
        public string DentingKMReading { get; set; }
        public string DentingAmount { get; set; }
        public string DentingRemarks { get; set; }
        public string MinorVendorName { get; set; }
        public string MinorInvoiceNo { get; set; }
        public string MinorInvoiceDate { get; set; }
        public string MinorKMReading { get; set; }
        public string MinorAmount { get; set; }
        public string MinorRemarks { get; set; }
        public string SeatVendorname { get; set; }
        public string SeatInvoiceNo { get; set; }
        public string SeatInvoiceDate { get; set; }
        public string SeatKMReading { get; set; }
        public string SeatAmount { get; set; }
        public string SeatRemarks { get; set; }
        public string SelfVendorName { get; set; }
        public string SelfDealerType { get; set; }
        public string SelfInvoiceNo { get; set; }
        public string SelfInvoiceDate { get; set; }
        public string SelfKMReading { get; set; }
        public string SelfAmount { get; set; }
        public string SelfRemarks { get; set; }
        public string ElectricalVendorName { get; set; }
        public string ElectricalDealerType { get; set; }
        public string ElectricalInvoiceNo { get; set; }
        public string ElectricalInvoiceDate { get; set; }
        public string ElectricalKMReading { get; set; }
        public string ElectricalAmount { get; set; }
        public string ElectricalRemarks { get; set; }
        public string ClutchVendorName { get; set; }
        public string ClutchDealerType { get; set; }
        public string ClutchInvoiceNo { get; set; }
        public string ClutchInvoiceDate { get; set; }
        public string ClutchKMReading { get; set; }
        public string ClutchAmount { get; set; }
        public string ClutchRemarks { get; set; }
        public string AlternatorVendorName { get; set; }
        public string AlternatorDealerType { get; set; }
        public string AlternatorInvoiceNo { get; set; }
        public string AlternatorInvoiceDate { get; set; }
        public string AlternatorKMReading { get; set; }
        public string AlternatorAmount { get; set; }
        public string AlternatorRemarks { get; set; }
        public string LeafVendorName { get; set; }
        public string LeafDealerType { get; set; }
        public string LeafInvoiceNo { get; set; }
        public string LeafInvoiceDate { get; set; }
        public string LeafKMReading { get; set; }
        public string LeafAmount { get; set; }
        public string LeafRemarks { get; set; }
        public string SuspensionVendorName { get; set; }
        public string SuspensionDealerType { get; set; }
        public string SuspensionInvoiceNo { get; set; }
        public string SuspensionInvoiceDate { get; set; }
        public string SuspensionKMReading { get; set; }
        public string SuspensionAmount { get; set; }
        public string SuspensionRemarks { get; set; }
        public string GearBoxVendorName { get; set; }
        public string GearBoxDealerType { get; set; }
        public string GearBoxInvoiceNo { get; set; }
        public string GearBoxInvoiceDate { get; set; }
        public string GearBoxKMReading { get; set; }
        public string GearBoxAmount { get; set; }
        public string GearBoxRemarks { get; set; }
        public string BreakWorkVendorName { get; set; }
        public string BreakWorkDealerType { get; set; }
        public string BreakWorkInvoiceNo { get; set; }
        public string BreakWorkInvoiceDate { get; set; }
        public string BreakWorkKMReading { get; set; }
        public string BreakWorkAmount { get; set; }
        public string BreakWorkRemarks { get; set; }
        public string EngineWorkVendorName { get; set; }
        public string EngineWorkDealerType { get; set; }
        public string EngineWorkInvoiceNo { get; set; }
        public string EngineWorkInvoiceDate { get; set; }
        public string EngineWorkKMReading { get; set; }
        public string EngineWorkAmount { get; set; }
        public string EngineWorkRemarks { get; set; }
        public string FuelVendorName { get; set; }
        public string FuelDealerType { get; set; }
        public string FuelInvoiceNo { get; set; }
        public string FuelInvoiceDate { get; set; }
        public string FuelKMReading { get; set; }
        public string FuelAmount { get; set; }
        public string FuelRemarks { get; set; }
        public string PuncherVendorName { get; set; }
        public string PuncherNoofPuncher { get; set; }
        public string PuncherInvoiceNo { get; set; }
        public string PuncherInvoiceDate { get; set; }
        public string PuncherKMReading { get; set; }
        public string PuncherAmount { get; set; }
        public string PuncherRemarks { get; set; }
        public string OilVendorName { get; set; }
        public string OilLtr { get; set; }
        public string OilInvoiceNo { get; set; }
        public string OilInvoiceDate { get; set; }
        public string OilKMReading { get; set; }
        public string OilAmount { get; set; }
        public string OilRemarks { get; set; }
        public string RadiatorVendorName { get; set; }
        public string RadiatorDealerType { get; set; }
        public string RadiatorInvoiceNo { get; set; }
        public string RadiatorInvoiceDate { get; set; }
        public string RadiatorKMReading { get; set; }
        public string RadiatorAmount { get; set; }
        public string RadiatorRemarks { get; set; }
        public string AxleVendorName { get; set; }
        public string AxleDealerType { get; set; }
        public string AxleInvoiceNo { get; set; }
        public string AxleInvoiceDate { get; set; }
        public string AxleKMReading { get; set; }
        public string AxleAmount { get; set; }
        public string AxleRemarks { get; set; }
        public string DifferentialVendorName { get; set; }
        public string DifferentialDealerType { get; set; }
        public string DifferentialInvoiceNo { get; set; }
        public string DifferentialInvoiceDate { get; set; }
        public string DifferentialKMReading { get; set; }
        public string DifferentialAmount { get; set; }
        public string DifferentialRemarks { get; set; }
        public string TurboVendorName { get; set; }
        public string TurboDealerType { get; set; }
        public string TurboInvoiceNo { get; set; }
        public string TurboInvoiceDate { get; set; }
        public string TurboKMReading { get; set; }
        public string TurboAmount { get; set; }
        public string TurboNarration { get; set; }
        public string EcmVendorName { get; set; }
        public string EcmDealerType { get; set; }
        public string EcmInvoiceNo { get; set; }
        public string EcmInvoiceDate { get; set; }
        public string EcmKMReading { get; set; }
        public string EcmAmount { get; set; }
        public string EcmNarration { get; set; }
        public string AccidentalVendorName { get; set; }
        public string AccidentalDealerType { get; set; }
        public string AccidentalInvoiceNo { get; set; }
        public string AccidentalInvoiceDate { get; set; }
        public string AccidentalKMReading { get; set; }
        public string AccidentalInsCoveredAmount { get; set; }
        public string AccidentalDifferenceAmount { get; set; }
        public string AccidentalTotalAmount { get; set; }
        public string AccidentalNarration { get; set; }

        public string RecId { get; set; }
        public string RegionId { get; set; }

        public string SalesDate { get; set; }
        public string ActualDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string TotalAmount { get; set; }
        public string Type { get; set; }
        
    }

    public class SalesDetails
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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "Get_from_config_FleetApp" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - Get_from_config_FleetApp", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
        }

        public DataSet GetBranch(string RegionId)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "SELECT BranchId,BranchName FROM [dbo].[Fleet_Branch_Details] WITH (NOLOCK) WHERE RegionId = " + RegionId + "";

                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.Text;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();


            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetBranch" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetBranch", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

        public DataSet GetBranch()
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "SELECT BranchId,BranchName FROM [dbo].[Fleet_Branch_Details] WITH (NOLOCK) WHERE BranchId != 85 ";

                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.Text;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();


            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetBranch" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetBranch", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

        public DataSet GetRegionName(string RegionId)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "SELECT RegionName FROM [dbo].[Fleet_Region_Details] WITH (NOLOCK) WHERE RegionId = " + RegionId + "";

                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.Text;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();


            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetRegionName" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetRegionName", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

        public DataSet GetVehicleMaster(string BranchId)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "SELECT VehicleNo FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) WHERE BranchId = " + BranchId + "";

                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.Text;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();


            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetVehicleMaster" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetVehicleMaster", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

        public DataSet GetVehicleDetails(string VehicleNumber)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "SELECT RegionName,BranchName,Tbl.VehicleNo,Make,Manufacturing_Year,Tbl2.RouteNo FROM" +
                        "( " +
                        "SELECT FRD.RegionName,FBD.BranchName,FVD.VehicleNo,FVD.Make,FVD.Manufacturing_Year  " +
                        "FROM [dbo].[Fleet_Vehicle_Details] FVD WITH (NOLOCK)  " +
                        "INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH (NOLOCK) ON (FVD.BranchId = FBD.BranchId) " +
                        "INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH (NOLOCK) ON (FVD.RegionId = FRD.RegionId) " +
                        "WHERE VehicleNo = '" + VehicleNumber + "' " +
                        ")Tbl " +
                        "LEFT JOIN " +
                        "( " +
                        "SELECT VehicleNo,RouteNo FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK) " +
                        "WHERE RecId = " +
                        "( " +
                        "SELECT MAX(RecId) as RecId FROM [dbo].[Fleet_Reporting_Details] FRD WITH (NOLOCK) WHERE VehicleNo ='" + VehicleNumber + "' " +
                        ") " +
                        ")Tbl2 ON (Tbl.VehicleNo = Tbl2.VehicleNo) " ;

                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.Text;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();


            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetVehicleMaster" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetVehicleMaster", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

        public int SalesDetailsInsert(string NoOfTyres, string Tyre, string TyreCompanyName, string TyreVendorName, string TyreSize, string TyreInvoiceNo, string TyreInvoiceDate,
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
                                    string RegionId, string BranchId, string VechileNumber, string RouteNumber,string Name)
        {
            int iReturn = 0;
            SqlTransaction trans = null;
            //SqlTransaction trans1 = null;
            int iRecID = 0;
            int Action = 0;

            Get_from_config();

            try
            {
                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                string sSql = "Fleet_Sales_Master_Insert";

                //Open Database Connection
                con.Open();

                using (trans = con.BeginTransaction())
                {
                    //Insert User Details
                    SqlCommand cmd = new SqlCommand(sSql, con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Date", DateTime.Now.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@VehicleNumber", VechileNumber);
                    cmd.Parameters.AddWithValue("@RouteNo", RouteNumber);
                    cmd.Parameters.AddWithValue("@RegionId", RegionId);
                    cmd.Parameters.AddWithValue("@BranchId", BranchId);
                    cmd.Parameters.AddWithValue("@CreatedBy", Name);
                    cmd.Parameters.AddWithValue("@ModifiedBy", Name);

                    cmd.Parameters.AddWithValue("@Rec_id", SqlDbType.BigInt);
                    cmd.Parameters["@Rec_id"].Direction = ParameterDirection.Output;


                    cmd.Parameters.AddWithValue("@Action", SqlDbType.BigInt);
                    cmd.Parameters["@Action"].Direction = ParameterDirection.Output;

                    iReturn = cmd.ExecuteNonQuery();


                    iRecID = Convert.ToInt32(cmd.Parameters["@Rec_id"].Value);
                    Action = Convert.ToInt32(cmd.Parameters["@Action"].Value);


                    if (iRecID > 0)
                    {
                        //Insert Fleet_Sales_Minor_Service Details
                        string sSql1 = "Fleet_Sales_Minor_Service_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd1 = new SqlCommand(sSql1, con, trans);
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd1.Parameters.AddWithValue("@NoOfTyres",NoOfTyres);
                        cmd1.Parameters.AddWithValue("@Tyre",Tyre);
                        cmd1.Parameters.AddWithValue("@TyreCompanyName",TyreCompanyName);
                        cmd1.Parameters.AddWithValue("@TyreVendorName",TyreVendorName);
                        cmd1.Parameters.AddWithValue("@TyreSize",TyreSize);
                        cmd1.Parameters.AddWithValue("@TyreInvoiceNo",TyreInvoiceNo);
                        cmd1.Parameters.AddWithValue("@TyreInvoiceDate",TyreInvoiceDate);
                        cmd1.Parameters.AddWithValue("@TyreKMReading",TyreKMReading);
                        cmd1.Parameters.AddWithValue("@TyreAmount", TyreAmount.Replace(",", ""));
                        cmd1.Parameters.AddWithValue("@TyreRemarks",TyreRemarks);
                        cmd1.Parameters.AddWithValue("@NoOfBattery",NoOfBattery);
                        cmd1.Parameters.AddWithValue("@Battery",Battery);
                        cmd1.Parameters.AddWithValue("@BatteryCompanyName",BatteryCompanyName);
                        cmd1.Parameters.AddWithValue("@BatteryVendorName",BatteryVendorName);
                        cmd1.Parameters.AddWithValue("@BatteryInvoiceNo",BatteryInvoiceNo);
                        cmd1.Parameters.AddWithValue("@BatteryMSDMPR",BatteryMSDMPR);
                        cmd1.Parameters.AddWithValue("@BatteryInvoiceDate",BatteryInvoiceDate);
                        cmd1.Parameters.AddWithValue("@BatteryKMReading",BatteryKMReading);
                        cmd1.Parameters.AddWithValue("@BatteryAmount", BatteryAmount.Replace(",", ""));
                        cmd1.Parameters.AddWithValue("@BatteryRemarks",BatteryRemarks);
                        cmd1.Parameters.AddWithValue("@RoutineVendorName",RoutineVendorName);
                        cmd1.Parameters.AddWithValue("@RoutineDealerType",RoutineDealerType);
                        cmd1.Parameters.AddWithValue("@RoutineInvoiceNo",RoutineInvoiceNo);
                        cmd1.Parameters.AddWithValue("@RoutineInvoiceDate",RoutineInvoiceDate);
                        cmd1.Parameters.AddWithValue("@RoutineKMReading",RoutineKMReading);
                        cmd1.Parameters.AddWithValue("@RoutineAmount", RoutineAmount.Replace(",", ""));
                        cmd1.Parameters.AddWithValue("@RoutineRemarks",RoutineRemarks);
                        cmd1.Parameters.AddWithValue("@Name", Name);

                        cmd1.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd1.ExecuteNonQuery();

                    }
                    else
                    {
                        trans.Rollback();
                    }

                    if (iRecID > 0)
                    {
                        //Insert Sales_Minor_Repairing Details
                        string sSql2 = "Fleet_Sales_Minor_Repairing_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd2 = new SqlCommand(sSql2, con, trans);
                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd2.Parameters.AddWithValue("@DentingPaintingType", DentingPaintingType);
                        cmd2.Parameters.AddWithValue("@DentingVendorName", DentingVendorName);
                        cmd2.Parameters.AddWithValue("@DentingInvoiceNo", DentingInvoiceNo);
                        cmd2.Parameters.AddWithValue("@DentingInvoiceDate", DentingInvoiceDate);
                        cmd2.Parameters.AddWithValue("@DentingKMReading", DentingKMReading);
                        cmd2.Parameters.AddWithValue("@DentingAmount", DentingAmount.Replace(",", ""));
                        cmd2.Parameters.AddWithValue("@DentingRemarks", DentingRemarks);
                        cmd2.Parameters.AddWithValue("@MinorVendorName", MinorVendorName);
                        cmd2.Parameters.AddWithValue("@MinorInvoiceNo", MinorInvoiceNo);
                        cmd2.Parameters.AddWithValue("@MinorInvoiceDate", MinorInvoiceDate);
                        cmd2.Parameters.AddWithValue("@MinorKMReading", MinorKMReading);
                        cmd2.Parameters.AddWithValue("@MinorAmount", MinorAmount.Replace(",",""));
                        cmd2.Parameters.AddWithValue("@MinorRemarks", MinorRemarks);
                        cmd2.Parameters.AddWithValue("@SeatVendorname", SeatVendorname);
                        cmd2.Parameters.AddWithValue("@SeatInvoiceNo", SeatInvoiceNo);
                        cmd2.Parameters.AddWithValue("@SeatInvoiceDate", SeatInvoiceDate);
                        cmd2.Parameters.AddWithValue("@SeatKMReading", SeatKMReading);
                        cmd2.Parameters.AddWithValue("@SeatAmount", SeatAmount.Replace(",", ""));
                        cmd2.Parameters.AddWithValue("@SeatRemarks", SeatRemarks);
                        cmd2.Parameters.AddWithValue("@Created_By", Name);

                        cmd2.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd2.ExecuteNonQuery();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                    if (iRecID > 0)
                    {
                        //Insert Sales_Major_Service Details
                        string sSql3 = "[Fleet_Sales_Major_Service_Insert]";

                        //Insert Region Branch Details 
                        SqlCommand cmd3 = new SqlCommand(sSql3, con, trans);
                        cmd3.CommandType = CommandType.StoredProcedure;

                        cmd3.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd3.Parameters.AddWithValue("@SelfVendorName", SelfVendorName);
                        cmd3.Parameters.AddWithValue("@SelfDealerType", SelfDealerType);
                        cmd3.Parameters.AddWithValue("@SelfInvoiceNo", SelfInvoiceNo);
                        cmd3.Parameters.AddWithValue("@SelfInvoiceDate", SelfInvoiceDate);
                        cmd3.Parameters.AddWithValue("@SelfKMReading", SelfKMReading);
                        cmd3.Parameters.AddWithValue("@SelfAmount", SelfAmount.Replace(",", ""));
                        cmd3.Parameters.AddWithValue("@SelfRemarks", SelfRemarks);
                        cmd3.Parameters.AddWithValue("@ElectricalVendorName", ElectricalVendorName);
                        cmd3.Parameters.AddWithValue("@ElectricalDealerType", ElectricalDealerType);
                        cmd3.Parameters.AddWithValue("@ElectricalInvoiceNo", ElectricalInvoiceNo);
                        cmd3.Parameters.AddWithValue("@ElectricalInvoiceDate", ElectricalInvoiceDate);
                        cmd3.Parameters.AddWithValue("@ElectricalKMReading", ElectricalKMReading);
                        cmd3.Parameters.AddWithValue("@ElectricalAmount", ElectricalAmount.Replace(",", ""));
                        cmd3.Parameters.AddWithValue("@ElectricalRemarks", ElectricalRemarks);
                        cmd3.Parameters.AddWithValue("@ClutchVendorName", ClutchVendorName);
                        cmd3.Parameters.AddWithValue("@ClutchDealerType", ClutchDealerType);
                        cmd3.Parameters.AddWithValue("@ClutchInvoiceNo", ClutchInvoiceNo);
                        cmd3.Parameters.AddWithValue("@ClutchInvoiceDate", ClutchInvoiceDate);
                        cmd3.Parameters.AddWithValue("@ClutchKMReading", ClutchKMReading);
                        cmd3.Parameters.AddWithValue("@ClutchAmount", ClutchAmount.Replace(",", ""));
                        cmd3.Parameters.AddWithValue("@ClutchRemarks", ClutchRemarks);
                        cmd3.Parameters.AddWithValue("@Name", Name);

                        cmd3.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd3.ExecuteNonQuery();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                    if (iRecID > 0)
                    {
                        //Insert Fleet_Sales_Major_Repairing Details
                        string sSql4 = "Fleet_Sales_Major_Repairing_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd4 = new SqlCommand(sSql4, con, trans);
                        cmd4.CommandType = CommandType.StoredProcedure;

                        cmd4.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd4.Parameters.AddWithValue("@AlternatorVendorName", AlternatorVendorName);
                        cmd4.Parameters.AddWithValue("@AlternatorDealerType", AlternatorDealerType);
                        cmd4.Parameters.AddWithValue("@AlternatorInvoiceNo", AlternatorInvoiceNo);
                        cmd4.Parameters.AddWithValue("@AlternatorInvoiceDate", AlternatorInvoiceDate);
                        cmd4.Parameters.AddWithValue("@AlternatorKMReading", AlternatorKMReading);
                        cmd4.Parameters.AddWithValue("@AlternatorAmount", AlternatorAmount.Replace(",", ""));
                        cmd4.Parameters.AddWithValue("@AlternatorRemarks", AlternatorRemarks);
                        cmd4.Parameters.AddWithValue("@LeafVendorName", LeafVendorName);
                        cmd4.Parameters.AddWithValue("@LeafDealerType", LeafDealerType);
                        cmd4.Parameters.AddWithValue("@LeafInvoiceNo", LeafInvoiceNo);
                        cmd4.Parameters.AddWithValue("@LeafInvoiceDate", LeafInvoiceDate);
                        cmd4.Parameters.AddWithValue("@LeafKMReading", LeafKMReading);
                        cmd4.Parameters.AddWithValue("@LeafAmount", LeafAmount.Replace(",", ""));
                        cmd4.Parameters.AddWithValue("@LeafRemarks", LeafRemarks);
                        cmd4.Parameters.AddWithValue("@SuspensionVendorName", SuspensionVendorName);
                        cmd4.Parameters.AddWithValue("@SuspensionDealerType", SuspensionDealerType);
                        cmd4.Parameters.AddWithValue("@SuspensionInvoiceNo", SuspensionInvoiceNo);
                        cmd4.Parameters.AddWithValue("@SuspensionInvoiceDate", SuspensionInvoiceDate);
                        cmd4.Parameters.AddWithValue("@SuspensionKMReading", SuspensionKMReading);
                        cmd4.Parameters.AddWithValue("@SuspensionAmount", SuspensionAmount.Replace(",", ""));
                        cmd4.Parameters.AddWithValue("@SuspensionRemarks", SuspensionRemarks);
                        cmd4.Parameters.AddWithValue("@Name", Name);


                        cmd4.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd4.ExecuteNonQuery();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                    if (iRecID > 0)
                    {
                        //Insert Fleet_Sales_WorkBox_Service Details
                        string sSql5 = "Fleet_Sales_WorkBox_Service_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd5 = new SqlCommand(sSql5, con, trans);
                        cmd5.CommandType = CommandType.StoredProcedure;

                        cmd5.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd5.Parameters.AddWithValue("@GearBoxVendorName", GearBoxVendorName);
                        cmd5.Parameters.AddWithValue("@GearBoxDealerType", GearBoxDealerType);
                        cmd5.Parameters.AddWithValue("@GearBoxInvoiceNo", GearBoxInvoiceNo);
                        cmd5.Parameters.AddWithValue("@GearBoxInvoiceDate", GearBoxInvoiceDate);
                        cmd5.Parameters.AddWithValue("@GearBoxKMReading", GearBoxKMReading);
                        cmd5.Parameters.AddWithValue("@GearBoxAmount", GearBoxAmount.Replace(",", ""));
                        cmd5.Parameters.AddWithValue("@GearBoxRemarks", GearBoxRemarks);
                        cmd5.Parameters.AddWithValue("@BreakWorkVendorName", BreakWorkVendorName);
                        cmd5.Parameters.AddWithValue("@BreakWorkDealerType", BreakWorkDealerType);
                        cmd5.Parameters.AddWithValue("@BreakWorkInvoiceNo", BreakWorkInvoiceNo);
                        cmd5.Parameters.AddWithValue("@BreakWorkInvoiceDate", BreakWorkInvoiceDate);
                        cmd5.Parameters.AddWithValue("@BreakWorkKMReading", BreakWorkKMReading);
                        cmd5.Parameters.AddWithValue("@BreakWorkAmount", BreakWorkAmount.Replace(",", ""));
                        cmd5.Parameters.AddWithValue("@BreakWorkRemarks", BreakWorkRemarks);
                        cmd5.Parameters.AddWithValue("@EngineWorkVendorName", EngineWorkVendorName);
                        cmd5.Parameters.AddWithValue("@EngineWorkDealerType", EngineWorkDealerType);
                        cmd5.Parameters.AddWithValue("@EngineWorkInvoiceNo", EngineWorkInvoiceNo);
                        cmd5.Parameters.AddWithValue("@EngineWorkInvoiceDate", EngineWorkInvoiceDate);
                        cmd5.Parameters.AddWithValue("@EngineWorkKMReading", EngineWorkKMReading);
                        cmd5.Parameters.AddWithValue("@EngineWorkAmount", EngineWorkAmount.Replace(",", ""));
                        cmd5.Parameters.AddWithValue("@EngineWorkRemarks", EngineWorkRemarks);

                        cmd5.Parameters.AddWithValue("@Name", Name);


                        cmd5.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd5.ExecuteNonQuery();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                    if (iRecID > 0)
                    {
                        //Insert Fleet_Sales_Topup_Service Details
                        string sSql6 = "Fleet_Sales_Topup_Service_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd6 = new SqlCommand(sSql6, con, trans);
                        cmd6.CommandType = CommandType.StoredProcedure;

                        cmd6.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd6.Parameters.AddWithValue("@FuelVendorName", FuelVendorName);
                        cmd6.Parameters.AddWithValue("@FuelDealerType", FuelDealerType);
                        cmd6.Parameters.AddWithValue("@FuelInvoiceNo", FuelInvoiceNo);
                        cmd6.Parameters.AddWithValue("@FuelInvoiceDate", FuelInvoiceDate);
                        cmd6.Parameters.AddWithValue("@FuelKMReading", FuelKMReading);
                        cmd6.Parameters.AddWithValue("@FuelAmount", FuelAmount.Replace(",", ""));
                        cmd6.Parameters.AddWithValue("@FuelRemarks", FuelRemarks);
                        cmd6.Parameters.AddWithValue("@PuncherVendorName", PuncherVendorName);
                        cmd6.Parameters.AddWithValue("@PuncherNoofPuncher", PuncherNoofPuncher);
                        cmd6.Parameters.AddWithValue("@PuncherInvoiceNo", PuncherInvoiceNo);
                        cmd6.Parameters.AddWithValue("@PuncherInvoiceDate", PuncherInvoiceDate);
                        cmd6.Parameters.AddWithValue("@PuncherKMReading", PuncherKMReading);
                        cmd6.Parameters.AddWithValue("@PuncherAmount", PuncherAmount.Replace(",", ""));
                        cmd6.Parameters.AddWithValue("@PuncherRemarks", PuncherRemarks);
                        cmd6.Parameters.AddWithValue("@OilVendorName", OilVendorName);
                        if (OilLtr == "")
                        {
                            OilLtr = "0.00";
                            cmd6.Parameters.AddWithValue("@OilLtr", Convert.ToDecimal(OilLtr));
                        }
                        else
                        {
                            cmd6.Parameters.AddWithValue("@OilLtr", Convert.ToDecimal(OilLtr));
                        }
                        cmd6.Parameters.AddWithValue("@OilInvoiceNo", OilInvoiceNo);
                        cmd6.Parameters.AddWithValue("@OilInvoiceDate", OilInvoiceDate);
                        cmd6.Parameters.AddWithValue("@OilKMReading", OilKMReading);
                        cmd6.Parameters.AddWithValue("@OilAmount", OilAmount.Replace(",", ""));
                        cmd6.Parameters.AddWithValue("@OilRemarks", OilRemarks);
                        cmd6.Parameters.AddWithValue("@Name", Name);

                        cmd6.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd6.ExecuteNonQuery();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                    if (iRecID > 0)
                    {
                        //Insert Fleet_Sales_Topup_Service Details
                        string sSql7 = "Fleet_Sales_Other_Service_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd7 = new SqlCommand(sSql7, con, trans);
                        cmd7.CommandType = CommandType.StoredProcedure;

                        cmd7.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd7.Parameters.AddWithValue("@RadiatorVendorName", RadiatorVendorName);
                        cmd7.Parameters.AddWithValue("@RadiatorDealerType", RadiatorDealerType);
                        cmd7.Parameters.AddWithValue("@RadiatorInvoiceNo", RadiatorInvoiceNo);
                        cmd7.Parameters.AddWithValue("@RadiatorInvoiceDate", RadiatorInvoiceDate);
                        cmd7.Parameters.AddWithValue("@RadiatorKMReading", RadiatorKMReading);
                        cmd7.Parameters.AddWithValue("@RadiatorAmount", RadiatorAmount.Replace(",", ""));
                        cmd7.Parameters.AddWithValue("@RadiatorRemarks", RadiatorRemarks);
                        cmd7.Parameters.AddWithValue("@AxleVendorName", AxleVendorName);
                        cmd7.Parameters.AddWithValue("@AxleDealerType", AxleDealerType);
                        cmd7.Parameters.AddWithValue("@AxleInvoiceNo", AxleInvoiceNo);
                        cmd7.Parameters.AddWithValue("@AxleInvoiceDate", AxleInvoiceDate);
                        cmd7.Parameters.AddWithValue("@AxleKMReading", AxleKMReading);
                        cmd7.Parameters.AddWithValue("@AxleAmount", AxleAmount.Replace(",", ""));
                        cmd7.Parameters.AddWithValue("@AxleRemarks", AxleRemarks);
                        cmd7.Parameters.AddWithValue("@DifferentialVendorName", DifferentialVendorName);
                        cmd7.Parameters.AddWithValue("@DifferentialDealerType", DifferentialDealerType);
                        cmd7.Parameters.AddWithValue("@DifferentialInvoiceNo", DifferentialInvoiceNo);
                        cmd7.Parameters.AddWithValue("@DifferentialInvoiceDate", DifferentialInvoiceDate);
                        cmd7.Parameters.AddWithValue("@DifferentialKMReading", DifferentialKMReading);
                        cmd7.Parameters.AddWithValue("@DifferentialAmount", DifferentialAmount.Replace(",", ""));
                        cmd7.Parameters.AddWithValue("@DifferentialRemarks", DifferentialRemarks);
                        cmd7.Parameters.AddWithValue("@Name", Name);

                        cmd7.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd7.ExecuteNonQuery();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                    if (iRecID > 0)
                    {
                        //Insert Fleet_Sales_Topup_Service Details
                        string sSql8 = "Fleet_Sales_Turbo_Service_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd8 = new SqlCommand(sSql8, con, trans);
                        cmd8.CommandType = CommandType.StoredProcedure;
                        cmd8.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd8.Parameters.AddWithValue("@TurboVendorName", TurboVendorName);
                        cmd8.Parameters.AddWithValue("@TurboDealerType", TurboDealerType);
                        cmd8.Parameters.AddWithValue("@TurboInvoiceNo", TurboInvoiceNo);
                        cmd8.Parameters.AddWithValue("@TurboInvoiceDate", TurboInvoiceDate);
                        cmd8.Parameters.AddWithValue("@TurboKMReading", TurboKMReading);
                        cmd8.Parameters.AddWithValue("@TurboAmount", TurboAmount.Replace(",", ""));
                        cmd8.Parameters.AddWithValue("@TurboNarration", TurboNarration);
                        cmd8.Parameters.AddWithValue("@EcmVendorName", EcmVendorName);
                        cmd8.Parameters.AddWithValue("@EcmDealerType", EcmDealerType);
                        cmd8.Parameters.AddWithValue("@EcmInvoiceNo", EcmInvoiceNo);
                        cmd8.Parameters.AddWithValue("@EcmInvoiceDate", EcmInvoiceDate);
                        cmd8.Parameters.AddWithValue("@EcmKMReading", EcmKMReading);
                        cmd8.Parameters.AddWithValue("@EcmAmount", EcmAmount.Replace(",", ""));
                        cmd8.Parameters.AddWithValue("@EcmNarration", EcmNarration);
                        cmd8.Parameters.AddWithValue("@AccidentalVendorName", AccidentalVendorName);
                        cmd8.Parameters.AddWithValue("@AccidentalDealerType", AccidentalDealerType);
                        cmd8.Parameters.AddWithValue("@AccidentalInvoiceNo", AccidentalInvoiceNo);
                        cmd8.Parameters.AddWithValue("@AccidentalInvoiceDate", AccidentalInvoiceDate);
                        cmd8.Parameters.AddWithValue("@AccidentalKMReading", AccidentalKMReading);
                        cmd8.Parameters.AddWithValue("@AccidentalInsCoveredAmount", AccidentalInsCoveredAmount.Replace(",", ""));
                        cmd8.Parameters.AddWithValue("@AccidentalDifferenceAmount", AccidentalDifferenceAmount.Replace(",", ""));
                        cmd8.Parameters.AddWithValue("@AccidentalTotalAmount", AccidentalTotalAmount.Replace(",", ""));
                        cmd8.Parameters.AddWithValue("@AccidentalNarration", AccidentalNarration);
                        
                        cmd8.Parameters.AddWithValue("@Name", Name);
                        cmd8.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd8.ExecuteNonQuery();
                    }
                    else
                    {
                        trans.Rollback();
                    }
                    trans.Commit();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                iReturn = 0;
                trans.Rollback();

                ci.ErrorLog("C:\\RMS_Fleet_App\\", "SalesDetailsInsert" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - SalesDetailsInsert", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturn;
        }

        public DataSet GetSalesEntryDetails(string VehicleNumber, string BranchID, string Date,string RegionID)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "SELECT * FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK) " +
                       " INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMR.Rec_Id) " +
                       " INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMS.Rec_Id) " +
                       " INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMMS WITH(NOLOCK) ON (FSMD.Rec_Id = FSMMS.Rec_Id) " +
                       " INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMMR WITH(NOLOCK) ON (FSMD.Rec_Id = FSMMR.Rec_Id) " +
                       " INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON (FSMD.Rec_Id = FSWS.Rec_Id) " +
                       " INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTS.Rec_Id) " +
                       " INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON (FSMD.Rec_Id = FSOS.Rec_Id) " +
                       " INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTTS WITH(NOLOCK) ON (FSMD.Rec_Id = FSTTS.Rec_Id) " +
                       " WHERE [VechileNumber] ='" + VehicleNumber + "' AND [SalesDate] = '" + Date + "' AND [RegionId] ='" + RegionID + "' AND [BranchId] ='" + BranchID + "' ";

                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.Text;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();


            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetSalesEntryDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetSalesEntryDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

        public DataSet GetRegionIdDetails(string VechileNumber, string BranchId)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();
            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "SELECT RegionId FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) WHERE VehicleNo = '" + VechileNumber + "' AND BranchId = " + BranchId +" ";

                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.Text;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();


            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetSalesEntryDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetSalesEntryDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

        public DataSet GetRegionName()
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                sSql = "SELECT RegionId,RegionName FROM [dbo].[Fleet_Region_Details] WITH (NOLOCK)";

                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.Text;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();


            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetRegionName" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetRegionName", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
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

        public DataSet SearchVehicleDetails(string FromDate, string ToDate, int RegionId, int BranchId, string VehicleNumber)
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
                // Query Changed On 2019-05-16 for addition of type segment in the query

                //sSql = "SELECT tab.Rec_Id,VechileNumber,SalesDate,RouteNumber," +
                //        "tab.RegionId,Region.RegionName,tab.BranchId,Branch.BranchName, " +
                //        "Actual_Date_Time,tab.Created_By, " +
                //        "SUM([AlternatorAmount]+[LeafAmount]+[SuspensionAmount]+ " +
                //        "	[SelfAmount]+[ElectricalAmount]+[ClutchAmount]+ " +
                //        "	[TyreAmount]+[BatteryAmount]+[RoutineAmount]+ " +
                //        "	[RadiatorAmount]+[AxleAmount]+[DifferentialAmount]+ " +
                //        "	[FuelAmount]+[PuncherAmount]+[OilAmount]+ " +
                //        "	[TurboAmount]+[EcmAmount]+[AccidentalTotalAmount]+ " +
                //        "	[GearBoxAmount]+[BreakWorkAmount]+[EngineWorkAmount]+ " +
                //        "	[DentingAmount]+[MinorAmount]+[SeatAmount]) as TotalAmount " +
                //        "FROM ( " +
                //        "SELECT FSMD.Rec_Id,SalesDate,VechileNumber,RouteNumber, " +
                //        "RegionId,BranchId,Actual_Date_Time,FSMD.Created_By, " +
                //        "[AlternatorAmount],[LeafAmount],[SuspensionAmount], " +
                //        "[SelfAmount],[ElectricalAmount],[ClutchAmount], " +
                //        "[TyreAmount],[BatteryAmount],[RoutineAmount], " +
                //        "[RadiatorAmount],[AxleAmount],[DifferentialAmount], " +
                //        "[FuelAmount],[PuncherAmount],[OilAmount], " +
                //        "[TurboAmount],[EcmAmount],[AccidentalTotalAmount], " +
                //        "[GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount], " +
                //        "[DentingAmount],[MinorAmount],[SeatAmount] " +
                //        "FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK) " +
                //        "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON FSMD.Rec_Id=FSMR.Rec_Id " +
                //        "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON FSMD.Rec_Id=FSMS.Rec_Id " +
                //        "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMMS WITH(NOLOCK) ON FSMD.Rec_Id=FSMMS.Rec_Id " +
                //        "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON FSMD.Rec_Id=FSOS.Rec_Id " +
                //        "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON FSMD.Rec_Id=FSTS.Rec_Id " +
                //        "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTTS WITH(NOLOCK) ON FSMD.Rec_Id=FSTTS.Rec_Id " +
                //        "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON FSMD.Rec_Id=FSWS.Rec_Id " +
                //        "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMMR WITH(NOLOCK) ON FSMD.Rec_Id=FSMMR.Rec_Id " +
                //        ")tab " +
                //        "INNER JOIN [dbo].[Fleet_Region_Details] Region WITH(NOLOCK) ON tab.RegionId=Region.RegionId " +
                //        "INNER JOIN [dbo].[Fleet_Branch_Details] Branch WITH (NOLOCK) ON tab.BranchId=Branch.BranchID " +
                //        " " +
                //        "WHERE SalesDate >='" + FromDate + "' AND SalesDate <= '" + ToDate + "' AND tab.RegionId = '" + RegionId + "' AND tab.BranchId='" + BranchId + "' AND  " +
                //        "VechileNumber ='" + VehicleNumber + "' " +
                //        "GROUP BY tab.Rec_Id,VechileNumber,SalesDate,RouteNumber,tab.RegionId,tab.BranchId, " +
                //        "Actual_Date_Time,tab.Created_By,Region.RegionName,Branch.BranchName " ;

                // Update query with Type selected
                sSql ="SELECT Rec_Id,VechileNumber,SalesDate,RouteNumber," +
                    "RegionId,RegionName,BranchId,BranchName, " +
                    "Actual_Date_Time,Created_By,TotalAmount, " +
                    "REPLACE(RTRIM(LTRIM(CONCAT(a,+' '+ b,+' '+ c, +' '+ d,+' '+ e ,+' '+ f ,+' '+ g ,+' '+ h ,+' '+ i ,+' '+ j ,+' '+ k ,+' '+ l ,+' '+ m ,+' '+ n ,+' '+ o ,+' '+ p ,+' '+ q ,+' '+ r ,+' '+ s ,+' '+ t ,+' '+ u ,+' '+ v ,+' '+ w ,+' '+ x))),' ','') As Type " +
                    " FROM  " +
                    "( " +
                    "	SELECT Rec_Id,VechileNumber,SalesDate,RouteNumber, " +
                    "	tab.RegionId,Region.RegionName,tab.BranchId,Branch.BranchName, " +
                    "	Actual_Date_Time,tab.Created_By, " +
                    "	SUM([AlternatorAmount]+[LeafAmount]+[SuspensionAmount]+ " +
                    "		[SelfAmount]+[ElectricalAmount]+[ClutchAmount]+ " +
                    "		[TyreAmount]+[BatteryAmount]+[RoutineAmount]+ " +
                    "		[RadiatorAmount]+[AxleAmount]+[DifferentialAmount]+ " +
                    "		[FuelAmount]+[PuncherAmount]+[OilAmount]+ " +
                    "		[TurboAmount]+[EcmAmount]+[AccidentalTotalAmount]+ " +
                    "		[GearBoxAmount]+[BreakWorkAmount]+[EngineWorkAmount]+ " +
                    "		[DentingAmount]+[MinorAmount]+[SeatAmount]) as TotalAmount, " +
                    "		A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X " +
                    "	FROM ( " +
                    "			SELECT FSMD.Rec_Id,SalesDate,VechileNumber,RouteNumber, " +
                    "			RegionId,BranchId,Actual_Date_Time,FSMD.Created_By, " +
                    "			[AlternatorAmount],[LeafAmount],[SuspensionAmount], " +
                    "			[SelfAmount],[ElectricalAmount],[ClutchAmount], " +
                    "			[TyreAmount],[BatteryAmount],[RoutineAmount], " +
                    "			[RadiatorAmount],[AxleAmount],[DifferentialAmount], " +
                    "			[FuelAmount],[PuncherAmount],[OilAmount], " +
                    "			[TurboAmount],[EcmAmount],[AccidentalTotalAmount], " +
                    "			[GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount], " +
                    "			[DentingAmount],[MinorAmount],[SeatAmount], " +
                    "				CASE WHEN [AlternatorAmount]>0 THEN '[AlternatorAmount]' ELSE '' END AS a, " +
                    "				CASE WHEN [LeafAmount]>0 THEN '[LeafAmount]'  ELSE '' END AS b, " +
                    "				CASE WHEN [SuspensionAmount]>0 THEN '[SuspensionAmount]'  ELSE '' END AS c, " +
                    "				CASE WHEN [SelfAmount]>0 THEN '[SelfAmount]'  ELSE '' END AS d, " +
                    "				CASE WHEN [ElectricalAmount]>0 THEN '[ElectricalAmount]'  ELSE '' END AS e, " +
                    "				CASE WHEN [ClutchAmount]>0 THEN '[ClutchAmount]'  ELSE '' END AS f, " +
                    "				CASE WHEN [TyreAmount]>0 THEN '[TyreAmount]'  ELSE '' END AS g, " +
                    "				CASE WHEN [BatteryAmount]>0 THEN '[BatteryAmount]'  ELSE '' END AS h, " +
                    "				CASE WHEN [RoutineAmount]>0 THEN '[RoutineAmount]'  ELSE '' END AS i, " +
                    "				CASE WHEN [RadiatorAmount]>0 THEN '[RadiatorAmount]'  ELSE '' END AS j, " +
                    "				CASE WHEN [AxleAmount]>0 THEN '[AxleAmount]'  ELSE '' END AS k, " +
                    "				CASE WHEN [DifferentialAmount]>0 THEN '[DifferentialAmount]'  ELSE '' END AS L, " +
                    "				CASE WHEN [FuelAmount]>0 THEN '[FuelAmount]'  ELSE '' END AS m, " +
                    "				CASE WHEN [PuncherAmount]>0 THEN '[PuncherAmount]'  ELSE '' END AS n, " +
                    "				CASE WHEN [OilAmount]>0 THEN '[OilAmount]'  ELSE '' END AS o, " +
                    "				CASE WHEN [TurboAmount]>0 THEN '[TurboAmount]'  ELSE '' END AS p, " +
                    "				CASE WHEN [EcmAmount]>0 THEN '[EcmAmount]'  ELSE '' END AS q, " +
                    "				CASE WHEN [AccidentalTotalAmount]>0 THEN '[AccidentalTotalAmount]'  ELSE '' END AS r, " +
                    "				CASE WHEN [GearBoxAmount]>0 THEN '[GearBoxAmount]'  ELSE '' END AS s, " +
                    "				CASE WHEN [BreakWorkAmount]>0 THEN '[BreakWorkAmount]'  ELSE '' END AS t, " +
                    "				CASE WHEN [EngineWorkAmount]>0 THEN '[EngineWorkAmount]'  ELSE '' END AS u, " +
                    "				CASE WHEN [DentingAmount]>0 THEN '[DentingAmount]'  ELSE '' END AS v, " +
                    "				CASE WHEN [MinorAmount]>0 THEN '[MinorAmount]'  ELSE '' END AS w, " +
                    "				CASE WHEN [SeatAmount]>0 THEN '[SeatAmount]'  ELSE '' END AS x " +
                    " " +
                    "				FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK) " +
                    "				INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON FSMD.Rec_Id=FSMR.Rec_Id " +
                    "				INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON FSMD.Rec_Id=FSMS.Rec_Id " +
                    "				INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMMS WITH(NOLOCK) ON FSMD.Rec_Id=FSMMS.Rec_Id " +
                    "				INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON FSMD.Rec_Id=FSOS.Rec_Id " +
                    "				INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON FSMD.Rec_Id=FSTS.Rec_Id " +
                    "				INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTTS WITH(NOLOCK) ON FSMD.Rec_Id=FSTTS.Rec_Id " +
                    "				INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON FSMD.Rec_Id=FSWS.Rec_Id " +
                    "				INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMMR WITH(NOLOCK) ON FSMD.Rec_Id=FSMMR.Rec_Id " +
                    " " +
                    "				GROUP BY FSMD.Rec_Id,SalesDate,VechileNumber,RouteNumber,RegionId,BranchId,Actual_Date_Time,FSMD.Created_By, " +
                    "				[AlternatorAmount],[LeafAmount],[SuspensionAmount], " +
                    "				[SelfAmount],[ElectricalAmount],[ClutchAmount], " +
                    "				[TyreAmount],[BatteryAmount],[RoutineAmount], " +
                    "				[RadiatorAmount],[AxleAmount],[DifferentialAmount], " +
                    "				[FuelAmount],[PuncherAmount],[OilAmount], " +
                    "				[TurboAmount],[EcmAmount],[AccidentalTotalAmount], " +
                    "				[GearBoxAmount],[BreakWorkAmount],[EngineWorkAmount], " +
                    "				[DentingAmount],[MinorAmount],[SeatAmount],VechileNumber " +
                    "				)tab " +
                    "		INNER JOIN [dbo].[Fleet_Region_Details] Region WITH(NOLOCK) ON tab.RegionId=Region.RegionId " +
                    "		INNER JOIN [dbo].[Fleet_Branch_Details] Branch WITH (NOLOCK) ON tab.BranchId=Branch.BranchID " +
                    " " +
                    "		WHERE SalesDate >='" + FromDate + "' AND SalesDate <= '" + ToDate + "' AND tab.RegionId = '" + RegionId + "' AND tab.BranchId='" + BranchId + "' AND VechileNumber ='" + VehicleNumber + "'  " +
                    "		GROUP BY Rec_Id,VechileNumber,SalesDate,RouteNumber,tab.RegionId,tab.BranchId, " +
                    "		Actual_Date_Time,tab.Created_By,Region.RegionName,Branch.BranchName,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V, " +
                    "		W,X " +
                    "	)tbl " ;
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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "SearchVehicleDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - SearchVehicleDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetEntryDetails(string RecId)
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
                sSql = "SELECT FSMD.Rec_Id,SalesDate,VechileNumber,RouteNumber,RegionId,BranchId,Actual_Date_Time," +
                        "AlternatorVendorName,AlternatorDealerType,AlternatorInvoiceNo,AlternatorInvoiceDate,AlternatorKMReading, " +
                        "AlternatorAmount,AlternatorRemarks,LeafVendorName,LeafDealerType,LeafInvoiceNo,LeafInvoiceDate,LeafKMReading, " +
                        "LeafAmount,LeafRemarks,SuspensionVendorName,SuspensionDealerType,SuspensionInvoiceNo,SuspensionInvoiceDate, " +
                        "SuspensionKMReading,SuspensionAmount,SuspensionRemarks,SelfVendorName,SelfDealerType, " +
                        "SelfInvoiceNo,SelfInvoiceDate,SelfKMReading,SelfAmount,SelfRemarks,ElectricalVendorName,ElectricalDealerType, " +
                        "ElectricalInvoiceNo,ElectricalInvoiceDate,ElectricalKMReading,ElectricalAmount,ElectricalRemarks,ClutchVendorName, " +
                        "ClutchDealerType,ClutchInvoiceNo,ClutchInvoiceDate,ClutchKMReading,ClutchAmount,ClutchRemarks, " +
                        "DentingPaintingType,DentingVendorName,DentingInvoiceNo,DentingInvoiceDate,DentingKMReading,DentingAmount, " +
                        "DentingRemarks,MinorVendorName,MinorInvoiceNo,MinorInvoiceDate,MinorKMReading,MinorAmount,MinorRemarks, " +
                        "SeatVendorname,SeatInvoiceNo,SeatInvoiceDate,SeatKMReading,SeatAmount,SeatRemarks,NoOfTyres,Tyre,TyreCompanyName, " +
                        "TyreVendorName,TyreSize,TyreInvoiceNo,TyreInvoiceDate,TyreKMReading,TyreAmount,TyreRemarks,NoOfBattery,Battery, " +
                        "BatteryCompanyName,BatteryVendorName,BatteryInvoiceNo,BatteryMSDMPR,BatteryInvoiceDate,BatteryKMReading,BatteryAmount, " +
                        "BatteryRemarks,RoutineVendorName,RoutineDealerType,RoutineInvoiceNo,RoutineInvoiceDate,RoutineKMReading, " +
                        "RoutineAmount,RoutineRemarks,RadiatorVendorName,RadiatorDealerType,RadiatorInvoiceNo, " +
                        "RadiatorInvoiceDate,RadiatorKMReading,RadiatorAmount,RadiatorRemarks,AxleVendorName,AxleDealerType,AxleInvoiceNo, " +
                        "AxleInvoiceDate,AxleKMReading,AxleAmount,AxleRemarks,DifferentialVendorName,DifferentialDealerType, " +
                        "DifferentialInvoiceNo,DifferentialInvoiceDate,DifferentialKMReading,DifferentialAmount,DifferentialRemarks, " +
                        "FuelVendorName,FuelDealerType,FuelInvoiceNo,FuelInvoiceDate,FuelKMReading,FuelAmount,FuelRemarks,PuncherVendorName, " +
                        "PuncherNoofPuncher,PuncherInvoiceNo,PuncherInvoiceDate,PuncherKMReading,PuncherAmount,PuncherRemarks, " +
                        "OilVendorName,OilLtr,OilInvoiceNo,OilInvoiceDate,OilKMReading,OilAmount,OilRemarks,TurboVendorName,TurboDealerType, " +
                        "TurboInvoiceNo,TurboInvoiceDate,TurboKMReading,TurboAmount,TurboNarration,EcmVendorName,EcmDealerType,EcmInvoiceNo, " +
                        "EcmInvoiceDate,EcmKMReading,EcmAmount,EcmNarration,AccidentalVendorName,AccidentalDealerType,AccidentalInvoiceNo, " +
                        "AccidentalInvoiceDate,AccidentalKMReading,AccidentalInsCoveredAmount,AccidentalDifferenceAmount,AccidentalTotalAmount, " +
                        "AccidentalNarration,GearBoxVendorName,GearBoxDealerType,GearBoxInvoiceNo, " +
                        "GearBoxInvoiceDate,GearBoxKMReading,GearBoxAmount,GearBoxRemarks,BreakWorkVendorName,BreakWorkDealerType, " +
                        "BreakWorkInvoiceNo,BreakWorkInvoiceDate,BreakWorkKMReading,BreakWorkAmount,BreakWorkRemarks,EngineWorkVendorName, " +
                        "EngineWorkDealerType,EngineWorkInvoiceNo,EngineWorkInvoiceDate,EngineWorkKMReading,EngineWorkAmount, " +
                        "EngineWorkRemarks " +
                        " " +
                        "FROM [dbo].[Fleet_Sales_Master_Details] FSMD WITH(NOLOCK) " +
                        "INNER JOIN [dbo].[Fleet_Sales_Major_Repairing] FSMR WITH(NOLOCK) ON FSMD.Rec_id = FSMR.Rec_id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Major_Service] FSMS WITH(NOLOCK) ON FSMD.Rec_id = FSMS.Rec_id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Minor_Repairing] FSMRR WITH(NOLOCK) ON FSMD.Rec_id = FSMRR.Rec_id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Minor_Service] FSMSS WITH(NOLOCK) ON FSMD.Rec_id = FSMSS.Rec_id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Other_Service] FSOS WITH(NOLOCK) ON FSMD.Rec_id = FSOS.Rec_id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Topup_Service] FSTS WITH(NOLOCK) ON FSMD.Rec_id = FSTS.Rec_id " +
                        "INNER JOIN [dbo].[Fleet_Sales_Turbo_Service] FSTSS WITH(NOLOCK) ON FSMD.Rec_id = FSTSS.Rec_id " +
                        "INNER JOIN [dbo].[Fleet_Sales_WorkBox_Service] FSWS WITH(NOLOCK) ON FSMD.Rec_id = FSWS.Rec_id " +
                        "WHERE FSMD.Rec_id ='" + RecId + "' ";

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "SearchVehicleDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - SearchVehicleDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public int SalesDetailsUpdate(string NoOfTyres, string Tyre, string TyreCompanyName, string TyreVendorName, string TyreSize, string TyreInvoiceNo, string TyreInvoiceDate,
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
                                    string RecId, string Name)
        {
            int iReturn = 0;
            SqlTransaction trans = null;
            //SqlTransaction trans1 = null;
            int iRecID = 0;
            int Action = 0;
           
            Get_from_config();

            try
            {
                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //Open Database Connection
                con.Open();

                using (trans = con.BeginTransaction())
                {

                    iRecID = Convert.ToInt32(RecId);
                    Action = Convert.ToInt32(2);


                    if (iRecID > 0)
                    {
                        //Insert Fleet_Sales_Minor_Service Details
                        string sSql1 = "Fleet_Sales_Minor_Service_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd1 = new SqlCommand(sSql1, con, trans);
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd1.Parameters.AddWithValue("@NoOfTyres", NoOfTyres);
                        cmd1.Parameters.AddWithValue("@Tyre", Tyre);
                        cmd1.Parameters.AddWithValue("@TyreCompanyName", TyreCompanyName);
                        cmd1.Parameters.AddWithValue("@TyreVendorName", TyreVendorName);
                        cmd1.Parameters.AddWithValue("@TyreSize", TyreSize);
                        cmd1.Parameters.AddWithValue("@TyreInvoiceNo", TyreInvoiceNo);
                        cmd1.Parameters.AddWithValue("@TyreInvoiceDate", TyreInvoiceDate);
                        cmd1.Parameters.AddWithValue("@TyreKMReading", TyreKMReading);
                        cmd1.Parameters.AddWithValue("@TyreAmount", TyreAmount.Replace(",", ""));
                        cmd1.Parameters.AddWithValue("@TyreRemarks", TyreRemarks);
                        cmd1.Parameters.AddWithValue("@NoOfBattery", NoOfBattery);
                        cmd1.Parameters.AddWithValue("@Battery", Battery);
                        cmd1.Parameters.AddWithValue("@BatteryCompanyName", BatteryCompanyName);
                        cmd1.Parameters.AddWithValue("@BatteryVendorName", BatteryVendorName);
                        cmd1.Parameters.AddWithValue("@BatteryInvoiceNo", BatteryInvoiceNo);
                        cmd1.Parameters.AddWithValue("@BatteryMSDMPR", BatteryMSDMPR);
                        cmd1.Parameters.AddWithValue("@BatteryInvoiceDate", BatteryInvoiceDate);
                        cmd1.Parameters.AddWithValue("@BatteryKMReading", BatteryKMReading);
                        cmd1.Parameters.AddWithValue("@BatteryAmount", BatteryAmount.Replace(",", ""));
                        cmd1.Parameters.AddWithValue("@BatteryRemarks", BatteryRemarks);
                        cmd1.Parameters.AddWithValue("@RoutineVendorName", RoutineVendorName);
                        cmd1.Parameters.AddWithValue("@RoutineDealerType", RoutineDealerType);
                        cmd1.Parameters.AddWithValue("@RoutineInvoiceNo", RoutineInvoiceNo);
                        cmd1.Parameters.AddWithValue("@RoutineInvoiceDate", RoutineInvoiceDate);
                        cmd1.Parameters.AddWithValue("@RoutineKMReading", RoutineKMReading);
                        cmd1.Parameters.AddWithValue("@RoutineAmount", RoutineAmount.Replace(",", ""));
                        cmd1.Parameters.AddWithValue("@RoutineRemarks", RoutineRemarks);
                        cmd1.Parameters.AddWithValue("@Name", Name);

                        cmd1.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd1.ExecuteNonQuery();

                    }
                    else
                    {
                        trans.Rollback();
                    }

                    if (iRecID > 0)
                    {
                        //Insert Sales_Minor_Repairing Details
                        string sSql2 = "Fleet_Sales_Minor_Repairing_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd2 = new SqlCommand(sSql2, con, trans);
                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd2.Parameters.AddWithValue("@DentingPaintingType", DentingPaintingType);
                        cmd2.Parameters.AddWithValue("@DentingVendorName", DentingVendorName);
                        cmd2.Parameters.AddWithValue("@DentingInvoiceNo", DentingInvoiceNo);
                        cmd2.Parameters.AddWithValue("@DentingInvoiceDate", DentingInvoiceDate);
                        cmd2.Parameters.AddWithValue("@DentingKMReading", DentingKMReading);
                        cmd2.Parameters.AddWithValue("@DentingAmount", DentingAmount.Replace(",", ""));
                        cmd2.Parameters.AddWithValue("@DentingRemarks", DentingRemarks);
                        cmd2.Parameters.AddWithValue("@MinorVendorName", MinorVendorName);
                        cmd2.Parameters.AddWithValue("@MinorInvoiceNo", MinorInvoiceNo);
                        cmd2.Parameters.AddWithValue("@MinorInvoiceDate", MinorInvoiceDate);
                        cmd2.Parameters.AddWithValue("@MinorKMReading", MinorKMReading);
                        cmd2.Parameters.AddWithValue("@MinorAmount", MinorAmount.Replace(",", ""));
                        cmd2.Parameters.AddWithValue("@MinorRemarks", MinorRemarks);
                        cmd2.Parameters.AddWithValue("@SeatVendorname", SeatVendorname);
                        cmd2.Parameters.AddWithValue("@SeatInvoiceNo", SeatInvoiceNo);
                        cmd2.Parameters.AddWithValue("@SeatInvoiceDate", SeatInvoiceDate);
                        cmd2.Parameters.AddWithValue("@SeatKMReading", SeatKMReading);
                        cmd2.Parameters.AddWithValue("@SeatAmount", SeatAmount.Replace(",", ""));
                        cmd2.Parameters.AddWithValue("@SeatRemarks", SeatRemarks);
                        cmd2.Parameters.AddWithValue("@Created_By", Name);

                        cmd2.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd2.ExecuteNonQuery();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                    if (iRecID > 0)
                    {
                        //Insert Sales_Major_Service Details
                        string sSql3 = "[Fleet_Sales_Major_Service_Insert]";

                        //Insert Region Branch Details 
                        SqlCommand cmd3 = new SqlCommand(sSql3, con, trans);
                        cmd3.CommandType = CommandType.StoredProcedure;

                        cmd3.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd3.Parameters.AddWithValue("@SelfVendorName", SelfVendorName);
                        cmd3.Parameters.AddWithValue("@SelfDealerType", SelfDealerType);
                        cmd3.Parameters.AddWithValue("@SelfInvoiceNo", SelfInvoiceNo);
                        cmd3.Parameters.AddWithValue("@SelfInvoiceDate", SelfInvoiceDate);
                        cmd3.Parameters.AddWithValue("@SelfKMReading", SelfKMReading);
                        cmd3.Parameters.AddWithValue("@SelfAmount", SelfAmount.Replace(",", ""));
                        cmd3.Parameters.AddWithValue("@SelfRemarks", SelfRemarks);
                        cmd3.Parameters.AddWithValue("@ElectricalVendorName", ElectricalVendorName);
                        cmd3.Parameters.AddWithValue("@ElectricalDealerType", ElectricalDealerType);
                        cmd3.Parameters.AddWithValue("@ElectricalInvoiceNo", ElectricalInvoiceNo);
                        cmd3.Parameters.AddWithValue("@ElectricalInvoiceDate", ElectricalInvoiceDate);
                        cmd3.Parameters.AddWithValue("@ElectricalKMReading", ElectricalKMReading);
                        cmd3.Parameters.AddWithValue("@ElectricalAmount", ElectricalAmount.Replace(",", ""));
                        cmd3.Parameters.AddWithValue("@ElectricalRemarks", ElectricalRemarks);
                        cmd3.Parameters.AddWithValue("@ClutchVendorName", ClutchVendorName);
                        cmd3.Parameters.AddWithValue("@ClutchDealerType", ClutchDealerType);
                        cmd3.Parameters.AddWithValue("@ClutchInvoiceNo", ClutchInvoiceNo);
                        cmd3.Parameters.AddWithValue("@ClutchInvoiceDate", ClutchInvoiceDate);
                        cmd3.Parameters.AddWithValue("@ClutchKMReading", ClutchKMReading);
                        cmd3.Parameters.AddWithValue("@ClutchAmount", ClutchAmount.Replace(",", ""));
                        cmd3.Parameters.AddWithValue("@ClutchRemarks", ClutchRemarks);
                        cmd3.Parameters.AddWithValue("@Name", Name);

                        cmd3.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd3.ExecuteNonQuery();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                    if (iRecID > 0)
                    {
                        //Insert Fleet_Sales_Major_Repairing Details
                        string sSql4 = "Fleet_Sales_Major_Repairing_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd4 = new SqlCommand(sSql4, con, trans);
                        cmd4.CommandType = CommandType.StoredProcedure;

                        cmd4.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd4.Parameters.AddWithValue("@AlternatorVendorName", AlternatorVendorName);
                        cmd4.Parameters.AddWithValue("@AlternatorDealerType", AlternatorDealerType);
                        cmd4.Parameters.AddWithValue("@AlternatorInvoiceNo", AlternatorInvoiceNo);
                        cmd4.Parameters.AddWithValue("@AlternatorInvoiceDate", AlternatorInvoiceDate);
                        cmd4.Parameters.AddWithValue("@AlternatorKMReading", AlternatorKMReading);
                        cmd4.Parameters.AddWithValue("@AlternatorAmount", AlternatorAmount.Replace(",", ""));
                        cmd4.Parameters.AddWithValue("@AlternatorRemarks", AlternatorRemarks);
                        cmd4.Parameters.AddWithValue("@LeafVendorName", LeafVendorName);
                        cmd4.Parameters.AddWithValue("@LeafDealerType", LeafDealerType);
                        cmd4.Parameters.AddWithValue("@LeafInvoiceNo", LeafInvoiceNo);
                        cmd4.Parameters.AddWithValue("@LeafInvoiceDate", LeafInvoiceDate);
                        cmd4.Parameters.AddWithValue("@LeafKMReading", LeafKMReading);
                        cmd4.Parameters.AddWithValue("@LeafAmount", LeafAmount.Replace(",", ""));
                        cmd4.Parameters.AddWithValue("@LeafRemarks", LeafRemarks);
                        cmd4.Parameters.AddWithValue("@SuspensionVendorName", SuspensionVendorName);
                        cmd4.Parameters.AddWithValue("@SuspensionDealerType", SuspensionDealerType);
                        cmd4.Parameters.AddWithValue("@SuspensionInvoiceNo", SuspensionInvoiceNo);
                        cmd4.Parameters.AddWithValue("@SuspensionInvoiceDate", SuspensionInvoiceDate);
                        cmd4.Parameters.AddWithValue("@SuspensionKMReading", SuspensionKMReading);
                        cmd4.Parameters.AddWithValue("@SuspensionAmount", SuspensionAmount.Replace(",", ""));
                        cmd4.Parameters.AddWithValue("@SuspensionRemarks", SuspensionRemarks);
                        cmd4.Parameters.AddWithValue("@Name", Name);


                        cmd4.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd4.ExecuteNonQuery();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                    if (iRecID > 0)
                    {
                        //Insert Fleet_Sales_WorkBox_Service Details
                        string sSql5 = "Fleet_Sales_WorkBox_Service_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd5 = new SqlCommand(sSql5, con, trans);
                        cmd5.CommandType = CommandType.StoredProcedure;

                        cmd5.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd5.Parameters.AddWithValue("@GearBoxVendorName", GearBoxVendorName);
                        cmd5.Parameters.AddWithValue("@GearBoxDealerType", GearBoxDealerType);
                        cmd5.Parameters.AddWithValue("@GearBoxInvoiceNo", GearBoxInvoiceNo);
                        cmd5.Parameters.AddWithValue("@GearBoxInvoiceDate", GearBoxInvoiceDate);
                        cmd5.Parameters.AddWithValue("@GearBoxKMReading", GearBoxKMReading);
                        cmd5.Parameters.AddWithValue("@GearBoxAmount", GearBoxAmount.Replace(",", ""));
                        cmd5.Parameters.AddWithValue("@GearBoxRemarks", GearBoxRemarks);
                        cmd5.Parameters.AddWithValue("@BreakWorkVendorName", BreakWorkVendorName);
                        cmd5.Parameters.AddWithValue("@BreakWorkDealerType", BreakWorkDealerType);
                        cmd5.Parameters.AddWithValue("@BreakWorkInvoiceNo", BreakWorkInvoiceNo);
                        cmd5.Parameters.AddWithValue("@BreakWorkInvoiceDate", BreakWorkInvoiceDate);
                        cmd5.Parameters.AddWithValue("@BreakWorkKMReading", BreakWorkKMReading);
                        cmd5.Parameters.AddWithValue("@BreakWorkAmount", BreakWorkAmount.Replace(",", ""));
                        cmd5.Parameters.AddWithValue("@BreakWorkRemarks", BreakWorkRemarks);
                        cmd5.Parameters.AddWithValue("@EngineWorkVendorName", EngineWorkVendorName);
                        cmd5.Parameters.AddWithValue("@EngineWorkDealerType", EngineWorkDealerType);
                        cmd5.Parameters.AddWithValue("@EngineWorkInvoiceNo", EngineWorkInvoiceNo);
                        cmd5.Parameters.AddWithValue("@EngineWorkInvoiceDate", EngineWorkInvoiceDate);
                        cmd5.Parameters.AddWithValue("@EngineWorkKMReading", EngineWorkKMReading);
                        cmd5.Parameters.AddWithValue("@EngineWorkAmount", EngineWorkAmount.Replace(",", ""));
                        cmd5.Parameters.AddWithValue("@EngineWorkRemarks", EngineWorkRemarks);

                        cmd5.Parameters.AddWithValue("@Name", Name);


                        cmd5.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd5.ExecuteNonQuery();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                    if (iRecID > 0)
                    {
                        //Insert Fleet_Sales_Topup_Service Details
                        string sSql6 = "Fleet_Sales_Topup_Service_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd6 = new SqlCommand(sSql6, con, trans);
                        cmd6.CommandType = CommandType.StoredProcedure;

                        cmd6.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd6.Parameters.AddWithValue("@FuelVendorName", FuelVendorName);
                        cmd6.Parameters.AddWithValue("@FuelDealerType", FuelDealerType);
                        cmd6.Parameters.AddWithValue("@FuelInvoiceNo", FuelInvoiceNo);
                        cmd6.Parameters.AddWithValue("@FuelInvoiceDate", FuelInvoiceDate);
                        cmd6.Parameters.AddWithValue("@FuelKMReading", FuelKMReading);
                        cmd6.Parameters.AddWithValue("@FuelAmount", FuelAmount.Replace(",", ""));
                        cmd6.Parameters.AddWithValue("@FuelRemarks", FuelRemarks);
                        cmd6.Parameters.AddWithValue("@PuncherVendorName", PuncherVendorName);
                        cmd6.Parameters.AddWithValue("@PuncherNoofPuncher", PuncherNoofPuncher);
                        cmd6.Parameters.AddWithValue("@PuncherInvoiceNo", PuncherInvoiceNo);
                        cmd6.Parameters.AddWithValue("@PuncherInvoiceDate", PuncherInvoiceDate);
                        cmd6.Parameters.AddWithValue("@PuncherKMReading", PuncherKMReading);
                        cmd6.Parameters.AddWithValue("@PuncherAmount", PuncherAmount.Replace(",", ""));
                        cmd6.Parameters.AddWithValue("@PuncherRemarks", PuncherRemarks);
                        cmd6.Parameters.AddWithValue("@OilVendorName", OilVendorName);
                        if (OilLtr == "")
                        {
                            OilLtr = "0.00";
                            cmd6.Parameters.AddWithValue("@OilLtr", Convert.ToDecimal(OilLtr));
                        }
                        else
                        {
                            cmd6.Parameters.AddWithValue("@OilLtr", Convert.ToDecimal(OilLtr));
                        }
                        cmd6.Parameters.AddWithValue("@OilInvoiceNo", OilInvoiceNo);
                        cmd6.Parameters.AddWithValue("@OilInvoiceDate", OilInvoiceDate);
                        cmd6.Parameters.AddWithValue("@OilKMReading", OilKMReading);
                        cmd6.Parameters.AddWithValue("@OilAmount", OilAmount.Replace(",", ""));
                        cmd6.Parameters.AddWithValue("@OilRemarks", OilRemarks);
                        cmd6.Parameters.AddWithValue("@Name", Name);

                        cmd6.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd6.ExecuteNonQuery();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                    if (iRecID > 0)
                    {
                        //Insert Fleet_Sales_Topup_Service Details
                        string sSql7 = "Fleet_Sales_Other_Service_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd7 = new SqlCommand(sSql7, con, trans);
                        cmd7.CommandType = CommandType.StoredProcedure;

                        cmd7.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd7.Parameters.AddWithValue("@RadiatorVendorName", RadiatorVendorName);
                        cmd7.Parameters.AddWithValue("@RadiatorDealerType", RadiatorDealerType);
                        cmd7.Parameters.AddWithValue("@RadiatorInvoiceNo", RadiatorInvoiceNo);
                        cmd7.Parameters.AddWithValue("@RadiatorInvoiceDate", RadiatorInvoiceDate);
                        cmd7.Parameters.AddWithValue("@RadiatorKMReading", RadiatorKMReading);
                        cmd7.Parameters.AddWithValue("@RadiatorAmount", RadiatorAmount.Replace(",", ""));
                        cmd7.Parameters.AddWithValue("@RadiatorRemarks", RadiatorRemarks);
                        cmd7.Parameters.AddWithValue("@AxleVendorName", AxleVendorName);
                        cmd7.Parameters.AddWithValue("@AxleDealerType", AxleDealerType);
                        cmd7.Parameters.AddWithValue("@AxleInvoiceNo", AxleInvoiceNo);
                        cmd7.Parameters.AddWithValue("@AxleInvoiceDate", AxleInvoiceDate);
                        cmd7.Parameters.AddWithValue("@AxleKMReading", AxleKMReading);
                        cmd7.Parameters.AddWithValue("@AxleAmount", AxleAmount.Replace(",", ""));
                        cmd7.Parameters.AddWithValue("@AxleRemarks", AxleRemarks);
                        cmd7.Parameters.AddWithValue("@DifferentialVendorName", DifferentialVendorName);
                        cmd7.Parameters.AddWithValue("@DifferentialDealerType", DifferentialDealerType);
                        cmd7.Parameters.AddWithValue("@DifferentialInvoiceNo", DifferentialInvoiceNo);
                        cmd7.Parameters.AddWithValue("@DifferentialInvoiceDate", DifferentialInvoiceDate);
                        cmd7.Parameters.AddWithValue("@DifferentialKMReading", DifferentialKMReading);
                        cmd7.Parameters.AddWithValue("@DifferentialAmount", DifferentialAmount.Replace(",", ""));
                        cmd7.Parameters.AddWithValue("@DifferentialRemarks", DifferentialRemarks);
                        cmd7.Parameters.AddWithValue("@Name", Name);

                        cmd7.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd7.ExecuteNonQuery();
                    }
                    else
                    {
                        trans.Rollback();
                    }

                    if (iRecID > 0)
                    {
                        //Insert Fleet_Sales_Topup_Service Details
                        string sSql8 = "Fleet_Sales_Turbo_Service_Insert";

                        //Insert Region Branch Details 
                        SqlCommand cmd8 = new SqlCommand(sSql8, con, trans);
                        cmd8.CommandType = CommandType.StoredProcedure;
                        cmd8.Parameters.AddWithValue("@Rec_id", iRecID);
                        cmd8.Parameters.AddWithValue("@TurboVendorName", TurboVendorName);
                        cmd8.Parameters.AddWithValue("@TurboDealerType", TurboDealerType);
                        cmd8.Parameters.AddWithValue("@TurboInvoiceNo", TurboInvoiceNo);
                        cmd8.Parameters.AddWithValue("@TurboInvoiceDate", TurboInvoiceDate);
                        cmd8.Parameters.AddWithValue("@TurboKMReading", TurboKMReading);
                        cmd8.Parameters.AddWithValue("@TurboAmount", TurboAmount.Replace(",", ""));
                        cmd8.Parameters.AddWithValue("@TurboNarration", TurboNarration);
                        cmd8.Parameters.AddWithValue("@EcmVendorName", EcmVendorName);
                        cmd8.Parameters.AddWithValue("@EcmDealerType", EcmDealerType);
                        cmd8.Parameters.AddWithValue("@EcmInvoiceNo", EcmInvoiceNo);
                        cmd8.Parameters.AddWithValue("@EcmInvoiceDate", EcmInvoiceDate);
                        cmd8.Parameters.AddWithValue("@EcmKMReading", EcmKMReading);
                        cmd8.Parameters.AddWithValue("@EcmAmount", EcmAmount.Replace(",", ""));
                        cmd8.Parameters.AddWithValue("@EcmNarration", EcmNarration);
                        cmd8.Parameters.AddWithValue("@AccidentalVendorName", AccidentalVendorName);
                        cmd8.Parameters.AddWithValue("@AccidentalDealerType", AccidentalDealerType);
                        cmd8.Parameters.AddWithValue("@AccidentalInvoiceNo", AccidentalInvoiceNo);
                        cmd8.Parameters.AddWithValue("@AccidentalInvoiceDate", AccidentalInvoiceDate);
                        cmd8.Parameters.AddWithValue("@AccidentalKMReading", AccidentalKMReading);
                        cmd8.Parameters.AddWithValue("@AccidentalInsCoveredAmount", AccidentalInsCoveredAmount.Replace(",", ""));
                        cmd8.Parameters.AddWithValue("@AccidentalDifferenceAmount", AccidentalDifferenceAmount.Replace(",", ""));
                        cmd8.Parameters.AddWithValue("@AccidentalTotalAmount", AccidentalTotalAmount.Replace(",", ""));
                        cmd8.Parameters.AddWithValue("@AccidentalNarration", AccidentalNarration);

                        cmd8.Parameters.AddWithValue("@Name", Name);
                        cmd8.Parameters.AddWithValue("@Action", Action);

                        iReturn = cmd8.ExecuteNonQuery();
                    }
                    else
                    {
                        trans.Rollback();
                    }
                    trans.Commit();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                iReturn = 0;
                trans.Rollback();

                ci.ErrorLog("C:\\RMS_Fleet_App\\", "SalesDetailsInsert" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - SalesDetailsInsert", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturn;
        }
    }
}