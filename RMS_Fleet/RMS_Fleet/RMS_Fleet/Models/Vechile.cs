using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMS_Fleet.Models
{
    public class Vechile
    {
        public string VehicleNumber { get; set; }
        public string Make { get; set; }
        public string ChassisNumber { get; set; }
        public string ManufacturingYear { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Region { get; set; }
        public string Branch { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int RegionIdBindValue { get; set; }
        public int Recid { get; set; }
        public bool Selected { get; set; }
        public string Status { get; set; }

        public string PetroCardNumber { get; set; }
        public string STDKMPL { get; set; }
        public string BPNONBP { get; set; }
        public string FuelType { get; set; }

    }

    public class VechileData
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

        public DataSet GetVehicleMaster()
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
                sSql = "SELECT FVD.[Recid],[VehicleNo],[Make],[ChesisNo],[Manufacturing_Year],[Password],FRD.[RegionName],FBD.[BranchName],FVED.PetroCardNumber " +
                        " FROM [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK)  " +
                        " INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH(NOLOCK) ON FVD.RegionId = FRD.RegionId  " +
                        " INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON FVD.BranchId = FBD.BranchId " +
                        " LEFT JOIN [dbo].[Fleet_Vechile_Extra_Details] FVED WITH(NOLOCK) ON FVD.[VehicleNo] = FVED.[VechileNumber] ";

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetVehicleMaster" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetVehicleMaster", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public int DeleteVechile(string Recid)
        {
            int iReturnsql = 0;

            Get_from_config();

            //Sql Connection
            SqlConnection con = new SqlConnection(strSqlConnectionString);
            SqlTransaction trans = null;

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //SQL Statement 
                sSql = " DELETE FROM [dbo].[Fleet_Vehicle_Details] WHERE [RecId] = " + Recid + " ";

                //Open Database Connection
                con.Open();

                using (trans = con.BeginTransaction())
                {
                    //Command text pass in my sql
                    SqlCommand cmd = new SqlCommand(sSql, con, trans);

                    //Connection Time Out
                    cmd.CommandTimeout = 1200;

                    //Data Adapter
                    iReturnsql = cmd.ExecuteNonQuery();

                    trans.Commit();

                    con.Close();
                }
            }

            catch (Exception ex)
            {
                trans.Rollback();
                ci.ErrorLog("C:\\RMS_Delete_Portal\\", "DeleteVechile" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - DeleteVechile", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturnsql;
        }

        public DataSet GetEditDetailsValue(string Recid)
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
                sSql = " Select [RecId],[VehicleNo],[Make],[ChesisNo],[Manufacturing_Year],[Password],FRD.[RegionId],FRD.[RegionName],FBD.[BranchId],FBD.[BranchName], " +
                       " FVED.[PetroCardNumber],FVED.[StdKmpl],FVED.[BpNonBp],FVED.[FuelType] " +
                       " FROM [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK) " +
                       " INNER JOIN  [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON FVD.BranchId = FBD.BranchId " +
                       " INNER JOIN  [dbo].[Fleet_Region_Details] FRD  WITH(NOLOCK) ON FVD.RegionId = FRD.RegionId " +
                       " LEFT JOIN  [dbo].[Fleet_Vechile_Extra_Details] FVED WITH(NOLOCK) ON FVD.VehicleNo = FVED.VechileNumber " +
                       " WHERE [RecId] = " + Recid + " ";

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetEditDetailsValue" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetEditDetailsValue", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetMasterRegion()
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
                sSql = " SELECT Distinct RegionName  FROM [dbo].[Fleet_Region_Details] WITH (NOLOCK) Order By RegionName ";

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetMasterRegion" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetMasterRegion", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public DataSet GetMasterBranch()
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
                sSql = " SELECT Distinct BranchName  FROM [dbo].[Fleet_Branch_Details] WITH (NOLOCK) Order By BranchName ";

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetMasterRegion" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "", "", "Error Occurred: - GetMasterRegion", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }
    }
}