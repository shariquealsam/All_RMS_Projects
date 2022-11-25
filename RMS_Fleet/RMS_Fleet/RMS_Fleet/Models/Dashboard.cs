using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMS_Fleet.Models
{
    public class Dashboard
    {
        public string RegionID { get; set; }
        public string Region { get; set; }
        public string NoOfVehicles { get; set; }
        public string NoOfOpenings { get; set; }
        public string NoOfClosings { get; set; }
        public string NoReportingToday { get; set; }
        public string NoReportingTillDate { get; set; }
        public string BranchID { get; set; }
        public string Branch { get; set; }
    }

    public class DashboardData
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

        public DataSet GetDashboardDetails(string Date)
        {
            //Date = "2018-12-19";

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
                sSql = "SELECT Tbl.RegionId,RegionName,NoOfVechile,NoOfOpening," +
                        "NoOfClosing,(NoOfVechile-NoOfOpening) As [No Reporting Today],  " +
                        "NoReportingTillDate FROM   " +
                        "(  " +
                        "SELECT Tbl.RegionId,RegionName,NoOfVechile,sum(NoOfOpening) as NoOfOpening,  " +
                        "sum(NoOfClosing) as NoOfClosing,  " +
                        "NoReporting.NoReportingTillDate  " +
                        "FROM    " +
                        "(    " +
                        "SELECT  Region.RegionId,Region.RegionName,MasterVechile.NoOfVechile,    " +
                        "ISNULL(Opening.NoOfOpening,0) As NoOfOpening,    " +
                        "ISNULL(Closing.NoOfClosing,0) As NoOfClosing    " +
                        "FROM    " +
                        "(    " +
                        "Select  RegionId,RegionName from [dbo].[Fleet_Region_Details] WITH (NOLOCK)    " +
                        ")Region    " +
                        "LEFT JOIN     " +
                        "(    " +
                        "SELECT Count(1) As NoOfVechile,RegionId FROM [dbo].[Fleet_Vehicle_Details] WITH (NOLOCK)    " +
                        "GROUP BY RegionId    " +
                        ")MasterVechile ON (Region.RegionId = MasterVechile.RegionId)    " +
                        "LEFT JOIN    " +
                        "(    " +
                        "SELECT  Count(1) as NoOfOpening,RegionId,OCKey FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)    " +
                        "WHERE Reporting_Type=1 AND CONVERT(DATE,Created_Datetime) ='" + Date + "'    " +
                        "GROUP BY RegionId,OCKey    " +
                        ")Opening ON (Region.RegionId = Opening.RegionId)    " +
                        "LEFT JOIN    " +
                        "(    " +
                        "SELECT Count(1) as NoOfClosing,RegionId,OCKey FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)    " +
                        "WHERE Reporting_Type=2 AND CONVERT(DATE,Created_Datetime) ='" + Date + "'    " +
                        "GROUP BY RegionId,OCKey     " +
                        ")Closing ON (Region.RegionId = Closing.RegionId and Opening.OCKey = Closing.OCKey)    " +
                        ")Tbl    " +
                        "LEFT JOIN    " +
                        "(    " +
                        "SELECT COUNT(NoReportingTillDate) As NoReportingTillDate,RegionId FROM    " +
                        "(    " +
                        "SELECT CASE when MatchingVechile != '0' THEN '1' ELSE '0' end As NoReportingTillDate,RegionId    " +
                        "FROM    " +
                        "(    " +
                        "SELECT MsVechile.VehicleNo,MsVechile.RegionId,ISNULL(Reporting.VehicleNo,0) AS MatchingVechile  FROM    " +
                        "(    " +
                        "SELECT DISTINCT VehicleNo,RegionId FROM [dbo].[Fleet_Vehicle_Details] WITH (NOLOCK)    " +
                        ")MsVechile    " +
                        "LEFT JOIN     " +
                        "(    " +
                        "SELECT DISTINCT VehicleNo,RegionId FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)    " +
                        ")Reporting ON (MsVechile.RegionId = Reporting.RegionId AND MsVechile.VehicleNo = Reporting.VehicleNo)    " +
                        ")tbl    " +
                        ")tblData    " +
                        "WHERE NoReportingTillDate = 0    " +
                        "group by RegionId    " +
                        ")NoReporting ON (Tbl.RegionId = NoReporting.RegionId) where tbl.RegionId != 15   " +
                        "group by Tbl.RegionId,RegionName,NoOfVechile,NoReportingTillDate  " +
                        ")tbl   " +
                        "UNION All " +
                        "( " +
                        "SELECT 500,'TOTAL',SUM(NoOfVechile) As NoOfVechile, SUM(NoOfOpening) As NoOfOpening, " +
                        "SUM(NoOfClosing) As NoOfClosing,SUM((NoOfVechile-NoOfOpening)) As [No Reporting Today],  " +
                        "SUM(NoReportingTillDate) as NoReportingTillDate FROM   " +
                        "(  " +
                        "SELECT Tbl.RegionId,RegionName,NoOfVechile,sum(NoOfOpening) as NoOfOpening,  " +
                        "sum(NoOfClosing) as NoOfClosing,  " +
                        "NoReporting.NoReportingTillDate  " +
                        "FROM    " +
                        "(    " +
                        "SELECT  Region.RegionId,Region.RegionName,MasterVechile.NoOfVechile,    " +
                        "ISNULL(Opening.NoOfOpening,0) As NoOfOpening,    " +
                        "ISNULL(Closing.NoOfClosing,0) As NoOfClosing    " +
                        "FROM    " +
                        "(    " +
                        "Select  RegionId,RegionName from [dbo].[Fleet_Region_Details] WITH (NOLOCK)    " +
                        ")Region    " +
                        "LEFT JOIN     " +
                        "(    " +
                        "SELECT Count(1) As NoOfVechile,RegionId FROM [dbo].[Fleet_Vehicle_Details] WITH (NOLOCK)    " +
                        "GROUP BY RegionId    " +
                        ")MasterVechile ON (Region.RegionId = MasterVechile.RegionId)    " +
                        "LEFT JOIN    " +
                        "(    " +
                        "SELECT  Count(1) as NoOfOpening,RegionId,OCKey FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)    " +
                        "WHERE Reporting_Type=1 AND CONVERT(DATE,Created_Datetime) ='" + Date + "'    " +
                        "GROUP BY RegionId,OCKey    " +
                        ")Opening ON (Region.RegionId = Opening.RegionId)    " +
                        "LEFT JOIN    " +
                        "(    " +
                        "SELECT Count(1) as NoOfClosing,RegionId,OCKey FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)    " +
                        "WHERE Reporting_Type=2 AND CONVERT(DATE,Created_Datetime) ='" + Date + "'    " +
                        "GROUP BY RegionId,OCKey     " +
                        ")Closing ON (Region.RegionId = Closing.RegionId and Opening.OCKey = Closing.OCKey)    " +
                        ")Tbl    " +
                        "LEFT JOIN    " +
                        "(    " +
                        "SELECT COUNT(NoReportingTillDate) As NoReportingTillDate,RegionId FROM    " +
                        "(    " +
                        "SELECT CASE when MatchingVechile != '0' THEN '1' ELSE '0' end As NoReportingTillDate,RegionId    " +
                        "FROM    " +
                        "(    " +
                        "SELECT MsVechile.VehicleNo,MsVechile.RegionId,ISNULL(Reporting.VehicleNo,0) AS MatchingVechile  FROM    " +
                        "(    " +
                        "SELECT DISTINCT VehicleNo,RegionId FROM [dbo].[Fleet_Vehicle_Details] WITH (NOLOCK)    " +
                        ")MsVechile    " +
                        "LEFT JOIN     " +
                        "(    " +
                        "SELECT DISTINCT VehicleNo,RegionId FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)    " +
                        ")Reporting ON (MsVechile.RegionId = Reporting.RegionId AND MsVechile.VehicleNo = Reporting.VehicleNo)    " +
                        ")tbl    " +
                        ")tblData    " +
                        "WHERE NoReportingTillDate = 0    " +
                        "group by RegionId    " +
                        ")NoReporting ON (Tbl.RegionId = NoReporting.RegionId) where tbl.RegionId != 15   " +
                        "group by Tbl.RegionId,RegionName,NoOfVechile,NoReportingTillDate  " +
                        ")tbl   " +
                        ") "  ;

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

        public DataSet GetDashboardRegionDetails(string Date, int Regionid)
        {
            //Date = "2018-12-19";

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
                sSql = "SELECT [BranchId],[RegionId],[BranchName],NoOfVechile," +
                        "NoOfOpening,   " +
                        "NoOfClosing,   " +
                        "(NoOfVechile - NoOfOpening) As [NoReportingToday],   " +
                        "NoReportingTillDate FROM   " +
                        "(   " +
                        "SELECT [BranchId],[RegionId],[BranchName],NoOfVechile,   " +
                        "SUM(NoOfOpening) as NoOfOpening,   " +
                        "SUM(NoOfClosing) as NoOfClosing,   " +
                        "NoReportingTillDate   " +
                        "FROM    " +
                        "(    " +
                        "SELECT tblMaster.[BranchId],tblMaster.[RegionId],    " +
                        "tblMaster.[BranchName],    " +
                        "ISNULL(NoOfVechile,0) As NoOfVechile,    " +
                        "ISNULL(Opening.NoOfOpening,0) As NoOfOpening,    " +
                        "ISNULL(NoOfClosing,0) As NoOfClosing,    " +
                        "ISNULL(NoReportingTill.NoReportingTillDate,0) As NoReportingTillDate  " +
                        "                          " +
                        "FROM    " +
                        "(    " +
                        "SELECT [BranchId],[BranchName],[RegionId] FROM [dbo].[Fleet_Branch_Details]     " +
                        "where RegionId = " + Regionid + " " +
                        ")tblMaster    " +
                        "LEFT JOIN     " +
                        "(    " +
                        "SELECT  Count(1) as NoOfOpening,RegionId,BranchId,OCKey FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)     " +
                        "WHERE Reporting_Type=1    " +
                        "AND CONVERT(DATE,Created_Datetime) ='" + Date + "'     " +
                        "GROUP BY BranchId,RegionId,OCKey    " +
                        ")Opening On (tblMaster.[BranchId] = Opening.BranchId and tblMaster.RegionId = Opening.RegionId)    " +
                        "LEFT JOIN    " +
                        "(    " +
                        "SELECT Count(1) as NoOfClosing,RegionId,BranchId,OCKey FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)     " +
                        "WHERE Reporting_Type=2     " +
                        "AND CONVERT(DATE,Created_Datetime) ='" + Date + "'     " +
                        "GROUP BY BranchId,RegionId,OCKey     " +
                        ")Closing On (tblMaster.[BranchId] = Closing.BranchId and tblMaster.RegionId = Closing.RegionId And Opening.OCKey = Closing.OCKey)    " +
                        "LEFT JOIN    " +
                        "(    " +
                        "SELECT COUNT(NoReportingTillDate) As NoReportingTillDate,RegionId,BranchId FROM     " +
                        "(     " +
                        "SELECT CASE when MatchingVechile != '0' THEN '1' ELSE '0' end As NoReportingTillDate,RegionId,BranchId     " +
                        "FROM     " +
                        "(     " +
                        "SELECT MsVechile.VehicleNo,MsVechile.RegionId,ISNULL(Reporting.VehicleNo,0) AS MatchingVechile,BranchId  FROM     " +
                        "(     " +
                        "SELECT DISTINCT VehicleNo,RegionId,BranchId FROM [dbo].[Fleet_Vehicle_Details] WITH (NOLOCK)     " +
                        ")MsVechile     " +
                        "LEFT JOIN      " +
                        "(     " +
                        "SELECT DISTINCT VehicleNo,RegionId FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)     " +
                        ")Reporting ON (MsVechile.RegionId = Reporting.RegionId AND MsVechile.VehicleNo = Reporting.VehicleNo)     " +
                        ")tbl     " +
                        ")tblData     " +
                        "WHERE NoReportingTillDate = 0     " +
                        "group by BranchId,RegionId     " +
                        ") NoReportingTill ON (tblMaster.[BranchId] = NoReportingTill.BranchId and tblMaster.RegionId = NoReportingTill.RegionId)    " +
                        "LEFT JOIN    " +
                        "(    " +
                        "SELECT Count(1) As NoOfVechile,RegionId,BranchId FROM [dbo].[Fleet_Vehicle_Details] WITH (NOLOCK)     " +
                        "GROUP BY BranchId,RegionId     " +
                        ")VechileCount On (tblMaster.[BranchId] = VechileCount.BranchId and tblMaster.RegionId = VechileCount.RegionId)    " +
                        ")Tblselect    " +
                        "group by [BranchId],[RegionId],[BranchName],NoOfVechile,NoReportingTillDate   " +
                        ")tbl   " +
                        "  " +
                        "UNION ALL " +
                        "( " +
                        "SELECT '9999','500','TOTAL',SUM(NoOfVechile) As NoOfVechile, " +
                        "SUM(NoOfOpening) As NoOfOpening,   " +
                        "SUM(NoOfClosing) As NoOfClosing,   " +
                        "SUM((NoOfVechile - NoOfOpening)) As [NoReportingToday],   " +
                        "SUM(NoReportingTillDate) As NoReportingTillDate FROM   " +
                        "(   " +
                        "SELECT [BranchId],[RegionId],[BranchName],NoOfVechile,   " +
                        "SUM(NoOfOpening) as NoOfOpening,   " +
                        "SUM(NoOfClosing) as NoOfClosing,   " +
                        "NoReportingTillDate   " +
                        "FROM    " +
                        "(    " +
                        "SELECT tblMaster.[BranchId],tblMaster.[RegionId],    " +
                        "tblMaster.[BranchName],    " +
                        "ISNULL(NoOfVechile,0) As NoOfVechile,    " +
                        "ISNULL(Opening.NoOfOpening,0) As NoOfOpening,    " +
                        "ISNULL(NoOfClosing,0) As NoOfClosing,    " +
                        "ISNULL(NoReportingTill.NoReportingTillDate,0) As NoReportingTillDate  " +
                        "                          " +
                        "FROM    " +
                        "(    " +
                        "SELECT [BranchId],[BranchName],[RegionId] FROM [dbo].[Fleet_Branch_Details]     " +
                        "where RegionId =   " + Regionid + "  " +
                        ")tblMaster    " +
                        "LEFT JOIN     " +
                        "(    " +
                        "SELECT  Count(1) as NoOfOpening,RegionId,BranchId,OCKey FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)     " +
                        "WHERE Reporting_Type=1    " +
                        "AND CONVERT(DATE,Created_Datetime) ='" + Date + "'     " +
                        "GROUP BY BranchId,RegionId,OCKey    " +
                        ")Opening On (tblMaster.[BranchId] = Opening.BranchId and tblMaster.RegionId = Opening.RegionId)    " +
                        "LEFT JOIN    " +
                        "(    " +
                        "SELECT Count(1) as NoOfClosing,RegionId,BranchId,OCKey FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)     " +
                        "WHERE Reporting_Type=2     " +
                        "AND CONVERT(DATE,Created_Datetime) ='" + Date + "'     " +
                        "GROUP BY BranchId,RegionId,OCKey     " +
                        ")Closing On (tblMaster.[BranchId] = Closing.BranchId and tblMaster.RegionId = Closing.RegionId And Opening.OCKey = Closing.OCKey)    " +
                        "LEFT JOIN    " +
                        "(    " +
                        "SELECT COUNT(NoReportingTillDate) As NoReportingTillDate,RegionId,BranchId FROM     " +
                        "(     " +
                        "SELECT CASE when MatchingVechile != '0' THEN '1' ELSE '0' end As NoReportingTillDate,RegionId,BranchId     " +
                        "FROM     " +
                        "(     " +
                        "SELECT MsVechile.VehicleNo,MsVechile.RegionId,ISNULL(Reporting.VehicleNo,0) AS MatchingVechile,BranchId  FROM     " +
                        "(     " +
                        "SELECT DISTINCT VehicleNo,RegionId,BranchId FROM [dbo].[Fleet_Vehicle_Details] WITH (NOLOCK)     " +
                        ")MsVechile     " +
                        "LEFT JOIN      " +
                        "(     " +
                        "SELECT DISTINCT VehicleNo,RegionId FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)     " +
                        ")Reporting ON (MsVechile.RegionId = Reporting.RegionId AND MsVechile.VehicleNo = Reporting.VehicleNo)     " +
                        ")tbl     " +
                        ")tblData     " +
                        "WHERE NoReportingTillDate = 0     " +
                        "group by BranchId,RegionId     " +
                        ") NoReportingTill ON (tblMaster.[BranchId] = NoReportingTill.BranchId and tblMaster.RegionId = NoReportingTill.RegionId)    " +
                        "LEFT JOIN    " +
                        "(    " +
                        "SELECT Count(1) As NoOfVechile,RegionId,BranchId FROM [dbo].[Fleet_Vehicle_Details] WITH (NOLOCK)     " +
                        "GROUP BY BranchId,RegionId     " +
                        ")VechileCount On (tblMaster.[BranchId] = VechileCount.BranchId and tblMaster.RegionId = VechileCount.RegionId)    " +
                        ")Tblselect    " +
                        "group by [BranchId],[RegionId],[BranchName],NoOfVechile,NoReportingTillDate   " +
                        ")tbl   " +
                        ") " +
                        "Order By BranchId  " ;

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

        public DataSet GetDashboardSelectedRegionDetails(string Date, string UserType, string RegionId)
        {
            //Date = "2018-12-19";

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
                sSql = "SELECT Tbl.RegionId,RegionName,NoOfVechile,NoOfOpening," +
                        "NoOfClosing,(NoOfVechile-NoOfOpening) As [No Reporting Today], " +
                        "NoReportingTillDate FROM  " +
                        "( " +
                        "SELECT Tbl.RegionId,RegionName,NoOfVechile,sum(NoOfOpening) as NoOfOpening, " +
                        "sum(NoOfClosing) as NoOfClosing, " +
                        "NoReporting.NoReportingTillDate " +
                        "FROM   " +
                        "(   " +
                        "SELECT  Region.RegionId,Region.RegionName,MasterVechile.NoOfVechile,   " +
                        "ISNULL(Opening.NoOfOpening,0) As NoOfOpening,   " +
                        "ISNULL(Closing.NoOfClosing,0) As NoOfClosing   " +
                        "FROM   " +
                        "(   " +
                        "Select  RegionId,RegionName from [dbo].[Fleet_Region_Details] WITH (NOLOCK)   " +
                        ")Region   " +
                        "LEFT JOIN    " +
                        "(   " +
                        "SELECT Count(1) As NoOfVechile,RegionId FROM [dbo].[Fleet_Vehicle_Details] WITH (NOLOCK)   " +
                        "GROUP BY RegionId   " +
                        ")MasterVechile ON (Region.RegionId = MasterVechile.RegionId)   " +
                        "LEFT JOIN   " +
                        "(   " +
                        "SELECT  Count(1) as NoOfOpening,RegionId,OCKey FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)   " +
                        "WHERE Reporting_Type=1 AND CONVERT(DATE,Created_Datetime) ='" + Date + "'   " +
                        "GROUP BY RegionId,OCKey   " +
                        ")Opening ON (Region.RegionId = Opening.RegionId)   " +
                        "LEFT JOIN   " +
                        "(   " +
                        "SELECT Count(1) as NoOfClosing,RegionId,OCKey FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)   " +
                        "WHERE Reporting_Type=2 AND CONVERT(DATE,Created_Datetime) ='" + Date + "'   " +
                        "GROUP BY RegionId,OCKey    " +
                        ")Closing ON (Region.RegionId = Closing.RegionId and Opening.OCKey = Closing.OCKey)   " +
                        ")Tbl   " +
                        "LEFT JOIN   " +
                        "(   " +
                        "SELECT COUNT(NoReportingTillDate) As NoReportingTillDate,RegionId FROM   " +
                        "(   " +
                        "SELECT CASE when MatchingVechile != '0' THEN '1' ELSE '0' end As NoReportingTillDate,RegionId   " +
                        "FROM   " +
                        "(   " +
                        "SELECT MsVechile.VehicleNo,MsVechile.RegionId,ISNULL(Reporting.VehicleNo,0) AS MatchingVechile  FROM   " +
                        "(   " +
                        "SELECT DISTINCT VehicleNo,RegionId FROM [dbo].[Fleet_Vehicle_Details] WITH (NOLOCK)   " +
                        ")MsVechile   " +
                        "LEFT JOIN    " +
                        "(   " +
                        "SELECT DISTINCT VehicleNo,RegionId FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)   " +
                        ")Reporting ON (MsVechile.RegionId = Reporting.RegionId AND MsVechile.VehicleNo = Reporting.VehicleNo)   " +
                        ")tbl   " +
                        ")tblData   " +
                        "WHERE NoReportingTillDate = 0   " +
                        "group by RegionId   " +
                        ")NoReporting ON (Tbl.RegionId = NoReporting.RegionId) where tbl.RegionId != 15   And tbl.RegionId = " + RegionId + "" +
                        "group by Tbl.RegionId,RegionName,NoOfVechile,NoReportingTillDate " +
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