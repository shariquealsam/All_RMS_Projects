using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMS_Fleet.Models
{
    public class User
    {
        //update
        public string RecId { get; set; }
        //-----------------------------------------
        public string UserName{ get; set; }
        public string EmailID { get; set; }
        public string ContactNo { get; set; }
        public string UserType { get; set; }
        public string Status { get; set; }
        public string ConfirmPassword { get; set; }
        public string Password { get; set; }
        public string RegionID { get; set; }
    }

    public class UserData
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

        public int UpdateUserDetail(string RecId, string status)
        {
            Get_from_config();
            //Dataset 
            //DataSet ds = new DataSet();
            //DataTable dt = new DataTable();
            int iReturn = 0;

            try
            {
                //Variable Declaration
                string sSql = string.Empty;

                //My Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                if (status == "Active")
                {
                    sSql = " Update [dbo].[Fleet_User_Details] " +
                        " Set Active = 0 where Rec_Id = '" + RecId + "'";
                }
                else
                {
                    sSql = " Update [dbo].[Fleet_User_Details] " +
                        " Set Active = 1 where Rec_Id = '" + RecId + "'";
                }

                //Open Database Connection
                con.Open();

                //Command text pass in my sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                iReturn = cmd.ExecuteNonQuery();
               
                //Connection Time Out
                //cmd.CommandTimeout = 1200;

                //Data Adapter
                //SqlDataAdapter da = new SqlDataAdapter(cmd);

                //da.Fill(ds);

                //Close Database connection
                con.Close();
            }

            catch (Exception ex)
            {
                ci.ErrorLog("C:\\RMS_Fleet_App\\", "GetBranchDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - UpdateUserStatus", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }
            return iReturn;
           
        }


        public DataSet GetUserDetails()
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
                sSql = " SELECT * FROM [dbo].[Fleet_User_Details] where Rec_Id NOT IN (2,3,4,5)";

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

        public int InsertUserDetails(string UserName,string EmailID,string ContactNo,string Password,string UserType,string Status,string CreatedBy,int RegionID)
        {
            int iReturn = 0;
            SqlTransaction trans = null;

            Get_from_config();

            try
            {
                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                string sSql = "Fleet_User_Insert";

                //Open Database Connection
                con.Open();

                using (trans = con.BeginTransaction())
                {
                    //Insert User Details
                    SqlCommand cmd = new SqlCommand(sSql, con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@EmailId", EmailID);
                    cmd.Parameters.AddWithValue("@Password", ci.Encrypt(Password));
                    cmd.Parameters.AddWithValue("@ContactNos", ContactNo);
                    cmd.Parameters.AddWithValue("@UserType", UserType);
                    cmd.Parameters.AddWithValue("@Active", Status);
                    cmd.Parameters.AddWithValue("@Created_By", CreatedBy);
                    cmd.Parameters.AddWithValue("@RegionId", RegionID);

                    iReturn = cmd.ExecuteNonQuery();

                    trans.Commit();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                iReturn = 0;
                trans.Rollback();

                ci.ErrorLog("C:\\RMS_Fleet_App\\", "User_Insert" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - User_Insert", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturn;
        }
    }
}