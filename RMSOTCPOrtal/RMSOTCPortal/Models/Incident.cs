using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMSOTCPortal.Models
{

    public class Incident
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
                ci.ErrorLog("C:\\RMS_OTC_Log\\", "Get_from_config" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Get_from_config", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

        }

        public DataSet RegionList()
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
                sSql = "Sp_All_Region";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Log\\", "Get_from_config" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetUserDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;

        }

        public DataSet GetBranchDetails(string Region)
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
                sSql = " select distinct Branch,Branch_ID from [dbo].[Master_ATM_Details] with(nolock) where Branch_ID !=9999 and Region=@Region and Status='Active'  order by Branch_ID";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.Parameters.AddWithValue("@Region", Region);
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

        public int insertIncident(incideDetails inc,string ConfiguredBy)
        {
            
            int iReturn = 0;
            Get_from_config();
            SqlTransaction trans = null;
            try
            {
                SqlConnection con = new SqlConnection(strSqlConnectionString);
                string sSql = "Sp_OTC_Incident_Insert";
                con.Open();
                using (trans = con.BeginTransaction())
                {
                    //Insert User Details
                    SqlCommand cmd = new SqlCommand(sSql, con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Rec_Id", inc.Rec_Id);
                    cmd.Parameters.AddWithValue("@OTC_Date", inc.OTC_Date);
                    cmd.Parameters.AddWithValue("@Company", inc.Company);
                    cmd.Parameters.AddWithValue("@Region", inc.Region);
                    cmd.Parameters.AddWithValue("@Branch", inc.Branch);
                    cmd.Parameters.AddWithValue("@RoutNo", inc.RoutNo);
                    cmd.Parameters.AddWithValue("@KeyName", inc.KeyName);
                    cmd.Parameters.AddWithValue("@TouchKey", inc.TouchKey);
                    cmd.Parameters.AddWithValue("@KeyType", inc.KeyType);
                    cmd.Parameters.AddWithValue("@ATM_ID", inc.ATM_ID);
                    cmd.Parameters.AddWithValue("@Purpose", inc.Purpose);
                    cmd.Parameters.AddWithValue("@KeyCorrupt", inc.KeyCorrupt);
                    cmd.Parameters.AddWithValue("@ConfigureDate", inc.ConfigureDate);
                    cmd.Parameters.AddWithValue("@SendDateIst", inc.SendDateIst);
                    cmd.Parameters.AddWithValue("@SendDateIInd", inc.SendDateIInd);
                    cmd.Parameters.AddWithValue("@ResolutionDate", inc.ResolutionDate);
                    cmd.Parameters.AddWithValue("@BatteryStatus", inc.BatteryStatus);
                    cmd.Parameters.AddWithValue("@Status", inc.Status);
                    cmd.Parameters.AddWithValue("@Remarks", inc.Remarks);
                    cmd.Parameters.AddWithValue("@ConfiguredBy", ConfiguredBy);

                    iReturn = cmd.ExecuteNonQuery();

                    trans.Commit();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Protal\\", "Get_from_config" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Get_from_config", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturn;
        }
        public DataSet allIncidenceList()
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
                sSql = "Sp_All_OTC_Incidence_Details";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Log\\", "Get_from_config" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Incidence List", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;

        }

        public DataSet incidenceDetailsByRecId(int Rec_Id )
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
                sSql = "Sp_OTC_Incidence_Details_by_Rec_Id";

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Rec_Id", Rec_Id);

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_OTC_Log\\", "Get_from_config" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Incidence List", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;

        }

    }
    public class RegionDetails
    {
        public string Region { get; set; }
    }
    public class BranchDetails
    {
        public string Branch { get; set; }
        public string Branch_Id { get; set; }
    }
    public class incideDetails
    {
        public Int64 Rec_Id { get; set; }
        public string OTC_Date { get; set; }
        public string Company { get; set; }
        public string Region { get; set; }
        public string Branch { get; set; }
        public string BranchId { get; set; }
        public int RoutNo { get; set; }
        public string KeyName { get; set; }
        public string TouchKey { get; set; }
        public string KeyType { get; set; }
        public string ATM_ID { get; set; }
        public string Purpose { get; set; }
        public string KeyCorrupt { get; set; }
        public string ConfigureDate { get; set; }
        public string SendDateIst { get; set; }
        public string SendDateIInd { get; set; }
        public string ResolutionDate { get; set; }
        public string BatteryStatus { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string ConfiguredBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}