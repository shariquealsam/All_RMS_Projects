using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMSOTCPortal.Models
{
    public class Report
    {
        public string UserID
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string TotalCount
        {
            get;
            set;
        }
        
    }
    public class BranchDtls
    {
        public string Branch_Id { get; set; }
        public string Branch { get; set; }
    }

    public class RouteDtls
    {
        public string Route_No { get; set; }
        public string Route_Name { get; set; }
    }

    public class ReportData
    {
        clsImportant ci = new clsImportant();
        string strMySqlConnectionString = "";
        string strMySqlConnectionStringBulk = "";
        string strSqlConnectionString = "";
        string strSCOConnectionString = "";
        string strFLMConnectionString = "";

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
                    strFLMConnectionString = node.SelectSingleNode("FLM_Connection_String").InnerText;
                }

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "Get_from_config" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Get_from_config", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
        }

        public DataSet GetUSerIDDetails()
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
                sSql = " SELECT UserId,UserName FROM [dbo].[OTC_User_Details] WHERE UserId NOT IN ('PAT089885','PAT087498','PAT090128') ";

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
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetUSerIDDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetUSerIDDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetUserCountDetails(string Date)
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
                sSql = " SELECT UserCount,[User_Id],Upper(TblUser.UserName) AS UserName FROM " +
                        " ( " +
                        " SELECT SUM(TotalValue) As UserCount,[User_Id] FROM " +
                        " ( " +
                        " SELECT Count(1) As TotalValue,[User_Id] FROM [dbo].[OTC_Sco_Details] With (NOLOCK) WHERE Convert(DATE,[Actual_Date_Time]) = '" + Date + "' " +
                        " GROUP BY [User_Id] " +
                        " UNION ALL " +
                        " SELECT Count(1) As TotalValue,[UserId] FROM [dbo].[OTC_Flm_Details] With (NOLOCK) WHERE Convert(DATE,[ActualDateTime]) = '" + Date + "' " +
                        " Group By [UserId] " +
                        " UNION ALL " +
                        " SELECT Count(1) As TotalValue,[UserId] FROM [dbo].[OTC_Other_Details] With (NOLOCK) WHERE Convert(DATE,[ActualDateTime]) = '" + Date + "' " +
                        " GROUP BY [UserId] " +
                        " )TblData " +
                        " GROUP BY [User_Id] " +
                        " )TblUnit " +
                        " INNER JOIN  " +
                        " ( " +
                        " SELECT [Userid],USERNAME FROM [dbo].[OTC_User_Details] With (NOLOCK) " +
                        " )TblUser ON TblUnit.[User_Id] = TblUser.UserId " +
                        " WHERE [User_Id] != 'PATO89885' " +
                        " Union ALL " +
                        " SELECT SUM (TotalValue) AS [Total Count],'','TOTAL : -' " +
                        " FROM " +
                        " ( " +
                        " SELECT Count(1) As TotalValue FROM [dbo].[OTC_Sco_Details] With (NOLOCK) WHERE Convert(DATE,[Actual_Date_Time]) = '" + Date + "' " +
                        " GROUP BY [User_Id] " +
                        " UNION ALL " +
                        " SELECT Count(1) As TotalValue FROM [dbo].[OTC_Flm_Details] With (NOLOCK) WHERE Convert(DATE,[ActualDateTime]) = '" + Date + "' " +
                        " Group By [UserId] " +
                        " UNION ALL " +
                        " SELECT Count(1) As TotalValue FROM [dbo].[OTC_Other_Details] With (NOLOCK) WHERE Convert(DATE,[ActualDateTime]) = '" + Date + "' " +
                        " GROUP BY [UserId] " +
                        " )tblcount; ";

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
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetUSerIDDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetUSerIDDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetCustodianStatusDetails(string CustodianId)
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
                sSql = " SELECT COUNT(1) As CustodianAvl,[Status],BranchName,Company FROM [dbo].[ERP_RMS_Master_Details] WITH(NOLOCK) WHERE regnoerp = '" + CustodianId + "' Group BY [Status],BranchName,Company";

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
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetUSerIDDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetUSerIDDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetBranchDetails()
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
                sSql = " select distinct Branch,Branch_ID from [dbo].[Master_ATM_Details] with(nolock) where Branch_ID !=9999 and Status='Active' order by Branch_ID";

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
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetUSerIDDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetUSerIDDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
                }

            return ds;
            }

        public DataSet GetRouteDetails(string BranchId)
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
                sSql = " select distinct Route_No,CONCAT(Route_No,' - ',Route_Name) as Route_Name from [dbo].[Master_ATM_Details] with(nolock) where Branch_ID !=9999 and Branch_ID=@BranchId and Status='Active' Order By Route_No";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.Parameters.AddWithValue("@BranchId", BranchId);

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
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "GetUSerIDDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetUSerIDDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
                }

            return ds;
            }
        }
}