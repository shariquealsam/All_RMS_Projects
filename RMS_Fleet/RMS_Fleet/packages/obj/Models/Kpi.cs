using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMS_Fleet.Models
{
    public class Kpi
    {
        public string BranchName { get; set; }
        public string VehicleNumber { get; set; }
        public int BranchId { get; set; }
        public string RegionName { get; set; }
        public string VehicleNo { get; set; }
        public string Make { get; set; }
        public string Manufacturing_Year { get; set; }
        public string RouteNo { get; set; }
        public string ChesisNo { get; set; }
        public string FuelType { get; set; }
        public string PetroCardNumber { get; set; }

        public string OpeningKM { get; set; }
        public string OpeningFuel { get; set; }
        public string ModifiedVechileByBranch { get; set; }
        public string TotalVechileByBranch { get; set; }
        public string MonthData { get; set; }

        public string ServiceType { get; set; }
    }
    public class AllRegion
    {
        public string Region_id { get; set; }
        public string Region_Name { get; set; }
    }
    public class KPLBulkUpload
    {
        public string VechileNumber { get; set; }
        public string FuelConsumptionInLtr { get; set; }
        public string FuelPurchasedThroughPetroCard { get; set; }
        public string Remarks { get; set; }
    }

    public class KPLVechileMasterDetails
    {
        public string OpeningKM { get; set; }
        public string OpeningFuelLiter { get; set; }
        public string StdKMPL { get; set; }
        public string BpNonBp { get; set; }
        public string FuelConsumption { get; set; }
        public string FuelPurchasedThroughPetroCard { get; set; }
        public string VechileNumber { get; set; }
    }

    public class KPLVechileBranchDetails
    {
        public string VechileNumber { get; set; }
        public string ClosingKM { get; set; }
        public string ClosingFuelLtr { get; set; }
        public string NonBillingKM { get; set; }
        public string RouteNumber { get; set; }
        public string UnitName { get; set; }
        public string FuelPurchasedInCash { get; set; }
        public string FuelPuchasedInLtr { get; set; }
        public string VendorName { get; set; }
        public string DriverName { get; set; }
        public string DriverPatId { get; set; }
        public string TypeOfServices { get; set; }
        public string Remarks { get; set; }
        public string Month { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public decimal TotalKM { get; set; }
        public decimal KMPL { get; set; }
        public decimal Fuel { get; set; }

    }

    public class KPLData
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
                sSql = "SELECT BranchId,BranchName FROM [dbo].[Fleet_Branch_Details] WITH (NOLOCK) WHERE RegionId = " + RegionId + "  ORDER BY BranchName";

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
                sSql = "SELECT BranchId,BranchName FROM [dbo].[Fleet_Branch_Details] WITH (NOLOCK) WHERE BranchId != 85 ORDER BY BranchName";

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
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRegionName", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
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
                sSql = "SELECT * FROM [dbo].[Fleet_Region_Details] WITH (NOLOCK)";

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetRegionNameWithoutParameter" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetRegionNameWithoutParameter", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

        public DataSet GetVehicleMaster(string RegionId,string BranchId)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                string RegionIdIfItIsZero = string.Empty;
                if (Convert.ToInt32(RegionId)==0)
                {
                    DataSet dsN = GetRegionIdByBranchId(Convert.ToInt32(BranchId));
                    RegionIdIfItIsZero = dsN.Tables[0].Rows[0]["RegionId"].ToString();
                }
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);
                if (Convert.ToInt32(RegionId) == 0)
                {
                    sSql = "SELECT VehicleNo FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) WHERE RegionId=" + RegionIdIfItIsZero + " and BranchId = " + BranchId + "";
                }
                else
                {
                    //SQL Statement 
                    sSql = "SELECT VehicleNo FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) WHERE RegionId=" + RegionId + " and BranchId = " + BranchId + "";
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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetVehicleMaster" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetVehicleMaster", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            //Return Result
            return ds;
        }

        public DataSet GetVechileListWhichIsNotInsertedInKPLBranch(string RegionId, string BranchId)
        {
            Get_from_config();

            //Dataset 
            DataSet ds = new DataSet();

            try
            {
                //it will be zero in case of admin
                string RegionIdIfItIsZero = string.Empty;
                if (Convert.ToInt32(RegionId) == 0)
                {
                    DataSet dsN = GetRegionIdByBranchId(Convert.ToInt32(BranchId));
                    RegionIdIfItIsZero = dsN.Tables[0].Rows[0]["RegionId"].ToString();
                }
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);
                if (Convert.ToInt32(RegionId) == 0)
                {
                    sSql = @"SELECT VehicleNo FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) WHERE RegionId=@RegionIdIfItIsZero and BranchId=@BranchId 
                            EXCEPT
                            SELECT VechileNumber FROM Fleet_KPL_Branch_Details WHERE VechileNumber IN(
                            SELECT VehicleNo FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) WHERE RegionId=@RegionIdIfItIsZero and BranchId=@Branchid AND VechileNumber IS NOT NULL
                            )
                            AND FORMAT(Month,'MMMM-yyyy')=FORMAT(DATEADD(MONTH,-1,GETDATE()),'MMMM-yyyy')
                            ";
                }
                else
                {
                    //SQL Statement 
                    sSql = @"SELECT VehicleNo FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) WHERE RegionId=@RegionId and BranchId=@BranchId 
                            EXCEPT
                            SELECT VechileNumber FROM Fleet_KPL_Branch_Details WHERE VechileNumber IN(
                            SELECT VehicleNo FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK) WHERE RegionId=@RegionId and BranchId=@BranchId AND VechileNumber IS NOT NULL
                            )
                            AND FORMAT(Month,'MMMM-yyyy')=FORMAT(DATEADD(MONTH,-1,GETDATE()),'MMMM-yyyy')
                            ";
                }
                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@RegionIdIfItIsZero", RegionIdIfItIsZero);
                cmd.Parameters.AddWithValue("@RegionId", RegionId);
                cmd.Parameters.AddWithValue("@BranchId",BranchId);

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
                sSql = " SELECT FRD.RegionName,FBD.BranchName,FVD.ChesisNo,FVD.VehicleNo,FVD.Make,FVD.Manufacturing_Year, " +
                        " FVED.FuelType,FVED.PetroCardNumber " +
                        " FROM [dbo].[Fleet_Vehicle_Details] FVD WITH (NOLOCK)  " +
                        " INNER JOIN [dbo].[Fleet_Branch_Details] FBD WITH (NOLOCK) ON (FVD.BranchId = FBD.BranchId) " +
                        " INNER JOIN [dbo].[Fleet_Region_Details] FRD WITH (NOLOCK) ON (FVD.RegionId = FRD.RegionId) " +
                        " LEFT JOIN [dbo].[Fleet_Vechile_Extra_Details] FVED WITH (NOLOCK) ON (FVD.VehicleNo = FVED.VechileNumber) " +
                        " WHERE VehicleNo = '" + VehicleNumber + "'";

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

        public DataSet GetAllVechileNumber()
        {
            Get_from_config();

            DataSet ds = new DataSet();
            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sSql = "SELECT ROW_NUMBER() OVER (ORDER BY VehicleNo) AS SR,VehicleNo,'' as FuelConsumptionInLtr,'' as FuelPurchasedThroughPetroCard FROM [dbo].[Fleet_Vehicle_Details] WITH(NOLOCK)";

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
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetAllVechileNumber" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetAllVechileNumber", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
          
            }
            return ds;
        }

        public DataSet GetAllMasterDetailsForUser(string VechileNumber)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                DateTime PreviousMonthDate = DateTime.Now.AddMonths(-1);

                string PreviousMonth = PreviousMonthDate.ToString("MM");
                string PreviousYear = PreviousMonthDate.ToString("yyyy");
                if (PreviousMonth == "10" || PreviousMonth == "11" || PreviousMonth == "12")
                {
                     PreviousMonth = PreviousMonthDate.ToString("MM");
                }
                else
                {
                     PreviousMonth = PreviousMonthDate.ToString("MM").Substring(1);
                }
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                string sql = "SELECT VehicleNo,StdKmpl,BpNonBp,FuelConsuption, " +
                                "FuelPuchasedThroughPetroCard,  " +
                                "(select case when  " +
                                "(  " +
                                " SELECT [OpeningKM] FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK)   " +
                                " where VechileNumber= @VechileNUmber and MONTH([Month]) = @PreviousMonth and year([Month]) = @PreviousYear  " +
                                ") is not null then   " +
                                "(  " +
                                " SELECT [OpeningKM]  FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK)   " +
                                " where VechileNumber= @VechileNUmber and MONTH([Month]) = @PreviousMonth and year([Month]) = @PreviousYear  " +
                                ")  " +
                                "else   " +
                                "(  " +
                                "SELECT ClosingKM FROM [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)    " +
                                "WHERE rec_id =    " +
                                "(    " +
                                "select max(rec_id) from [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)    " +
                                "where Format([Month],'yyyy-MM') < Format(@PreviousMonthDate,'yyyy-MM') and VechileNumber= @VechileNUmber    " +
                                ")      " +
                                ") end) as OpeningKM,  " +
                                "  " +
                                "(select case when  " +
                                "(  " +
                                " SELECT [OpeningFuel] FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK)   " +
                                " where VechileNumber= @VechileNUmber and MONTH([Month]) = @PreviousMonth and year([Month]) = @PreviousYear  " +
                                ") is not null then   " +
                                "(  " +
                                " SELECT [OpeningFuel]  FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK)   " +
                                " where VechileNumber= @VechileNUmber and MONTH([Month]) = @PreviousMonth and year([Month]) = @PreviousYear  " +
                                ")  " +
                                "else   " +
                                "(  " +
                                "SELECT [ClosingFuelLtr] FROM [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)    " +
                                "WHERE rec_id =    " +
                                "(    " +
                                "select max(rec_id) from [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)    " +
                                "where Format([Month],'yyyy-MM') < Format(@PreviousMonthDate,'yyyy-MM') and VechileNumber= @VechileNUmber    " +
                                ")      " +
                                ") end) as OpeningFuelLtr,   " +
                                "FORMAT(TblAll.[Month],'yyyy-MM') As Months    " +
                                "FROM    " +
                                "(    " +
                                "SELECT VehicleNo,StdKmpl,BpNonBp,TblAdmin.FuelConsuption,    " +
                                "TblAdmin.FuelPuchasedThroughPetroCard,[Month]    " +
                                " FROM     " +
                                "(    " +
                                "SELECT FVI.VehicleNo,FVED.StdKmpl,FVED.BpNonBp    " +
                                "FROM [dbo].[Fleet_Vehicle_Details] FVI WITH(NOLOCK)     " +
                                "LEFT JOIN [dbo].[Fleet_Vechile_Extra_Details] FVED WITH(NOLOCK) ON FVI.VehicleNo = FVED.VechileNumber    " +
                                "WHERE FVI.VehicleNo =  @VechileNUmber    " +
                                ")TblMaster    " +
                                "LEFT JOIN     " +
                                "(    " +
                                "SELECT VechileNumber,FuelConsuption,FuelPuchasedThroughPetroCard,[Month] FROM [Fleet_KPL_Bulk_Upload_Data] WITH(NOLOCK)    " +
                                "WHERE MONTH([Month]) = @PreviousMonth AND VechileNumber =  @VechileNUmber   " +
                                ")TblAdmin ON  TblMaster.VehicleNo = TblAdmin.VechileNumber    " +
                                ")TblAll  Order by [Month] desc";

                                SqlCommand cmd = new SqlCommand(sql, con);
                                cmd.Parameters.AddWithValue("@VechileNUmber", VechileNumber);
                                cmd.Parameters.AddWithValue("@PreviousMonth", PreviousMonth);
                                cmd.Parameters.AddWithValue("@PreviousYear", PreviousYear);
                                cmd.Parameters.AddWithValue("@PreviousMonthDate", PreviousMonthDate);
                                cmd.CommandType = CommandType.Text;


                // Changes made on 2019-11-07 for year paramenter addition 
                //string sql = " SELECT VehicleNo,StdKmpl,BpNonBp,FuelConsuption, " +
                //                "  FuelPuchasedThroughPetroCard, " +
                //                "  (select case when " +
                //                "  ( " +
                //                "   SELECT [OpeningKM] FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK)  " +
                //                "   where VechileNumber= @VechileNUmber and MONTH([Month]) = @PreviousMonth and year([Month]) = @PreviousYear " +
                //                "  ) is not null then  " +
                //                "  ( " +
                //                "   SELECT [OpeningKM]  FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK)  " +
                //                "   where VechileNumber= @VechileNUmber and MONTH([Month]) = @PreviousMonth and year([Month]) = @PreviousYear " +
                //                "  ) " +
                //                "  else  " +
                //                "  ( " +
                //                "  SELECT ClosingKM FROM [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)   " +
                //                "  WHERE rec_id =   " +
                //                "  (   " +
                //                "  select max(rec_id) from [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)   " +
                //                "  where MONTH([Month]) < @PreviousMonth and VechileNumber= @VechileNUmber   " +
                //                "  )     " +
                //                "  ) end) as OpeningKM, " +
                //                " " +
                //                "  (select case when " +
                //                "  ( " +
                //                "   SELECT [OpeningFuel] FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK)  " +
                //                "   where VechileNumber= @VechileNUmber and MONTH([Month]) = @PreviousMonth and year([Month]) = @PreviousYear " +
                //                "  ) is not null then  " +
                //                "  ( " +
                //                "   SELECT [OpeningFuel]  FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK)  " +
                //                "   where VechileNumber= @VechileNUmber and MONTH([Month]) = @PreviousMonth and year([Month]) = @PreviousYear " +
                //                "  ) " +
                //                "  else  " +
                //                "  ( " +
                //                "  SELECT [ClosingFuelLtr] FROM [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)   " +
                //                "  WHERE rec_id =   " +
                //                "  (   " +
                //                "  select max(rec_id) from [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)   " +
                //                "  where MONTH([Month]) < @PreviousMonth and VechileNumber= @VechileNUmber   " +
                //                "  )     " +
                //                "  ) end) as OpeningFuelLtr,  " +
                //                "  FORMAT(TblAll.[Month],'yyyy-MM') As Months   " +
                //                "  FROM   " +
                //                "  (   " +
                //                "  SELECT VehicleNo,StdKmpl,BpNonBp,TblAdmin.FuelConsuption,   " +
                //                "  TblAdmin.FuelPuchasedThroughPetroCard,[Month]   " +
                //                "   FROM    " +
                //                "  (   " +
                //                "  SELECT FVI.VehicleNo,FVED.StdKmpl,FVED.BpNonBp   " +
                //                "  FROM [dbo].[Fleet_Vehicle_Details] FVI WITH(NOLOCK)    " +
                //                "  LEFT JOIN [dbo].[Fleet_Vechile_Extra_Details] FVED WITH(NOLOCK) ON FVI.VehicleNo = FVED.VechileNumber   " +
                //                "  WHERE FVI.VehicleNo =  @VechileNUmber   " +
                //                "  )TblMaster   " +
                //                "  LEFT JOIN    " +
                //                "  (   " +
                //                "  SELECT VechileNumber,FuelConsuption,FuelPuchasedThroughPetroCard,[Month] FROM [Fleet_KPL_Bulk_Upload_Data] WITH(NOLOCK)   " +
                //                "  WHERE MONTH([Month]) = @PreviousMonth AND VechileNumber =  @VechileNUmber  " +
                //                "  )TblAdmin ON  TblMaster.VehicleNo = TblAdmin.VechileNumber   " +
                //                "  )TblAll   ";
                
                //SqlCommand cmd = new SqlCommand(sql,con);
                //cmd.Parameters.AddWithValue("@VechileNUmber", VechileNumber);
                //cmd.Parameters.AddWithValue("@PreviousMonth", PreviousMonth);
                //cmd.Parameters.AddWithValue("@PreviousYear", PreviousYear);
                //cmd.CommandType = CommandType.Text;

           
                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();
                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetAllMasterDetailsForUser" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetAllMasterDetailsForUser", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
            return ds;
        }

        public DataSet GetOpeningKM_OpeningFuel(string VechileNumber)
        {
            Get_from_config();
            DateTime PreviousMonthDate = DateTime.Now.AddMonths(-1);

            string PreviousMonth = PreviousMonthDate.ToString("MM");
            if (PreviousMonth == "10" || PreviousMonth == "11" || PreviousMonth == "12")
            {
                PreviousMonth = PreviousMonthDate.ToString("MM");
            }
            else
            {
                PreviousMonth = PreviousMonthDate.ToString("MM").Substring(1);
            }
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(strSqlConnectionString);
                string sql = @"SELECT VechileNumber,
(select case when
                                (
                                 SELECT top 1 [OpeningKM] FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK) 
                                 where VechileNumber= @VechileNumber and MONTH([Month]) = Month(@Month) and year([Month]) = Year(@Month)
                                ) is not null then 
                                (
                                 SELECT top 1  [OpeningKM]  FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK) 
                                 where VechileNumber= @VechileNumber and MONTH([Month]) = Month(@Month) and year([Month]) = Year(@Month)
                                )
                                else 
                                (
                                SELECT top 1  ClosingKM FROM [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)  
                                WHERE rec_id =  
                                (  
                                select max(rec_id) from [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)  
                                where Format([Month],'yyyy-MM') < Format(Cast(@Month as date),'yyyy-MM') and VechileNumber= @VechileNumber  
                                )    
                                ) end) as OpeningKM,
                                                                                                                                (select case when
                                (
                                 SELECT top 1  [OpeningFuel] FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK) 
                                 where VechileNumber= @VechileNumber and MONTH([Month]) = Month(@Month) and year([Month]) = Year(@Month)
                                ) is not null then 
                                (
                                 SELECT top 1  [OpeningFuel]  FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK) 
                                 where VechileNumber= @VechileNumber and MONTH([Month]) = Month(@Month) and year([Month]) = Year(@Month)
                                )
                                else 
                                (
                                SELECT [ClosingFuelLtr] FROM [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)  
                                WHERE rec_id =  
                                (  
                                select max(rec_id) from [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)  
                                where Format([Month],'yyyy-MM') < Format(Cast(@Month as date),'yyyy-MM') and VechileNumber= @VechileNumber  
                                )    
                                ) end) as OpeningFuel,[Month] FROM [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK) 
                            WHERE rec_id = 
                            ( 
                            select max(rec_id) from [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK) 
                            where Format([Month],'yyyy-MM') < Format(Cast(@Month as date),'yyyy-MM') and VechileNumber= @VechileNumber) ";

                //Change made on 2019-11-07 Addition Year segment
//                string sql = @"SELECT VechileNumber,
//                                (select case when
//                                (
//                                 SELECT top 1 [OpeningKM] FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK) 
//                                 where VechileNumber= @VechileNumber and MONTH([Month]) = @Month and year([Month]) = '2019'
//                                ) is not null then 
//                                (
//                                 SELECT top 1  [OpeningKM]  FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK) 
//                                 where VechileNumber= @VechileNumber and MONTH([Month]) = @Month and year([Month]) = '2019'
//                                )
//                                else 
//                                (
//                                SELECT top 1  ClosingKM FROM [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)  
//                                WHERE rec_id =  
//                                (  
//                                select max(rec_id) from [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)  
//                                where MONTH([Month]) < @Month and VechileNumber= @VechileNumber  
//                                )    
//                                ) end) as OpeningKM,
//                                                                                                                                (select case when
//                                (
//                                 SELECT top 1  [OpeningFuel] FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK) 
//                                 where VechileNumber= @VechileNumber and MONTH([Month]) = @Month and year([Month]) = '2019'
//                                ) is not null then 
//                                (
//                                 SELECT top 1  [OpeningFuel]  FROM [dbo].[Fleet_KPL_Opening_Closing] WITH(NOLOCK) 
//                                 where VechileNumber= @VechileNumber and MONTH([Month]) = @Month and year([Month]) = '2019'
//                                )
//                                else 
//                                (
//                                SELECT [ClosingFuelLtr] FROM [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)  
//                                WHERE rec_id =  
//                                (  
//                                select max(rec_id) from [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK)  
//                                where MONTH([Month]) < @Month and VechileNumber= @VechileNumber  
//                                )    
//                                ) end) as OpeningFuel,[Month] FROM [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK) 
//                            WHERE rec_id = 
//                            ( 
//                            select max(rec_id) from [dbo].[Fleet_KPL_Branch_Details] WITH(NOLOCK) 
//                            where MONTH([Month]) < @Month and VechileNumber= @VechileNumber 
//                            )";

                SqlCommand cmd = new SqlCommand(sql,con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@VechileNumber", VechileNumber);
                cmd.Parameters.AddWithValue("@Month", PreviousMonthDate);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);

                con.Close();
            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetOpeningKM_OpeningFuel" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetOpeningKM_OpeningFuel", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
            return ds;
        }

        public DataSet GetOpeningKM_OpeningFuelFromOpeningClosingTable(string VechileNumber)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //string sql = "SELECT OpeningKM,OpeningFuel from Fleet_KPL_Opening_Closing where VechileNumber=@VechileNumber";
                //Changes made On 2019-11-07 and added year check
                DateTime PreviousMonthDate = DateTime.Now.AddMonths(-1);
              
                string sql = "SELECT OpeningKM,OpeningFuel,[Month] from Fleet_KPL_Opening_Closing where VechileNumber=@VechileNumber and Year([Month])=Year(@PreviousMonthDate) and Month([Month])=Month(@PreviousMonthDate)";

                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@VechileNumber", VechileNumber);
                cmd.Parameters.AddWithValue("@PreviousMonthDate", PreviousMonthDate);

                //string sql = "SELECT OpeningKM,OpeningFuel,[Month] from Fleet_KPL_Opening_Closing where VechileNumber=@VechileNumber and Year([Month])=Year(Getdate()) and Month([Month])=Month(DATEADD(MONTH,-1,GETDATE()))";
                
                //SqlCommand cmd = new SqlCommand(sql, con);
                //cmd.CommandType = CommandType.Text;
                //cmd.Parameters.AddWithValue("@VechileNumber", VechileNumber);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);

                con.Close();
            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetOpeningKM_OpeningFuel" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetOpeningKM_OpeningFuel", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
            return ds;
        }

        public bool CheckVechileNumberExistingOrNot(string VechileNumber)
        {
            Get_from_config();
            bool check = false;
            int x = 0;
            try
            {
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                string sql = "SELECT * from Fleet_KPL_Opening_Closing where VechileNumber=@VechileNumber";
                SqlCommand cmd = new SqlCommand(sql,con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@VechileNumber", VechileNumber);
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                
                while (dr.Read())
                {
                    if (dr[0].ToString() == "")
                    {
                        check = false;
                    }
                    else {
                        check = true;
                    }
                }
                con.Close();
            }
            catch (Exception ex)
            {
                //ci.ErrorLog("C:\\RMS_Fleet_App\\", "CheckVechileNumberExistingOrNot" + ex.Message);
                //ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - CheckVechileNumberExistingOrNot", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
         
            }
            return check;
        }

        public DataSet CountDetailsOfVechileByBranch(int BranchId,int RegionId)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(strSqlConnectionString);
//                string sql = @"SELECT MODIFIED,(SELECT COUNT(*)  FROM Fleet_Vehicle_Details WHERE VehicleNo IN (SELECT VehicleNo FROM Fleet_Vehicle_Details WITH (NOLOCK) WHERE BranchId=@BranchId )) AS TOTAL FROM (
//                                SELECT COUNT(*) AS MODIFIED FROM Fleet_KPL_Opening_Closing WHERE VechileNumber IN (SELECT VehicleNo FROM Fleet_Vehicle_Details WITH (NOLOCK) WHERE BranchId=@BranchId AND ClosingKM!=0.00 AND ClosingFuel!=0.00) 
//                                )TAB";
//                string sql = @"SELECT (SELECT COUNT(*) FROM Fleet_Vehicle_Details WITH (NOLOCK) WHERE BranchId=@BranchId) AS TOTAL,MODIFIED,MonthData FROM (
//                            SELECT COUNT(*) AS MODIFIED,format(Month,'MMMM') as MonthData  FROM Fleet_KPL_Branch_Details WITH(NOLOCK) WHERE VechileNumber IN (SELECT VehicleNo FROM Fleet_Vehicle_Details WHERE BranchId=@BranchId)
//                            GROUP BY [Month]
//                            )TAB
//                            WHERE MonthData=format(DATEADD(MONTH,-1,GETDATE()),'MMMM')
//
//                    ";
                string sql = @"SELECT (SELECT COUNT(*) FROM Fleet_Vehicle_Details WHERE RegionId=@RegionId AND BranchId=@BranchId) AS TOTAL,MODI AS MODIFI FROM (
                            SELECT CASE WHEN COUNT(MODIFIED)>0 THEN COUNT(MODIFIED) ELSE 0 END AS MODI FROM (
                            SELECT COUNT(*) AS MODIFIED,VechileNumber FROM Fleet_KPL_Branch_Details WHERE VechileNumber IN (SELECT VehicleNo FROM Fleet_Vehicle_Details WHERE RegionId=@RegionId AND BranchId=@BranchId)
                            AND FORMAT(Month,'MMMM-yyyy')=FORMAT(DATEADD(MONTH,-1,GETDATE()),'MMMM-yyyy')
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

        public DataSet GetRegionIdByBranchId(int BranchID)
        { 
            Get_from_config();
            DataSet ds0 = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(strSqlConnectionString);
                string sql = "SELECT TOP 1 RegionId FROM Fleet_Vehicle_Details WHERE BranchId=@BranchId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@BranchId", BranchID);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Open();
                sda.Fill(ds0);
                con.Close();

                if (ds0.Tables[0].Rows.Count > 0)
                {
                    string RegionId = ds0.Tables[0].Rows[0]["RegionId"].ToString();
                }
            }
            catch (Exception ex)
            { 
                
            }
            return ds0;
        }

        public DataSet CountDetailsOfVechileByBranch(int BranchId)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                SqlConnection con = new SqlConnection(strSqlConnectionString);
                string sql = "SELECT TOP 1 RegionId FROM Fleet_Vehicle_Details WHERE BranchId=@BranchId";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@BranchId", BranchId);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataSet ds0 = new DataSet();
                con.Open();
                sda.Fill(ds0);
                con.Close();

                if (ds0.Tables[0].Rows.Count > 0)
                {
                    string RegionId = ds0.Tables[0].Rows[0]["RegionId"].ToString();

                    string sql2 = @"SELECT (SELECT COUNT(*) FROM Fleet_Vehicle_Details WHERE RegionId=@RegionId2 AND BranchId=@BranchId2) AS TOTAL,MODI AS MODIFI FROM (
                            SELECT CASE WHEN COUNT(MODIFIED)>0 THEN COUNT(MODIFIED) ELSE 0 END AS MODI FROM (
                            SELECT COUNT(*) AS MODIFIED,VechileNumber FROM Fleet_KPL_Branch_Details WHERE VechileNumber IN (SELECT VehicleNo FROM Fleet_Vehicle_Details WHERE RegionId=@RegionId2 AND BranchId=@BranchId2)
                            AND FORMAT(Month,'MMMM-yyyy')=FORMAT(DATEADD(MONTH,-1,GETDATE()),'MMMM-yyyy')
                            GROUP BY VechileNumber
                            )TAB
                            )TAB2";
                    SqlCommand cmd2 = new SqlCommand(sql2, con);
                    cmd2.CommandType = CommandType.Text;
                    cmd2.Parameters.AddWithValue("@BranchId2", BranchId);
                    cmd2.Parameters.AddWithValue("@RegionId2", RegionId);

                    con.Open();
                    SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);
                    con.Close();

                    sda2.Fill(ds);
                }
               

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "CountDetailsOfVechileByBranch" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - CountDetailsOfVechileByBranch", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetBranchDetailsByVechileNumberOfPreviousMonth(string VechileNumber)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            try
            {
                DateTime PreviousMonthDate = DateTime.Now.AddMonths(-1);
                string PreviousMonth = PreviousMonthDate.ToString("yyyy-MM");

                SqlConnection con = new SqlConnection(strSqlConnectionString);
                string sql = "SELECT * FROM Fleet_KPL_Branch_Details WHERE VechileNumber=@VechileNumber and FORMAT([Month],'yyyy-MM')=@PreviousMonth";
                SqlCommand cmd = new SqlCommand(sql,con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@VechileNumber", VechileNumber);
                cmd.Parameters.AddWithValue("@PreviousMonth", PreviousMonth);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();
                sda.Fill(ds);

            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetBranchDetailsByVechileNumberOfPreviousMonth" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetBranchDetailsByVechileNumberOfPreviousMonth", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
        }

        public DataSet GetRouteServiceDetails(string VechileNumber)
        {
            Get_from_config();
            DataSet ds=new DataSet();
            try
            {
                string sql=string.Empty;

                SqlConnection con=new SqlConnection(strSqlConnectionString);

                sql=@"SELECT VehicleNo,RouteNo,Service_Type FROM [dbo].[Fleet_Reporting_Details] WITH (NOLOCK)
                        WHERE RecId = 
                        ( 
                        SELECT MAX(RecId) as RecId FROM [dbo].[Fleet_Reporting_Details] FRD WITH (NOLOCK) WHERE VehicleNo =@VechileNumber
                        )";
                SqlCommand cmd=new SqlCommand(sql,con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@VechileNumber",VechileNumber);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);

                
            }
            catch(Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetRouteServiceDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "Error Occurred: - GetRouteServiceDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());

            }
            return ds;
            
        }
    }
}