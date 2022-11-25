using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace RMS_Fleet.Models
{
    public class Search_Repair_Maintenance_Overview_Attributes
    { 
        public string FromMonth { get; set; }
        public string FromYear { get; set; }
        public string ToMonth { get; set; }
        public string ToYear { get; set; }
        public string cmbRegion { get; set; }
        public string cmbBranch { get; set; }
        public string cmbVechile { get; set; }
    }

    public class KPL_Overview_Attributes
    {
        public string FromMonthKPL { get; set; }
        public string FromYearKPL { get; set; }
        public string ToMonthKPL { get; set; }
        public string ToYearKPL { get; set; }
        public string cmbRegionKPL { get; set; }
        public string cmbBranchKPL { get; set; }
        public string cmbVechileKPL { get; set; }
    }

    public class OverviewDetails
    { 		
         public string TotalKM { get; set; }
         public string TotalAmount { get; set; }
         public string PerKMCost { get; set; }
         public string TotalExpenditureMaster { get; set; }
         public string Difference { get; set; }
         public string TotalVehicles { get; set; }
    }

    public class KPL_OverviewDetails
    {
        public string StdKmpl { get; set; }
        public string KMPL { get; set; }
        public string ExpectedFuleExpense { get; set; }
        public string ActualFuelExpense { get; set; }
        public string Difference { get; set; }
        public string TotalKM { get; set; }
        public string FuelRatePerltr { get; set; }
        public string TotalVehicles { get; set; }
    }

    public class Branch
    {
        public string BranchId { get; set; }
        public string BranchName { get; set; }
    }

    public class Overview
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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Get_from_config_FleetApp", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
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
                sSql = "SELECT BranchId,BranchName FROM [dbo].[Fleet_Branch_Details] WITH (NOLOCK) WHERE RegionId  in ( " + RegionId + ")  ORDER BY BranchName";

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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetBranch", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

        public DataSet GetVehicleMaster(string RegionId, string BranchId)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //string RegionIdIfItIsZero = string.Empty;
                //if (Convert.ToInt32(RegionId) == 0)
                //{
                //    DataSet dsN = GetRegionIdByBranchId(Convert.ToInt32(BranchId));
                //    RegionIdIfItIsZero = dsN.Tables[0].Rows[0]["RegionId"].ToString();
                //}
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);
                //if (Convert.ToInt32(RegionId) == 0)
                //{
                //    sSql = "SELECT VehicleNo FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) WHERE RegionId=" + RegionIdIfItIsZero + " and BranchId = " + BranchId + "";
                //}
                //else
                //{
                    //SQL Statement 
                    sSql = "SELECT VehicleNo FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) WHERE RegionId in (" + RegionId + ") and BranchId  in  ( " + BranchId + ")";
                //}
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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetVehicleMaster", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

        public DataSet Repair_Maintenance_Overview(Search_Repair_Maintenance_Overview_Attributes obj)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            string FromDate = obj.FromYear +"-"+ obj.FromMonth +"-"+ "01";
            string ToDate = obj.ToYear +"-"+ obj.ToMonth +"-"+ "01";
            if (obj.cmbRegion == null)
            {
                obj.cmbRegion= "";
            }
            if (obj.cmbBranch == null)
            {
                obj.cmbBranch = "";
            }
            if (obj.cmbVechile== null)
            {
                obj.cmbVechile= "";
            }
            SqlConnection con = new SqlConnection(strSqlConnectionString);


            try
            {
                string Sql = "Fleet_Repair_Maintenance_Overview";
                SqlCommand cmd = new SqlCommand(Sql,con);
                cmd.Parameters.AddWithValue("@StartDate", FromDate);
                cmd.Parameters.AddWithValue("@EndDate", ToDate);
                cmd.Parameters.AddWithValue("@RegionId", obj.cmbRegion);
                cmd.Parameters.AddWithValue("@BranchId", obj.cmbBranch);
                cmd.Parameters.AddWithValue("@VehicleNo", obj.cmbVechile);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandTimeout = 600;
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "Repair_Maintenance_Overview" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Repair_Maintenance_Overview", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
         
            }

            return ds;
        }


        public DataSet KPL_Overview(Search_Repair_Maintenance_Overview_Attributes obj)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            string FromDate = obj.FromYear + "-" + obj.FromMonth + "-" + "01";
            string ToDate = obj.ToYear + "-" + obj.ToMonth + "-" + "01";
            if (obj.cmbRegion == null)
            {
                obj.cmbRegion = "";
            }
            if (obj.cmbBranch == null)
            {
                obj.cmbBranch = "";
            }
            if (obj.cmbVechile == null)
            {
                obj.cmbVechile = "";
            }
            SqlConnection con = new SqlConnection(strSqlConnectionString);


            try
            {
                string Sql = "Fleet_KPL_Overview";
                SqlCommand cmd = new SqlCommand(Sql, con);
                cmd.Parameters.AddWithValue("@StartDate", FromDate);
                cmd.Parameters.AddWithValue("@EndDate", ToDate);
                cmd.Parameters.AddWithValue("@RegionId", obj.cmbRegion);
                cmd.Parameters.AddWithValue("@BranchId", obj.cmbBranch);
                cmd.Parameters.AddWithValue("@VehicleNo", obj.cmbVechile);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandTimeout = 600;
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "Repair_Maintenance_Overview" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Repair_Maintenance_Overview", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }

            return ds;
        }

        public DataSet Repair_Maintenance_Overview_Total_Vehicles(Search_Repair_Maintenance_Overview_Attributes obj)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            if (obj.cmbRegion == null)
            {
                obj.cmbRegion = "";
            }
            if (obj.cmbBranch == null)
            {
                obj.cmbBranch = "";
            }
            if (obj.cmbVechile == null)
            {
                obj.cmbVechile = "";
            }
            SqlConnection con = new SqlConnection(strSqlConnectionString);


            try
            {
                string Sql = "Fleet_Repair_Maintenance_Overview_Totalvehicle";
                SqlCommand cmd = new SqlCommand(Sql, con);
                cmd.Parameters.AddWithValue("@RegionId", obj.cmbRegion);
                cmd.Parameters.AddWithValue("@BranchId", obj.cmbBranch);
                cmd.Parameters.AddWithValue("@VehicleNo", obj.cmbVechile);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandTimeout = 600;
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "Repair_Maintenance_Overview" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Repair_Maintenance_Overview", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }

            return ds;
        }


        public DataSet Repair_Maintenance_Overview_Report(Search_Repair_Maintenance_Overview_Attributes obj)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            string FromDate = obj.FromYear + "-" + obj.FromMonth + "-" + "01";
            string ToDate = obj.ToYear + "-" + obj.ToMonth + "-" + "01";
            if (obj.cmbRegion == null)
            {
                obj.cmbRegion = "";
            }
            if (obj.cmbBranch == null)
            {
                obj.cmbBranch = "";
            }
            if (obj.cmbVechile == null)
            {
                obj.cmbVechile = "";
            }
            SqlConnection con = new SqlConnection(strSqlConnectionString);


            try
            {
                string Sql = "Fleet_Repair_Maintenance_Overview_Report";
                SqlCommand cmd = new SqlCommand(Sql, con);
                cmd.Parameters.AddWithValue("@StartDate", FromDate);
                cmd.Parameters.AddWithValue("@EndDate", ToDate);
                cmd.Parameters.AddWithValue("@RegionId", obj.cmbRegion);
                cmd.Parameters.AddWithValue("@BranchId", obj.cmbBranch);
                cmd.Parameters.AddWithValue("@VehicleNo", obj.cmbVechile);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandTimeout = 600;
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "Repair_Maintenance_Overview" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Repair_Maintenance_Overview", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }

            return ds;
        }

        public DataSet Repair_Maintenance_Overview_Report(Search_Repair_Maintenance_Overview_Attributes obj, string RegionId)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            string FromDate = obj.FromYear + "-" + obj.FromMonth + "-" + "01";
            string ToDate = obj.ToYear + "-" + obj.ToMonth + "-" + "01";
            if (obj.cmbRegion == null)
            {
                obj.cmbRegion = "";
            }
            if (obj.cmbBranch == null)
            {
                obj.cmbBranch = "";
            }
            if (obj.cmbVechile == null)
            {
                obj.cmbVechile = "";
            }
            SqlConnection con = new SqlConnection(strSqlConnectionString);


            try
            {
                string Sql = "Fleet_Repair_Maintenance_Overview_Report";
                SqlCommand cmd = new SqlCommand(Sql, con);
                cmd.Parameters.AddWithValue("@StartDate", FromDate);
                cmd.Parameters.AddWithValue("@EndDate", ToDate);
                cmd.Parameters.AddWithValue("@RegionId", RegionId);
                cmd.Parameters.AddWithValue("@BranchId", obj.cmbBranch);
                cmd.Parameters.AddWithValue("@VehicleNo", obj.cmbVechile);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandTimeout = 600;
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "Repair_Maintenance_Overview" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Repair_Maintenance_Overview", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }

            return ds;
        }

        public DataSet KPL_Overview_Report(KPL_Overview_Attributes obj)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            string FromDate = obj.FromYearKPL + "-" + obj.FromMonthKPL + "-" + "01";
            string ToDate = obj.ToYearKPL + "-" + obj.ToMonthKPL + "-" + "01";
            if (obj.cmbRegionKPL == null)
            {
                obj.cmbRegionKPL = "";
            }
            if (obj.cmbBranchKPL == null)
            {
                obj.cmbBranchKPL = "";
            }
            if (obj.cmbVechileKPL == null)
            {
                obj.cmbVechileKPL = "";
            }
            SqlConnection con = new SqlConnection(strSqlConnectionString);


            try
            {
                string Sql = "Fleet_KPL_Overview_Report";
                SqlCommand cmd = new SqlCommand(Sql, con);
                cmd.Parameters.AddWithValue("@StartDate", FromDate);
                cmd.Parameters.AddWithValue("@EndDate", ToDate);
                cmd.Parameters.AddWithValue("@RegionId", obj.cmbRegionKPL);
                cmd.Parameters.AddWithValue("@BranchId", obj.cmbBranchKPL);
                cmd.Parameters.AddWithValue("@VehicleNo", obj.cmbVechileKPL);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandTimeout = 600;
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "KPL_Overview" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Repair_Maintenance_Overview", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }

            return ds;
        }

        public DataSet KPL_Overview_Report(KPL_Overview_Attributes obj, string RegionId)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            string FromDate = obj.FromYearKPL + "-" + obj.FromMonthKPL + "-" + "01";
            string ToDate = obj.ToYearKPL + "-" + obj.ToMonthKPL + "-" + "01";
            if (obj.cmbRegionKPL == null)
            {
                obj.cmbRegionKPL = "";
            }
            if (obj.cmbBranchKPL == null)
            {
                obj.cmbBranchKPL = "";
            }
            if (obj.cmbVechileKPL == null)
            {
                obj.cmbVechileKPL = "";
            }
            SqlConnection con = new SqlConnection(strSqlConnectionString);


            try
            {
                string Sql = "Fleet_KPL_Overview_Report";
                SqlCommand cmd = new SqlCommand(Sql, con);
                cmd.Parameters.AddWithValue("@StartDate", FromDate);
                cmd.Parameters.AddWithValue("@EndDate", ToDate);
                cmd.Parameters.AddWithValue("@RegionId", RegionId);
                cmd.Parameters.AddWithValue("@BranchId", obj.cmbBranchKPL);
                cmd.Parameters.AddWithValue("@VehicleNo", obj.cmbVechileKPL);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandTimeout = 600;
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "KPL_Overview" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - Repair_Maintenance_Overview", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }

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
                if (RegionId == "0")
                {
                    //SQL Statement 
                    sSql = "SELECT * FROM [dbo].[Fleet_Region_Details] WITH (NOLOCK)";
                }
                else
                {
                    //SQL Statement 
                    sSql = "SELECT * FROM [dbo].[Fleet_Region_Details] WITH (NOLOCK) WHERE RegionId = " + RegionId + "";
                }
                

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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRegionName", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

    }

}