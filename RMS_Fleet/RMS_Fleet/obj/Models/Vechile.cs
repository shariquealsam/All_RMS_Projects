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
        public string ReportingDate { get; set; }
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
        public string CurrentDate { get; set; }

        public string PetroCardNumber { get; set; }
        public string STDKMPL { get; set; }
        public string BPNONBP { get; set; }
        public string FuelType { get; set; }

        public string DateOfInvoice { get; set; }
        public string InvoiceNumber { get; set; }
        public string FileNumber { get; set; }
        public string FinancierName { get; set; }
        public string ROI { get; set; }
        public string EMIStartDate { get; set; }
        public string EMIEndDate { get; set; }
        public string EMIAmount { get; set; }

        //MIS
        public string MISData { get; set; }
        public string Reason { get; set; }
        public string VendorName { get; set; }
        public string Segment { get; set; }
        public string Company { get; set; }
        public string PresentCompany { get; set; }
        public string RouteNumber { get; set; }
        public string RouteId { get; set; }
        public string Customer { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
        public string DriverPat { get; set; }
        public string DriverName { get; set; }
        public string DriverMobile { get; set; }
        

        public string VechileNumber { get; set; }
        public string EngineNumber { get; set; }
        public string FabricationName { get; set; }
        public string FabricationAmount { get; set; }
        public string ChassisAmount { get; set; }
        public string BodyAmount { get; set; }
        public string ChassisEMI { get; set; }
        public string BodyEMI { get; set; }
        public string TotalEMI { get; set; }
        public string ChassisLAN { get; set; }
        public string BodyLAN { get; set; }
        public string Tenure { get; set; }
        public string Other { get; set; }
        public string DepreciationValue { get; set; }
        public string TypeOfBody { get; set; }
        public string VehicleAge { get; set; }
        public string OwnerName { get; set; }
        public string NOCStatus { get; set; }
        public string DateOfNOC { get; set; } 
        public string VehicleSoldStatus { get; set; }
        public string VendarName { get; set; }
        public string VehicleSoldDate { get; set; }
        public string VehicleSoldAmount { get; set; }

       
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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetVehicleMaster", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - DeleteVechile", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
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
                       " FVED.[PetroCardNumber],FVED.[StdKmpl],FVED.[BpNonBp],FVED.[FuelType],FVED.[DateOfInvoice],FVED.[InvoiceNumber],FVED.[FileNumber],FVED.[FinancierName],FVED.[ROI],FVED.[EMIStartDate], " +
                       " FVED.[EMIEndDate],FVED.[EMIAmount],FVEDV.VechileNumber,FVEDV.EngineNumber,FVEDV.Location,FVEDV.Customer,FVEDV.FabricationName, " +
                       " FVEDV.FabricationAmount,FVEDV.ChassisAmount,FVEDV.BodyAmount,FVEDV.ChassisEMI,FVEDV.BodyEMI,FVEDV.TotalEMI, " +
                       " FVEDV.ChassisLAN,FVEDV.BodyLAN,FVEDV.Tenure,FVEDV.Other,FVEDV.DepreciationValue,FVEDV.TypeOfBody, " +
                       " FVEDV.VehicleAge,FVEDV.VehicleAge,FVEDV.OwnerName, FVEDV.NOCStatus, FORMAT(FVEDV.DateOfNOC,'yyyy-MM-dd') as DateOfNOC, FVEDV.VehicleSoldStatus, FVEDV.VendarName, FORMAT(FVEDV.VehicleSoldDate,'yyyy-MM-dd') as VehicleSoldDate, FVEDV.VehicleSoldAmount      " +
                       " FROM [dbo].[Fleet_Vehicle_Details] FVD WITH(NOLOCK) " +
                       " INNER JOIN  [dbo].[Fleet_Branch_Details] FBD WITH(NOLOCK) ON FVD.BranchId = FBD.BranchId " +
                       " INNER JOIN  [dbo].[Fleet_Region_Details] FRD  WITH(NOLOCK) ON FVD.RegionId = FRD.RegionId " +
                       " LEFT JOIN   [dbo].[Fleet_Vechile_Extra_Details] FVED WITH(NOLOCK) ON FVD.VehicleNo = FVED.VechileNumber " +
                       " LEFT JOIN   [dbo].[Fleet_Vechile_Extra_Details_V1] FVEDV WITH(NOLOCK) ON FVD.VehicleNo = FVEDV.VechileNumber " +
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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetEditDetailsValue", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetMasterRegion", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetMasterRegion", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }
    }
}