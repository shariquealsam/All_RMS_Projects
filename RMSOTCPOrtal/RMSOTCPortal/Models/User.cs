using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;

namespace RMSOTCPortal.Models
{
    public class User
    {
        public string UserId
        {
            get;
            set;
        }

        public string UserName
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string EmailId
        {
            get;
            set;
        }

        public string ContactNos
        {
            get;
            set;
        }

        public string UserType
        {
            get;
            set;
        }

        public string LockUnlok
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public string Company
        {
            get;
            set;
        }

        public string CreatedBy
        {
            get;
            set;
        }

        public string CreatedOn
        {
            get;
            set;
        }

        public string ModifiedBy
        {
            get;
            set;
        }

        public string ModifiedOn
        {
            get;
            set;
        }

        public string Designation
        {
            get;
            set;
        }
        public string Department { get; set; }
    }

    public class UserDetails
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
                sSql = " SELECT * FROM [dbo].[OTC_User_Details] WHERE  UserId != 'PAT089885' AND UserId != 'PAT087498' AND UserId != 'PAT090128'";

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
                ci.ErrorLog("C:\\RMS_Cyclo_Manual\\", "GetUserDetails" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - GetUserDetails", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return ds;
        }

        public int UserDetailsInsert(string UserId, string UserName, string EmailId, string Password, string ContactNos, string UserType, string Active, string Company, string Created_By, string LastModified_By,string Department)
        {
            int iReturn = 0;
            SqlTransaction trans = null;

            Get_from_config();

            try
            {
                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                string sSql = "OTC_User_Insert";
                //end sql

                //Open Database Connection
                con.Open();

                using (trans = con.BeginTransaction())
                {
                    //Insert User Details
                    SqlCommand cmd = new SqlCommand(sSql, con, trans);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@UserId", UserId);
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@EmailId", EmailId);
                    cmd.Parameters.AddWithValue("@Password", ci.Encrypt(Password));
                    cmd.Parameters.AddWithValue("@ContactNos", ContactNos);
                    cmd.Parameters.AddWithValue("@UserType", UserType);
                    

                    if (Active == "ACTIVE")
                    {
                        cmd.Parameters.AddWithValue("@Active", 0);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Active", 1);
                    }

                    cmd.Parameters.AddWithValue("@Company", Company);
                    cmd.Parameters.AddWithValue("@Created_By", Created_By);
                    cmd.Parameters.AddWithValue("@LastModified_By", LastModified_By);
                    cmd.Parameters.AddWithValue("@Department", Department);
                    iReturn = cmd.ExecuteNonQuery();

                    trans.Commit();

                    con.Close();
                }
            }
            catch (Exception ex)
            {
                iReturn = 0;
                trans.Rollback();

                ci.ErrorLog("C:\\RMS_Cyclo_Manual\\", "OTCUserDetailsInsert" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - OTCUserDetailsInsert", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturn;
        }

        public bool CheckUserExist(string UserId)
        {
            bool isExist = false;
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
                sSql = " SELECT * FROM [dbo].[OTC_User_Details] where UserId = '" + UserId + "'";

                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);
                cmd.CommandType = CommandType.Text;

                //Data Adapter
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                da.Fill(ds);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        isExist = true;
                    }
                }

                //Close Database connection
                con.Close();
            }
            catch (Exception)
            {
            }

            //Return Result
            return isExist;
        }

        public int UserDetailsUpdate(string UserId, string UserName, string EmailId, string ContactNos, string UserType, string Active, string Company, string LastModified_By, string Password,string Department)
        {
            int iReturn = 0;

            Get_from_config();

            try
            {
                //Sql Connection
                SqlConnection con = new SqlConnection(strSqlConnectionString);

                //SQL Statement 
                string sSql = "";

                if (Active == "ACTIVE")
                {
                    Active = "0";
                }
                else
                {
                    Active = "1";
                }

                //SQL Statement 
                sSql = "Update [dbo].[OTC_User_Details] SET UserName ='" + UserName + "', EmailId ='" + EmailId + "', ContactNos ='" + ContactNos + "', UserType ='" + UserType + "', Active ='" + Active + "', Company ='" + Company + "', LastModified_By ='" + LastModified_By + "', Password ='" + ci.Encrypt(Password) + "', Department ='" + Department + "'  where UserId = '" + UserId + "'";

                //Open Database Connection
                con.Open();

                //Command text pass in sql
                SqlCommand cmd = new SqlCommand(sSql, con);

                iReturn = cmd.ExecuteNonQuery();

                con.Close();
            }
            catch (Exception ex)
            {
                iReturn = 0;
                ci.ErrorLog("C:\\RMS_Cyclo_Manual\\", "UserDetailsUpdate" + ex.Message);
                ci.SendMailMessage("support@sisprosegur.com", "sharique.aslam@sisprosegur.com", "", "", "Error Occurred: - UserDetailsUpdate", ex.Message.ToString() + "|" + ex.StackTrace.ToString());
            }

            return iReturn;
        }
    }
}