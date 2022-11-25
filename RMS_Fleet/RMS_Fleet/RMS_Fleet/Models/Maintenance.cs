using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace RMS_Fleet.Models
{
    public class MaintenanceAttributes
    {
      public string Make { get; set; }
      public string Model { get; set; }
      public string ChessisNumber { get; set; }

      public string ExpiryDate { get; set; }
      public string Document { get; set; }

      public string OneYearCompleteOfDocument { get; set; }
      public string CurrentDate { get; set; }
    }
    public class Maintenance
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

        public DataSet GetMakeModelChessisNumber(int RegionId, int BranchId, string VechileNumber)
        {
            Get_from_config();
            DataSet ds = new DataSet();
            ds.Tables.Add("0");
            ds.Tables.Add("1");
            try
            {
                string sql = string.Empty;
                string sql2 = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);
                //getting make,model,chessis no
                sql = "SELECT Make,Manufacturing_Year AS Model,ChesisNo FROM Fleet_Vehicle_Details " +
                       "WHERE RegionId=@RegionId AND BranchId=@BranchId AND VehicleNo=@VechileNumber ";

                SqlCommand cmd = new SqlCommand(sql,con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@RegionId", RegionId);
                cmd.Parameters.AddWithValue("@BranchId", BranchId);
                cmd.Parameters.AddWithValue("@VechileNumber", VechileNumber);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds.Tables[0]);

                //getting expirydate and document if is already inserted
                sql2 = @"SELECT ExpiryDate,Document FROM Fleet_Maintenance_Tax_Details WHERE VehicleNumber=@VechileNumber ";

                SqlCommand cmd2 = new SqlCommand(sql2, con);
                cmd2.CommandType = CommandType.Text;
                cmd2.Parameters.AddWithValue("@VechileNumber", VechileNumber);

                con.Open();
                SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);
                con.Close();

                sda2.Fill(ds.Tables[1]);
            }
            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetMakeModelChessisNumber" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "shiva.brata@sisprosegur.com", "suraj.kumar@sisprosegur.com", "", "Error Occurred: - GetMakeModelChessisNumber", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
            return ds;
        }

        public DataSet GetDocumentExpiryDate(int RegionId, int BranchId, string VechileNo, string Document)
        {
            Get_from_config();
            DataSet ds = new DataSet();

            try
            {
                string sql = string.Empty;

                SqlConnection con = new SqlConnection(strSqlConnectionString);

                sql = "SELECT * FROM Fleet_Maintenance_Tax_Details	WHERE RegionId=@RegionId AND BranchId=@BranchId " + 
                      "and VehicleNumber=@VehicleNumber and Document=@Document";

                SqlCommand cmd = new SqlCommand(sql,con);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@RegionId", RegionId);
                cmd.Parameters.AddWithValue("@BranchId", BranchId);
                cmd.Parameters.AddWithValue("@VehicleNumber", VechileNo);
                cmd.Parameters.AddWithValue("@Document", Document);

                con.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);
            }
            catch (Exception ex)
            { 
            
            }

            return ds;
        }

        public DataSet GetMaintenanceDocumentDate(string RegionId, string BranchId, string VechileNo, string document)
        {
            DataSet ds=new DataSet();
            try
            {
                Get_from_config();
                
                string sql=string.Empty;

                SqlConnection con=new SqlConnection(strSqlConnectionString);

                sql = "SELECT ExpiryDate,DATEADD(DAY,-4,ExpiryDate) AS OneYearComplete FROM Fleet_Maintenance_Tax_Details WHERE RegionId=@RegionId AND BranchId=@BranchId AND VehicleNumber=@VechileNo AND Document=@document";

                SqlCommand cmd=new SqlCommand(sql,con);
                cmd.Parameters.AddWithValue("@RegionId",RegionId);
                cmd.Parameters.AddWithValue("@BranchId",BranchId);
                cmd.Parameters.AddWithValue("@VechileNo",VechileNo);
                cmd.Parameters.AddWithValue("@document",document);

                con.Open();
                SqlDataAdapter sda=new SqlDataAdapter(cmd);
                con.Close();

                sda.Fill(ds);
            }
            catch(Exception ex)
            {
            
            }

            return ds;
        }
    }
    
}